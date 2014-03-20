using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ItemTable : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{		
		// Update content 
		List<ItemProductData> itemProducts = ItemModel.Instance.itemProducts;
		for (int i = 0; i < this.transform.childCount; i++)
		{
			ItemProductData productData = itemProducts[i];
			ItemData itemData = ItemModel.Instance.itemDataDict[productData.itemName];
			
			GameObject cellObj = transform.GetChild(i).gameObject;
			DiamondCell cellCom = cellObj.GetComponent<DiamondCell>();
			cellCom.diamondProductID = i;
			
			// item name 
			GameObject nameLabelObj = cellCom.transform.Find("NameLabel").gameObject;
			UILabel nameLabelCom = nameLabelObj.GetComponent<UILabel>();
			nameLabelCom.text = Language.Get(itemData.textKey); // itemData.name;
			
			// item sprite 
			GameObject itemSpriteObj = cellCom.transform.Find("ItemSprite").gameObject;
			UISprite itemSpriteCom = itemSpriteObj.GetComponent<UISprite>();
			itemSpriteCom.spriteName = itemData.spriteName;
			
			if (productData.itemName == ItemModel.MAGIC_POWER)
			{
				itemSpriteObj.transform.localScale = new Vector3(62, 100, 0);
			}
			
			// item introduction 
			GameObject introLabelObj = cellCom.transform.Find("IntroLabel").gameObject;
			UILabel introLabelCom = introLabelObj.GetComponent<UILabel>();
			introLabelCom.text = Language.Get(itemData.descKey);
			
			// item number 
			GameObject itemNumObj = cellCom.transform.Find("ItemNumLabel").gameObject;
			UILabel itemNumCom = itemNumObj.GetComponent<UILabel>();
			itemNumCom.text = productData.itemNum.ToString();
			itemNumCom.transform.localScale = new Vector3(40, 40, 0);
			itemNumCom.transform.localPosition = new Vector3(40, -30, 0);
			
			// diamond number 
			GameObject diamondNumObj = cellCom.transform.Find("ConfirmButton/NumLabel").gameObject;
			UILabel diamondNumCom = diamondNumObj.GetComponent<UILabel>();
			diamondNumCom.text = productData.diamondNum.ToString();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void UpdateContent()
	{
	}
}
