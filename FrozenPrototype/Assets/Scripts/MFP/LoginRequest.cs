using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class LoginRequest : MonoBehaviour {
	public PlayMakerFSM newVersionPanelFsm;

	public string appUrl;
	
	public  int MM_TYPE = 1;

	// Use this for initialization
	void Start () 
	{
		Dictionary<string, object> data = new Dictionary<string, object> ();
		data["cmd"] = "login";
		data["version"] = AppSettings.frontEndVersion;
		data["device"] = MFPDeviceAndroid.Instance.getDeviceModel();
		
		HttpRequestService.sendRequest(data, new HttpRequestService.RequestSuccessDelegate(onLoginSuccess), null);
	}
	
	void onLoginSuccess(string jsonData)
	{
		Dictionary<string, object> dataDict = jsonData.dictionaryFromJson();
		if (dataDict == null)
		{
			return;
		}
		
		// whether show "360 login" button or not
		if (dataDict.ContainsKey("qihu"))
		{
			AppSettings.Is360Platform = true;
			bool show360Button = Convert.ToBoolean(dataDict["qihu"]);
			if (show360Button)
			{
				GameObject qihooObj = GameObject.Find("360 Button");
				qihooObj.SendMessage("activate");
			}
		}
		
		//dataDict["updateUrl"] = "http://www.baidu.com";
		// whether show new version window or not
		if (dataDict.ContainsKey("updateUrl"))
		{
			newVersionPanelFsm.SendEvent("Has New Version");
			appUrl = Convert.ToString(dataDict["updateUrl"]);
		}
		
		if (dataDict.ContainsKey("mm"))
		{
			// 1. mm 2. MDO 3.game base
			int mmType = Convert.ToInt32(dataDict["mm"]);
			Debug.Log("before set " + PlayerPrefs.GetInt("mm_type") +" back value:"+mmType+",front value:"+MM_TYPE);
			
			//如果支付方式改变，则重新初始化
			if(mmType != PlayerPrefs.GetInt("mm_type",MFPBillingAndroid.defaultMobileWay)){
				 Debug.Log("pay way changed: " + mmType );
				PlayerPrefs.SetInt("mm_type", mmType);
				Debug.Log("after set " + PlayerPrefs.GetInt("mm_type",1) );
				MFPBillingAndroid.Instance.init();
			}
	
			
		  Debug.Log("return pay@@@@@@@@@@@@@@@@@@@@@@@ : " + mmType );
		}
		
		if (dataDict.ContainsKey("device_key_words"))
		{
			string devices = Convert.ToString(dataDict["device_key_words"]);
			PlayerPrefs.SetString("device_key_words", devices);
		}
	}
	
	void onLoginFail(string jsonStr)
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
