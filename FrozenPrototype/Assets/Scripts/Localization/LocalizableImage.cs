using UnityEngine;
using System.Collections;

public class LocalizableImage : MonoBehaviour 
{
	public string path;
	
	// Use this for initialization
	void Start () 
	{
		string lang = "en";
		if (Language.CurrentLanguage().ToString().ToLower() != "zh")
		{
			lang = "ru";
		}
		Texture newTexture = Resources.Load(path + lang/*Language.CurrentLanguage().ToString().ToLower()*/) as Texture;
		
		if (newTexture == null) {
			newTexture = Resources.Load(path + "en") as Texture;
		}
		
		renderer.material.mainTexture = newTexture;
	}
}
