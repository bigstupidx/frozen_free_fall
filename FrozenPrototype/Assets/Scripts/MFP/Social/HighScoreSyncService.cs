using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;

using System.IO;

public class HighScoreSyncService : MonoBehaviour {
	
	void OutputLevelScores()
	{
		StreamWriter sw = new StreamWriter("/Users/JianYu/Desktop/score.txt");
		
		// Show scores
		for (int i = 1; i <= 105; i++)
		{
			string name = "/Users/JianYu/Desktop/360Frozen/FrozenPrototype/Assets/Resources/" + "Game/Levels/Level" + i.ToString() + ".prefab";
			string value1 = "";
			string value2 = "";
			string value3 = "";
			
			StreamReader srReadFile = new StreamReader(name);
			while (!srReadFile.EndOfStream)
            {
                string strReadLine = srReadFile.ReadLine(); 
                Console.WriteLine(strReadLine);
				
				if (strReadLine.Contains("targetScore:"))
				{
					string [] parts = strReadLine.Split(new char[] {':'});
					value1 = parts[1];
				}
				
				if (strReadLine.Contains("targetScore2Stars:"))
				{
					string [] parts = strReadLine.Split(new char[] {':'});
					value2 = parts[1];
				}
				
				if (strReadLine.Contains("targetScore3Stars:"))
				{
					string [] parts = strReadLine.Split(new char[] {':'});
					value3 = parts[1];
				}
            }
			srReadFile.Close();
			
			string score = value1 + " " + value2 + " " + value3;
			sw.WriteLine(score);			
		}
		sw.Close();
	}
	
	// Use this for initialization
	void Start () 
	{
		
		//int x = PlayerPrefs.GetInt("TUTORIAL_TORCH_ITEM", 0);
		//PlayerPrefs.SetInt("TUTORIAL_TORCH_ITEM", 1);
		// OutputLevelScores
		//PlayerPrefs.SetInt("key_360", 1);
		
		// record that user has already get to map level
		QihooSnsModel.Instance.goToMapLevelOnce = true;
		if (QihooSnsModel.Instance.Using360Login)
		{
			//UserSNSManager.Instance.showFloatWnd();
		}
		
		// [1] sync cached high scores
		HighScoreModel.Instance.Deserialize();	
		Dictionary<int, int> cachedHighScores = HighScoreModel.Instance._highScores;
		if (cachedHighScores != null && cachedHighScores.Count > 0)
		{
			Dictionary<string, object> data = new Dictionary<string, object> ();
			data["cmd"] = "saveData";
			data["deviceId"] = SystemInfo.deviceUniqueIdentifier;
			data["platformId"] = QihooSnsModel.Instance.UserID;
			data["levelScore"] = HighScoreModel.Instance._highScores;
		
			HttpRequestService.sendRequest(data, new HttpRequestService.RequestSuccessDelegate(onGetNoticesSuccess), null);
		}
		
		// [2] sync user friend scores
		Dictionary<string, object> data1 = new Dictionary<string, object> ();
		data1["cmd"] = "getFriend";
		data1["platformIds"] = QihooSnsModel.Instance.FriendListStr;
		//data1["platformIds"] ="599315361,29886669,384531921,630180399,291875798,291201194";
		//data1["platformIds"] = "622792586,384531921,291201194,291257815,371301281";
		//data1["platformIds"] = "612146777,291201194,404357426,317138868,291257815,401061065,622792586,599315361,371301281,29886669,626838463,271149288";
		long curTime = LivesSystem.TimeSeconds();
		if (data1["platformIds"] != "" && curTime - QihooSnsModel.Instance.LastGetFriendTime > 300)
		{
			// no secode get friend service in 5 minutes
			QihooSnsModel.Instance.LastGetFriendTime = LivesSystem.TimeSeconds();
			HttpRequestService.sendRequest(data1, new HttpRequestService.RequestSuccessDelegate(onGetFriendScoreSuccess), new HttpRequestService.RequestFailDelegate(onGetFriendScoreFailure));
		}
		else
		{
			UpdateFriendNameInMap();
		}
		
		
		/*
		if (QihooSnsModel.Instance.FriendListStr == "")
		{
			data1["platformIds"] = QihooSnsModel.Instance.FriendListStr + "1,2,3,4,5,6,7";
		}
		else
		{
			data1["platformIds"] = QihooSnsModel.Instance.FriendListStr + ",1,2,3,4,5,6,7";
		}
		*/
		
		// [3] sync current level with 360
		if (QihooSnsModel.Instance.Using360Login)
		{
			UserSNSManager.Instance.UploadData(QihooSnsModel.Instance.UserID, UserManagerCloud.Instance.CurrentUser.LastFinishedLvl.ToString());
		}
	}
	
	void onGetNoticesSuccess (string jsonData)
	{
		Debug.Log("Send High Score Cache Service Success");
		
		HighScoreModel.Instance.DeleteCache();
	}
	
	void onGetFriendScoreSuccess(string jsonData)
	{
		Debug.Log("onGetFriendScoreSuccess");
		
		Dictionary<string, object> dataDict = jsonData.dictionaryFromJson();
		if (dataDict == null || !dataDict.ContainsKey("data"))
		{
			return;
		}
			
		List<object> friendDataObjList = dataDict["data"] as List<object>;
		if (friendDataObjList == null)
		{
			return;
		}
		
		List<FriendData> friendDatas = new List<FriendData>();
		for (int i = 0; i < friendDataObjList.Count; i++)
		{
			Dictionary<string, object> oneData = friendDataObjList[i] as Dictionary<string, object>;
			
			FriendData oneFriendData = new FriendData();
			
			if (oneData.ContainsKey("userId"))
			{
				oneFriendData.userId = Convert.ToInt32(oneData["userId"]);
			}
			
			if (oneData.ContainsKey("deviceId"))
			{
				oneFriendData.deviceId = Convert.ToString(oneData["deviceId"]);
			}
			
			if (oneData.ContainsKey("platformId"))
			{
				oneFriendData.platformId = Convert.ToString(oneData["platformId"]);
				
				oneFriendData.userName = QihooSnsModel.Instance.getNameByPlatformID(oneFriendData.platformId);
			}
			
			if (oneData.ContainsKey("levelMap"))
			{
				int maxLevelIndex = 0;
				Dictionary<string, object> oneUserData = oneData["levelMap"] as Dictionary<string, object>;
				foreach (KeyValuePair<string, object> pair in oneUserData)
				{
					int level = int.Parse(pair.Key);
					int score = 0;
					
					maxLevelIndex = level > maxLevelIndex ? level : maxLevelIndex;
					
					Dictionary<string, object> oneLevelData = pair.Value as Dictionary<string, object>;
					if (oneLevelData.ContainsKey("score"))
					{
						score = Convert.ToInt32(oneLevelData["score"]);
					}
					
					oneFriendData.levelMap[level] = score;
					oneFriendData.maxLevelIndex = maxLevelIndex;
				}
			}
			
			friendDatas.Add(oneFriendData);
		}
		
		HighScoreModel.Instance.FriendDataList = friendDatas;
		
		UpdateFriendNameInMap();
	}
	
	void onGetFriendScoreFailure()
	{
		UpdateFriendNameInMap();
	}
	
	public void UpdateFriendNameInMap()
	{
		Dictionary<int, FriendData> dataDict = HighScoreModel.Instance.getLevelFriendDict();
		
		for (int i = 0; i < LoadLevelButton.maxLevels; i++)
		{
			if (!dataDict.ContainsKey(i + 1))
			{
				continue;
			}
	
			FriendData data = dataDict[i + 1];
			string buttonName = (i + 1).ToString() + " Level Button";
			if (i + 1 < 10)
			{
				buttonName = "0" + buttonName;
			}
			
			Debug.Log("Level = " + (i + 1).ToString());
			Debug.Log("buttonName = " + buttonName);
			
			GameObject buttonObj = GameObject.Find("Map Panel Portrait/Contents/" + buttonName);
			if (buttonObj != null)
			{
				/*
				GameObject boardObj = buttonObj.transform.Find("NameBoard").gameObject;
				boardObj.transform.localScale = new Vector3(155, 56, 1);
				
				GameObject labelObj = buttonObj.transform.Find("NameLabel").gameObject;
				labelObj.transform.localScale = new Vector3(24, 24, 1);
				
				UILabel labelCom = labelObj.GetComponent<UILabel>();
				labelCom.text = data.userName.ToString();
				*/
				//buttonObj.SendMessage("AddFriendNameLabel", 
				LoadLevelButton buttonCom= buttonObj.GetComponent<LoadLevelButton>();
				buttonCom.AddFriendNameLabel(data.userName.ToString());
			}
		}
		
	}
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
