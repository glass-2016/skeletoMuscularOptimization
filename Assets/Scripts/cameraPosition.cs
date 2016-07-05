using UnityEngine;
using System.Collections;

public class cameraPosition : MonoBehaviour 
{
	private Vector3 oldPosition;
	public GameObject debugPrefab;
	public bool debug = false;
	private GameObject instance;
	private float currentAngleX = 0;
	private float currentAngleY = 0;
	public float xSpeed = 1f;
	public float ySpeed = 1f;
	private Vector3 distanceVector;
	// Use this for initialization
	void Start () 
	{
		distanceVector = new Vector3(0.0f,0.0f,-10.0f);
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
		if (Input.GetMouseButton (0) && Input.GetKey(KeyCode.LeftAlt) && Input.mousePosition != oldPosition) 
		{
			currentAngleX += Input.GetAxis("Mouse X") * xSpeed;
			currentAngleY += -Input.GetAxis("Mouse Y")* ySpeed;
			Rotate (currentAngleX, currentAngleY);
		}
		if (Input.GetKey (KeyCode.LeftArrow))
			transform.Translate(Vector3.left * 0.1f, Space.Self);
		if (Input.GetKey (KeyCode.RightArrow))
			transform.Translate(-Vector3.left * 0.1f, Space.Self);
		if (Input.GetKey (KeyCode.UpArrow))
			transform.Translate(Vector3.forward * 0.1f, Space.Self);
		if (Input.GetKey (KeyCode.DownArrow))
			transform.Translate(-Vector3.forward * 0.1f, Space.Self);
		transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel"), Space.Self);
		oldPosition = Input.mousePosition;
	}
}
