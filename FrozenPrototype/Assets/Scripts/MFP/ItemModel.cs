using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiamondProductData
{
	public int id;
	public int price;
	public int diamondNum;
	public string name;
	public string productID;
	
	public string displayPrice;
	
	public DiamondProductData(int _id, int _price, int _diamondNum, string _name, string _productID)
	{
		id = _id;
		price = _price;
		diamondNum = _diamondNum;
		name = _name;
		productID = _productID;
		displayPrice = "";
	}
	
	public void UpdateLocalPrice(string _displayPrice)
	{
		Debug.Log("Update localPrice:" + _displayPrice);
		displayPrice =  _displayPrice;
	}
};

public struct ItemData
{
	public string name;
	public string spriteName;
	public string textKey;
	public string descKey;
	
	public ItemData(string _name, string _textKey, string _spriteName, string _descKey)
	{
		name = _name;
		textKey = _textKey;
		spriteName = _spriteName;
		descKey = _descKey;
	}
};

public struct ItemProductData
{
	public string itemName;
	public int itemNum;
	public int diamondNum;
	
	public ItemProductData( string _itemName, int _itemNum, int _diamondNum)
	{
		itemName = _itemName;
		itemNum = _itemNum;
		diamondNum = _diamondNum;
	}
};

public class ItemModel
{
	public static string ICE_PICK = "IcePick";
	public static string MAGIC_POWER = "MagicPower";
	public static string SNOW_BALL = "Snowball";
	public static string HOUR_GLASS = "Hourglass";
	
	public Dictionary<string, ItemData> itemDataDict = new Dictionary<string, ItemData>();
	public List<ItemProductData> itemProducts = new List<ItemProductData>();
	public List<DiamondProductData> diamondProducts = new List<DiamondProductData>();
	
	protected static ItemModel instance;
	
	public static ItemModel Instance {
		get 
		{
			if (instance == null) 
			{		
				
				Debug.Log("ItemModel instance initialization!!!");
				
				instance = new ItemModel();
				
				// Diamond product definitions
#if UNITY_IPHONE 
				instance.diamondProducts.Add(new DiamondProductData(1, 6, 30, "DIAMOND_PRODUCT_1", "com.mfp.frozen.cash_30"));
				instance.diamondProducts.Add(new DiamondProductData(1, 12, 65, "DIAMOND_PRODUCT_2", "com.mfp.frozen.cash_65"));
				instance.diamondProducts.Add(new DiamondProductData(1, 30, 180, "DIAMOND_PRODUCT_3", "com.mfp.frozen.cash_180"));
				instance.diamondProducts.Add(new DiamondProductData(1, 98, 600, "DIAMOND_PRODUCT_4", "com.mfp.frozen.cash_600"));
#elif UNITY_ANDROID
				instance.diamondProducts.Add(new DiamondProductData(1, 2, 20, "DIAMOND_PRODUCT_1", ""));
				instance.diamondProducts.Add(new DiamondProductData(1, 6, 65, "DIAMOND_PRODUCT_2", ""));
				instance.diamondProducts.Add(new DiamondProductData(1, 10, 110, "DIAMOND_PRODUCT_3", ""));
				instance.diamondProducts.Add(new DiamondProductData(1, 15, 170, "DIAMOND_PRODUCT_4", ""));
#else
				instance.diamondProducts.Add(new DiamondProductData(1, 2, 20, "DIAMOND_PRODUCT_1", ""));
				instance.diamondProducts.Add(new DiamondProductData(1, 6, 65, "DIAMOND_PRODUCT_2", ""));
				instance.diamondProducts.Add(new DiamondProductData(1, 10, 110, "DIAMOND_PRODUCT_3", ""));
				instance.diamondProducts.Add(new DiamondProductData(1, 15, 170, "DIAMOND_PRODUCT_4", ""));
#endif
				// Item definitions
				instance.itemDataDict.Add(ICE_PICK, new ItemData(ICE_PICK, "ICE_PICKS", "gui_powerup_icon1", "MFP_ICE_PICK_DESCRIPTION"));
				instance.itemDataDict.Add(MAGIC_POWER, new ItemData(MAGIC_POWER, "MFP_MAGIC_POTION", "magic_potion", "MFP_MAGIC_POTION_DESCRIPTION"));
				instance.itemDataDict.Add(SNOW_BALL, new ItemData(SNOW_BALL, "SNOWBALLS", "gui_powerup_icon2", "MFP_SNOWBALL_DESCRIPTION"));
				instance.itemDataDict.Add(HOUR_GLASS, new ItemData(HOUR_GLASS, "MFP_HOURGLASS", "gui_powerup_icon10", "MFP_HOURGLASS_DESCRIPTION"));
			
				// Item product definitions
				instance.itemProducts.Add(new ItemProductData(ICE_PICK, 2, 15));
				instance.itemProducts.Add(new ItemProductData(MAGIC_POWER, 2, 20));
				instance.itemProducts.Add(new ItemProductData(SNOW_BALL, 2, 20));
				instance.itemProducts.Add(new ItemProductData(HOUR_GLASS, 2, 20));

				instance.itemProducts.Add(new ItemProductData(ICE_PICK, 5, 32));
				instance.itemProducts.Add(new ItemProductData(MAGIC_POWER, 5, 45));
				instance.itemProducts.Add(new ItemProductData(SNOW_BALL, 5, 45));
				instance.itemProducts.Add(new ItemProductData(HOUR_GLASS, 5, 45));

				instance.itemProducts.Add(new ItemProductData(ICE_PICK, 10, 60));
				instance.itemProducts.Add(new ItemProductData(MAGIC_POWER, 10, 80));
				instance.itemProducts.Add(new ItemProductData(SNOW_BALL, 10, 80));
				instance.itemProducts.Add(new ItemProductData(HOUR_GLASS, 10, 80));
			}
			
			return instance;
		}
	}
	
	public bool buyItem(ItemProductData productData)
	{
		bool isOK = true;
		
		int userDiamondNum = UserManagerCloud.Instance.CurrentUser.UserGoldCoins;
		if (userDiamondNum >= productData.diamondNum)
		{
			UserManagerCloud.Instance.CurrentUser.UserGoldCoins -= productData.diamondNum;
			
			if (productData.itemName == ICE_PICK)
			{
				UserManagerCloud.Instance.CurrentUser.IcePick += productData.itemNum;
			}
			else if (productData.itemName == MAGIC_POWER)
			{
				UserManagerCloud.Instance.CurrentUser.MagicPower += productData.itemNum;
			}
			else if (productData.itemName == SNOW_BALL)
			{
				UserManagerCloud.Instance.CurrentUser.SnowBall += productData.itemNum;
			}
			else if (productData.itemName == HOUR_GLASS)
			{
				UserManagerCloud.Instance.CurrentUser.Hourglass += productData.itemNum;
			}
			
			// Update user.data
			UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);
			
			BIModel.Instance.addPurchaseData(productData.itemName, productData.itemNum, productData.diamondNum);
		}
		else
		{
			return false;
		}
		return true;
	}
	
	public bool buyItem(int productID)
	{
		ItemProductData productData = itemProducts[productID];
		return buyItem(productData);
	}
	
	public ItemProductData getItemProductData(string itemName, int index)
	{
		if (itemName != SNOW_BALL && itemName != HOUR_GLASS && itemName != ICE_PICK)
		{
			itemName = MAGIC_POWER;
		}
		
		List<ItemProductData> products = new List<ItemProductData>();
		
		for (int i = 0; i < itemProducts.Count; i++)
		{
			ItemProductData productData = itemProducts[i];
			if (productData.itemName == itemName)
			{
				products.Add(productData);
			}
		}
		
		return products[index];
	}
};
