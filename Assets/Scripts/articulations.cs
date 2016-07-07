using UnityEngine;
using System.Collections;

public class articulations : MonoBehaviour {
	private musclesController controller;
	public ConfigurableJointMotion[] axis;
	public Vector3 axisLimits;
	private int index = 0;
	// Use this for initialization
	void Awake () 
	{
		axis = new ConfigurableJointMotion[3];
		axis [0] = ConfigurableJointMotion.Limited;
		axis [1] = ConfigurableJointMotion.Limited;
		axis [2] = ConfigurableJointMotion.Limited;
	}

	public int getIndex()
	{
		return (index);
	}

	public void setController(musclesController current, int i)
	{
		index = i;
		controller = current;
	}

	public void setLimitsAxis(Vector3 limits)
	{
		axisLimits = limits;
		controller.setLimitsAxis (axisLimits, index);
	}

	public void setAxis(ConfigurableJointMotion[] _axis)
	{
//		for (int i = 0; i < 3; i++)
//		{
//			if (_axis [i] == ConfigurableJointMotion.Free)
//				axis [i] = ConfigurableJointMotion.Free;
//		}
		axis = _axis;
		controller.setAxis (axis, index);
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
