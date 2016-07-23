using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class muscle : MonoBehaviour 
{
	public Vector3 angularDirection;
	public float force = 10f;
	public string key1 = "A";
	private KeyCode kc1;
	public GameObject[] anchors;
	public articulations currentArticulation;
	public Vector3[] attachPoints;
	// keep positions on bones models
	public Vector3[] offsets;
	public Vector3[] position;
	public Vector3[] normals;
	public int indexMuscle = 0;
	public bool started = false;
	public bool onPlay = false;

	// Use this for initialization
	void Awake () 
	{
		anchors = new GameObject[2];
		position = new Vector3[2];
		offsets = new Vector3[2];
		normals = new Vector3[2];
		attachPoints = new Vector3[2];
	}

	void Start()
	{
		started = true;
	}

	// add bone as muscle anchor
	public void setAnchor(GameObject current, int index)
	{
		position [index] = current.transform.position;
		anchors[index] = current;
	}

	public void setArticulation(articulations current)
	{
		currentArticulation = current;
		key1 = ((char)(key1.ToCharArray()[0] + currentArticulation.index + currentArticulation.muscles.Count - 1)).ToString();
	}

	// update position with bone movement
	void changePosition(int index, Vector3 value)
	{
		Debug.Log (attachPoints[0]);
		Debug.Log (attachPoints[1]);
		Debug.Log ("index = " + index);
		position [index] = value;
		attachPoints [index] = value + offsets[index];
		transform.position = Vector3.Lerp (attachPoints[0], attachPoints[1], 0.5f);
		transform.rotation = Quaternion.FromToRotation(Vector3.forward, attachPoints[0] - attachPoints[1]);
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Vector3.Distance(attachPoints[0], attachPoints[1]) * 1.25f);
	}

	public void setIndex(int i)
	{
		indexMuscle = i;
	}

	void updateAngularDirection()
	{
		Vector3 offset = new Vector3 (Mathf.Abs(attachPoints [0].x - attachPoints [1].x), Mathf.Abs(attachPoints [0].y - attachPoints [1].y), Mathf.Abs(attachPoints [0].z - attachPoints [1].z));
		angularDirection = Vector3.Cross (offset.normalized, normals[0] + normals[1]);
//		angularDirection = new Vector3 (Mathf.Abs(angularDirection.x), Mathf.Abs(angularDirection.y), Mathf.Abs(angularDirection.z));
	}

	// configure prefabs to get attach to bones
	public void setLimits(Vector3[] attaches, Vector3 pos1, Vector3 pos2, Vector3 norm1, Vector3 norm2)
	{
		attachPoints[0] = attaches[0];
		attachPoints[1] = attaches[1];
		offsets [0] = attaches [0] - pos1;
		offsets [1] = attaches [1] - pos2;
		normals [0] = norm1;
		normals [1] = norm2;
		transform.position = Vector3.Lerp (attachPoints[0], attachPoints[1], 0.5f);
		transform.rotation = Quaternion.FromToRotation(Vector3.forward, attachPoints[0] - attachPoints[1]);
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Vector3.Distance(attachPoints[0], attachPoints[1]) * 1.25f);
		// define direction forces
		updateAngularDirection();
		if (onPlay)
			currentArticulation.addDirection (angularDirection);
	}

	// Update is called once per frame
	void Update () 
	{
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

		for (int i = 0; i < 2; i++)
		{
			// changes position when bones position changes
			if (position [i] != anchors [i].transform.position)
				changePosition ((i + 1) % 2, anchors [i].transform.position);
		}

		if (onPlay && Input.GetKey (kc1))
		{
			currentArticulation.setForce (force, this);
		}
	}
}
