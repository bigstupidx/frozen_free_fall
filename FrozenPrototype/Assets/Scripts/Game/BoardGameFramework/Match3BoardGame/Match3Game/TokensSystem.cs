using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Security.Cryptography;

public class TokensSystem : MonoBehaviour
{
	protected static TokensSystem instance;
	
	public static TokensSystem Instance {
		get {
			if (instance == null) {
				instance = (new GameObject("TokensSystem")).AddComponent<TokensSystem>();
			}
			
			return instance;
		}
	}
	
	public int icePicks = 0;
	public int snowballs = 0;
	public int hourglasses = 0;
	public int itemTokens = 0;
	
	protected string saveFile;
	
	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
		
		saveFile = Application.persistentDataPath + "/Local.tmp";
		
		LoadItems();
	}
	
	public void SaveItems()
	{
		MemoryStream ms = null;
		BinaryWriter writer = null;
//		CryptoStream encStream = null;
//		FileStream file = null;
		
		try 
		{
			ms = new MemoryStream();
			
//			DESCryptoServiceProvider mDES = new DESCryptoServiceProvider();
//			mDES.Mode = CipherMode.ECB;
//			mDES.Key = System.Text.Encoding.UTF8.GetBytes(TweaksSystem.CRYPTO_KEY);
//			
//			CryptoStream encStream = new CryptoStream(ms, mDES.CreateEncryptor(), CryptoStreamMode.Write);
			
//			BinaryWriter writer = new BinaryWriter(encStream);
			writer = new BinaryWriter(ms);
			
			if (writer != null) {
				writer.Write(UserCloud.USER_DATA_VERSION);
				writer.Write(icePicks);
				writer.Write(snowballs);
				writer.Write(hourglasses);
				writer.Write(itemTokens);
				
				writer.Close();
			}
			
//			encStream.Close();
			
			ms.Close();
			File.WriteAllBytes(saveFile, ms.ToArray());
		}
		catch (Exception ex)
		{
			Debug.Log("Error in create or save user file data. Exception: " + ex.Message);
		}
		finally 
		{
			if (writer != null) {
				writer.Close();
			}
			if (ms != null) {
				ms.Close();
			}
		}
	}
	
	void LoadItems()
	{
		Stream readStream = null;
		BinaryReader reader = null;
		
		try 
		{
			if (File.Exists(saveFile)) {
				readStream = File.OpenRead(saveFile);
			}
			else {
				return;
			}
			
//			DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();
//			cryptic.Mode = CipherMode.ECB;
//			cryptic.Key = System.Text.Encoding.UTF8.GetBytes(TweaksSystem.CRYPTO_KEY);
//			
//			CryptoStream crStream = new CryptoStream(readStream, cryptic.CreateDecryptor(), CryptoStreamMode.Read);
//			
//			BinaryReader reader = new BinaryReader(crStream);
			reader = new BinaryReader(readStream);
			
			if (reader != null) 
			{
				reader.ReadSingle(); // test this in future updates
				icePicks = reader.ReadInt32();
				snowballs = reader.ReadInt32();
				hourglasses = reader.ReadInt32();
				itemTokens = reader.ReadInt32();
				
				reader.Close();
			}
					
//			crStream.Close();
			readStream.Close();
		} 
		catch (Exception ex)
		{
			Debug.Log("Error in loading user file data. Exception: " + ex.Message);
		}
		finally
		{
			if (reader != null) {
				reader.Close();
			}
			if (readStream != null) {
				readStream.Close();
			}
		}
	}
}

