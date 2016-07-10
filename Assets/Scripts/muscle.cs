using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class muscle : MonoBehaviour 
{
	public musclesController controller;
	private Vector3 direction;
	public float force = 10f;
	public string key1 = "A";
	public string key2 = "B";
	private KeyCode kc1;
	private KeyCode kc2;
	public List<GameObject> anchors;
	public articulations currentArticulation;
	private Vector3[] attachPoints;
	// keep positions on bones models
	private Vector3[] offsets;
	private Vector3[] position;
	public int index = 0;
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
	}

	// set controller for configurableJoint
	public void setController(musclesController currentController)
	{
		controller = currentController;
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

	// configure prefabs to get attach to bones
	public void setLimits(Vector3[] attaches, Vector3 pos1, Vector3 pos2)
	{
		attachPoints = attaches;
		offsets [0] = attaches [0] - pos1;
		offsets [1] = attaches [1] - pos2;
		transform.position = attaches[1] + ((attaches [0] - attaches [1]) / 2.0f);
		transform.rotation = Quaternion.FromToRotation(Vector3.up, attachPoints[0] - attachPoints[1]);
		transform.localScale = new Vector3(transform.localScale.x, Vector3.Distance(attachPoints[0], attachPoints[1]) * 0.5f, transform.localScale.z);
		// define direction forces 
		direction = -transform.right;
		currentArticulation.addDirection (direction);
	}

	// Update is called once per frame
	void Update () 
	{

		//reading the string input chosen by the player and converting it to keycode
		//trying to find if a number was entered
		int asck1 = 0;
		int asck2 = 0;

		if (key1.Length < 2) {
			asck1 = System.Convert.ToChar (key1);
		}
		if (key2.Length < 2) {
			asck2 = System.Convert.ToChar (key2);
		}

		//excluding the utility Fkeys and converting to numpad if a number was entered
		if ( asck1 > 47 && asck1 < 58 && !key1.Contains("F")) 
		{
			key1 = key1.Insert (0, "Keypad");
		}

		if ( asck2 > 47 && asck2 < 58 && !key2.Contains("F")) 
		{
			key2 = key2.Insert (0, "Keypad"); 
		}
		kc1 = (KeyCode)System.Enum.Parse (typeof(KeyCode), key1);
		kc2 = (KeyCode)System.Enum.Parse (typeof(KeyCode), key2);


		for (int i = 0; i < anchors.Count; i++)
		{
			// changes position when bones position changes
			if (position [i] != anchors[i].transform.position)
				changePosition (i, anchors[i].transform.position);
		}
		if (Input.GetKey (kc1))
			currentArticulation.setForce (force);

//		if (Input.GetKey (kc2))
//			currentArticulation.setForce (-force);
	}
}
