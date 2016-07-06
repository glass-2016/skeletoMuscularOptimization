using UnityEngine;
using System.Collections;

public class articulations : MonoBehaviour {
	private musclesController controller;
	public ConfigurableJointMotion[] axis;
	// Use this for initialization
	void Start () 
	{
		axis = new ConfigurableJointMotion[3];
	}

	public void setController(musclesController current)
	{
		controller = current;
	}

	public void setAxis(ConfigurableJointMotion[] _axis)
	{
		axis = _axis;
		controller.setAxis (_axis);
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
