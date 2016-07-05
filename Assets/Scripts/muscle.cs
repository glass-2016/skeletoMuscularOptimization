using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class muscle : MonoBehaviour 
{
	public musclesController controller;
	public List<GameObject> anchors;
	private Vector3[] attachPoints;
	private Vector3[] offsets;
	private Vector3[] position;
	// Use this for initialization
	void Awake () 
	{
		anchors = new List<GameObject> ();
		position = new Vector3[2];
		offsets = new Vector3[2];
	}

	public void setAnchor(GameObject current)
	{
		position [anchors.Count] = current.transform.position;
		anchors.Add(current);
	}

	public void setController(musclesController currentController)
	{
		controller = currentController;
	}

	void changePosition(int index, Vector3 value)
	{
		attachPoints [index] += value - attachPoints[index] + offsets[index];
		transform.position = attachPoints[1] + (attachPoints [0] - attachPoints [1]) / 2.0f;
//		transform.localScale = new Vector3(transform.localScale.x, (attachPoints [1].y - attachPoints [0].y) * 0.6f, transform.localScale.z);
		transform.localRotation = Quaternion.FromToRotation(Vector3.up, attachPoints[0] - attachPoints[1]);
	}

	public void setLimits(Vector3[] attaches, Vector3 pos1, Vector3 pos2)
	{
		attachPoints = attaches;
		offsets [0] = attaches [0] - pos1;
		offsets [1] = attaches [1] - pos2;
		transform.position = attaches[1] + ((attaches [0] - attaches [1]) / 2.0f);
		transform.localScale = new Vector3(transform.localScale.x, (attachPoints [1].y - attachPoints [0].y) * 0.5f, transform.localScale.z);
		transform.localRotation = Quaternion.FromToRotation(Vector3.up, attachPoints[0] - attachPoints[1]);
	}

	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < anchors.Count; i++)
		{
			if (position [i] != anchors[i].transform.position)
				changePosition (i, anchors[i].transform.position);
		}
	}
}
