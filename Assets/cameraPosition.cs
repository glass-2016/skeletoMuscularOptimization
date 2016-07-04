using UnityEngine;
using System.Collections;

public class cameraPosition : MonoBehaviour 
{
	private Vector3 oldPosition;
	private Vector3 target = Vector3.zero;
	public GameObject debugPrefab;
	public bool debug = false;
	private GameObject instance;
	// Use this for initialization
	void Start () 
	{
		if (debug)
			instance = Instantiate (debugPrefab, target, Quaternion.identity) as GameObject;
	}

	// Update is called once per frame
	void Update () 
	{
		if (debug)
			instance.transform.position = target;
		if (Input.GetMouseButton (0) && Input.mousePosition != oldPosition) 
		{
			Camera.main.transform.RotateAround (target, new Vector3 (0.0f, 1.0f, 0.0f), (oldPosition.x - Input.mousePosition.x));	
//			Camera.main.transform.RotateAround (Vector3.zero, new Vector3 (Mathf.Abs(Mathf.Cos(Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad)), 0.0f, 0.0f), (oldPosition.y - Input.mousePosition.y));
//			Camera.main.transform.RotateAround (Vector3.zero, new Vector3 (0.0f, 0.0f, Mathf.Abs(Mathf.Sin(Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad))), (oldPosition.z - Input.mousePosition.z));

		}
		if (Input.GetKey (KeyCode.LeftArrow))
		{
			target += Vector3.left * 0.1f;
			transform.position += Vector3.left * 0.1f;
		}
		if (Input.GetKey (KeyCode.RightArrow))
		{
			target -= Vector3.left * 0.1f;
			transform.position -= Vector3.left * 0.1f;
		}
		if (Input.GetKey (KeyCode.UpArrow))
		{
			target += Vector3.forward * 0.1f;
			transform.position += Vector3.forward * 0.1f;
		}
		if (Input.GetKey (KeyCode.DownArrow))
		{
			target -= Vector3.forward * 0.1f;
			transform.position -= Vector3.forward * 0.1f;
		}
		transform.Translate((Vector3.forward - target.normalized) * Input.GetAxis("Mouse ScrollWheel"));
		oldPosition = Input.mousePosition;
	}
}
