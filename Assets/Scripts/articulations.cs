using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class articulations : MonoBehaviour {
	public musclesController[] controllers;
//	public ConfigurableJointMotion[] axis;
	public HingeJoint joint;
	public Dictionary<int, muscle> muscles;
	public Vector3 axisLimits;
	private int muscleIndex = 0;
	public bool useMotor = false;
	public int index;
	public bool colliding = false;
	public float targetVelocity = 0;
	// Use this for initialization
	void Awake () 
	{
//		axis = new ConfigurableJointMotion[3];
//		axis [0] = ConfigurableJointMotion.Limited;
//		axis [1] = ConfigurableJointMotion.Limited;
//		axis [2] = ConfigurableJointMotion.Limited;
		muscles = new Dictionary<int, muscle> ();
		controllers = new musclesController[2];
	}

	public void setLinearLimit(GameObject first, GameObject other)
	{
//		SoftJointLimit tmpLimit = joint.linearLimit;
//		Vector3 firstSize = first.GetComponent<Renderer> ().bounds.size;
//		Vector3 otherSize = other.GetComponent<Renderer> ().bounds.size;
//		tmpLimit.limit = Mathf.Max (new Vector3(firstSize.x * transform.up.x, firstSize.y * transform.up.y, firstSize.z * transform.up.z).magnitude,
//			new Vector3(otherSize.x * transform.up.x, otherSize.y * transform.up.y, otherSize.z * transform.up.z).magnitude);
////		tmpLimit.limit = 1f;
//		tmpLimit.bounciness = 10f;
//		if (tmpLimit.limit > joint.linearLimit.limit)
//			joint.linearLimit = tmpLimit;
//		SoftJointLimitSpring tmpSpring = joint.linearLimitSpring;
//		tmpSpring.damper = 0.1f;
//		tmpSpring.spring = 20.0f;
//		joint.linearLimitSpring = tmpSpring;
	}

	public void setLimitsAxis(Vector2 axisLimits)
	{
		JointLimits tmpLimits = joint.limits;
		tmpLimits.min = axisLimits.x;
		tmpLimits.max = axisLimits.y;
//		SoftJointLimit tmp = joint.lowAngularXLimit;
//		tmp.limit = -axisLimits.x / 2.0f;
//		joint.lowAngularXLimit = tmp;
//		tmp = joint.highAngularXLimit;
//		tmp.limit = axisLimits.x / 2.0f;
//		joint.highAngularXLimit = tmp;
//		tmp = joint.angularYLimit;
//		tmp.limit = axisLimits.y;
//		tmp = joint.angularZLimit;
//		joint.angularYLimit = tmp;
//		tmp.limit = axisLimits.z;
//		joint.angularZLimit = tmp;
	}

	public void addDirection(Vector3 dir)
	{
		joint.axis = (joint.axis + new Vector3 (dir.x, dir.y, dir.z)).normalized;
//		joint.targetRotation = Quaternion.Euler(joint.axis);
		setLimitsAxis (new Vector2(0, 180));
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
		joint.anchor = (Vector3.Max(first.transform.position, other.transform.position) - Vector3.Min(first.transform.position, other.transform.position)) / 2.5f;
		joint.connectedAnchor = (Vector3.Min(first.transform.position, other.transform.position) - Vector3.Max(first.transform.position, other.transform.position)) / 2.5f;
//		joint.xMotion = ConfigurableJointMotion.Limited;
//		joint.yMotion = ConfigurableJointMotion.Limited;
//		joint.zMotion = ConfigurableJointMotion.Limited;
//		joint.angularXMotion = ConfigurableJointMotion.Limited;
//		joint.angularYMotion = ConfigurableJointMotion.Limited;
//		joint.angularZMotion = ConfigurableJointMotion.Limited;
//		joint.secondaryAxis = Vector3.zero;
		initLimitAxis ();
	}

	void Rotate(float force, muscle mscle)
	{
//		if (!joint.connectedBody.GetComponent<musclesController>().colliding)
			joint.connectedBody.transform.RotateAround (transform.position, mscle.angularDirection, force);
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
		targetVelocity += force;
		JointMotor tmpMotor = joint.motor;
		tmpMotor.force = force * 10;
		tmpMotor.targetVelocity = targetVelocity;
		joint.motor = tmpMotor;
		joint.axis = mscle.angularDirection;
//		joint.targetPosition = transform.position + mscle.angularDirection * force;
//		joint.targetAngularVelocity = mscle.angularDirection * force;
//		joint.targetVelocity = mscle.angularDirection * force;
//		if (joint.targetVelocity.magnitude > 150f)
//			joint.targetVelocity = joint.targetVelocity.normalized * 150f;
//		if (joint.targetAngularVelocity.magnitude > 150f)
//			joint.targetAngularVelocity = joint.targetAngularVelocity.normalized * 150f;
////		joint.rotationDriveMode = RotationDriveMode.Slerp;
//		joint.configuredInWorldSpace = true;
////		JointDrive tmp = joint.slerpDrive;
////		tmp.positionSpring = 20;
////		joint.slerpDrive = tmp;
//		joint.connectedBody.velocity = joint.targetVelocity * Time.deltaTime;
//		joint.connectedBody.angularVelocity = joint.targetAngularVelocity * Time.deltaTime;
//		Rotate (force, mscle);
		StartCoroutine("muscleDeactivate");
	}

	void setIndex(int i)
	{
		index = i;
	}

	void initLimitAxis()
	{
		setLimitsAxis (new Vector3(180, 180, 180));
	}

	void setController(musclesController current, int index)
	{
		controllers [index] = current;
	}

	// Update is called once per frame
	void Update () 
	{
		joint.useMotor = useMotor;
		transform.position = Vector3.Lerp(controllers [0].transform.position + joint.anchor, controllers[1].transform.position + joint.connectedAnchor, 0.5f); 
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

	void OnColliderStay(Collision other)
	{
		colliding = true;
	}

	void OnColliderExit(Collision other)
	{
		colliding = false;
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "bones" || other.tag == "articulations")
			colliding = false;
	}
}
