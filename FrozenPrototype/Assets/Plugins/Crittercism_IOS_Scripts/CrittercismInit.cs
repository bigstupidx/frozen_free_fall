using UnityEngine;
using System.Collections;

public class CrittercismInit : MonoBehaviour {
	
	private const string CrittercismAppID	= "52614de48b2e337efe000004";
	private const string CrittercismAndroidAppID = "52614e17558d6a7129000004";
	private const bool bDelaySendingAppLoad = false;
	private const bool bShouldCollectLogcat = false;
	private const string CustomVersionName = "";/*Your Custom Version Name Here*/
	
	void Awake ()
	{
		/*
		#if UNITY_IOS
		CrittercismIOS.Init(CrittercismAppID);
		#elif UNITY_ANDROID
		CrittercismAndroid.Init(CrittercismAndroidAppID, bDelaySendingAppLoad, bShouldCollectLogcat, CustomVersionName);
		#endif
		Destroy(this);
		*/
	}
}
