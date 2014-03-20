using UnityEngine;
using System.Collections;
using Prime31;

public class LocalizedMovieAd : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{	
		/*
		string lang = Language.CurrentLanguage().ToString().ToLower();
		if (lang != "zh")
		{
			GameObject movieAdObj = GameObject.Find("MovieTexture");
			movieAdObj.SetActive(false);
			
			Application.LoadLevel("Home");
		}
		else
		{
			StartCoroutine(DelayLoadLevel());	
		}
		 */
	}
	
	/*
	IEnumerator DelayLoadLevel()
	{
		
		string platform = MFPBillingAndroid.CURRENT_PLATFORM;
		if (platform == MFPBillingAndroid.DIANXIN_ZIYOU || platform == MFPBillingAndroid.LIANTONG_ZIYOU)
		{
			Application.LoadLevel("Home");
		}
		else
		{
			yield return new WaitForSeconds(1.6f);
			Application.LoadLevel("Home");
		}
		
	}
	*/
	// Update is called once per frame
	void Update () {
	
	}
}
