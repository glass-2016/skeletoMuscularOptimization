using UnityEngine;
using System.Collections;

public class bones : MonoBehaviour {

	public bool isSelected;
	public GameObject manipulator;

	// Use this for initialization
	void Start () {
		manipulator = this.transform.FindChild ("manipulators").gameObject;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isSelected) 
		{
			manipulator.SetActive (true);			
		} else {
			manipulator.SetActive (false);		}
	
		RaycastHit hit; 
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
		if (Physics.Raycast (ray, out hit) && hit.collider.tag == "manipulator") {
			Debug.Log ("Manipulator hit");
		}
	}
}
