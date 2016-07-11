using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class bones : MonoBehaviour {

	public bool isSelected;
	public float moveSpeed = 0.1f;

	public GameObject manipulator;

	public GameObject positionner;
	public GameObject rotationner;
	public GameObject scaler;
	Vector3 selfPos;
	Vector3 mousePos;
	GameObject px;
	GameObject py;
	GameObject pz;
	GameObject rx;
	GameObject ry;
	GameObject rz;
	GameObject sx;
	GameObject sy;
	GameObject sz;

	//find mouse position
	Ray ray;
	RaycastHit hit;

	// Use this for initialization
	void Start () {
		manipulator = this.transform.FindChild ("manipulators").gameObject;
		positionner = manipulator.transform.FindChild ("position").gameObject;
		rotationner = manipulator.transform.FindChild ("rotation").gameObject;
		scaler = manipulator.transform.FindChild ("scale").gameObject;

		px = positionner.transform.FindChild ("x").gameObject;
		py = positionner.transform.FindChild ("y").gameObject;
		pz = positionner.transform.FindChild ("z").gameObject;

		rx = rotationner.transform.FindChild ("x").gameObject;
		ry = rotationner.transform.FindChild ("y").gameObject;
		rz = rotationner.transform.FindChild ("z").gameObject;

		sx = scaler.transform.FindChild ("x").gameObject;
		sy = scaler.transform.FindChild ("y").gameObject;
		sz = scaler.transform.FindChild ("z").gameObject;


	
	}
	
	// Update is called once per frame
	void Update () 
	{
		selfPos = this.transform.position;
		mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Debug.Log (mousePos);

		if (isSelected) 
		{

			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			manipulator.SetActive (true);			
		} else {
			manipulator.SetActive (false);		}

		if(Input.GetMouseButton(0) )
			{

				Scaler ();
				Positionner ();
				Rotationner ();
			}
			
	}

				

	void OnMouseEnter()
	{
		Camera.main.GetComponent<manager> ().isManipulating = true;
	}

	void OnMouseUp()
	{
		Camera.main.GetComponent<manager> ().isManipulating = false;

	}

void Positionner()
	{

		if (px.GetComponent<Collider>().Raycast(ray, out hit, 100.0F))
	
		{
			float xMovement = Input.GetAxis("Mouse X") * moveSpeed;

				this.transform.position = new Vector3 (selfPos.x + xMovement, selfPos.y, selfPos.z);


	
		}

		if (py.GetComponent<Collider>().Raycast(ray, out hit, 100.0F))

		{
			float yMovement = Input.GetAxis("Mouse Y") * moveSpeed;

			this.transform.position = new Vector3 (selfPos.x, selfPos.y + yMovement, selfPos.z);

		}

		if (pz.GetComponent<Collider>().Raycast(ray, out hit, 100.0F))

		{
			float zMovement = Input.GetAxis("Mouse X") * moveSpeed;

			this.transform.position = new Vector3 (selfPos.x, selfPos.y, selfPos.z  - zMovement);
		}
	}

	void Scaler()
	{
	}



	void Rotationner()
	{
	}
}
