using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class GiftConfirmButton : MonoBehaviour {
	
	public static System.DateTime baseDate = new System.DateTime(2013, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	
	public PlayMakerFSM fsm;

	private long _lastClickTime = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		long curTime = (long)System.DateTime.Now.Subtract(baseDate).TotalSeconds;
		if (curTime - _lastClickTime < 10)
		{
			GameObject TipObj = GameObject.Find("MFP Gift Panel Portrait/Tip1");
			UILabel tipLabel = TipObj.GetComponent<UILabel>();
			tipLabel.text = Language.Get("INPUT_TOO_FAST");
			return;
		}
		
		_lastClickTime = (long)System.DateTime.Now.Subtract(baseDate).TotalSeconds;	// Can't send new request in 10 seconds
		
		GameObject labelObj = GameObject.Find("MFP Gift Panel Portrait/Input/Label");
		UILabel labelCom = labelObj.GetComponent<UILabel>();
		
		Dictionary<string, object> data = new Dictionary<string, object> ();
		data["cmd"] = "convert";
		data["id"] = labelCom.text;
		
		HttpRequestService.sendRequest(data, new HttpRequestService.RequestSuccessDelegate(onAskGiftSuccess), new HttpRequestService.RequestFailDelegate(onAskGiftFail));
	}
	
	void onAskGiftSuccess (string jsonData)
	{
		enabled = true;
		
		Debug.Log("Send High Score Cache Service Success");
		
		Dictionary<string, object> data = jsonData.dictionaryFromJson();
		if (data.ContainsKey("reward"))
		{
			int rewardValue = Convert.ToInt32(data["reward"]);
			
			UserManagerCloud.Instance.CurrentUser.UserGoldCoins += rewardValue;
			UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);
			
			GameObject TipObj = GameObject.Find("MFP Gift Panel Portrait/Tip1");
			UILabel tipLabel = TipObj.GetComponent<UILabel>();
			tipLabel.text = Language.Get("GIFT_SUCCESS_TIP") + "+" + rewardValue.ToString() +  Language.Get("MFP_MALL_TAB_DIAMOND");
			
			PopupMessage.Show(Language.Get("GIFT_SUCCESS_TIP") + "+" + rewardValue.ToString() +  Language.Get("MFP_MALL_TAB_DIAMOND"));
			//fsm.SendEvent(NGuiPlayMakerProxy.GetFsmEventEnumValue(NGuiPlayMakerDelegates.OnClickEvent));
		}
		else
		{
			GameObject TipObj = GameObject.Find("MFP Gift Panel Portrait/Tip1");
			UILabel tipLabel = TipObj.GetComponent<UILabel>();
			tipLabel.text = Language.Get("TIP_INVALID_GIFT_CODE");
			
		}
	}
	
	void onAskGiftFail ()
	{		
		enabled = true;
		
		GameObject TipObj = GameObject.Find("MFP Gift Panel Portrait/Tip1");
		UILabel tipLabel = TipObj.GetComponent<UILabel>();
		tipLabel.text = Language.Get("TIP_NO_NET_GIFT_CODE");
	}
}
