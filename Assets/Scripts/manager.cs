using UnityEngine;
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
	private int globalIndex = 0;

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
//	public InputField key2;
	//articulations
	public InputField rangeX;
	public InputField rangeY;
	public InputField rangeZ;

	// those are here to be read by other scripts
	public string itemSelected; 	//either "none", "bone", "muscle"
	public bool isPlaying;
	public bool isManipulating = false;

	void Start ()
	{
		list = new List<GameObject> ();
	}

	// play mode
	public void play()
	{
		isPlaying = true;
		deselect ();
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
			if (list [i].tag == "bones" || list [i].tag == "articulations")
			{
				list [i].GetComponent<Rigidbody> ().isKinematic = false;
				list [i].GetComponent<Collider> ().isTrigger = false;
			}
			else if (list [i].tag == "muscles")
			{
				muscle tmpMuscle = list [i].GetComponent<muscle> ();
				tmpMuscle.currentArticulation.setForce (0.1f, tmpMuscle);
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
//					currentObject.gameObject.GetComponent<muscle> ().key2 = key2.text.ToUpper();




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
		foreach (KeyValuePair<int, articulations> tmpArticulations in tmpController.listArticulations)
		{
			for (int i = 0; i < 2; i++)
			{
				if (tmpArticulations.Value.controllers [i] != tmpController)
					tmpArticulations.Value.controllers [i].listArticulations.Remove (tmpArticulations.Key);
			}
			foreach (KeyValuePair<int, muscle> tmpMuscle in tmpArticulations.Value.muscles)
			{
				muscle tmp = tmpMuscle.Value;
				list.Remove (tmp.gameObject);
				Destroy (tmp.gameObject);
			}
			list.Remove (tmpArticulations.Value.gameObject);
			tmpArticulations.Value.muscles.Clear ();
			Destroy (tmpArticulations.Value.gameObject);
		}
		tmpController.listArticulations.Clear ();
		list.Remove (tmpController.gameObject);
		Destroy (tmpController.gameObject);
	}

	void deleteArticulation()
	{
		articulations tmpArticulation = currentObject.GetComponent<articulations> ();
		foreach (KeyValuePair<int, muscle> tmpMuscle in tmpArticulation.muscles)
		{
			muscle tmp = tmpMuscle.Value;
			list.Remove (tmp.gameObject);
			Destroy (tmp.gameObject);
		}
		tmpArticulation.muscles.Clear();
		tmpArticulation.controllers [0].listArticulations.Remove (tmpArticulation.index);
		tmpArticulation.controllers [1].listArticulations.Remove (tmpArticulation.index);
	}

	void deleteMuscle()
	{
		muscle tmpMuscle = currentObject.GetComponent<muscle> ();
		list.Remove (currentObject);
		Destroy (tmpMuscle);
		tmpMuscle.currentArticulation.muscles.Remove (tmpMuscle.index);
		if (tmpMuscle.currentArticulation.muscles.Count == 0)
		{
			Destroy (tmpMuscle.gameObject);
			currentObject = tmpMuscle.currentArticulation.gameObject;
			deleteArticulation ();
		}
	}
		
	void deselect()
	{
		currentObject = null;
		for (int i = 0; i < list.Count; i++)
		{
			if (list [i].tag == "bones")
				list [i].GetComponent<musclesController> ().colliding = false;
			else if (list [i].tag == "articulations")
				list [i].GetComponent<articulations> ().colliding = false;
			if (list [i].tag == "bones")
				list [i].GetComponent<bones> ().isSelected = false;
			list [i].GetComponent<Renderer> ().material.color = Vector4.one;
		}
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
			else if (currentObject.tag == "articulations")
				deleteArticulation ();
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
//		key2.text = "0.0";
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
//				key2.text = currentObject.gameObject.GetComponent<muscle> ().key2;


			}

		}

		//To do : ADD ARTICULATIONS WHEN NEEDED

	}

	// change colors and parameters value to newly selected object
	void changeFocus()
	{
		currentObject.GetComponent<Renderer> ().material.color = Color.yellow;
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
		if (currentObject) {
			switch (currentObject.tag) {
			case "bones":
				itemSelected = "bone";
				if (currentObject.GetComponent<bones> ()) {
					currentObject.GetComponent<bones> ().isSelected = true;
				}
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

		if ((firstAttach || secondAttach) && currentObject.tag == "bones")
		{
			currentObject.GetComponent<bones> ().isSelected = false;
//			currentObject.GetComponent<bones> ().manipulator.SetActive (false);
		}

		if (Input.GetMouseButtonDown (0) && !isPlaying) 
		{
			//removes the object manipulator if another is selected
			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			if (Physics.Raycast (ray, out hit) && (hit.collider.tag == "bones" || hit.collider.tag == "muscles" || hit.collider.tag == "articulations"))
			{
				if (currentObject)
				{
					if (currentObject.GetComponent<bones> () && currentObject != hit.collider.gameObject) {
						currentObject.GetComponent<bones> ().isSelected = false;
					}

				}

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
					muscle tmpMuscle = Instantiate (musclePrefab, attaches[0] + (attaches[1] - attaches[0]), Quaternion.identity) as muscle;
					list.Add (tmpMuscle.gameObject);
					musclesController tmpController = currentObject.GetComponent<musclesController> ();
					tmpMuscle.setAnchor (tmpController.gameObject);
					currentObject = hit.collider.gameObject;
					musclesController otherController = hit.collider.gameObject.GetComponent<musclesController>();
					articulations currentArticulations = tmpController.addAnchor (tmpMuscle, otherController, globalIndex);
					if (!list.Contains(currentArticulations.gameObject))
						list.Add (currentArticulations.gameObject);
					otherController.addArticulation (currentArticulations);
					globalIndex = currentArticulations.index + 1;
					tmpMuscle.setArticulation (currentArticulations);
					tmpMuscle.setLimits (attaches, tmpController.gameObject.transform.position, currentObject.transform.position);
					changeFocus ();
					tmpMuscle.setAnchor (currentObject);
				}
				else
				{
					// select object
					/*if (currentObject.GetComponent<bones> () != null) {
						currentObject.GetComponent<bones> ().isSelected = false;
					}*/

					currentObject = hit.collider.gameObject;

					/*if (currentObject.GetComponent<bones> ()!= null) {
						currentObject.GetComponent<bones> ().isSelected = true;
					}*/

					changeFocus ();
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.C))
		{
			// deselect current object
			deselect();
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
