﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class manager : MonoBehaviour 
{
	// selected object
	private GameObject currentObject = null;
	// bone prefab
	public GameObject bone;
	public muscle musclePrefab;
	// list of all objects added to scene
	private List<GameObject> list;
	// boolean to indicate parent mode
	private bool searchParent = false;
	// muscles attaches
	private bool firstAttach = false;
	private bool secondAttach = false;
	private Vector3[] attaches = null;
	// indicator to update values of fields parameters
	private Vector3 oldPosition;
	private Quaternion oldRotation;
	private Vector3 oldScale;

	// parameters fields
	//bones
	public InputField positionX;
	public InputField positionY;
	public InputField positionZ;
	public InputField rotationX;
	public InputField rotationY;
	public InputField rotationZ;
	public InputField scaleX;
	public InputField scaleY;
	public InputField scaleZ;
	//muscles
	public InputField muscleForce;
	public InputField key1;
	public InputField key2;
	//articulations
	public InputField rangeX;
	public InputField rangeY;
	public InputField rangeZ;

	// those are here to be read by other scripts
	public string itemSelected; 	//either "none", "bone", "muscle"
	public bool isPlaying;

	void Start ()
	{
		list = new List<GameObject> ();
	}

	// play mode
	public void play()
	{
		isPlaying = true;
		for (int i = 0; i < list.Count; i++)
		{
			if (list [i].tag == "bones")
			{
				if (list [i].GetComponent<musclesController> ().colliding)
					return;
			}
			else if (list [i].tag == "articulations")
			{
				if (list [i].GetComponent<articulations> ().colliding)
					return;
			}	
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list [i].tag == "bones")
			{
				list [i].GetComponent<Rigidbody> ().isKinematic = false;
				list [i].GetComponent<Collider> ().isTrigger = false;
			}
			else if (list[i].tag == "articulations")
				list [i].GetComponent<Collider> ().isTrigger = false;
			else if (list [i].tag == "muscles")
			{
				musclesController tmpController = list [i].GetComponent<muscle> ().controller;
				for (int j = 0; j < tmpController.currentIndex; j++)
					tmpController.setForce (0.1f, j);
			}
		}
	}

	public void updateScale()
	{
		if (currentObject)
		{
			float tmpX, tmpY, tmpZ = 0.0f;
			float.TryParse(scaleX.text, out tmpX);
			float.TryParse(scaleY.text, out tmpY);
			float.TryParse(scaleZ.text, out tmpZ);
			currentObject.transform.localScale = new Vector3(tmpX, tmpY, tmpZ);

			if(currentObject.tag == "muscles")
			{
				if (currentObject.gameObject.GetComponent<muscle> () == null) {
					return;
				} else {

					float force = currentObject.gameObject.GetComponent<muscle> ().force;
					float.TryParse (muscleForce.text, out force);
					currentObject.gameObject.GetComponent<muscle> ().force = force;
					currentObject.gameObject.GetComponent<muscle> ().key1 = key1.text.ToUpper();
					currentObject.gameObject.GetComponent<muscle> ().key2 = key2.text.ToUpper();




				}
			
			}
		}
	}

	// reset parameters to current parent
	public void resetLocalPosition()
	{
		if (currentObject)
		{
			currentObject.transform.localPosition = Vector3.zero;
			currentObject.transform.localScale = Vector3.one;
			currentObject.transform.localRotation = Quaternion.Euler (Vector3.zero);
		}
	}

	// add bone
	public void spawn()
	{
		list.Add(Instantiate (bone));
	}

	public void removeParent()
	{
		if (currentObject)
			currentObject.transform.parent = null;
	}

	public void addMuscle()
	{
		if (currentObject)
		{
			attaches = new Vector3[2];
			secondAttach = false;
			searchParent = false;
			firstAttach = true;
		}
	}

	public void setParent()
	{
		if (currentObject)
		{
			searchParent = true;
			firstAttach = false;
			secondAttach = false;
		}
	}

	public void updateAxis()
	{
		if (currentObject)
		{
			float tmpX, tmpY, tmpZ = 0.0f;
			float.TryParse(scaleX.text, out tmpX);
			float.TryParse(scaleY.text, out tmpY);
			float.TryParse(scaleZ.text, out tmpZ);
			currentObject.GetComponent<articulations> ().setLimitsAxis (new Vector3(tmpX, tmpY, tmpZ));
		}
	}

	public void updateRotation()
	{
		if (currentObject)
		{
			float tmpX, tmpY, tmpZ = 0.0f;
			float.TryParse(rotationX.text, out tmpX);
			float.TryParse(rotationY.text, out tmpY);
			float.TryParse(rotationZ.text, out tmpZ);
			currentObject.transform.rotation = Quaternion.Euler(new Vector3(tmpX, tmpY, tmpZ));
		}
	}

	void deleteBone()
	{
		musclesController tmpController = currentObject.GetComponent<musclesController> ();
		while (tmpController.listMuscles.Count > 0)
		{
			muscle tmp = tmpController.listMuscles[tmpController.listMuscles.Count - 1];
			list.Remove (tmp.gameObject);
			if (tmp.controller == tmpController)
			{
				list.Remove (tmpController.anchors [tmp.index].gameObject);
				if (tmp.anchors [0].GetComponent<musclesController> () == tmpController)
					tmp.anchors [1].GetComponent<musclesController> ().listMuscles.Remove (tmp);
				else
					tmp.anchors [0].GetComponent<musclesController> ().listMuscles.Remove (tmp);
				Destroy (tmpController.anchors [tmp.index].gameObject);
			} 
			else
			{
				list.Remove (tmp.controller.anchors [tmp.index].gameObject);
				Destroy (tmp.controller.anchors [tmp.index].gameObject);
				Destroy(tmp.controller.joint[tmp.index]);
				tmp.controller.joint.RemoveAt (tmp.index);
				tmp.controller.anchors.RemoveAt (tmp.index);
			}
			tmpController.listMuscles.Remove (tmp);
			Destroy (tmp.gameObject);
		}
		Destroy (tmpController.gameObject);
	}

	void deleteMuscle()
	{
		muscle tmpMuscle = currentObject.GetComponent<muscle> ();
		list.Remove (currentObject);
		list.Remove (tmpMuscle.controller.anchors[tmpMuscle.index].gameObject);
		Destroy (tmpMuscle.controller.joint [tmpMuscle.index]);
		Destroy (tmpMuscle.controller.anchors [tmpMuscle.index].gameObject);
		Destroy (tmpMuscle);
		tmpMuscle.controller.anchors.RemoveAt (tmpMuscle.index);
		tmpMuscle.controller.listMuscles.RemoveAt (tmpMuscle.index);
	}
		
	// delete current selected object
	public void delete()
	{
		if (currentObject)
		{
			if (currentObject.tag == "bones")
				deleteBone ();
			else if (currentObject.tag == "muscles")
				deleteMuscle ();
			list.Remove (currentObject);
			Destroy (currentObject.gameObject);
			currentObject = null;
		}	
	}

	public void updatePosition()
	{
		if (currentObject)
		{
			float tmpX, tmpY, tmpZ = 0.0f;
			float.TryParse(positionX.text, out tmpX);
			float.TryParse(positionY.text, out tmpY);
			float.TryParse(positionZ.text, out tmpZ);
			currentObject.transform.position = new Vector3 (tmpX, tmpY, tmpZ);
		}
	}

	void resetValues()
	{
		positionX.text = "0.0";
		positionY.text = "0.0";
		positionZ.text = "0.0";
		rotationX.text = "0.0";
		rotationY.text = "0.0";
		rotationZ.text = "0.0";
		scaleX.text = "0.0";
		scaleY.text = "0.0";
		scaleZ.text = "0.0";
		muscleForce.text = "0.0";
		key1.text = "0.0";
		key2.text = "0.0";
		rangeX.text = "0.0";
		rangeY.text = "0.0";
		rangeZ.text = "0.0";
	}

	void updateValues()
	{
		positionX.text = currentObject.transform.position.x.ToString ();
		positionY.text = currentObject.transform.position.y.ToString ();
		positionZ.text = currentObject.transform.position.z.ToString ();
		rotationX.text = currentObject.transform.rotation.x.ToString ();
		rotationY.text = currentObject.transform.rotation.y.ToString ();
		rotationZ.text = currentObject.transform.rotation.z.ToString ();
		scaleX.text = currentObject.transform.localScale.x.ToString ();
		scaleY.text = currentObject.transform.localScale.y.ToString ();
		scaleZ.text = currentObject.transform.localScale.z.ToString ();

		if (currentObject.tag == "muscles") 
		{
			if (currentObject.gameObject.GetComponent<muscle> () == null) {
				return;
			} else {
				muscleForce.text = currentObject.gameObject.GetComponent<muscle> ().force.ToString();
				key1.text = currentObject.gameObject.GetComponent<muscle> ().key1;
				key2.text = currentObject.gameObject.GetComponent<muscle> ().key2;


			}

		}

		//To do : ADD ARTICULATIONS WHEN NEEDED

	}

	// change colors and parameters value to newly selected object
	void changeFocus()
	{
		currentObject.GetComponent<Renderer> ().material.color = Color.green;
		oldPosition = currentObject.transform.position;
		oldRotation = currentObject.transform.rotation;
		oldScale = currentObject.transform.localScale;
		updateValues ();
		List<Renderer> tmpList = currentObject.GetComponentsInChildren<Renderer> ().ToList();
		for (int i = 0; i < list.Count; i++)
		{
			if (list [i] != currentObject)
			{
				Renderer tmp = list [i].GetComponent<Renderer> ();
				if (tmpList.Contains (tmp))
					tmp.material.color = Color.red;
				else
					tmp.material.color = Vector4.one;	
			}
		}
	}

	void updatePublicItem()
	{
		if (currentObject != null) {
			switch (currentObject.tag) {
			case "bones":
				itemSelected = "bone";
				if (currentObject.gameObject.transform.parent != null) {
					itemSelected = "boneHasParent";
				}
				break;

			case "muscles":
				itemSelected = "muscle";
				if (currentObject.gameObject.transform.parent != null) {
					itemSelected = "muscleHasParent";
				}
				break;
			case "articulations":
				itemSelected = "articulation";
				break;
			default :
				itemSelected = "none";
				break;
			}
		} 
		else 
		{
			itemSelected = "none";
		}
	}
		

	void Update () 
	{
		updatePublicItem ();

		if (Input.GetMouseButtonDown (0)) 
		{
			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			if (Physics.Raycast (ray, out hit) && (hit.collider.tag == "bones" || hit.collider.tag == "muscles" || hit.collider.tag == "articulations"))
			{
				if (searchParent && currentObject)
				{
					// add parent to current object
					currentObject.transform.parent = hit.collider.gameObject.transform;
					searchParent = false;
				} 
				else if (firstAttach)
				{
					// add first attach to object clicked
					attaches [0] = hit.point;
					firstAttach = false;
					secondAttach = true;
					currentObject = hit.collider.gameObject;
					changeFocus ();
				} 
				else if (secondAttach && currentObject != hit.collider.gameObject)
				{
					// add second attach to object clicked and add muscle
					attaches [1] = hit.point;
					secondAttach = false;
					muscle tmp = Instantiate (musclePrefab, attaches[0] + (attaches[1] - attaches[0]), Quaternion.identity) as muscle;
					list.Add (tmp.gameObject);
					musclesController tmpController = currentObject.GetComponent<musclesController> ();
					tmp.setAnchor (tmpController.gameObject);
					currentObject = hit.collider.gameObject;
					tmpController.addMuscle (tmp, currentObject.GetComponent<Rigidbody> ());
					list.Add (tmpController.anchors[tmpController.currentIndex - 1].gameObject);
					currentObject.GetComponent<musclesController> ().setMuscle (tmp);
					tmp.setLimits (attaches, tmpController.gameObject.transform.position, currentObject.transform.position);
					changeFocus ();
					tmp.setAnchor (currentObject);
				}
				else
				{
					// select object
					currentObject = hit.collider.gameObject;
					changeFocus ();
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.C))
		{
			// deselect current object
			currentObject = null;
			for (int i = 0; i < list.Count; i++)
				list [i].GetComponent<Renderer> ().material.color = Vector4.one;
		}
		if (currentObject && 
			(oldPosition != currentObject.transform.position ||
				oldRotation != currentObject.transform.rotation ||
				oldScale != currentObject.transform.localScale))
		{
			// update parameters when they change
			updateValues ();
			oldPosition = currentObject.transform.position;
			oldRotation = currentObject.transform.rotation;
			oldScale = currentObject.transform.localScale;
		}
		else if (!currentObject)
		{
			resetValues ();
		}
	}
}
