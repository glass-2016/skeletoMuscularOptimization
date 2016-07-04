using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class musclesController : MonoBehaviour 
{
	public ConfigurableJoint joint = null;
	// Use this for initialization
	void Start () 
	{
	}

	public void addMuscle(muscle tmp)
	{
		if (!joint)
			joint = gameObject.AddComponent<ConfigurableJoint> ();
		tmp.setController (this);
	}

	public void setForce()
	{
	}

	// Update is called once per frame
	void Update () 
	{

	}
}
