using UnityEngine;
using System.Collections;

public class AnalyticsInitializerAndroid : MonoBehaviour {

#if UNITY_ANDROID
        private string FLURRY_API = "623QCF7K5ZVMGRQT68TM";
#elif UNITY_IPHONE
        private string FLURRY_API = "32WYHPZKHTB6GRWYRHDN";
#else
        private string FLURRY_API = "";
#endif
	
	// Use this for initialization
	void Start () 
	{
		Debug.Log("Flurry Start. Key:" + FLURRY_API);
        FlurryAgent.Instance.onStartSession(FLURRY_API);
    }
	
	#if !UNITY_EDITOR && UNITY_ANDROID
	void Awake()
	{
		Debug.Log(" wenming 1111111111111111111111111111");

		MFPBillingAndroid.Instance.init();
		//AnalyticsBinding.Init("","");
		Debug.Log(" wenming 2222222222222222222222222222222222222");
	}
	#endif
}
