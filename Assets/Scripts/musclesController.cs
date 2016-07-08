using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class musclesController : MonoBehaviour 
{
	public Dictionary<muscle, ConfigurableJoint> joint;
	// anchor prefab for joint anchors only for debug
	public articulations anchorPrefab;
	private Rigidbody rb;
	public Dictionary<int, muscle> listMuscles;
	public Dictionary<muscle, articulations> anchors;
	public bool colliding = false;

	// Use this for initialization
	void Start () 
	{
		joint = new Dictionary<muscle, ConfigurableJoint> ();
		anchors = new Dictionary<muscle, articulations>();
		listMuscles = new Dictionary<int, muscle> ();
		rb = GetComponent<Rigidbody> ();
	}

	public void setLimitsAxis(Vector3 axisLimits, muscle index)
	{
		SoftJointLimit tmp = joint[index].lowAngularXLimit;
		tmp.limit = -axisLimits.x / 2.0f;
		joint[index].lowAngularXLimit = tmp;
		tmp = joint[index].highAngularXLimit;
		tmp.limit = axisLimits.x / 2.0f;
		joint[index].highAngularXLimit = tmp;
		tmp = joint[index].angularYLimit;
		tmp.limit = axisLimits.y;
		tmp = joint[index].angularZLimit;
		joint[index].angularYLimit = tmp;
		tmp.limit = axisLimits.z;
		joint[index].angularZLimit = tmp;
	}

	public void addDirection(Vector3 dir, muscle index)
	{
		joint[index].axis += new Vector3 (dir.x, dir.y, dir.z);
//		ConfigurableJointMotion[] axisTmp = new ConfigurableJointMotion[3];
//		for (int i = 0; i < 3; i++)
//		{
//			if (dir.values [2 - i] > 0)
//				axisTmp [i] = ConfigurableJointMotion.Free;
//			else
//				axisTmp [i] = ConfigurableJointMotion.Limited;
//		}
//		anchors [0].setAxis (axisTmp);
		joint[index].targetRotation = Quaternion.Euler(joint[index].axis);
	}

	bool checkAnchors(Dictionary<muscle, articulations> list, Vector3 anchor)
	{
		foreach (KeyValuePair<muscle, articulations> tmpMuscle in list)
		{
			if (tmpMuscle.Value.transform.position == anchor)
				return (false);
		}
		return (true);
	}

	// configure ConfigurableJoint
	void addRigidBody(Rigidbody rb, muscle index)
	{
		joint[index].connectedBody = rb;
		joint[index].enableCollision = true;
		joint[index].autoConfigureConnectedAnchor = false;
		joint[index].anchor = (rb.transform.position - transform.position) / 2.0f;
		joint[index].connectedAnchor = -(rb.transform.position - transform.position) / 2.0f;
		if (checkAnchors(rb.gameObject.GetComponent<musclesController> ().anchors, joint[index].anchor))
		{
			anchors.Add(index, Instantiate (anchorPrefab, joint[index].anchor, Quaternion.identity) as articulations);
			anchors[index].setController (this, index);
		}
		joint[index].xMotion = ConfigurableJointMotion.Limited;
		joint[index].yMotion = ConfigurableJointMotion.Limited;
		joint[index].zMotion = ConfigurableJointMotion.Limited;
		joint[index].angularXMotion = ConfigurableJointMotion.Free;
		joint[index].angularYMotion = ConfigurableJointMotion.Free;
		joint[index].angularZMotion = ConfigurableJointMotion.Free;
		joint[index].axis = Vector3.zero;
		joint[index].secondaryAxis = Vector3.zero;
	}


	// add connected muscle but isn't his controller
	public void setMuscle(muscle current)
	{
		listMuscles.Add (current.index, current);
	}

	// add muscle, set as controller and create joint if needed
	public void addMuscle(muscle tmp, Rigidbody rb)
	{
		joint.Add(tmp, gameObject.AddComponent<ConfigurableJoint> ());
		addRigidBody (rb, tmp);
		tmp.setController (this);
		listMuscles.Add (tmp.index, tmp);
	}
		
	public void setForce(float force, muscle index)
	{
		joint[index].targetVelocity += joint[index].axis * force;
		rb.angularVelocity = joint[index].targetVelocity * Time.deltaTime;
	}

	public void setAxis(ConfigurableJointMotion[] type, muscle index)
	{
		joint[index].angularXMotion = type [0];
		joint[index].angularYMotion = type [1];
		joint[index].angularZMotion = type [2];
	}

	// Update is called once per frame
	void Update () 
	{
		foreach (KeyValuePair<muscle, articulations> tmpMuscle in anchors)
		{
			tmpMuscle.Value.gameObject.transform.position = transform.position + (joint[tmpMuscle.Key].connectedBody.transform.position - transform.position) / 2.0f;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
		{
			Debug.Log ("Some bones collision!!!");
			colliding = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
			colliding = false;
	}
}
