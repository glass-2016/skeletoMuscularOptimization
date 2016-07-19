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
	public int index = 0;
	private float reverse = 1.0f;
	private bool changedReverse = false; 
	// Use this for initialization
	void Awake () 
	{
		anchors = new List<GameObject> ();
		position = new Vector3[2];
		offsets = new Vector3[2];
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
		transform.rotation = Quaternion.FromToRotation(Vector3.up, attachPoints[0] - attachPoints[1]);
		transform.localScale = new Vector3(transform.localScale.x, Vector3.Distance(attachPoints[0], attachPoints[1]) * 0.5f, transform.localScale.z);
	}

	public void setIndex(int i)
	{
		index = i;
	}

	void updateAngularDirection()
	{
		Vector3 tmp = Vector3.zero;
		if (Mathf.Abs (offsets [0].x) + Mathf.Abs (offsets [1].x) > Mathf.Abs (offsets [0].y) + Mathf.Abs (offsets [1].y) && Mathf.Abs (offsets [0].x) + Mathf.Abs (offsets [1].x) > Mathf.Abs (offsets [0].z) + Mathf.Abs (offsets [1].z))
			tmp = Vector3.forward * (offsets [0].x + offsets [1].x);
		else if (Mathf.Abs(offsets[0].y) + Mathf.Abs (offsets [1].y) > Mathf.Abs(offsets[0].x) + Mathf.Abs (offsets [1].x) && Mathf.Abs(offsets[0].y) + Mathf.Abs (offsets [1].y) > Mathf.Abs(offsets[0].z) + Mathf.Abs (offsets [1].z))
			tmp = Vector3.right * (offsets [0].y + offsets [1].y);
		else
			tmp = Vector3.up * (offsets [0].z + offsets [1].z);
		angularDirection = Vector3.Cross (transform.up, -tmp).normalized;
	}

	// configure prefabs to get attach to bones
	public void setLimits(Vector3[] attaches, Vector3 pos1, Vector3 pos2)
	{
		attachPoints = attaches;
		offsets [0] = attaches [0] - pos1;
		offsets [1] = attaches [1] - pos2;
		transform.position = attaches[1] + ((attaches [0] - attaches [1]) / 2.0f);
		transform.rotation = Quaternion.FromToRotation(Vector3.forward, attachPoints[0] - attachPoints[1]);
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Vector3.Distance(attachPoints[0], attachPoints[1]) * 1.25f);
		// define direction forces
		updateAngularDirection();
		direction = transform.right * reverse;
		currentArticulation.addDirection (direction);
	}

	// Update is called once per frame
	void Update () 
	{
//		direction = new Vector3(-transform.up.x * (offsets[1].x - Mathf.Abs(offsets[0].x)), -transform.up.y * (offsets[1].y - Mathf.Abs(offsets[0].y)), -transform.up.z * (offsets[1].z - Mathf.Abs(offsets[0].z))).normalized;
		direction = transform.right * reverse;
		//reading the string input chosen by the player and converting it to keycode
		//trying to find if a number was entered
		int asck1 = 0;
		int asck2 = 0;

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
		if (currentScale < transform.localScale.y && !changedReverse)
		{
			changedReverse = true;
			reverse = -reverse;
		} else if (currentScale >= transform.localScale.y)
			changedReverse = false;
		if (Input.GetKey (kc1))
		{
			currentArticulation.setForce (force, this);
		}
//		else
//		{
//			currentArticulation.targetVelocity = 0;
//			currentArticulation.useMotor = false;
//		}
	}
}
