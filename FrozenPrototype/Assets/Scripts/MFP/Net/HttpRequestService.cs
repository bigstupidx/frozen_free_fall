using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using System;

public class HttpRequestService : MonoBehaviour {
	
	private string encoded_key = "ycyujian";
	
	private static string uid_key = "user_id_key";
	
	private string getDeviceID()
	{
		string deviceID = "";
#if UNITY_IPHONE && !UNITY_EDITOR
		deviceID = PlayerPrefs.GetString("iOS_vendorId");
#elif UNITY_ANDROID && !UNITY_EDITOR
		deviceID = MFPDeviceAndroid.Instance.getVendorID();
#else
		deviceID = "mfpUnityEditorVendorID";
#endif
		return deviceID;
	}
	
	private string getOpenID()
	{
		string deviceID = "";
#if UNITY_IPHONE && !UNITY_EDITOR
		deviceID = PlayerPrefs.GetString("iOS_openId");
#else
		deviceID = "mfpUnityEditorVendorID";
#endif
		return deviceID;
	}
	
	public static void sendRequest(Dictionary<string, object> data, RequestSuccessDelegate successDelegate, RequestFailDelegate failDelgate, string formKey = "data")
	{
		AppSettings.setServerUrlAccordingToPlatform();
		
		GameObject oldServiceObj = GameObject.Find("HttpRequestService");
		GameObject newServiceObj = (GameObject)Instantiate(oldServiceObj);
		HttpRequestService service = newServiceObj.GetComponent<HttpRequestService>();

		if (formKey == "data")
		{
			data["Diamond"] = UserManagerCloud.Instance.CurrentUser.UserGoldCoins;
			data["SnowBall"] = UserManagerCloud.Instance.CurrentUser.SnowBall;
			data["Hourglass"] = UserManagerCloud.Instance.CurrentUser.Hourglass;
			data["IcePick"] = UserManagerCloud.Instance.CurrentUser.IcePick;
			data["MagicPower"] = UserManagerCloud.Instance.CurrentUser.MagicPower;
		}
		
		service._data = data;
		service._formKey = formKey;
		service._successDelegate = successDelegate;
		service._failDelgate = failDelgate;
		
		System.Random rd = new System.Random();
		service._authCode = rd.Next();
		
		service.sendHttpRequest();
	}
	
	
	public static int getUserID()
	{
		return PlayerPrefs.GetInt(uid_key, -1);
	}
	
	public static void setUserID(int userID)
	{
		PlayerPrefs.SetInt(uid_key, userID);
	}
	
	public delegate void RequestSuccessDelegate(string jsonData);
	public delegate void RequestFailDelegate();
	
	private Dictionary<string, object> _data;
	public event RequestSuccessDelegate _successDelegate;
	public event RequestFailDelegate _failDelgate;
	private string _formKey;
	
	private int _authCode;
	
	// Use this for initialization
	void Start () 
	{
//		string decodedStr = "7EC83AB6AC279B5475E2BF9BFA3185FF40CB54ABBE6944F65F1583749F68C2BBCFD60E8ACB2F390822E7EA4DE87808ABB4AD92435F46A0721E2764AF872C46FF49BF6F5864A0F1554CA083FCA8769A273C67554A69F992CA494A5E76284A0C66BECE77818E5576D4A6B39EF4FE4DD2BC8FA4F43329F317A6579BF8B64C746AFEC83329C6E77F4B339F3CCC0494FA13233FA1F401CFBF70BCA7788EEC2BE1719586EE27FE896AB48EE71A6E140E46ED1E7B63631B96459846";
//		string result = DesSecurity.DesDecrypt(decodedStr, encoded_key);
//		Debug.Log(result);
//		Resources.UnloadUnusedAssets();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void sendHttpRequest()
	{
		StartCoroutine(send());
	}

	public IEnumerator send()
	{
		string jsonContent = _data.toJson();
		
		string encodedJson = jsonContent;
		if (_formKey == "data")
		{
			encodedJson = DesSecurity.DesEncrypt(jsonContent, encoded_key);
		}
		
		WWWForm form = new WWWForm ();  
		form.AddField("deviceId", getDeviceID());
		form.AddField("openId", getOpenID());
		form.AddField (_formKey, encodedJson);
		
		if (_formKey == "data" && _data != null && _data.ContainsKey("cmd"))
		{
			form.AddField("cmd", _data["cmd"].ToString());
		}   
		else
		{
			form.AddField("cmd", "bi");
		}
		
		//form.AddField("debug", "1");
		form.AddField("auth", _authCode.ToString());
		
		int userID = getUserID();
		if (userID != -1)
		{
			form.AddField("userId", userID.ToString());
		}
		
		string serverUrl = _formKey == "data" ? AppSettings.gameServerUrl : AppSettings.biServerUrl;
				
		WWW getData = new WWW (serverUrl, form);  
		yield return getData;  
		
		if (getData.error == null) 
		{  
			//string result = getData.text;
			string result = DesSecurity.DesDecrypt(getData.text, encoded_key);
			
			// Cache UID
			Dictionary<string, object> dataDict = result.dictionaryFromJson();
			if (dataDict != null && dataDict.ContainsKey("userId"))
			{
				int uid = Convert.ToInt32(dataDict["userId"]);
				Debug.Log("Cache User ID: " + uid.ToString());
				setUserID(uid);
			}
			
			// verify auth code
			bool verifyOK = true;
			if (dataDict.ContainsKey("auth"))
			{
				int returnAuthCode = Convert.ToInt32(dataDict["auth"]);
				if (returnAuthCode != _authCode)
				{
					verifyOK = false;
				}
			}
			
			if (dataDict.ContainsKey("cheat"))
			{
				int cheat = Convert.ToInt32(dataDict["cheat"]);
				PlayerPrefs.SetInt("cheat", cheat);
			}
			
			if (verifyOK && _successDelegate != null && dataDict.ContainsKey("data"))
			{
				Dictionary<string, object> dict = dataDict["data"] as Dictionary<string, object>;
				_successDelegate(dict.toJson());
			}
			else if (!verifyOK && _failDelgate != null)
			{
				_failDelgate();
			}
			
			Destroy(this.gameObject);
		} 
		else 
		{  			
			if (_failDelgate != null)
			{
				_failDelgate();
			}
			Debug.Log (getData.error);
			
			Destroy(this.gameObject);
		}
	}
}
