using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vec3i
{
	public int x;
	public int y;
	public int z;
	public int[] values;
	public vec3i(int _x, int _y, int _z)
	{
		x = _x;
		y = _y;
		z = _z;
		values = new int[3];
		values [0] = _x;
		values [1] = _y;
		values [2] = _z;
	}
};

public class muscle : MonoBehaviour 
{
	public musclesController controller;
	private vec3i direction;
	public float force = 10f;
	public List<GameObject> anchors;
	private Vector3[] attachPoints;
	// keep positions on bones models
	private Vector3[] offsets;
	private Vector3[] position;
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

	// configure prefabs to get attach to bones
	public void setLimits(Vector3[] attaches, Vector3 pos1, Vector3 pos2)
	{
		attachPoints = attaches;
		offsets [0] = attaches [0] - pos1;
		offsets [1] = attaches [1] - pos2;
		transform.position = attaches[1] + ((attaches [0] - attaches [1]) / 2.0f);
		transform.rotation = Quaternion.FromToRotation(Vector3.up, attachPoints[0] - attachPoints[1]);
		transform.localScale = new Vector3(transform.localScale.x, Vector3.Distance(attachPoints[0], attachPoints[1]) * 0.5f, transform.localScale.z);
		Vector3 tmpVec = (offsets [0] + offsets [1]);
		direction = new vec3i (Mathf.FloorToInt(Mathf.Abs(tmpVec.x)), Mathf.FloorToInt(Mathf.Abs(tmpVec.y)), Mathf.FloorToInt(Mathf.Abs(tmpVec.z)));
		controller.addDirection (direction);
	}

	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < anchors.Count; i++)
		{
			// changes position when bones position changes
			if (position [i] != anchors[i].transform.position)
				changePosition (i, anchors[i].transform.position);
		}
		if (Input.GetKey (KeyCode.F))
			controller.setForce (force);
	}
}
