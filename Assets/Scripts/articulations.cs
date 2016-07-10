using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class articulations : MonoBehaviour {
	public musclesController[] controllers;
	public ConfigurableJointMotion[] axis;
	public ConfigurableJoint joint;
	public Dictionary<int, muscle> muscles;
	public Vector3 axisLimits;
	private int muscleIndex = 0;
	public int index;
	public bool colliding = false;
	// Use this for initialization
	void Awake () 
	{
		axis = new ConfigurableJointMotion[3];
		axis [0] = ConfigurableJointMotion.Limited;
		axis [1] = ConfigurableJointMotion.Limited;
		axis [2] = ConfigurableJointMotion.Limited;
		muscles = new Dictionary<int, muscle> ();
		controllers = new musclesController[2];
	}

	public void setLimitsAxis(Vector3 axisLimits)
	{
		SoftJointLimit tmp = joint.lowAngularXLimit;
		tmp.limit = -axisLimits.x / 2.0f;
		joint.lowAngularXLimit = tmp;
		tmp = joint.highAngularXLimit;
		tmp.limit = axisLimits.x / 2.0f;
		joint.highAngularXLimit = tmp;
		tmp = joint.angularYLimit;
		tmp.limit = axisLimits.y;
		tmp = joint.angularZLimit;
		joint.angularYLimit = tmp;
		tmp.limit = axisLimits.z;
		joint.angularZLimit = tmp;
	}

	public void addDirection(Vector3 dir)
	{
		joint.axis += new Vector3 (dir.x, dir.y, dir.z);
		ConfigurableJointMotion[] axisTmp = new ConfigurableJointMotion[3];
		for (int i = 0; i < 3; i++)
		{
			if (dir[i] > 0.5)
				axisTmp [i] = ConfigurableJointMotion.Limited;
			else
				axisTmp [i] = ConfigurableJointMotion.Locked;
		}
		axis = axisTmp;
		joint.targetRotation = Quaternion.Euler(joint.axis);
	}

	void addMuscle(muscle current)
	{
		muscles.Add (muscleIndex, current);
		current.setIndex (muscleIndex);
		muscleIndex++;
	}

	// configure ConfigurableJoint
	public void addRigidBody(Rigidbody rb, muscle current)
	{
		joint = gameObject.AddComponent<ConfigurableJoint> ();
		joint.connectedBody = rb;
		joint.enableCollision = true;
		joint.autoConfigureConnectedAnchor = false;
		joint.anchor = (rb.transform.position - transform.position) / 2.0f;
		joint.connectedAnchor = -(rb.transform.position - transform.position) / 2.0f;
		joint.xMotion = ConfigurableJointMotion.Limited;
		joint.yMotion = ConfigurableJointMotion.Limited;
		joint.zMotion = ConfigurableJointMotion.Limited;
		joint.angularXMotion = ConfigurableJointMotion.Free;
		joint.angularYMotion = ConfigurableJointMotion.Free;
		joint.angularZMotion = ConfigurableJointMotion.Free;
		joint.secondaryAxis = Vector3.zero;
		addMuscle (current);
	}

	public void setForce(float force)
	{
		//		rb.WakeUp ();
		//		joint [index].connectedBody.WakeUp ();
		//		joint[index].targetAngularVelocity += joint[index].axis * force;
		joint.targetVelocity += joint.axis * force;
		joint.connectedBody.angularVelocity = transform.TransformDirection(joint.targetVelocity) * Time.deltaTime;
	}

	public void setControllerAndIndex(musclesController current, int i)
	{
		index = i;
		controllers[0] = current;
		setLimitsAxis (new Vector3(180, 180, 180));
	}

	public void setOtherController(musclesController current)
	{
		controllers [1] = current;
	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
		{
			Debug.Log ("Some articulations collision!!!");
			colliding = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
			colliding = false;
	}
}
