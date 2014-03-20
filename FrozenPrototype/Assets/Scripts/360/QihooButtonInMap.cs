using UnityEngine;
using System.Collections;

public class QihooButtonInMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
	//PlayerPrefs.SetInt ("key_360", 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		/*
		GameObject scorePanel = GameObject.Find("ScoreRankPanel");
		if (scorePanel != null)
		{
			PlayerPrefs.SetInt ("key_360", 1);
			ScoreRankPanel scoreCom = scorePanel.GetComponent<ScoreRankPanel>();
			scoreCom.UpdateContentForCurrentLevel();
		}
		*/
		Debug.Log("222");
		UserSNSManager.Instance.snsLogin();
	}
}
