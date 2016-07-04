using UnityEngine;
using System.Collections;

public class cameraPosition : MonoBehaviour 
{
	private Vector3 oldPosition;
	// Use this for initialization
	void Start () 
	{
	
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButton (0) && Input.mousePosition != oldPosition) 
		{
			Camera.main.transform.RotateAround (Vector3.zero, new Vector3 (0.0f, 1.0f, 0.0f), (oldPosition.x - Input.mousePosition.x));	
//			Camera.main.transform.RotateAround (Vector3.zero, new Vector3 (Mathf.Abs(Mathf.Cos(Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad)), 0.0f, 0.0f), (oldPosition.y - Input.mousePosition.y));
//			Camera.main.transform.RotateAround (Vector3.zero, new Vector3 (0.0f, 0.0f, Mathf.Abs(Mathf.Sin(Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad))), (oldPosition.z - Input.mousePosition.z));

		}
		oldPosition = Input.mousePosition;
	}
}
