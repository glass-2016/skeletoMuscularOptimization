using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class musclesController : MonoBehaviour 
{
	public List<ConfigurableJoint> joint;
	// anchor prefab for joint anchors only for debug
	public articulations anchorPrefab;
	private Rigidbody rb;
	public List<muscle> listMuscles;
	public List<articulations> anchors;
	public int currentIndex = 0;

	// Use this for initialization
	void Start () 
	{
		joint = new List<ConfigurableJoint> ();
		anchors = new List<articulations>();
		listMuscles = new List<muscle> ();
		rb = GetComponent<Rigidbody> ();
	}

	public void setLimitsAxis(Vector3 axisLimits, int index)
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

	public void addDirection(vec3i dir, int index)
	{
		joint[index].axis += new Vector3 (dir.z, dir.x, dir.y);
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

	bool checkAnchors(List<articulations> list, Vector3 anchor)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list [i].transform.position == anchor)
				return (false);
		}
		return (true);
	}

	// configure ConfigurableJoint
	void addRigidBody(Rigidbody rb, int index)
	{
		joint[index].connectedBody = rb;
		joint[index].enableCollision = true;
		joint[index].autoConfigureConnectedAnchor = false;
		joint[index].anchor = (rb.transform.position - transform.position) / 2.0f;
		joint[index].connectedAnchor = -(rb.transform.position - transform.position) / 2.0f;
		if (checkAnchors(rb.gameObject.GetComponent<musclesController> ().anchors, joint[index].anchor))
		{
			anchors.Add(Instantiate (anchorPrefab, joint[index].anchor, Quaternion.identity) as articulations);
			anchors[anchors.Count - 1].setController (this, index);
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
		listMuscles.Add (current);
	}

	// add muscle, set as controller and create joint if needed
	public void addMuscle(muscle tmp, Rigidbody rb)
	{
		joint.Add(gameObject.AddComponent<ConfigurableJoint> ());
		addRigidBody (rb, currentIndex);
		tmp.setController (this, currentIndex);
		listMuscles.Add (tmp);
		currentIndex++;
	}
		
	public void setForce(float force, int index)
	{
		joint[index].targetVelocity += joint[index].axis * force;
		rb.angularVelocity = joint[index].targetVelocity * Time.deltaTime;
	}

	public void setAxis(ConfigurableJointMotion[] type, int index)
	{
		joint[index].angularXMotion = type [0];
		joint[index].angularYMotion = type [1];
		joint[index].angularZMotion = type [2];
	}

	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < anchors.Count; i++)
		{
			anchors[i].gameObject.transform.position = transform.position + (joint[i].connectedBody.transform.position - transform.position) / 2.0f;
		}
	}
}
