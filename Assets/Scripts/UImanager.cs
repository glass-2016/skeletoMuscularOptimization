using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour 
{
	//buttons
	public GameObject setparent;
	public GameObject resetparent;
	public GameObject delete;
	public GameObject addMuscle;
	public GameObject addBones;
	public GameObject menuPopUp;
	public GameObject optionsPopUp;

	public GameObject bonescale;
	public GameObject boneposition;
	public GameObject bonerotation;

	public GameObject muscleparameters;

	public GameObject articulationparameters;

	public GameObject play;
	public GameObject stop;
	public GameObject playmodeeffects;





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
			playmodeeffects.SetActive (true);

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
			addBones.SetActive (false);
			addMuscle.SetActive (false);
		}

		if (!manager.isPlaying)
		{
			playmodeeffects.SetActive (false);
			addBones.SetActive (true);
			delete.SetActive (true);
			if (manager.searchTwoBones ())
				addMuscle.SetActive (true);
			else
				addMuscle.SetActive (false);
			if (manager.searchArticulations())
				play.SetActive (true);
			else
				play.SetActive (false);
			stop.SetActive (false);

			switch(manager.itemSelected)
			{
			case "bone":
				setparent.SetActive (false);
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

			/*case "articulation":
				setparent.SetActive (false);
				resetparent.SetActive (false);
				delete.SetActive (false);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				muscleparameters.SetActive (false);
				articulationparameters.SetActive (true);
				break; */
				

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

	public void menuOn()
	{
		menuPopUp.SetActive (true);
	}

	public void menuOff()
	{
		menuPopUp.SetActive (false);

	}

	public void menuYes()
	{
		SceneManager.LoadScene ("mainTitle");
	}

	public void optionsOn()
	{
		optionsPopUp.SetActive (true);
	}

	public void optionsOff()
	{
		optionsPopUp.SetActive (false);

	}
}
