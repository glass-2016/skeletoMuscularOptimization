using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour 
{
	//buttons
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

	public Text collCounter;





	// Use this for initialization
	void Start () {
		collCounter.text = " ";

	}
	
	// Update is called once per frame
	void Update () 
	{
		//C# does not want public untyped vars, so... sorry. goes there. 
		var manager = GameObject.FindWithTag ("MainCamera").GetComponent<manager> ();

		if (manager.isPlaying) 
		{
			playmodeeffects.SetActive (true);

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

			collCounter.text = (manager.maxCollectible - manager.nbCollectible) + "/" + manager.maxCollectible;

		}

		if (!manager.isPlaying)
		{
			collCounter.text = " ";
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
				bonescale.SetActive (true);
				boneposition.SetActive (true);
				bonerotation.SetActive (true);
				muscleparameters.SetActive (false);
				articulationparameters.SetActive (false);
				break;

			case "muscle":
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				muscleparameters.SetActive (true);
				articulationparameters.SetActive (false);
				break;

			case "boneHasParent":
				bonescale.SetActive (true);
				boneposition.SetActive (true);
				bonerotation.SetActive (true);
				muscleparameters.SetActive (false);
				articulationparameters.SetActive (false);
				break;

			case "muscleHasParent":
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				muscleparameters.SetActive (true);
				articulationparameters.SetActive (false);
				break;

			case "articulation":
				delete.SetActive (true);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				muscleparameters.SetActive (false);
				articulationparameters.SetActive (true);
				break; 
				

			case "none":
				delete.SetActive (false);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				muscleparameters.SetActive (false);
				articulationparameters.SetActive (false);
				break;

			default:
				delete.SetActive (false);
				bonescale.SetActive (false);
				boneposition.SetActive (false);
				bonerotation.SetActive (false);
				muscleparameters.SetActive (false);
				articulationparameters.SetActive (false);
				break;

			}
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			menuOn ();
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
