using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;


public class UserCloud
{
	// 3.0f - unicom(lian tong)
	public const float USER_DATA_VERSION = 3.1f; // 1.4f; Modified to 1.5 by JianYu
	private static UserCloud currentUser;
	public static UserCloud CurrentCloudUser;
	
	private const string DECODED_KEY = "19830321";

	
	#region Serialized Info
	
	/*
	private int _UserGoldCoins;
	public int UserGoldCoins {
		get { return _UserGoldCoins; }
		set { _UserGoldCoins = value; 
				UpdateAt = DateTime.UtcNow;
		}
	}
	*/
	
	private string _decodedGoldCoins;
	public int UserGoldCoins
	{
		get
		{
			return getIntValueFromDecodedString(_decodedGoldCoins);
		}
		set
		{
			_decodedGoldCoins = DesSecurity.DesEncrypt(value.ToString(), DECODED_KEY);
			UpdateAt = DateTime.UtcNow;
		}
	}
	
	private int _UserSilverCoins;
	public int UserSilverCoins {
		get { return _UserSilverCoins; }
		set { _UserSilverCoins = value; 
			UpdateAt = DateTime.UtcNow;		
		}
	}
	
	private string _decodedNumsLive;
	public int NumsLiveLeft
	{
		get 
		{ 
			return getIntValueFromDecodedString(_decodedNumsLive);
		}
		set 
		{ 
			_decodedNumsLive = DesSecurity.DesEncrypt(value.ToString(), DECODED_KEY);
			 UpdateAt = DateTime.UtcNow;
		}
	}
	
	/*
	public int _NumsLiveLeft;
	public int NumsLiveLeft
	{
		get { return _NumsLiveLeft; }
		set { _NumsLiveLeft = value;
			 UpdateAt = DateTime.UtcNow;
		}
	}
	*/
	
	public long _LivesTime;
	public long LivesTime
	{
		get { return _LivesTime; }
		set { _LivesTime = value;
			UpdateAt = DateTime.UtcNow;	
		}
	}
	
	public int _MaxLives;
	public int MaxLives
	{
		get { return _MaxLives; }
		set { _MaxLives = value; }
	}
	
	private Dictionary<string, object> _FinishedLevels;
	public Dictionary<string, object> FinishedLevels
	{
		get { return _FinishedLevels;}
		set { _FinishedLevels = value;
			UpdateAt = DateTime.UtcNow;
		}
	}
	
	private int _LastFinishedLvl;
	public int LastFinishedLvl
	{
		get { return _LastFinishedLvl; }
		set {
			_LastFinishedLvl = value;
			UpdateAt = DateTime.UtcNow;
		}
	}
	private DateTime _UpadateAt = DateTime.MinValue;
	public DateTime UpdateAt {
		get	{ return _UpadateAt; }
		set { _UpadateAt = DateTime.UtcNow; }
	}
	
	private string _decodedSnowBall;
	public int SnowBall
	{
		get 
		{ 
			return getIntValueFromDecodedString(_decodedSnowBall);
		}
		set 
		{ 
			_decodedSnowBall = DesSecurity.DesEncrypt(value.ToString(), DECODED_KEY);
			 UpdateAt = DateTime.UtcNow;
		}
	}
	
	private string _decodedIcePick;
	public int IcePick
	{
		get 
		{ 
			return getIntValueFromDecodedString(_decodedIcePick);
		}
		set 
		{ 
			_decodedIcePick = DesSecurity.DesEncrypt(value.ToString(), DECODED_KEY);
			 UpdateAt = DateTime.UtcNow;
		}
	}
	
	private string _decodedHourglass;
	public int Hourglass
	{
		get 
		{ 
			return getIntValueFromDecodedString(_decodedHourglass);
		}
		set 
		{ 
			_decodedHourglass = DesSecurity.DesEncrypt(value.ToString(), DECODED_KEY);
			 UpdateAt = DateTime.UtcNow;
		}
	}
	
	private string _decodedMagicPower;
	public int MagicPower
	{
		get 
		{ 
			return getIntValueFromDecodedString(_decodedMagicPower);
		}
		set 
		{ 
			_decodedMagicPower = DesSecurity.DesEncrypt(value.ToString(), DECODED_KEY);
			 UpdateAt = DateTime.UtcNow;
		}
	}
	
	int getIntValueFromDecodedString(string decodedString)
	{
		string s = DesSecurity.DesDecrypt(decodedString, DECODED_KEY);
		int num = 0;
		int.TryParse(s, out num);
		return num;
	}
	
	public string md5Data
	{
		get
		{
			// Create a new instance of the MD5CryptoServiceProvider object.
	        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
			
			string input = /*_UserGoldCoins + */ UserGoldCoins + "," + _UserSilverCoins + "," + NumsLiveLeft + "," + _LastFinishedLvl + "," + SnowBall+ "," + IcePick +","+Hourglass+","+MagicPower;
			
			// get UnqiueID related device!
#if UNITY_IPHONE
			input += "," + PlayerPrefs.GetString("VendorID");
#elif UNITY_ANDROID
			input += "," + MFPDeviceAndroid.Instance.getVendorID();
#endif
			input += "," + _MaxLives;
			// Convert the input string to a byte array and compute the hash.
	        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
	
	        // Create a new Stringbuilder to collect the bytes
	        // and create a string.
	        StringBuilder sBuilder = new StringBuilder();
	
	        // Loop through each byte of the hashed data 
	        // and format each one as a hexadecimal string.
	        for (int i = 0; i < data.Length; i++)
	        {
	            sBuilder.Append(data[i].ToString("x2"));
	        }
	
	        // Return the hexadecimal string.
	        return sBuilder.ToString();
		}
	}
	
	#endregion
	
	#region Extra Info
		
	public Texture2D Avatar;
	
	public static UserCloud CurrentUser
	{
		get
		{
			// Always return currentUser
			if (currentUser != null)
				return currentUser;
				
			// Firstime, load userdata from disk. If exists, user is not new.
			currentUser = Deserialize();
			
			// If not exist, create a new User.
			if (currentUser == null)
			{
				// New User -> Save it into userdata file
				currentUser = new UserCloud();
				Serialize(UserManagerCloud.FILE_NAME_LOCAL);			
			}
			return currentUser;
		}
		set {
			currentUser = value;
		}
	}
	
	#endregion
		
	#region Constructor
	
	public UserCloud() 
	{	
		ResetUserInfo(null);
	}
	
		
	#endregion
	
	#region Public Methods
	
	public void ResetUserInfo(string filename)
	{		
		LastFinishedLvl = 0;
		UserGoldCoins = 20;
		UserSilverCoins = 0;
		NumsLiveLeft = 5;
		MaxLives = 5;
		LivesTime = 0;
		FinishedLevels = new Dictionary<string, object>();
		Dictionary<string, object> levelFinishedInfo = new Dictionary<string, object>();
		levelFinishedInfo["stars"] = 0;
		levelFinishedInfo["score"] = 0;
		FinishedLevels["0"] = levelFinishedInfo;
		
		SnowBall = 0;
		Hourglass = 0;
		IcePick = 0;
		MagicPower = 0;
		
		UpdateAt = DateTime.MinValue;	
		
		Avatar = null;
		
		if ( !string.IsNullOrEmpty(filename) ) {
			Delete(filename);
			Serialize(UserManagerCloud.FILE_NAME_LOCAL);
		}
		LivesSystem.maxLives = MaxLives;
		
//		LastFinishedLvl = 75;
//		UserGoldCoins = 2010;
	}
	
	#endregion 
	
	#region Serialization
	
	public static string GetPath(string objectId)
	{	
		string 	path = UserManagerCloud.DataPath + objectId;
		
		DirectoryInfo dir 	= new DirectoryInfo(UserManagerCloud.DataPath);
		
		if (!dir.Exists)
			dir.Create();
		
		return path;
	}
	
	public static bool Serialize(string filename)
	{
		bool detailedLog = false;
		
		MemoryStream ms = null;
//		CryptoStream encStream = null;
		BinaryWriter writeStream = null;
//		FileStream file = null;
		
		try
		{
//			float timeStart = Time.realtimeSinceStartup;
					
			ms = new MemoryStream();
//			DESCryptoServiceProvider mDES = new DESCryptoServiceProvider();
//			mDES.Mode = CipherMode.ECB;
//			mDES.Key = System.Text.Encoding.UTF8.GetBytes(TweaksSystem.CRYPTO_KEY);
//			
//			encStream = new CryptoStream(ms, mDES.CreateEncryptor(), CryptoStreamMode.Write);
//			
//			using (writeStream = new BinaryWriter(encStream))
			using (writeStream = new BinaryWriter(ms))
			{
				if (detailedLog) 
				{
					Debug.Log("Serializing");
					Debug.Log("USER DATA VERSION " + USER_DATA_VERSION);
				}
				writeStream.Write(USER_DATA_VERSION);
				
				if (detailedLog) {
					Debug.Log("Gold coins " + CurrentUser.UserGoldCoins);
				}
				writeStream.Write(CurrentUser.UserGoldCoins);
				
				if (detailedLog) {
					Debug.Log("Silver coins " + CurrentUser.UserSilverCoins);
				}
				writeStream.Write(CurrentUser.UserSilverCoins);
				
				if (detailedLog) {
					Debug.Log("Lives left " + CurrentUser.NumsLiveLeft);
				}
				writeStream.Write(CurrentUser.NumsLiveLeft);
				
				if (detailedLog) {
					Debug.Log("Last finished level " + CurrentUser.LastFinishedLvl);
				}
				writeStream.Write(CurrentUser.LastFinishedLvl);
				
				if (detailedLog) {
					Debug.Log("Finished levels count " + CurrentUser.FinishedLevels.Count);
				}
				writeStream.Write(CurrentUser.FinishedLevels.Count);
				
				foreach (var level in CurrentUser.FinishedLevels)
				{
					if (detailedLog) {
						Debug.Log("Level key: " + level.Key);
					}
					writeStream.Write(level.Key);
					
					if (detailedLog) {
						Debug.Log("Level dict count: " + ((Dictionary<string, object>)level.Value).Count);
					}
					writeStream.Write(((Dictionary<string, object>)level.Value).Count);
					
					foreach (var levelInfo in ((Dictionary<string, object>)level.Value))
					{
						if (detailedLog) {
							Debug.Log("Level info key: " + levelInfo.Key);
						}
						writeStream.Write(levelInfo.Key);
					
						if (detailedLog) {
							Debug.Log("Level info value: " + Convert.ToInt32(levelInfo.Value));
						}
						writeStream.Write(Convert.ToInt32(levelInfo.Value));
					}
				}
				
				// [JianYu]: add codes for storing item number
				writeStream.Write(CurrentUser.SnowBall);
				writeStream.Write(CurrentUser.Hourglass);
				writeStream.Write(CurrentUser.IcePick);
				writeStream.Write(CurrentUser.MagicPower);
								
				if (detailedLog) {
					Debug.Log("Ticks: " + CurrentUser.UpdateAt.Ticks);
				}
				writeStream.Write(CurrentUser.UpdateAt.Ticks);
				writeStream.Write(CurrentUser.md5Data);
				writeStream.Write(CurrentUser.MaxLives);
				writeStream.Write(CurrentUser.LivesTime.ToString());
			}
//			encStream.Close();
			writeStream.Close();
			
			ms.Close();
			
			System.IO.File.WriteAllBytes(GetPath(filename), ms.ToArray());
		
			Debug.Log("[UserManagerCloud] Saved user data file locally: " + (new FileInfo(GetPath(filename))).Length);
			
			return true;

		}
		catch (Exception ex)
		{
			Debug.LogWarning("Error in create or save user file data. Exception: " + ex.Message);
			Debug.LogWarning(ex.StackTrace);
			
			return false;
		}
		finally 
		{
			if (detailedLog) {
				Debug.Log("[Serialize] Closing possible open streams...");
			}
			
			if (writeStream != null)
			{
				if (detailedLog) {
					Debug.Log("Closing write stream...");
				}
				writeStream.Close();
			}
			
//			if (encStream != null)
//			{
//				Debug.Log("Closing encrypt stream...");
//				encStream.Close();
//			}
			
			if (ms != null)
			{
				if (detailedLog) {
					Debug.Log("Closing memory stream...");
				}
				ms.Close();
			}
			
		}
	}
	
	public static UserCloud Deserialize(byte[] data = null)
	{
		bool detailedLog = true;
		
		Stream str = null;
//		CryptoStream crStream = null;
		BinaryReader readStream = null;
		
		try
		{
			UserCloud nUser = new UserCloud();
			
//			float timeStart = Time.realtimeSinceStartup;
					
			if (data == null)
			{
				string path = GetPath(UserManagerCloud.FILE_NAME_LOCAL);
				Debug.Log("Load user data from local file: " + path);
				
				if (!File.Exists(path))
					return null;
				str = File.OpenRead(path);
			}
			else
			{
				str = new MemoryStream(data);
				Debug.Log("Load user data from data! ");
			}


//			DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();
//			cryptic.Mode = CipherMode.ECB;
//			cryptic.Key = System.Text.Encoding.UTF8.GetBytes(TweaksSystem.CRYPTO_KEY);
//			
//			crStream = new CryptoStream(str, cryptic.CreateDecryptor(), CryptoStreamMode.Read);
//					
//			using (readStream = new BinaryReader(crStream))
			using (readStream = new BinaryReader(str))
			{
				float versionFile = readStream.ReadSingle();
				if (detailedLog) {
					Debug.Log("File version " + versionFile);
				}
				
				/*if (versionFile < USER_DATA_VERSION)
				{
					Debug.LogWarning("[Deserialize] Trying to load older user data file!");
					
					if (data != null)
					{
						return nUser;
					}
					return null;
				}*/
										
				nUser.UserGoldCoins = readStream.ReadInt32();
			//	nUser.UserGoldCoins = 1000;
				if (detailedLog) {
					Debug.Log("Gold coins " + nUser.UserGoldCoins);
				}
				
				nUser.UserSilverCoins = readStream.ReadInt32();
				if (detailedLog) {
					Debug.Log("Silver coins " + nUser.UserSilverCoins);
				}
				
				nUser.NumsLiveLeft = readStream.ReadInt32();
				if (detailedLog) {
					Debug.Log("Lives left " + nUser.NumsLiveLeft);
				}
				
				nUser.LastFinishedLvl = readStream.ReadInt32();
				if (detailedLog) {
					Debug.Log("Last finished level " + nUser.LastFinishedLvl);
				}
					
				Dictionary<string, object> levels = new Dictionary<string, object>();
				int countLevels = readStream.ReadInt32();
				if (detailedLog) {
					Debug.Log("Finished levels count " + countLevels);
				}
				
				for (int i = 0; i < countLevels; i++)
				{
					string keyLevel = readStream.ReadString();
					if (detailedLog) {
						Debug.Log("Level key: " + keyLevel);
					}
					
					Dictionary<string, object> levelInfo = new Dictionary<string, object>();
					int countLevelInfo = readStream.ReadInt32();
					if (detailedLog) {
						Debug.Log("Level dict count: " + countLevelInfo);
					}
					
					for (int j = 0; j < countLevelInfo; j++)
					{
						string keyLevelInfo = readStream.ReadString();
						if (detailedLog) {
							Debug.Log("Level info key: " + keyLevelInfo);
						}
						
						int valueLevelInfo = readStream.ReadInt32();
						if (detailedLog) {
							Debug.Log("Level info value: " + valueLevelInfo);
						}
						
						levelInfo.Add(keyLevelInfo, valueLevelInfo);
					}
					
					levels.Add(keyLevel, levelInfo);
				}
				nUser.FinishedLevels = levels;
				
				// [JianYu]: add codes for loading items
				nUser.SnowBall = readStream.ReadInt32();
				nUser.Hourglass = readStream.ReadInt32();
				nUser.IcePick = readStream.ReadInt32();
				nUser.MagicPower = readStream.ReadInt32();
				
				nUser.UpdateAt = new DateTime(readStream.ReadInt64());
				if (detailedLog) {
					Debug.Log("Ticks: " + nUser.UpdateAt.Ticks);
				}
				if (versionFile > 3.0f)
				{
					string md5Data = readStream.ReadString();
					// Create a StringComparer an compare the hashes.
			        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
					nUser.MaxLives = readStream.ReadInt32();
					nUser.LivesTime = long.Parse(readStream.ReadString());
			        if (0 != comparer.Compare(md5Data, nUser.md5Data) )
			        {
			            nUser = null;
			        }
					else
					{
						LivesSystem.maxLives = nUser.MaxLives;
					}
				}
				else
				{
					nUser.NumsLiveLeft = 5; //PlayerPrefs.GetInt("Lives", 5);
				}
				readStream.Close();
			}
			
			str.Close();
//			float durationTime = Time.realtimeSinceStartup - timeStart;
//			Debug.Log(" =>Ecrypt Deserialization Time: " + durationTime * 1000);
			
			return nUser;
			
		}
		catch (Exception e)
		{
			Debug.LogWarning(e.Message + "\nUser Data saved in disk was corrupted. One or more fields were added.");
			Debug.LogWarning(e.StackTrace);
				
			return null;
		}
		finally 
		{
			if (detailedLog) {
				Debug.Log("[Deserialize] Closing possible open streams...");
			}
			
			if (readStream != null)
			{
				if (detailedLog) {
					Debug.Log("Closing readStream stream...");
				}
				readStream.Close();
			}
			
//			if (crStream != null)
//			{
//				Debug.Log("Closing crStream stream...");
//				crStream.Close();
//			}
			
			if (str != null)
			{
				if (detailedLog) {
					Debug.Log("Closing str stream...");
				}
				str.Close();
			}
			
		}
	}
	
	private void Delete(string filename)
	{
		string path = GetPath(filename);
		if (File.Exists(path))
			File.Delete(path);
	}
	
	
	#endregion
		
}

