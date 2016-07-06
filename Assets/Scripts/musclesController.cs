using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class musclesController : MonoBehaviour 
{
	private ConfigurableJoint joint = null;
	// anchor prefab for joint anchors only for debug
	public GameObject anchorPrefab;
	public bool debug = false;
	public List<muscle> listMuscles;
	private GameObject[] anchors;

	// Use this for initialization
	void Start () 
	{
		anchors = new GameObject[2];
		listMuscles = new List<muscle> ();
	}

	public void addDirection(vec3i dir)
	{
		joint.axis += new Vector3 (dir.x, dir.y, dir.z);
	}

	// configure ConfigurableJoint
	void addRigidBody(Rigidbody rb)
	{
		joint.connectedBody = rb;
		joint.enableCollision = true;
		joint.autoConfigureConnectedAnchor = false;
		joint.anchor = (rb.transform.position - transform.position) / 2.0f;
		joint.connectedAnchor = -(rb.transform.position - transform.position) / 2.0f;
		if (debug)
		{
			anchors[0] = Instantiate (anchorPrefab, joint.anchor, Quaternion.identity) as GameObject;
			anchors[1] = Instantiate (anchorPrefab, joint.connectedAnchor, Quaternion.identity) as GameObject;
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
	}

	// Update is called once per frame
	void Update () 
	{
		if (debug)
		{
			anchors[0].transform.position = (joint.connectedBody.transform.position - transform.position) / 2.0f;
			anchors[1].transform.position = -(joint.connectedBody.transform.position - transform.position) / 2.0f;
		}
	}
}
