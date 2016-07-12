﻿using UnityEngine;
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
	Vector3 selfScale;
	Quaternion selfRot;
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

	//current tool
	public string tool = "none"; //either "none", "positionner+X/Y/Z", "rotationner+X/Y/Z" or "scaler+X/Y/Z"


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
		selfScale = this.transform.localScale;
		selfRot = this.transform.rotation;

		mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

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
		tool = "none";

	}

	//moving the object
void Positionner()
	{


		if ((px.GetComponent<Collider>().Raycast(ray, out hit, 100.0F)) || tool == "positionnerX")
	
		{
			Camera.main.GetComponent<manager> ().isManipulating = true;

			tool = "positionnerX";

			float xMovement = Input.GetAxis("Mouse X") * moveSpeed;

				this.transform.position = new Vector3 (selfPos.x + xMovement, selfPos.y, selfPos.z);


	
		}

		if ((py.GetComponent<Collider>().Raycast(ray, out hit, 100.0F)) || tool == "positionnerY")

		{
			Camera.main.GetComponent<manager> ().isManipulating = true;

			tool = "positionnerY";

			float yMovement = Input.GetAxis("Mouse Y") * moveSpeed;

			this.transform.position = new Vector3 (selfPos.x, selfPos.y + yMovement, selfPos.z);

		}

		if ((pz.GetComponent<Collider>().Raycast(ray, out hit, 100.0F)) || tool == "positionnerZ")

		{
			Camera.main.GetComponent<manager> ().isManipulating = true;

			tool = "positionnerZ";

			float zMovement = Input.GetAxis("Mouse X") * moveSpeed;

			this.transform.position = new Vector3 (selfPos.x, selfPos.y, selfPos.z  - zMovement);
		}
	}

	//scaling/shaping the object
	void Scaler()
	{
		//X
		Collider[] collsX = sx.GetComponentsInChildren<Collider>();
		if (collsX[0].Raycast(ray, out hit, 100.0F) || collsX[1].Raycast(ray, out hit, 100.0F) || tool == "scalerX")
		{
			Camera.main.GetComponent<manager> ().isManipulating = true;

			tool = "scalerX";

			float xMovement = Input.GetAxis("Mouse X") * moveSpeed;

			this.transform.localScale = new Vector3 (selfScale.x + xMovement, selfScale.y, selfScale.z);



		}
		//Y
		Collider[] collsY = sy.GetComponentsInChildren<Collider>();

		if (collsY[0].Raycast(ray, out hit, 100.0F) ||collsY[1].Raycast(ray, out hit, 100.0F)  || tool == "scalerY")

		{
			Camera.main.GetComponent<manager> ().isManipulating = true;

			tool = "scalerY";

			float yMovement = Input.GetAxis("Mouse Y") * moveSpeed;

			this.transform.localScale = new Vector3 (selfScale.x, selfScale.y + yMovement, selfScale.z);

		}
		//Z
		Collider[] collsZ = sz.GetComponentsInChildren<Collider>();

		if (collsZ[0].Raycast(ray, out hit, 100.0F) ||collsZ[1].Raycast(ray, out hit, 100.0F) || tool == "scalerZ")

		{
			Camera.main.GetComponent<manager> ().isManipulating = true;

			tool = "scalerZ";

			float zMovement = Input.GetAxis("Mouse X") * moveSpeed;

			this.transform.localScale = new Vector3 (selfScale.x, selfScale.y, selfScale.z + zMovement);
		}
	}


	//rotating the object
	void Rotationner()
	{

		if ((rx.GetComponentInChildren<Collider>().Raycast(ray, out hit, 100.0F)) || tool == "rotationnerX")

		{
			tool = "rotationnerX";

			float xMovement = Input.GetAxis("Mouse X") * moveSpeed*200;

			this.transform.Rotate (this.transform.right, xMovement, Space.World);



		}

		if ((ry.GetComponentInChildren<Collider>().Raycast(ray, out hit, 100.0F)) || tool == "rotationnerY")

		{
			tool = "rotationnerY";

			float yMovement = Input.GetAxis("Mouse Y") * moveSpeed*200;

			this.transform.Rotate (this.transform.up, yMovement, Space.World);

		}

		if ((rz.GetComponentInChildren<Collider>().Raycast(ray, out hit, 100.0F)) || tool == "rotationnerZ")

		{
			tool = "rotationnerZ";

			float zMovement = Input.GetAxis("Mouse X") * moveSpeed*200;

			this.transform.Rotate (this.transform.forward, zMovement, Space.World);
		}
	}
}
