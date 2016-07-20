using UnityEngine;
using UnityEngine.SceneManagement;

public class mainTitle : MonoBehaviour {



	public GameObject settingsPanel;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void quit()
	{
		Application.Quit ();
	}

	public void newModel()
	{
		SceneManager.LoadScene ("editor");
	}

	public void settings()
	{
		settingsPanel.SetActive (!settingsPanel.activeInHierarchy);
	}


}
