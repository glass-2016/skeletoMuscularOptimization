using UnityEngine;
using System.Collections;

public class UImanager : MonoBehaviour 
{
	//buttons
	public GameObject setparent;
	public GameObject resetparent;
	public GameObject bonescale;
	public GameObject boneposition;
	public GameObject bonerotation;
	public GameObject delete;
	public GameObject playreset;




	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
		//C# does not want public untyped vars, so... sorry. goes there. 
		var manager = GameObject.FindWithTag ("MainCamera").GetComponent<manager> ();

		if (!manager.isPlaying)
		{
			delete.SetActive (true);


			switch(manager.itemSelected)
			{
			case "bone":
				setparent.SetActive (true);
				resetparent.SetActive (false);
				bonescale.SetActive (true);
				boneposition.SetActive (true);
				bonerotation.SetActive (true);
				break;

			case "muscle":
				setparent.SetActive (true);
				resetparent.SetActive (false);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				
				break;

			case "boneHasParent":
				setparent.SetActive (false);
				resetparent.SetActive (true);
				bonescale.SetActive (true);
				boneposition.SetActive (true);
				bonerotation.SetActive (true);
				break;

			case "muscleHasParent":
				setparent.SetActive (false);
				resetparent.SetActive (true);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				break;

			case "none":
				setparent.SetActive (false);
				resetparent.SetActive (false);
				delete.SetActive (false);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				
				break;

			default:
				
				break;

			}
		}

	}
}
