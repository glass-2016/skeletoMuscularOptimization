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
	public Vector3 oldDirection = Vector3.zero;
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

	}

	public void addDirection(Vector3 dir)
	{
		direction = new Vector3(Mathf.Max(direction.x - Mathf.Abs(dir.x), 0.0f), Mathf.Max(direction.y - Mathf.Abs(dir.y), 0.0f), Mathf.Max(direction.z - Mathf.Abs(dir.z), 0.0f));

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
		setIndex (index);
		setController (first, 0);
		setController (other, 1);
		initialAngle = first.transform.rotation.eulerAngles;
		first.transform.parent = other.transform;
	}

	public void setForce(float force, muscle mscle)
	{
		if ((!controllers[0].colliding && !controllers[1].colliding) || oldDirection != mscle.angularDirection)
			controllers [0].transform.RotateAround (controllers[1].transform.position - controllers[1].transform.right, mscle.angularDirection, force);
		else	
			controllers [0].transform.RotateAround (controllers[1].transform.position - controllers[1].transform.right, mscle.angularDirection, -force * 2);
		oldDirection = mscle.angularDirection;
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
			transform.position = Vector3.Lerp (controllers [0].transform.position + controllers[0].transform.right, controllers [1].transform.position - controllers[1].transform.right, 0.5f); 
		}
		//			Vector3.Lerp(controllers[0].transform.position, controllers[1].transform.position, 0.5f);
	}
}
