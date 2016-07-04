using UnityEngine;
using System.Collections;

public class muscle : MonoBehaviour 
{
	private LineRenderer line;
	public musclesController controller;
	private Vector3[] attachPoints;
	private Vector3[] position;
	// Use this for initialization
	void Awake () 
	{
		line = GetComponent<LineRenderer> ();
	}

	public void setController(musclesController currentController)
	{
		controller = currentController;
	}

	public void changePosition(int index, Vector3 value)
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
	
	}
}
