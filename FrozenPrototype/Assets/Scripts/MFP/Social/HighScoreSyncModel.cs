using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using System.IO;

public class FriendData
{
	public int userId;
	public string userName;
	public string deviceId;
	public string platformId;
	
	public int maxLevelIndex = 0;
	
	public Dictionary<int, int> levelMap = new Dictionary<int, int>();
};

public class FriendScoreData
{	
	public string platformID;
	public string name;
	public int score;
	public bool isMe;
	
	public FriendScoreData(string _platformID, string _name, int _score, bool _isMe)
	{
		platformID = _platformID;
		name = _name;
		score = _score;
		isMe = _isMe;
	}
}

public class Icp:IComparer<FriendScoreData>
{
    public int Compare(FriendScoreData x, FriendScoreData y)
    {
        return y.score.CompareTo(x.score);
    }
}

public class HighScoreModel 
{
	public const float HIGH_SCORE_DATA_VERSION = 1.0f;
	public const string HIGH_SCORE_CACAH_DATA = "HS.data";
	
	// user's score for each level
	public Dictionary<int, int> _highScores = new Dictionary<int, int>();	
	
	public List<FriendData> FriendDataList = new List<FriendData>();
	
	protected static HighScoreModel instance;
	
	public static HighScoreModel Instance 
	{
		get 
		{
			if (instance == null) 
			{		
				instance = new HighScoreModel();
			}
			return instance;
		}
	}
	
	public List<FriendScoreData> getFriendScoresForLevel(int level)
	{
		List<FriendScoreData> friendScoreList = new List<FriendScoreData>();
		
		string myName = QihooSnsModel.Instance.UserName;
		if (myName == null || myName == "")
		{
			myName = Language.Get("USER_DEFAULT_NAME");
		}
		string myPlatformId = QihooSnsModel.Instance.UserID;
		
		Dictionary<string, object> FinishedLevels = (Dictionary<string, object>)UserManagerCloud.Instance.CurrentUser.FinishedLevels;
		if (FinishedLevels.ContainsKey(level.ToString()))
		{
			Dictionary<string, object> levelInfo = FinishedLevels[level.ToString()] as Dictionary<string, object>;
			if (levelInfo.ContainsKey("score"))
			{
				int score = Convert.ToInt32(levelInfo["score"]);
				friendScoreList.Add(new FriendScoreData(myPlatformId, myName, score, true));
			}
		}	
		else
		{
			friendScoreList.Add(new FriendScoreData(myPlatformId, myName, 0, true));
		}
		
		for (int i = 0; i < FriendDataList.Count; i++)
		{
			FriendData friendData = FriendDataList[i];
			if (friendData.levelMap.ContainsKey(level))
			{
				string platformID = friendData.platformId;
				string name = QihooSnsModel.Instance.getNameByPlatformID(platformID);
				int friendScore = friendData.levelMap[level];
				
				friendScoreList.Add(new FriendScoreData(platformID, name, friendScore, false));
			}
		}
		
		friendScoreList.Sort(new Icp());
		
		return friendScoreList;
	}
	
	public Dictionary<int, FriendData> getLevelFriendDict()
	{
		Dictionary<int, FriendData> dict = new Dictionary<int, FriendData> ();
		
		for (int i = 0; i < FriendDataList.Count; i++)
		{
			FriendData data = FriendDataList[i];
			int maxLevel = data.maxLevelIndex + 1;
			dict[maxLevel] = data;
		}
		
		return dict;
	}
	
	
	
	public string getData()
	{
		return _highScores.toJson();
	}
	
	public void updateHighScoreCache(int level, int score)
	{
		_highScores[level] = score;
		
		Serialize();
	}
	
	public void DeleteCache()
	{
		string path = GetPath(HIGH_SCORE_CACAH_DATA);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}
	
	bool Serialize()
	{
		MemoryStream ms = null;
		BinaryWriter writeStream = null;
		
		try
		{
			ms = new MemoryStream();
			using (writeStream = new BinaryWriter(ms))
			{
				writeStream.Write(HIGH_SCORE_DATA_VERSION);
				writeStream.Write(_highScores.Count);
				foreach (KeyValuePair<int, int> pair in _highScores)  
				{  
					int level = pair.Key;
					int score = pair.Value;
					writeStream.Write(level);
					writeStream.Write(score);
				}  
			}
			
			writeStream.Close();
			ms.Close();
			
			System.IO.File.WriteAllBytes(GetPath(HIGH_SCORE_CACAH_DATA), ms.ToArray());
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Error in create or save user file data. Exception: " + ex.Message);
			Debug.LogWarning(ex.StackTrace);
			
			return false;
		}
		finally 
		{
			if (writeStream != null)
			{
				writeStream.Close();
			}
			
			if (ms != null)
			{
				ms.Close();
			}
		}
		
		return true;
	}
	
	public void Deserialize(byte[] data = null)
	{
		Stream str = null;
		BinaryReader readStream = null;
		
		try
		{
			string path = GetPath(HIGH_SCORE_CACAH_DATA);
			if (!File.Exists(path))
			{
				return;
			}	
			
			str = File.OpenRead(path);

			using (readStream = new BinaryReader(str))
			{
				float versionFile = readStream.ReadSingle();
				if (versionFile < HIGH_SCORE_DATA_VERSION)
				{
					return;
				}
				
				int count = readStream.ReadInt32();
				for (int i = 0; i < count; i++)
				{
					int level = readStream.ReadInt32();
					int score = readStream.ReadInt32();
					_highScores[level] = score;
				}
										
				readStream.Close();
			}
			
			str.Close();
		}
		catch (Exception e)
		{
			Debug.LogWarning(e.Message + "\nUser Data saved in disk was corrupted. One or more fields were added.");
			Debug.LogWarning(e.StackTrace);
				
			return;
		}
		finally 
		{
			if (readStream != null)
			{
				readStream.Close();
			}

			if (str != null)
			{
				str.Close();
			}
		}
	}
	
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static string GetPath(string objectId)
	{	
		string 	path = UserManagerCloud.DataPath + objectId;
		
		DirectoryInfo dir 	= new DirectoryInfo(UserManagerCloud.DataPath);
		
		if (!dir.Exists)
			dir.Create();
		
		return path;
	}
}
