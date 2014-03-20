using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public class DesSecurity
{
	
	/// <summary>
	/// 加密原函数
	/// </summary>
	/// <param name="pToEncrypt"></param>
	/// <param name="sKey"></param>
	/// <returns></returns>
	static public string DesEncrypt(string pToEncrypt, string sKey)
	{
		
		DESCryptoServiceProvider des = new DESCryptoServiceProvider();
		byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
		des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
		des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
		MemoryStream ms = new MemoryStream();
		CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
		cs.Write(inputByteArray, 0, inputByteArray.Length);
		cs.FlushFinalBlock();
		StringBuilder ret = new StringBuilder();
		foreach (byte b in ms.ToArray())
		{
			ret.AppendFormat("{0:X2}", b);
		}
		ret.ToString();
		return ret.ToString();
	}
	
	/// <summary>
	/// 解密原函数
	/// </summary>
	/// <param name="pToDecrypt"></param>
	/// <param name="sKey"></param>
	/// <returns></returns>
	static public string DesDecrypt(string pToDecrypt, string sKey)
	{
		DESCryptoServiceProvider des = new DESCryptoServiceProvider();
		byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
		for (int x = 0; x < pToDecrypt.Length / 2; x++)
		{
			int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
			inputByteArray[x] = (byte)i;
		}
		des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
		des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
		MemoryStream ms = new MemoryStream();
		CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
		cs.Write(inputByteArray, 0, inputByteArray.Length);
		cs.FlushFinalBlock();
		StringBuilder ret = new StringBuilder();
		
		return System.Text.Encoding.UTF8.GetString(ms.ToArray());
	}
	
}  