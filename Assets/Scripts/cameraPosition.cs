using UnityEngine;
using System.Collections;

public class cameraPosition : MonoBehaviour 
{
	private Vector3 oldPosition;
	public GameObject debugPrefab;
	public bool debug = false;
	private GameObject instance;
	// Use this for initialization
	void Start () 
	{
//		if (debug)
//			instance = Instantiate (debugPrefab, Vector3.zero, Quaternion.identity) as GameObject;
	}

	// Update is called once per frame
	void Update () 
	{
//		if (debug)
//			instance.transform.position = transform.position + Vector3.forward * 10.0f;
		if (Input.GetMouseButton (0) && Input.mousePosition != oldPosition) 
		{
			Camera.main.transform.RotateAround (transform.position + Vector3.forward, new Vector3 (0.0f, 1.0f, 0.0f), (oldPosition.x - Input.mousePosition.x));	
//			Camera.main.transform.RotateAround (Vector3.zero, new Vector3 (Mathf.Abs(Mathf.Cos(Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad)), 0.0f, 0.0f), (oldPosition.y - Input.mousePosition.y));
//			Camera.main.transform.RotateAround (Vector3.zero, new Vector3 (0.0f, 0.0f, Mathf.Abs(Mathf.Sin(Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad))), (oldPosition.z - Input.mousePosition.z));

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
