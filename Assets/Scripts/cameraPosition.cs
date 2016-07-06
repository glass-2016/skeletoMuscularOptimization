using UnityEngine;
using System.Collections;

public class cameraPosition : MonoBehaviour 
{
	private float currentAngleX = 0;
	private float currentAngleY = 0;
	// speed axis
	public float xSpeed = 1f;
	public float ySpeed = 1f;
	public float zSpeed = 50f;

	//camera zoom cap parameters
	public float zMax = 50f;
	public float zMin = -50f;

	private Vector3 distanceVector;

	// Use this for initialization
	void Start () 
	{

		distanceVector = new Vector3(0.0f, 0.0f, -10.0f);
		Vector2 angles = this.transform.localEulerAngles;
		currentAngleX = angles.x;
		currentAngleY = angles.y;
		Rotate (currentAngleX, currentAngleY);
	}

	void Rotate(float x, float y)
	{
		Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
		Vector3 position = rotation * distanceVector + Vector3.forward;
		transform.rotation = rotation;
		transform.position = position;
	}

	// Update is called once per frame
	void Update () 
	{

		if (Input.GetMouseButton (1)) 
		{
			// rotate camera around point
			currentAngleX += Input.GetAxis("Mouse X") * xSpeed;
			currentAngleY += -Input.GetAxis("Mouse Y")* ySpeed;
			Rotate (currentAngleX, currentAngleY);
		}
		// translate camera
		if (Input.GetKey (KeyCode.LeftArrow))
			transform.Translate(Vector3.left * 0.1f, Space.Self);
		if (Input.GetKey (KeyCode.RightArrow))
			transform.Translate(-Vector3.left * 0.1f, Space.Self);
		if (Input.GetKey (KeyCode.UpArrow))
			transform.Translate(Vector3.forward * 0.1f, Space.Self);
		if (Input.GetKey (KeyCode.DownArrow))
			transform.Translate(-Vector3.forward * 0.1f, Space.Self);
		
		// zoom
		float zoomAmount = Input.GetAxis ("Mouse ScrollWheel") * zSpeed;

		float zPos = transform.position.z;

		//zoom capping
		if (zPos >= zMin && zPos <= zMax) 
		{
			transform.Translate (Vector3.forward * zoomAmount, Space.Self);

		}
		if (zPos > zMax) 
		{
			//transform.Translate (Vector3.forward, zMax);
		}

		if (zPos < zMin) 
		{
			//transform.Translate (Vector3.forward, zMin);
		}

	}
}
