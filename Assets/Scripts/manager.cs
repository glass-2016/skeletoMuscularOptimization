using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class manager : MonoBehaviour 
{
	private GameObject currentObject = null;
	public GameObject target;
	public muscle musclePrefab;
	private List<GameObject> list;
	private bool searchParent = false;
	private bool firstAttach = false;
	private bool secondAttach = false;
	private Vector3[] attaches = null;
	private Vector3 oldPosition;
	private Quaternion oldRotation;
	private Vector3 oldScale;
	public InputField positionX;
	public InputField positionY;
	public InputField positionZ;
	public InputField rotationX;
	public InputField rotationY;
	public InputField rotationZ;
	public InputField scaleX;
	public InputField scaleY;
	public InputField scaleZ;

	void Start ()
	{
		list = new List<GameObject> ();
	}

	public void play()
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].tag == "bones")
				list [i].GetComponent<Rigidbody> ().isKinematic = false;
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
		}
	}

	public void resetLocalPosition()
	{
		if (currentObject)
		{
			currentObject.transform.localPosition = Vector3.zero;
			currentObject.transform.localScale = Vector3.one;
			currentObject.transform.localRotation = Quaternion.Euler (Vector3.zero);
		}
	}

	public void spawn()
	{
		list.Add(Instantiate (target));
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
//			currentObject.GetComponent<Renderer> ().material.color = Color.red;
			searchParent = true;
			firstAttach = false;
			secondAttach = false;
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
	}

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

	void LateUpdate()
	{
		if (currentObject && 
			(oldPosition != currentObject.transform.position ||
			oldRotation != currentObject.transform.rotation ||
			oldScale != currentObject.transform.localScale))
		{
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

	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			if (Physics.Raycast (ray, out hit) && (hit.collider.tag == "bones" || hit.collider.tag == "muscles"))
			{
				if (searchParent && currentObject)
				{
					currentObject.transform.parent = hit.collider.gameObject.transform;
					searchParent = false;
				} 
				else if (firstAttach)
				{
					attaches [0] = hit.point;
					firstAttach = false;
					secondAttach = true;
					currentObject = hit.collider.gameObject;
					changeFocus ();
				} 
				else if (secondAttach)
				{
					attaches [1] = hit.point;
					secondAttach = false;
					muscle tmp = Instantiate (musclePrefab, attaches[0] + (attaches[1] - attaches[0]), Quaternion.identity) as muscle;
					list.Add (tmp.gameObject);
					musclesController tmpController = currentObject.GetComponent<musclesController> ();
					tmp.setAnchor (tmpController.gameObject);
					currentObject = hit.collider.gameObject;
					tmpController.addMuscle (tmp, currentObject.GetComponent<Rigidbody> ());
					tmp.setLimits (attaches, tmpController.gameObject.transform.position, currentObject.transform.position);
					changeFocus ();
					tmp.setAnchor (currentObject);
				}
				else
				{
					Debug.Log ("You selected the " + hit.transform.name);
					currentObject = hit.collider.gameObject;
					changeFocus ();
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.C))
		{
			//				currentObject.GetComponent<Renderer> ().material.color = Vector4.one;
			currentObject = null;
			for (int i = 0; i < list.Count; i++)
				list [i].GetComponent<Renderer> ().material.color = Vector4.one;
		}
	}
}
