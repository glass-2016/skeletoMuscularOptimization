using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class musclesController : MonoBehaviour 
{
	private ConfigurableJoint joint = null;
	// anchor prefab for joint anchors only for debug
	public articulations anchorPrefab;
	private Rigidbody rb;
	public bool debug = false;
	public List<muscle> listMuscles;
	public articulations[] anchors;

	// Use this for initialization
	void Start () 
	{
		anchors = new articulations[2];
		listMuscles = new List<muscle> ();
		rb = GetComponent<Rigidbody> ();
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

	public void addDirection(vec3i dir)
	{
		joint.axis += new Vector3 (dir.z, dir.y, dir.x);
//		ConfigurableJointMotion[] axisTmp = new ConfigurableJointMotion[3];
//		for (int i = 0; i < 3; i++)
//		{
//			if (dir.values [2 - i] > 0)
//				axisTmp [i] = ConfigurableJointMotion.Free;
//			else
//				axisTmp [i] = ConfigurableJointMotion.Limited;
//		}
//		anchors [0].setAxis (axisTmp);
		joint.targetRotation = Quaternion.Euler(joint.axis);
	}

	// configure ConfigurableJoint
	void addRigidBody(Rigidbody rb)
	{
		joint.connectedBody = rb;
		joint.enableCollision = true;
		joint.autoConfigureConnectedAnchor = false;
		joint.anchor = (rb.transform.position - transform.position) / 2.0f;
		joint.connectedAnchor = -(rb.transform.position - transform.position) / 2.0f;
		anchors[0] = Instantiate (anchorPrefab, joint.anchor, Quaternion.identity) as articulations;
		anchors [0].setController (this);
		if (debug)
		{
			anchors[1] = Instantiate (anchorPrefab, joint.connectedAnchor, Quaternion.identity) as articulations;
		}
		joint.xMotion = ConfigurableJointMotion.Limited;
		joint.yMotion = ConfigurableJointMotion.Limited;
		joint.zMotion = ConfigurableJointMotion.Limited;
		joint.angularXMotion = ConfigurableJointMotion.Free;
		joint.angularYMotion = ConfigurableJointMotion.Free;
		joint.angularZMotion = ConfigurableJointMotion.Free;
		joint.axis = Vector3.zero;
		joint.secondaryAxis = Vector3.zero;
	}

	// add connected muscle but isn't his controller
	public void setMuscle(muscle current)
	{
		listMuscles.Add (current);
	}

	// add muscle, set as controller and create joint if needed
	public void addMuscle(muscle tmp, Rigidbody rb)
	{
		if (!joint)
		{
			joint = gameObject.AddComponent<ConfigurableJoint> ();
			addRigidBody (rb);
		}
		tmp.setController (this);
		listMuscles.Add (tmp);
	}
		
	public void setForce(float force)
	{
		joint.targetVelocity += joint.axis * force;
		rb.angularVelocity = joint.targetVelocity * Time.deltaTime;
	}

	public void setAxis(ConfigurableJointMotion[] type)
	{
		joint.angularXMotion = type [0];
		joint.angularYMotion = type [1];
		joint.angularZMotion = type [2];
	}

	// Update is called once per frame
	void Update () 
	{
		if (joint && joint.connectedBody)
		{
			anchors[0].gameObject.transform.position = transform.position + (joint.connectedBody.transform.position - transform.position) / 2.0f;
			if (debug)
				anchors[1].gameObject.transform.position = -(joint.connectedBody.transform.position - transform.position) / 2.0f;
		}
	}
}
