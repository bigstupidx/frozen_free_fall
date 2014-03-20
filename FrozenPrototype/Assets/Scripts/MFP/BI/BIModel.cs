using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using System.IO;

public class BIOrderData
{
	public int price;
	public int diamondNum;
	public string purchase_time;
	public int provider = 0;
	
	public BIOrderData(int _price, int _diamondNum, string purchaseTime)
	{
		price = _price;
		diamondNum = _diamondNum;
		purchase_time = purchaseTime;
#if UNITY_ANDROID && !UNITY_EDITOR
		provider = MFPDeviceAndroid.Instance.getProvider();
#endif
	}
}

public class BIPurchaseData
{
	public string item_name;
	public int item_num;
	public int diamond_num;
	public string purchase_time;
	
	public BIPurchaseData(string itemName, int itemNum, int diamondNum, string purchaseTime)
	{
		item_name = itemName;
		item_num = itemNum;
		diamond_num = diamondNum;
		purchase_time = purchaseTime;
	}
}

public class BIConsumeData
{
	public string item_name;
	public int level;
	public string purchase_time;
	
	public BIConsumeData(string itemName, int _level, string purchaseTime)
	{
		item_name = itemName;
		level = _level;
		purchase_time = purchaseTime;
	}
}

public class BIScoreData
{
	public int level;
	public int score;
	public int retry_counter;
	
	public BIScoreData(int _level, int  _score, int _retry_counter)
	{
		level = _level;
		score = _score;
		retry_counter = _retry_counter;
	}
}


public class BIModel
{
	
	public static string ChallengeTimesKey = "BIChallengeTimes";
	public const float BI_DATA_VERSION = 1.0f;
	public const string ORDER_CACAH_DATA = "o.dat";//order.dat
	public const string PURCHASE_CACHE_DATA = "p.dat";//purchase.dat
	public const string CONSUME_CACHE_DATA = "c.dat";//consume.dat
	public const string SCORE_CACHE_DATA = "s.dat";//score.dat
	
	public List<BIOrderData> BIOrderDataList = new List<BIOrderData>();
	
	public List<BIPurchaseData> BIPurchaseDataList = new List<BIPurchaseData>();
	
	public List<BIConsumeData> BIConsumeDataList = new List<BIConsumeData>();
	
	public List<BIScoreData> BIScoreDataList = new List<BIScoreData>();
	
	protected static BIModel instance;
	
	public static BIModel Instance 
	{
		get 
		{
			if (instance == null) 
			{		
				instance = new BIModel();
			}
			return instance;
		}
	}
	
	public void addOrderData(int price, int diamondNum)
	{
		string time = System.DateTime.Now.ToString();
		BIOrderDataList.Add(new BIOrderData(price, diamondNum, time));
		SerializeOrder();
	}
	
	public void addPurchaseData(string itemName, int itemNum, int diamondNum)
	{
		string time = System.DateTime.Now.ToString();
		BIPurchaseDataList.Add(new BIPurchaseData(itemName, itemNum, diamondNum, time));
		SerializePurchase();
	}
	
	public void addConsumeData(string itemName, int level)
	{
		string time = System.DateTime.Now.ToString();
		BIConsumeDataList.Add(new BIConsumeData(itemName, level, time));
		SerializeConsume();
	}
	
	public void addScoreData(int level, int score, int retryTimes)
	{
		BIScoreDataList.Add(new BIScoreData(level, score, retryTimes));
		SerializeScore();
	}
	
	public void DeleteCache(string target)
	{
		string path = GetPath(target);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}
	
	public void Serialize()
	{
		if (BIOrderDataList.Count > 0)
			SerializeOrder();
		if (BIPurchaseDataList.Count > 0)
			SerializePurchase();
		if (BIConsumeDataList.Count > 0)
			SerializeConsume();
		if (BIScoreDataList.Count > 0)
			SerializeScore();
	}
	
	void SerializeOrder()
	{
		MemoryStream ms = null;
		BinaryWriter writeStream = null;
		try
		{
			ms = new MemoryStream();
			using (writeStream = new BinaryWriter(ms))
			{
				writeStream.Write(BI_DATA_VERSION);
				writeStream.Write(BIOrderDataList.Count);
				foreach (BIOrderData data in BIOrderDataList)  
				{  
					writeStream.Write(data.price);
					writeStream.Write(data.diamondNum);
					writeStream.Write(data.purchase_time);
				}  
			}
			writeStream.Close();
			ms.Close();
			System.IO.File.WriteAllBytes(GetPath(ORDER_CACAH_DATA), ms.ToArray());
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Error in create or save user file data. Exception: " + ex.Message);
			Debug.LogWarning(ex.StackTrace);
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
	}
	
	void SerializePurchase()
	{
		MemoryStream ms = null;
		BinaryWriter writeStream = null;
		try
		{
			ms = new MemoryStream();
			using (writeStream = new BinaryWriter(ms))
			{
				writeStream.Write(BI_DATA_VERSION);
				writeStream.Write(BIPurchaseDataList.Count);
				foreach (BIPurchaseData data in BIPurchaseDataList)  
				{  
					writeStream.Write(data.item_name);
					writeStream.Write(data.item_num);
					writeStream.Write(data.diamond_num);
					writeStream.Write(data.purchase_time);
				}  
			}
			writeStream.Close();
			ms.Close();
			System.IO.File.WriteAllBytes(GetPath(PURCHASE_CACHE_DATA), ms.ToArray());
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Error in create or save user file data. Exception: " + ex.Message);
			Debug.LogWarning(ex.StackTrace);
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
	}
	
	void SerializeConsume()
	{
		MemoryStream ms = null;
		BinaryWriter writeStream = null;
		try
		{
			ms = new MemoryStream();
			using (writeStream = new BinaryWriter(ms))
			{
				writeStream.Write(BI_DATA_VERSION);
				writeStream.Write(BIConsumeDataList.Count);
				foreach (BIConsumeData data in BIConsumeDataList)  
				{  
					writeStream.Write(data.item_name);
					writeStream.Write(data.level);
					writeStream.Write(data.purchase_time);
				}  
			}
			writeStream.Close();
			ms.Close();
			System.IO.File.WriteAllBytes(GetPath(CONSUME_CACHE_DATA), ms.ToArray());
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Error in create or save user file data. Exception: " + ex.Message);
			Debug.LogWarning(ex.StackTrace);
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
	}
	
	void SerializeScore()
	{
		MemoryStream ms = null;
		BinaryWriter writeStream = null;
		try
		{
			ms = new MemoryStream();
			using (writeStream = new BinaryWriter(ms))
			{
				writeStream.Write(BI_DATA_VERSION);
				writeStream.Write(BIScoreDataList.Count);
				foreach (BIScoreData data in BIScoreDataList)  
				{  
					writeStream.Write(data.level);
					writeStream.Write(data.score);
					writeStream.Write(data.retry_counter);
				}  
			}
			writeStream.Close();
			ms.Close();
			System.IO.File.WriteAllBytes(GetPath(SCORE_CACHE_DATA), ms.ToArray());
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Error in create or save user file data. Exception: " + ex.Message);
			Debug.LogWarning(ex.StackTrace);
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
	}
	
	public void Deserialize()
	{
		BIOrderDataList.Clear();
		BIPurchaseDataList.Clear();
		BIConsumeDataList.Clear();
		BIScoreDataList.Clear();
		
		DeserializeOrder();
		DeserializePurchase();
		DeserializeConsume();
		DeserializeScore();
	}
	
	void DeserializeOrder()
	{
		Stream str = null;
		BinaryReader readStream = null;
		try
		{
			string path = GetPath(ORDER_CACAH_DATA);
			if (!File.Exists(path))
			{
				return;
			}	
			
			str = File.OpenRead(path);

			using (readStream = new BinaryReader(str))
			{
				float versionFile = readStream.ReadSingle();
				if (versionFile < BI_DATA_VERSION)
				{
					return;
				}
				
				int count = readStream.ReadInt32();
				for (int i = 0; i < count; i++)
				{
					int price = readStream.ReadInt32();
					int diamondNum = readStream.ReadInt32();
					string time = readStream.ReadString();
					BIOrderDataList.Add(new BIOrderData(price, diamondNum, time));
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
	
	void DeserializePurchase()
	{
		Stream str = null;
		BinaryReader readStream = null;
		try
		{
			string path = GetPath(PURCHASE_CACHE_DATA);
			if (!File.Exists(path))
			{
				return;
			}	
			
			str = File.OpenRead(path);

			using (readStream = new BinaryReader(str))
			{
				float versionFile = readStream.ReadSingle();
				if (versionFile < BI_DATA_VERSION)
				{
					return;
				}
				
				int count = readStream.ReadInt32();
				for (int i = 0; i < count; i++)
				{
					string itemName = readStream.ReadString();
					int itemNum = readStream.ReadInt32();
					int diamondNum = readStream.ReadInt32();
					string time = readStream.ReadString();
					BIPurchaseDataList.Add(new BIPurchaseData(itemName, itemNum, diamondNum, time));
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
	
	void DeserializeConsume()
	{
		Stream str = null;
		BinaryReader readStream = null;
		try
		{
			string path = GetPath(CONSUME_CACHE_DATA);
			if (!File.Exists(path))
			{
				return;
			}	
			
			str = File.OpenRead(path);

			using (readStream = new BinaryReader(str))
			{
				float versionFile = readStream.ReadSingle();
				if (versionFile < BI_DATA_VERSION)
				{
					return;
				}
				
				int count = readStream.ReadInt32();
				for (int i = 0; i < count; i++)
				{
					string itemName = readStream.ReadString();
					int level = readStream.ReadInt32();
					string time = readStream.ReadString();
					BIConsumeDataList.Add(new BIConsumeData(itemName, level, time));
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
	
	void DeserializeScore()
	{
		Stream str = null;
		BinaryReader readStream = null;
		try
		{
			string path = GetPath(SCORE_CACHE_DATA);
			if (!File.Exists(path))
			{
				return;
			}	
			
			str = File.OpenRead(path);

			using (readStream = new BinaryReader(str))
			{
				float versionFile = readStream.ReadSingle();
				if (versionFile < BI_DATA_VERSION)
				{
					return;
				}
				
				int count = readStream.ReadInt32();
				for (int i = 0; i < count; i++)
				{
					int level = readStream.ReadInt32();
					int score = readStream.ReadInt32();
					int retryTimes = readStream.ReadInt32();
					BIScoreDataList.Add(new BIScoreData(level, score, retryTimes));
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
	
	public static string GetPath(string objectId)
	{	
		string 	path = UserManagerCloud.DataPath + objectId;
		
		DirectoryInfo dir 	= new DirectoryInfo(UserManagerCloud.DataPath);
		
		if (!dir.Exists)
			dir.Create();
		
		return path;
	}
	
}