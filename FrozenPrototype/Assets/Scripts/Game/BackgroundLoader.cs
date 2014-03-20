using UnityEngine;
using System.Collections;

public class BackgroundLoader : MonoBehaviour
{
	private static BackgroundLoader instance = null;
	
	public static int levelBgIdx = 1;
	public static int levelIdx = 1;
	public static bool defaultLevelLoaded = true;
	
	public int maxBackgrounds = 8;

	
	public static BackgroundLoader Instance {
		get {
			return instance;
		}
	}
	
	void Awake() {
		instance = this;
	}

	void Start() {
		int bgIndex = levelIdx / 10 + 1;
		
	//	renderer.material.mainTexture = Resources.Load("Game/BG_0" + (levelBgIdx < 10 ? "0" : "") + levelBgIdx) as Texture;
		
		//string defaultResourceName = "Game/BG_002";
		string defaultResourceName = "Game/BG_0" + (bgIndex < 10 ? "0" : "") + bgIndex;
	/*	int idx = levelIdx / 10 + 1;
		if (idx >= 1 && idx <= 7)
		{
			defaultResourceName = "Game/BG_00" + idx.ToString();
		}
	*/	
		
		renderer.material.mainTexture = Resources.Load(defaultResourceName) as Texture;
		if (BackgroundLoader.defaultLevelLoaded)
		{
			levelBgIdx = levelBgIdx % maxBackgrounds + 1;
		}
	}
}

