using UnityEngine;
using System.Collections;

public class AppSettings : MonoBehaviour 
{
	static public string gameServerUrl = "http://frozendisney.microfunplus.com/third/net";
	static public string biServerUrl = "http://frozendisney.microfunplus.com/thirdbi/net";
	
	static public string frontEndVersion = "1.4.0";	
	static public bool Is360Platform = false;
	
	// Use this for initialization
	void Awake () 
	{
		DontDestroyOnLoad(gameObject);
	}
	
	public static void setServerUrlAccordingToPlatform()
	{
#if UNITY_IOS
		gameServerUrl = "http://frozen.microfunplus.com/v102/net";
		biServerUrl = "http://frozen.microfunplus.com/v102_bi/net";
#elif YIDONG_MM_ZIQIANMING
		gameServerUrl = "http://frozenandroid.microfunplus.com/mm_mfp/net";
 		biServerUrl = "http://frozenandroid.microfunplus.com/mm_mfp_bi/net";
#elif DIANXIN_ZIYOU
		gameServerUrl = "http://frozenandroid.microfunplus.com/dianxinnei/net";
		biServerUrl = "http://frozenandroid.microfunplus.com/dianxinnei/net";
#elif DIANXIN_WAIFANG
		gameServerUrl = "http://frozenandroid.microfunplus.com/mm_mfp/net";
 		biServerUrl = "http://frozenandroid.microfunplus.com/mm_mfp_bi/net";
#elif LIANTONG
		gameServerUrl = "http://frozenandroid.microfunplus.com/liantong/net";
		biServerUrl = "http://frozenandroid.microfunplus.com/liantong/net";
#elif YIDONG_MM_SHENHE
		gameServerUrl = "http://frozenandroid.microfunplus.com/mm/net";
		biServerUrl = "http://frozenandroid.microfunplus.com/mmbi/net";
#elif YIDONG_MDO
		gameServerUrl = "http://frozenandroid.microfunplus.com/mdo/net";
 		biServerUrl = "http://frozenandroid.microfunplus.com/mdo/net";
#endif
		
		// for ChuKong
		//gameServerUrl = "http://frozendisney.microfunplus.com/third/net";
		//biServerUrl = "http://frozendisney.microfunplus.com/thirdbi/net";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
