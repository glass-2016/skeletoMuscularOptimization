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
	private Vector3 initialAngle = Vector3.zero;
	public bool started = false;
//	public Vector3 anchor = Vector3.zero;

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
//		if (limits == Vector2.zero)
//			joint.useLimits = false;
//		else
//		{
//			JointLimits tmpLimits = joint.limits;
//			tmpLimits.min = axisLimits.x;
//			tmpLimits.max = axisLimits.y;
//			tmpLimits.bounciness = 0;
//			tmpLimits.contactDistance = 100;
//			joint.limits = tmpLimits;
////			joint.useLimits = true;
//		}
	}

	public void addDirection(Vector3 dir)
	{
		direction = new Vector3(Mathf.Max(direction.x - Mathf.Abs(dir.x), 0.0f), Mathf.Max(direction.y - Mathf.Abs(dir.y), 0.0f), Mathf.Max(direction.z - Mathf.Abs(dir.z), 0.0f));
//		direction -= controllers [0].transform.right;
//		RigidbodyConstraints tmpConstraints = joint.connectedBody.constraints;
//		if (direction.x >= 0.75)
//			tmpConstraints |= RigidbodyConstraints.FreezeRotationX;
//		if (direction.y >= 0.75)
//			tmpConstraints |= RigidbodyConstraints.FreezeRotationY;
//		if (direction.z >= 0.75)
//			tmpConstraints |= RigidbodyConstraints.FreezeRotationZ;
//		joint.connectedBody.constraints = tmpConstraints;
//		joint.gameObject.GetComponent<Rigidbody> ().constraints = tmpConstraints;
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
//		joint = first.gameObject.AddComponent<HingeJoint> ();
//		joint.connectedBody = other.GetComponent<Rigidbody>();
//		joint.enableCollision = true;
//		joint.enablePreprocessing = false;
//		joint.autoConfigureConnectedAnchor = false;
		setIndex (index);
		setController (first, 0);
		setController (other, 1);
		initialAngle = first.transform.rotation.eulerAngles;
		first.transform.parent = other.transform;
//		anchor = (Vector3.Max(first.transform.position, other.transform.position) - Vector3.Min(first.transform.position, other.transform.position)) / 1.75f;
//		joint.connectedAnchor = (Vector3.Min(first.transform.position, other.transform.position) - Vector3.Max(first.transform.position, other.transform.position)) / 1.75f;
	}

	IEnumerator muscleDeactivate()
	{
		yield return new WaitForSeconds (0.5f);
		useMotor = false;
		targetVelocity = 0;
	}

	public void setForce(float force, muscle mscle)
	{
//		StopCoroutine ("muscleDeactivate");
//		useMotor = true;
//		targetVelocity = targetVelocity + force;
//		targetVelocity = Mathf.Min(targetVelocity + force, force * 100);
//		JointMotor tmpMotor = joint.motor;
//		tmpMotor.force = force * 100;
//		tmpMotor.targetVelocity = targetVelocity;
//		joint.motor = tmpMotor;
//		joint.axis = mscle.angularDirection;
		Debug.Log ("AngularDirection = " + mscle.angularDirection);
		Debug.Log ("initialAngle = " + initialAngle);
		Debug.Log ("Distance = " + Vector3.Distance(controllers[0].transform.position, controllers[1].transform.position));
//		Debug.Log ("controllerAngleX = " + ((controllers[0].transform.rotation.eulerAngles.x * Mathf.Abs(mscle.angularDirection.x)) % 180 + 180 + force * mscle.angularDirection.x * 2 - (controllers[1].transform.rotation.eulerAngles.x * Mathf.Abs(mscle.angularDirection.x)) % 180) % 180);
//		Debug.Log ("controllerAngleY = " + ((controllers[0].transform.rotation.eulerAngles.y * Mathf.Abs(mscle.angularDirection.y)) % 180 + 180 + force * mscle.angularDirection.y * 2 - (controllers[1].transform.rotation.eulerAngles.y * Mathf.Abs(mscle.angularDirection.y)) % 180) % 180);
//		Debug.Log ("controllerAngleZ = " + ((controllers[0].transform.rotation.eulerAngles.z * Mathf.Abs(mscle.angularDirection.z)) % 180 + 180 + force * mscle.angularDirection.z * 2 - (controllers[1].transform.rotation.eulerAngles.z * Mathf.Abs(mscle.angularDirection.z)) % 180) % 180);
//
//		if (((controllers[0].transform.rotation.eulerAngles.x * Mathf.Abs(mscle.angularDirection.x)) % 180 + 180 + force * mscle.angularDirection.x * 2 - (controllers[1].transform.rotation.eulerAngles.x * Mathf.Abs(mscle.angularDirection.x)) % 180) % 180 > force * 2f
//			|| ((controllers[0].transform.rotation.eulerAngles.y * Mathf.Abs(mscle.angularDirection.y)) % 180 + 180 + force * mscle.angularDirection.y * 2 - (controllers[1].transform.rotation.eulerAngles.y * Mathf.Abs(mscle.angularDirection.y)) % 180) % 180 > force * 2f
//			|| ((controllers[0].transform.rotation.eulerAngles.z * Mathf.Abs(mscle.angularDirection.z)) % 180 + 180 + force * mscle.angularDirection.z * 2 - (controllers[1].transform.rotation.eulerAngles.z * Mathf.Abs(mscle.angularDirection.z)) % 180) % 180 > force * 2f)

		if (!controllers[0].colliding && !controllers[1].colliding)
			controllers [0].transform.RotateAround (controllers[1].transform.position - controllers[1].transform.right, mscle.angularDirection, force);
		else	
			controllers [0].transform.RotateAround (controllers[1].transform.position - controllers[1].transform.right, mscle.angularDirection, -force * 2);
//		StartCoroutine("muscleDeactivate");
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
//			joint.useMotor = useMotor;
			transform.position = Vector3.Lerp (controllers [0].transform.position + controllers[0].transform.right, controllers [1].transform.position - controllers[1].transform.right, 0.5f); 
		}
		//			Vector3.Lerp(controllers[0].transform.position, controllers[1].transform.position, 0.5f);
	}
}
