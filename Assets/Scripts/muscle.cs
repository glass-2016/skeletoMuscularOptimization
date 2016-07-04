using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class muscle : MonoBehaviour 
{
	private LineRenderer line;
	public musclesController controller;
	public List<GameObject> anchors;
	private Vector3[] attachPoints;
	private Vector3[] position;
	// Use this for initialization
	void Awake () 
	{
		line = GetComponent<LineRenderer> ();
		anchors = new List<GameObject> ();
		position = new Vector3[2];
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
		attachPoints [index] += value - attachPoints[index];
		line.SetPositions (attachPoints);
	}

	public void setLimits(Vector3[] attaches)
	{
		attachPoints = attaches;
		line.SetPositions(attaches);
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
