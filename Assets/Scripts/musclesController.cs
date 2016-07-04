using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class musclesController : MonoBehaviour 
{
	private ConfigurableJoint joint = null;
	public List<muscle> listMuscles;
	public List<int> listIndex;
	// Use this for initialization
	void Start () 
	{
	}

	public void addMuscle(muscle tmp)
	{
		if (!joint)
			joint = gameObject.AddComponent<ConfigurableJoint> ();
		tmp.setController (this);
//		tmp.setLimits (attaches);
		listMuscles.Add (tmp);
		listIndex.Add (0);
	}

	public void setMuscle(muscle current)
	{
		listMuscles.Add (current);
		listIndex.Add (1);
	}

	public void setForce()
	{
	}

	// Update is called once per frame
	void Update () 
	{

	}
}
