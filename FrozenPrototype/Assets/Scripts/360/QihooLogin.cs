using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using System.IO;

public class QihooLogin : MonoBehaviour {
	
	GameObject qihooButtonObj;
	
	// Use this for initialization
	void Start () 
	{
		//PlayerPrefs.SetInt("key_360", 1);
		transform.localPosition = new Vector3(2000, -300, 0);
		
		// [1] User has logined in with 360 before. [2]. First time entering "home" scene, not backing from "map" scene [3]. Having internet collection 
		if (QihooSnsModel.Instance.Using360Login && QihooSnsModel.Instance.goToMapLevelOnce == false && 
			MFPDeviceAndroid.Instance.getNetWorkState() == MFPDeviceAndroid.NETWORK_STATE_CONNECTED)
		{
			UserSNSManager.Instance.snsLogin();
		}
	}
	
	public void activate()
	{
		if (!QihooSnsModel.Instance.Using360Login && 
			MFPDeviceAndroid.Instance.getNetWorkState() == MFPDeviceAndroid.NETWORK_STATE_CONNECTED &&
			UserManagerCloud.Instance.CurrentUser.LastFinishedLvl >= 4)
		{
			transform.localPosition = new Vector3(0, -300, 0);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{
		/*
		PlayMakerFSM playFsm = GameObject.Find("Flow Control/Play Button").GetComponent<PlayMakerFSM>();
		playFsm.SendEvent(NGuiPlayMakerProxy.GetFsmEventEnumValue(NGuiPlayMakerDelegates.OnClickEvent));
		return;
		*/
		
		UserSNSManager.Instance.snsLogin();
		
		Debug.Log ("11111111");
		return;
		
		
		
		string path = UserManagerCloud.DataPath + "json.txt";
		StreamReader sr = new StreamReader(path);		
		string line = sr.ReadLine();
		Debug.Log(line);
		//QihooSnsModel.Instance.parseLoginResult(line);
		//line = "1";
		QihooSnsModel.Instance.onLoginFinished(line);
		
		
		string path2 = UserManagerCloud.DataPath + "json2.txt";
		StreamReader sr2 = new StreamReader(path2);		
		string line2 = sr2.ReadLine();
		Debug.Log(line2);
		//QihooSnsModel.Instance.parseUserFriendResult(line2);
		//line2 = "2";
		QihooSnsModel.Instance.onGetFriendFinished(line2);
		
		//line = "{\"state\":\"test_state111\",\"expires_in\":\"2592000\",\"errno\":0}";
		//var dict = line.dictionaryFromJson();
		
	}
}
