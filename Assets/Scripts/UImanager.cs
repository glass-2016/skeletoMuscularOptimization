using UnityEngine;
using System.Collections;

public class UImanager : MonoBehaviour 
{
	//buttons
	GameObject setparent;
	GameObject resetparent;
	GameObject newmuscle;
	GameObject newbone;
	GameObject bonescale;
	GameObject boneposition;
	GameObject bonerotation;
	GameObject delete;
	GameObject playreset;
	GameObject save;



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
			switch(manager.itemSelected)
			{
			case "bone":				
				break;

			case "muscle":
				
				break;

			case "boneHasParent":
				break;

			case "muscleHasParent":
				break;

			case "none":
				
				break;

			default:
				
				break;

			}
		}

	}
}
