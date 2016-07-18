using UnityEngine;
using System.Collections;

public class cameraPosition : MonoBehaviour 
{
	private float currentAngleX = 0;
	private float currentAngleY = 0;

//	private float currentPosX = 0;
//	private float currentPosY = 0;

	// speed axis
	public float xSpeed = 10f;
	public float ySpeed = 10f;
	public float zSpeed = 50f;
	public float mouseSensibility = 0.01f;

	//camera zoom cap parameters
	public float zMax = 50f;
	public float zMin = -50f;

	private Vector3 distanceVector;

	private Vector3 mouseDragBegin;
	private Vector3 mouseDrag;


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
			currentAngleY += Input.GetAxis("Mouse Y")* ySpeed;
			Rotate (currentAngleX, currentAngleY);
			//Reversed because it works better
//			transform.localEulerAngles = new Vector3(currentAngleY, currentAngleX, 0);

		}

		//translate camera
//		if (Input.GetMouseButtonDown (0) && Camera.main.GetComponent<manager>().isManipulating == false) 
//		{
//			mouseDragBegin = Input.mousePosition;
//		}
//
//		if (Input.GetMouseButton(0)  && Camera.main.GetComponent<manager>().isManipulating == false )
//		{
//			mouseDrag = Input.mousePosition - mouseDragBegin;
//			transform.Translate (mouseDrag.x*mouseSensibility,mouseDrag.y*mouseSensibility,0);
//			distanceVector += new Vector3 (mouseDrag.x * mouseSensibility, mouseDrag.y * mouseSensibility, 0);
//			//transform.Translate (currentPosY,currentPosX,0);
//			mouseDragBegin = Input.mousePosition;
//			
//		}

		// translate camera
		if (Input.GetKey (KeyCode.LeftArrow))
			transform.Translate (Vector3.left * 0.1f, Space.Self);
		if (Input.GetKey (KeyCode.RightArrow))
			transform.Translate(-Vector3.left * 0.1f, Space.Self);
		if (Input.GetKey (KeyCode.UpArrow))
			transform.Translate(Vector3.up * 0.1f, Space.Self);
		if (Input.GetKey (KeyCode.DownArrow))
			transform.Translate(-Vector3.up * 0.1f, Space.Self);
		
		// zoom
		float zoomAmount = Input.GetAxis ("Mouse ScrollWheel") * zSpeed;

		float zPos = transform.position.z;

		//zoom capping
		if (zPos >= zMin && zPos <= zMax) 
		{
			transform.Translate (Vector3.forward * zoomAmount, Space.Self);
			// adjust camera orbit point
			distanceVector += Vector3.forward * zoomAmount;
		}
		if (zPos > zMax) 
		{
			Vector3 camPos = transform.position;
			camPos.z = zMax;
			transform.position = camPos;
		}

		if (zPos < zMin) 
		{
			Vector3 camPos = transform.position;
			camPos.z = zMin;
			transform.position = camPos;
		}

	}
}
