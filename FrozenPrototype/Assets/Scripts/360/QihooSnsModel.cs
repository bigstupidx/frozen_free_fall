using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Prime31;

class QihooFriendData 
{
	public string UserID;
	public string UserName;
	
	public QihooFriendData(string _userID, string _userName)
	{
		UserID = _userID;
		UserName = _userName;
	}
	
	public string ToString()
	{
		string str = "ID = " + UserID + ", Name = " + UserName;
		return str;
	}
}

public class QihooSnsModel 
{
	public string UserID;
	public string UserName;
	List<QihooFriendData> FriendList = new List<QihooFriendData>();
	
	private static string FILE_LOGIN_CACHE = "sns_user_cache.date";
	private static string FILE_FRIEND_CACHE = "sns_friend_cache.date";
	
	private static string KEY_360 = "key_360";
	
	// A flag indicating user has already go to map level. so if user come to home level from map level, no need to call 360 login
	public bool goToMapLevelOnce = false;
	
	public long LastGetFriendTime = 0;
	
	//public bool ShowLoginButton = false;
	
	private static QihooSnsModel _instance;
	public static QihooSnsModel Instance
	{
		get
		{
			if (_instance==null)
			{
				_instance = new QihooSnsModel();
			}
			return _instance;
		}
	}
	
	public bool Using360Login
	{
		get
		{
			return PlayerPrefs.GetInt(KEY_360, 0) == 1;
		}
	}
	
	public string FriendListStr
	{
		get
		{
			string res = "";
			for (int i = 0; i < FriendList.Count; i++)
			{
				res += FriendList[i].UserID + ",";
			}
			if (res.Length > 0)
			{
				res = res.Substring(0, res.Length - 1);
			}
			return res;
		}
	}
	
	public string getNameByPlatformID(string platformID)
	{
		string name = platformID;
		for (int i = 0; i < FriendList.Count; i++)
		{
			QihooFriendData data = FriendList[i];
			if (platformID == data.UserID)
			{
				name = data.UserName;
				break;
			}
		}
		return name;
	}
	
	string loadTextFromFile(string fileName)
	{
		StreamReader sr = new StreamReader(fileName);		
		string line = "";
		if (sr != null)
		{
			line = sr.ReadLine();
			sr.Close();
		}
		return line;
	}
	
	void saveTextToFile(string fileName, string content)
	{
		StreamWriter sw = new StreamWriter(fileName);
		if (sw != null)
		{
			sw.Write(content);
			sw.Close();
		}
		return;
	}
	
	public void onLoginFinished(string jsonData)
	{
		Dictionary<string, object> rootDict = jsonData.dictionaryFromJson();
		if (rootDict != null && Convert.ToInt32(rootDict["errno"]) == 0)	// success. no error
		{
			Debug.Log("360 Login Success");
			bool isOk = parseLoginResult(jsonData);
			Debug.Log("Parse 360 information. Status = " + isOk.ToString());
			if (isOk)
			{
				PlayerPrefs.SetInt(KEY_360, 1);	// set a flag that user already use 360 login 
				
				string userCacheFile = UserCloud.GetPath(FILE_LOGIN_CACHE);
				saveTextToFile(userCacheFile, jsonData);
			}
		}
		else 
		{
			Debug.Log("360 Login Fail");
			
			string userCacheFile = UserCloud.GetPath(FILE_LOGIN_CACHE);
			string cachedJsonData = loadTextFromFile(userCacheFile);
			bool isOK = parseLoginResult(cachedJsonData);
			Debug.Log("Try load 360 information from cache. Status = " + isOK.ToString());
		}
	}
	
	public void onGetFriendFinished(string jsonData)
	{
		Debug.Log("360 SDK Friends: \n" + jsonData);
		
		Dictionary<string, object> rootDict = jsonData.dictionaryFromJson();
		if (rootDict != null && Convert.ToInt32(rootDict["errno"]) == 0)	// success. no error
		{
			Debug.Log("360 Get Friend Success");
			bool isOk = parseUserFriendResult(jsonData);
			Debug.Log("Parse 360 Friend. Status = " + isOk.ToString());
			if (isOk)
			{
				string cacheFile = UserCloud.GetPath(FILE_FRIEND_CACHE);
				saveTextToFile(cacheFile, jsonData);
			}
		}
		else 
		{
			Debug.Log("360 Get Friend Fail");
			
			string cacheFile = UserCloud.GetPath(FILE_FRIEND_CACHE);
			string cachedJsonData = loadTextFromFile(cacheFile);
			bool isOK = parseUserFriendResult(cachedJsonData);
			Debug.Log("Try load 360 Friend from cache. Status = " + isOK.ToString());
		}
	}
	
	public bool parseLoginResult(string jsonData)
	{
		Dictionary<string, object> rootDict = jsonData.dictionaryFromJson();
		if (rootDict == null || !rootDict.ContainsKey("data"))
		{
			return false;
		}
		
		Dictionary<string, object> dataDict = rootDict["data"] as Dictionary<string, object>;
		if (dataDict == null || !dataDict.ContainsKey("user_login_res"))
		{
			return false;
		}
		
		Dictionary<string, object> resDict = dataDict["user_login_res"] as Dictionary<string, object>;
		if (resDict == null || !resDict.ContainsKey("data"))
		{
			return false;
		}
		
		Dictionary<string, object> userDataDict = resDict["data"] as Dictionary<string, object>;
		if (userDataDict == null || !userDataDict.ContainsKey("qid") || !userDataDict.ContainsKey("nick"))
		{
			return false;
		}
		
		UserID = Convert.ToString(userDataDict["qid"]);
		UserName = Convert.ToString(userDataDict["nick"]);
		Debug.Log("360 ID = " + UserID.ToString() + ", 360 Name = " + UserName.ToString());
		return true;
	}
	
	public bool parseUserFriendResult(string jsonData)
	{
		Dictionary<string, object> rootDict = jsonData.dictionaryFromJson();
		if (rootDict == null || !rootDict.ContainsKey("data"))
		{
			return false;
		}
		
		List<object> FriendDataList = rootDict["data"] as List<object>;
		FriendList.Clear();
		for (int i = 0; i < FriendDataList.Count; i++)
		{
			Dictionary<string, object> oneFriendData = FriendDataList[i] as Dictionary<string, object>;
			if (oneFriendData == null || !oneFriendData.ContainsKey("nick") || !oneFriendData.ContainsKey("qid"))
			{
				continue;
			}
			
			string nickName = Convert.ToString(oneFriendData["nick"]);
			string id = Convert.ToString(oneFriendData["qid"]);
			
			FriendList.Add(new QihooFriendData(id, nickName));
		}
		
		for (int i = 0; i < FriendList.Count; i++)
		{
			Debug.Log("Friend " + (i + 1).ToString() + ": " + FriendList[i].ToString());
		}
		
		return true;
	}
}
