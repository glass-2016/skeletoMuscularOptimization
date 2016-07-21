using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerSettings : MonoBehaviour {
	
	public Slider soundSlider;
	public Slider mouseSlider;

	// Use this for initialization
	void Start () 
	{
		PlayerPrefs.SetInt ("soundVolume", 100);
		PlayerPrefs.Save ();
		PlayerPrefs.SetInt ("mouseSensibility", 6);
		PlayerPrefs.Save();

		soundSlider.value = PlayerPrefs.GetInt ("soundVolume");
		mouseSlider.value = PlayerPrefs.GetInt ("mouseSensibility");
		AudioListener.volume = (PlayerPrefs.GetInt ("soundVolume"))/100;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void soundVolume()
	{
		PlayerPrefs.SetInt ("soundVolume", Mathf.RoundToInt(soundSlider.value));
		PlayerPrefs.Save ();
		Debug.Log (PlayerPrefs.GetInt ("soundVolume"));
		AudioListener.volume = (PlayerPrefs.GetInt ("soundVolume"))/100;


	}

	public void mouseSensibility()
	{
		PlayerPrefs.SetInt ("mouseSensibility", Mathf.RoundToInt(mouseSlider.value));
		PlayerPrefs.Save ();
		Debug.Log (PlayerPrefs.GetInt ("mouseSensibility"));

	}
}
