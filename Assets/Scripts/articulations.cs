using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class articulations : MonoBehaviour {
	public musclesController[] controllers;
//	public ConfigurableJointMotion[] axis;
	public HingeJoint joint;
	public Dictionary<int, muscle> muscles;
	public Vector2 axisLimits;
	public int muscleIndex = 0;
	public bool useMotor = false;
	public int index;
	public bool colliding = false;
	public float targetVelocity = 0;
	public Vector3 direction = Vector3.one;
	public bool started = false;

	// Use this for initialization
	void Awake () 
	{
		muscles = new Dictionary<int, muscle> ();
		controllers = new musclesController[2];
	}

	void Start ()
	{
		started = true;
	}

	public void setLimitsAxis(Vector2 limits)
	{
		axisLimits = limits;
		if (limits == Vector2.zero)
			joint.useLimits = false;
		else
		{
			JointLimits tmpLimits = joint.limits;
			tmpLimits.min = axisLimits.x;
			tmpLimits.max = axisLimits.y;
			tmpLimits.bounciness = 0;
			tmpLimits.contactDistance = 100;
			joint.limits = tmpLimits;
			joint.useLimits = true;
		}
	}

	public void addDirection(Vector3 dir)
	{
		direction = new Vector3(Mathf.Max(direction.x - Mathf.Abs(dir.x), 0.0f), Mathf.Max(direction.y - Mathf.Abs(dir.y), 0.0f), Mathf.Max(direction.z - Mathf.Abs(dir.z), 0.0f));
//		direction -= controllers [0].transform.right;
		RigidbodyConstraints tmpConstraints = joint.connectedBody.constraints;
		if (direction.x >= 0.75)
			tmpConstraints |= RigidbodyConstraints.FreezeRotationX;
		if (direction.y >= 0.75)
			tmpConstraints |= RigidbodyConstraints.FreezeRotationY;
		if (direction.z >= 0.75)
			tmpConstraints |= RigidbodyConstraints.FreezeRotationZ;
		joint.connectedBody.constraints = tmpConstraints;
		joint.gameObject.GetComponent<Rigidbody> ().constraints = tmpConstraints;
//		setLimitsAxis (new Vector2(0, 180));
	}

	public void addMuscle(muscle current)
	{
		muscles.Add (muscleIndex, current);
		current.setIndex (muscleIndex);
		muscleIndex++;
	}

	// configure ConfigurableJoint
	public void addRigidBody(musclesController first, musclesController other, int index)
	{
		joint = first.gameObject.AddComponent<HingeJoint> ();
		joint.connectedBody = other.GetComponent<Rigidbody>();
		joint.enableCollision = true;
		joint.enablePreprocessing = false;
		joint.autoConfigureConnectedAnchor = false;
		setIndex (index);
		setController (first, 0);
		setController (other, 1);
		joint.anchor = (Vector3.Max(first.transform.position, other.transform.position) - Vector3.Min(first.transform.position, other.transform.position)) / 3.0f;
		joint.connectedAnchor = (Vector3.Min(first.transform.position, other.transform.position) - Vector3.Max(first.transform.position, other.transform.position)) / 3.0f;
	}

	IEnumerator muscleDeactivate()
	{
		yield return new WaitForSeconds (0.5f);
		useMotor = false;
		targetVelocity = 0;
	}

	public void setForce(float force, muscle mscle)
	{
		StopCoroutine ("muscleDeactivate");
		useMotor = true;
		targetVelocity = targetVelocity + force;
		targetVelocity = Mathf.Min(targetVelocity + force, force * 100);
		JointMotor tmpMotor = joint.motor;
		tmpMotor.force = force * 100;
		tmpMotor.targetVelocity = targetVelocity;
		joint.motor = tmpMotor;
		joint.axis = mscle.angularDirection;
		Debug.Log (mscle.angularDirection);
		StartCoroutine("muscleDeactivate");
	}

	void setIndex(int i)
	{
		index = i;
	}

	void setController(musclesController current, int index)
	{
		controllers [index] = current;
	}

	// Update is called once per frame
	void Update () 
	{
		if (controllers [0])
		{
			joint.useMotor = useMotor;
			transform.position = Vector3.Lerp (controllers [0].transform.position + joint.anchor, controllers [1].transform.position + joint.connectedAnchor, 0.5f); 
		}
		//			Vector3.Lerp(controllers[0].transform.position, controllers[1].transform.position, 0.5f);
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
		{
//			Debug.Log ("Some articulations collision!!!");
			colliding = true;
		} 
		else if (other.tag == "collectibles")
			other.gameObject.SetActive (false);
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
			colliding = false;
	}
}
