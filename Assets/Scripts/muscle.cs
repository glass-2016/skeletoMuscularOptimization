using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class muscle : MonoBehaviour 
{
	public Vector3 direction;
	public Vector3 angularDirection;
	public float force = 10f;
	public string key1 = "A";
	private KeyCode kc1;
	private KeyCode kc2;
	public List<GameObject> anchors;
	public articulations currentArticulation;
	private Vector3[] attachPoints;
	// keep positions on bones models
	private Vector3[] offsets;
	private Vector3[] position;
	private Vector3[] normals;
	public int index = 0;

	// Use this for initialization
	void Awake () 
	{
		anchors = new List<GameObject> ();
		position = new Vector3[2];
		offsets = new Vector3[2];
		normals = new Vector3[2];
	}

	// add bone as muscle anchor
	public void setAnchor(GameObject current)
	{
		position [anchors.Count] = current.transform.position;
		anchors.Add(current);
	}

	public void setArticulation(articulations current)
	{
		currentArticulation = current;
		key1 = ((char)(key1.ToCharArray()[0] + currentArticulation.index + currentArticulation.muscles.Count - 1)).ToString();
	}

	// update position with bone movement
	void changePosition(int index, Vector3 value)
	{
		attachPoints [index] += value - attachPoints[index] + offsets[index];
		transform.position = attachPoints[1] + (attachPoints [0] - attachPoints [1]) / 2.0f;
		transform.rotation = Quaternion.FromToRotation(Vector3.forward, attachPoints[0] - attachPoints[1]);
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Vector3.Distance(attachPoints[0], attachPoints[1]) * 1.25f);
	}

	public void setIndex(int i)
	{
		index = i;
	}

	void updateAngularDirection()
	{
		Vector3 offset = new Vector3 (Mathf.Abs(attachPoints [0].x - attachPoints [1].x), Mathf.Abs(attachPoints [0].y - attachPoints [1].y), Mathf.Abs(attachPoints [0].z - attachPoints [1].z));
		Debug.Log ("offsets = " + offset);
		Debug.Log ("normals[0] = " + normals[0]);
		Debug.Log ("normals[1] = " + normals[1]);
		angularDirection = Vector3.Cross (offset.normalized, normals[0] + normals[1]);
//		angularDirection = new Vector3 (Mathf.Abs(angularDirection.x), Mathf.Abs(angularDirection.y), Mathf.Abs(angularDirection.z));
	}

	// configure prefabs to get attach to bones
	public void setLimits(Vector3[] attaches, Vector3 pos1, Vector3 pos2, Vector3 norm1, Vector3 norm2)
	{
		attachPoints = attaches;
		offsets [0] = attaches [0] - pos1;
		offsets [1] = attaches [1] - pos2;
		normals [0] = norm1;
		normals [1] = norm2;
		transform.position = attaches[1] + ((attaches [0] - attaches [1]) / 2.0f);
		transform.rotation = Quaternion.FromToRotation(Vector3.forward, attachPoints[0] - attachPoints[1]);
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Vector3.Distance(attachPoints[0], attachPoints[1]) * 1.25f);
		// define direction forces
		updateAngularDirection();
//		direction = transform.up * reverse;
		currentArticulation.addDirection (angularDirection);
	}

	// Update is called once per frame
	void Update () 
	{
//		direction = new Vector3(-transform.up.x * (offsets[1].x - Mathf.Abs(offsets[0].x)), -transform.up.y * (offsets[1].y - Mathf.Abs(offsets[0].y)), -transform.up.z * (offsets[1].z - Mathf.Abs(offsets[0].z))).normalized;
//		direction = transform.up * reverse;
		//reading the string input chosen by the player and converting it to keycode
		//trying to find if a number was entered
		int asck1 = 0;

		if (key1.Length < 2) {
			asck1 = System.Convert.ToChar (key1);
		}

		//excluding the utility Fkeys and converting to numpad if a number was entered
		if ( asck1 > 47 && asck1 < 58 && !key1.Contains("F")) 
		{
			key1 = key1.Insert (0, "Keypad");
		}
			
		kc1 = (KeyCode)System.Enum.Parse (typeof(KeyCode), key1);

		float currentScale = transform.localScale.y;
		for (int i = 0; i < anchors.Count; i++)
		{
			// changes position when bones position changes
			if (position [i] != anchors [i].transform.position)
				changePosition (i, anchors [i].transform.position);
		}

		if (Input.GetKey (kc1))
		{
			currentArticulation.setForce (force, this);
		}
	}
}
