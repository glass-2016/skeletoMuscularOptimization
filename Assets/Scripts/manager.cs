using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class manager : MonoBehaviour 
{
	// selected object
	private GameObject currentObject = null;
	// bone prefab
	public musclesController bone;
	public muscle musclePrefab;
	// list of all objects added to scene
	private List<GameObject> list;
	//list to save when entering play mode
	private List<GameObject> saveList;
	// boolean to indicate parent mode
	private bool searchParent = false;
	// muscles attaches
	private bool firstAttach = false;
	private bool secondAttach = false;
	private Vector3[] attaches = null;
	private Vector3[] attachesNorm = null;
	// indicator to update values of fields parameters
	private Vector3 oldPosition;
	private Quaternion oldRotation;
	private Vector3 oldScale;
	private int globalIndex = 0;
	//selected/unselected materials
	public Material wireframe;
	public Material white;
	public Material muscle;
	public LineRenderer muscleFeedback;

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
	public InputField rangeZ;
	public int maxCollectible = 10;
	public int nbCollectible;
	private List<collectibles> listCollectibles; 
	// those are here to be read by other scripts
	public string itemSelected; 	//either "none", "bone", "muscle"
	public bool isPlaying;
	public bool isManipulating = false;
	public collectibles collectiblePrefab;
	public MeshFilter terrain;

	void Start ()
	{
		list = new List<GameObject> ();
		saveList = new List<GameObject> ();
		listCollectibles = new List<collectibles>();
		attaches = new Vector3[2];
		attachesNorm = new Vector3[2];
		nbCollectible = maxCollectible;
		terrain.mesh = ProceduralToolkit.Examples.TerrainMesh.TerrainDraft (100, 100, Random.Range (0, 50), Random.Range (0, 50), 1000).ToMesh();
		terrain.gameObject.GetComponent<MeshCollider> ().sharedMesh = terrain.mesh;
		AudioListener.volume = PlayerPrefs.GetInt ("soundVolume");
	}

	public void reset()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	// play mode
	public void play()
	{
		currentObject = null;
		changeFocus ();
		deselect ();

		saveList = new List<GameObject> ();

		for (int i = 0; i < list.Count; i++) 
		{
			saveList.Add (Instantiate (list [i]));
		}
		for (int i = 0; i < saveList.Count; i++)
		{
			if (saveList [i].tag == "bones")
			{
				musclesController tmpMuscleController = saveList [i].GetComponent<musclesController> ();
				musclesController tmpController = list [i].GetComponent<musclesController> ();
				Dictionary<int, articulations> tmpListArticulations = new Dictionary<int, articulations> ();
				foreach (KeyValuePair<int, articulations> tmpArticulations in tmpController.listArticulations)
				{
					articulations tmpArt = saveList[list.IndexOf(tmpController.listArticulations[tmpArticulations.Key].gameObject)].GetComponent<articulations>();
					Dictionary<int, muscle> tmpMuscles = new Dictionary<int, muscle> ();
					foreach (KeyValuePair<int, muscle> tmpMuscle in tmpArticulations.Value.muscles)
					{
						muscle tmp = saveList[list.IndexOf(tmpArticulations.Value.muscles[tmpMuscle.Key].gameObject)].GetComponent<muscle>();
						tmp.force = tmpMuscle.Value.force;
						tmp.key1 = tmpMuscle.Value.key1;
						tmp.currentArticulation = saveList[list.IndexOf(tmpArticulations.Value.gameObject)].GetComponent<articulations>();
						tmp.anchors = new List<GameObject> ();
						tmp.setAnchor (saveList[list.IndexOf(tmpArticulations.Value.controllers[0].gameObject)]);
						tmp.setAnchor (saveList[list.IndexOf(tmpArticulations.Value.controllers[1].gameObject)]);
						tmp.setLimits (tmpMuscle.Value.attachPoints, tmpMuscle.Value.attachPoints [0] + tmpMuscle.Value.offsets [0],
							tmpMuscle.Value.attachPoints [1] + tmpMuscle.Value.offsets [1], tmpMuscle.Value.normals [0], tmpMuscle.Value.normals [1]);
						tmp.setIndex(tmpMuscle.Value.index);
						tmpMuscles.Add (tmpMuscle.Key, tmp);
					}
					tmpArt.muscles = tmpMuscles;
					tmpArt.muscleIndex = tmpArticulations.Value.muscleIndex;
					tmpArt.index = tmpArticulations.Value.index;
					tmpListArticulations.Add (tmpArticulations.Key, tmpArt);
				}
				tmpMuscleController.listArticulations = tmpListArticulations;
			}
		}
		for (int i = 0; i < saveList.Count; i++)
		{
			if (saveList [i].tag == "articulations")
			{
				articulations tmpArt = saveList [i].GetComponent<articulations> ();
				Destroy(saveList [list.IndexOf (list [i].GetComponent<articulations> ().controllers [0].gameObject)].GetComponent<HingeJoint> ());
				tmpArt.addRigidBody (saveList [list.IndexOf (list [i].GetComponent<articulations> ().controllers [0].gameObject)].GetComponent<musclesController> (),
					saveList [list.IndexOf (list [i].GetComponent<articulations> ().controllers [1].gameObject)].GetComponent<musclesController> (), tmpArt.index);
				tmpArt.setLimitsAxis (list[i].GetComponent<articulations>().axisLimits);
				foreach (KeyValuePair<int, muscle> tmpMuscle in tmpArt.muscles)
					tmpArt.addDirection (tmpMuscle.Value.angularDirection);
			}
			saveList [i].transform.position -= new Vector3 (1000, 1000, 1000);
		}
		isPlaying = true;
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
				if (list[i].tag == "bones")
					list [i].GetComponent<Collider> ().isTrigger = false;
			}
			else if (list [i].tag == "muscles")
				list [i].GetComponent<muscle> ().onPlay = true;
		}

		for (int i = 0; i < maxCollectible; i++)
		{
			listCollectibles.Add (Instantiate (collectiblePrefab, new Vector3 (Random.Range (-terrain.mesh.bounds.size.x / 2.0f, terrain.mesh.bounds.size.x / 2.0f), -3f, Random.Range (-terrain.mesh.bounds.size.z / 2.0f, terrain.mesh.bounds.size.z / 2.0f)), Quaternion.identity) as collectibles);
		}
	}

	public void playReset()
	{
		isPlaying = false;

		for (int i = 0; i < listCollectibles.Count; i++) 
		{
			Destroy(listCollectibles[i].gameObject);
		}
		listCollectibles.Clear ();

		for (int i = 0; i < list.Count; i++) 
		{
			DestroyObject(list[i]);
		}
		list.Clear ();
		list = saveList;			
		for (int i = 0; i < list.Count; i++) 
		{
			list [i].transform.position += new Vector3 (1000, 1000, 1000);
		}
		currentObject = null;
		changeFocus ();
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
				if (currentObject.gameObject.GetComponent<muscle> ()) 
				{
					float force = currentObject.gameObject.GetComponent<muscle> ().force;
					float.TryParse (muscleForce.text, out force);
					currentObject.gameObject.GetComponent<muscle> ().force = force;
					currentObject.gameObject.GetComponent<muscle> ().key1 = key1.text.ToUpper();
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

	IEnumerator shift(musclesController tmpBone)
	{
		yield return new WaitForFixedUpdate();
		while (tmpBone.colliding)
		{
			tmpBone.transform.position += tmpBone.transform.right;
			yield return null;
		}
	}

	// add bone
	public void spawn()
	{
		musclesController tmpBone = Instantiate<musclesController> (bone);
		list.Add(tmpBone.gameObject);
		StartCoroutine (shift(tmpBone));
	}

	public void removeParent()
	{
		if (currentObject)
			currentObject.transform.parent = null;
	}

	public void addMuscle()
	{
		secondAttach = false;
		searchParent = false;
		firstAttach = true;
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
			float tmpX, tmpY = 0.0f;
			float.TryParse(rangeX.text, out tmpX);
			float.TryParse(rangeZ.text, out tmpY);
			currentObject.GetComponent<articulations> ().setLimitsAxis (new Vector2(tmpX, tmpY));
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
			{
				list [i].GetComponent<musclesController> ().colliding = false;
				list [i].GetComponent<bones> ().isSelected = false;
			}
			else if (list [i].tag == "articulations")
				list [i].GetComponent<articulations> ().colliding = false;
			if (list[i].tag != "muscles")
				list [i].GetComponent<Renderer> ().material.color = Vector4.one;
		}
	}

	public bool searchTwoBones()
	{
		int j = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (list [i].tag == "bones")
				j++;
			if (j >= 2)
				return (true);
		}
		return (false);
	}

	public bool searchArticulations()
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list [i].tag == "articulations")
				return (true);
		}
		return (false);
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
		rangeZ.text = "0.0";
	}

	void updateValues()
	{
		positionX.text = currentObject.transform.position.x.ToString ();
		positionY.text = currentObject.transform.position.y.ToString ();
		positionZ.text = currentObject.transform.position.z.ToString ();
		rotationX.text = currentObject.transform.rotation.eulerAngles.x.ToString ();
		rotationY.text = currentObject.transform.rotation.eulerAngles.y.ToString ();
		rotationZ.text = currentObject.transform.rotation.eulerAngles.z.ToString ();
		scaleX.text = currentObject.transform.localScale.x.ToString ();
		scaleY.text = currentObject.transform.localScale.y.ToString ();
		scaleZ.text = currentObject.transform.localScale.z.ToString ();
		if (currentObject && currentObject.tag == "articulations")
		{
			rangeX.text = currentObject.GetComponent<articulations> ().axisLimits.x.ToString ();
			rangeZ.text = currentObject.GetComponent<articulations> ().axisLimits.y.ToString ();
		}

		if (currentObject.tag == "muscles") 
		{
			if (currentObject.gameObject.GetComponent<muscle> ())  
			{
				muscleForce.text = currentObject.gameObject.GetComponent<muscle> ().force.ToString();
				key1.text = currentObject.gameObject.GetComponent<muscle> ().key1;
			}

		}

		//To do : ADD ARTICULATIONS WHEN NEEDED

	}

	// change colors and parameters value to newly selected object
	void changeFocus()
	{
		if (currentObject) {
			currentObject.GetComponent<Renderer> ().material.color = Color.yellow;
			oldPosition = currentObject.transform.position;
			oldRotation = currentObject.transform.rotation;
			oldScale = currentObject.transform.localScale;
			updateValues ();
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list [i] != currentObject) 
			{
				Renderer tmp = list [i].GetComponent<Renderer> ();
				if (list[i].tag == "muscles")
					tmp.material = muscle;
				else
					tmp.material = white;
			}
			else 
			{
				Renderer tmp = list [i].GetComponent<Renderer> ();
				tmp.material = wireframe;
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
		if (firstAttach)
		{
			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			if (Physics.Raycast (ray, out hit))
			{
				muscleFeedback.enabled = true;
				muscleFeedback.SetPosition (0, hit.point - Vector3.Cross(hit.normal, hit.barycentricCoordinate) / 5.0f);
				muscleFeedback.SetPosition (1, hit.point + Vector3.Cross(hit.normal, hit.barycentricCoordinate) / 5.0f);
			} else
				muscleFeedback.enabled = false;
				
		} else if (secondAttach)
		{
			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			if (Physics.Raycast (ray, out hit))
				muscleFeedback.SetPosition (1, hit.point);
			else
				muscleFeedback.SetPosition (1, attaches[0] + Vector3.Cross(hit.normal, hit.barycentricCoordinate) / 5.0f);
		} else
			muscleFeedback.enabled = false;
		if (isPlaying)
		{
			nbCollectible = maxCollectible;
			for (int i = 0; i < listCollectibles.Count; i++)
			{
				if (!listCollectibles [i].isActiveAndEnabled)
					nbCollectible--;
			}
		}
		if ((firstAttach || secondAttach) && currentObject && currentObject.tag == "bones")
		{
			currentObject.GetComponent<bones> ().isSelected = false;
//			currentObject.GetComponent<bones> ().manipulator.SetActive (false);
		}

		if (Input.GetMouseButtonDown (0) && !isPlaying) 
		{
			//removes the object manipulator if another is selected
			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			if (Physics.Raycast (ray, out hit) && (hit.collider.tag == "bones" || hit.collider.tag == "muscles" || hit.collider.tag == "articulations" || hit.collider.tag == "manipulator"))
			{
				if (currentObject)
				{
					if (currentObject.GetComponent<bones> () && currentObject != hit.collider.gameObject && hit.collider.tag != "manipulator")
					{
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
					attachesNorm [0] = hit.normal;
					firstAttach = false;
					muscleFeedback.SetPosition (0, attaches [0]);
					muscleFeedback.SetPosition (1, attaches [0] + Vector3.up / 4.0f);
					secondAttach = true;
					currentObject = hit.collider.gameObject;
					changeFocus ();
				} 
				else if (secondAttach && currentObject != hit.collider.gameObject)
				{
					// add second attach to object clicked and add muscle
					attaches [1] = hit.point;
					attachesNorm [1] = hit.normal;
					secondAttach = false;
					muscle tmpMuscle = Instantiate (musclePrefab, attaches [0] + (attaches [1] - attaches [0]), Quaternion.identity) as muscle;
					list.Add (tmpMuscle.gameObject);
					musclesController tmpController = currentObject.GetComponent<musclesController> ();
					tmpMuscle.setAnchor (tmpController.gameObject);
					currentObject = hit.collider.gameObject;
					musclesController otherController = hit.collider.gameObject.GetComponent<musclesController> ();
					articulations currentArticulations = tmpController.addAnchor (tmpMuscle, otherController, globalIndex);
					if (!list.Contains (currentArticulations.gameObject))
						list.Add (currentArticulations.gameObject);
					otherController.addArticulation (currentArticulations);
					globalIndex = currentArticulations.index + 1;
					tmpMuscle.setArticulation (currentArticulations);
					tmpMuscle.setLimits (attaches, tmpController.gameObject.transform.position, currentObject.transform.position, attachesNorm[0], attachesNorm[1]);
					changeFocus ();
					tmpMuscle.setAnchor (currentObject);
				} 
				else if (hit.collider.tag != "manipulator")
				{
					currentObject = hit.collider.gameObject;
					changeFocus ();
				}
			} 
			else if (!EventSystem.current.IsPointerOverGameObject() && currentObject)
			{
				if (currentObject.tag == "bones")
					currentObject.GetComponent<bones> ().isSelected = false;
				firstAttach = false;
				secondAttach = false;
				currentObject = null;
				changeFocus ();
			}
		}
		if (Input.GetKeyDown (KeyCode.C))
		{
			// deselect current object
			firstAttach = false;
			secondAttach = false;
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
