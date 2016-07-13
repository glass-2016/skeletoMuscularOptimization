using UnityEngine;
using System.Collections;

public class UImanager : MonoBehaviour 
{
	//buttons
	public GameObject setparent;
	public GameObject resetparent;
	public GameObject delete;

	public GameObject bonescale;
	public GameObject boneposition;
	public GameObject bonerotation;

	public GameObject muscleparameters;

	public GameObject articulationparameters;

	public GameObject play;
	public GameObject stop;





	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
		//C# does not want public untyped vars, so... sorry. goes there. 
		var manager = GameObject.FindWithTag ("MainCamera").GetComponent<manager> ();

		if (manager.isPlaying) 
		{
			setparent.SetActive (false);
			resetparent.SetActive (false);
			delete.SetActive (false);
			bonescale.SetActive (false);
			boneposition.SetActive (false);
			bonerotation.SetActive (false);
			muscleparameters.SetActive (false);
			articulationparameters.SetActive (false);
			play.SetActive (false);
			stop.SetActive (true);
		}

		if (!manager.isPlaying)
		{
			play.SetActive (true);
			stop.SetActive (false);

			delete.SetActive (true);


			switch(manager.itemSelected)
			{
			case "bone":
				setparent.SetActive (true);
				resetparent.SetActive (false);
				bonescale.SetActive (true);
				boneposition.SetActive (true);
				bonerotation.SetActive (true);
				muscleparameters.SetActive (false);
				articulationparameters.SetActive (false);
				break;

			case "muscle":
				setparent.SetActive (true);
				resetparent.SetActive (false);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				muscleparameters.SetActive (true);
				articulationparameters.SetActive (false);
				break;

			case "boneHasParent":
				setparent.SetActive (false);
				resetparent.SetActive (true);
				bonescale.SetActive (true);
				boneposition.SetActive (true);
				bonerotation.SetActive (true);
				muscleparameters.SetActive (false);
				articulationparameters.SetActive (false);

				break;

			case "muscleHasParent":
				setparent.SetActive (false);
				resetparent.SetActive (true);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				muscleparameters.SetActive (true);
				articulationparameters.SetActive (false);
				break;

			case "articulation":
				setparent.SetActive (false);
				resetparent.SetActive (false);
				delete.SetActive (false);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				muscleparameters.SetActive (false);
				articulationparameters.SetActive (true);
				break;
				

			case "none":
				setparent.SetActive (false);
				resetparent.SetActive (false);
				delete.SetActive (false);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				muscleparameters.SetActive (false);
				articulationparameters.SetActive (false);
				break;

			default:
				setparent.SetActive (false);
				resetparent.SetActive (false);
				delete.SetActive (false);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				muscleparameters.SetActive (false);
				articulationparameters.SetActive (false);
				break;

			}
		}

	}
}
