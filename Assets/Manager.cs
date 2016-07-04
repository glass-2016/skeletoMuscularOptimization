using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Manager : MonoBehaviour 
{
	private GameObject currentObject = null;
	public GameObject target;
	private List<GameObject> list;
	private bool searchParent = false;
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

	public void setParent()
	{
		if (currentObject)
		{
//			currentObject.GetComponent<Renderer> ().material.color = Color.red;
			searchParent = true;
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
		
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			if (Physics.Raycast (ray, out hit) && hit.collider.tag == "objects")
			{
				if (searchParent && currentObject)
				{
					currentObject.transform.parent = hit.collider.gameObject.transform;
//					currentObject.GetComponent<Renderer> ().material.color = Color.red;
					searchParent = false;
				}
				else
				{
//					if (currentObject)
//						currentObject.GetComponent<Renderer> ().material.color = Vector4.one;
					Debug.Log ("You selected the " + hit.transform.name);
					currentObject = hit.collider.gameObject;
					currentObject.GetComponent<Renderer> ().material.color = Color.green;
//					if (currentObject.GetComponentsInChildren<GameObject> ().Length > 0)
//					{
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
//					}
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
			}
			if (Input.GetKeyDown (KeyCode.C))
			{
//				currentObject.GetComponent<Renderer> ().material.color = Vector4.one;
				currentObject = null;
				for (int i = 0; i < list.Count; i++)
					list [i].GetComponent<Renderer> ().material.color = Vector4.one;
			}
			if (currentObject == null)
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
		}
	}
}
