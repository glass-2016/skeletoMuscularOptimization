using UnityEngine;
using System.Collections;

public class manipulatorScale : MonoBehaviour {
	public bool isSelectable;
	public bool isDrag;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) {
			isDrag = true;
		} else {
			isDrag = false;
		}

		if (isDrag) {
			dragging ();
		}
	
	}

	void OnMouseEnter(){
		isSelectable = true;
		
	}

	void OnMouseExit()
	{
		isSelectable = false;
	}

	void dragging()
	{
		Vector3 newPos;
		newPos = Input.mousePosition;
	}
}
