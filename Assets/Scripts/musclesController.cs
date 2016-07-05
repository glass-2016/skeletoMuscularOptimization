using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class musclesController : MonoBehaviour 
{
	private ConfigurableJoint joint = null;
	public GameObject anchorPrefab;
	private GameObject[] anchors;
	// Use this for initialization
	void Start () 
	{
		anchors = new GameObject[2];
	}

	void addRigidBody(Rigidbody rb)
	{
		joint.connectedBody = rb;
		joint.enableCollision = true;
		joint.anchor = rb.transform.position - transform.position;
		joint.connectedAnchor = rb.transform.position - transform.position;
//		anchors[0] = Instantiate (anchorPrefab, joint.anchor, Quaternion.identity) as GameObject;
//		anchors[1] = Instantiate (anchorPrefab, joint.connectedAnchor, Quaternion.identity) as GameObject;
		joint.xMotion = ConfigurableJointMotion.Limited;
		joint.yMotion = ConfigurableJointMotion.Limited;
		joint.zMotion = ConfigurableJointMotion.Limited;
		joint.angularXMotion = ConfigurableJointMotion.Free;
		joint.angularYMotion = ConfigurableJointMotion.Free;
		joint.angularZMotion = ConfigurableJointMotion.Free;
	}

	public void addMuscle(muscle tmp, Rigidbody rb)
	{
		if (!joint)
		{
			joint = gameObject.AddComponent<ConfigurableJoint> ();
			addRigidBody (rb);
		}
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
