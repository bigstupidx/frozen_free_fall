using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackageTable : MonoBehaviour {
	
	const int MIN_ITEM_ID = 1;
	const int MAX_ITEM_ID = 10;
	
	List<GameObject> packageCells;
	
	// Use this for initialization
	void Start () 
	{
		packageCells = new List<GameObject>();
		for (int i = 0; i < this.transform.childCount; i++)
		{			
			GameObject cellObj = transform.GetChild(i).gameObject;
			packageCells.Add(cellObj);
		}
		
		UpdateContent();
	}
	
	public void UpdateContent()
	{
		List<string> itemNames = new List<string>();
		List<int> itemNums = new List<int>();
		
		if (UserManagerCloud.Instance.CurrentUser.IcePick > 0)
		{
			itemNames.Add(ItemModel.ICE_PICK);
			itemNums.Add(UserManagerCloud.Instance.CurrentUser.IcePick);
		}
		if (UserManagerCloud.Instance.CurrentUser.MagicPower > 0)
		{
			itemNames.Add(ItemModel.MAGIC_POWER);
			itemNums.Add(UserManagerCloud.Instance.CurrentUser.MagicPower);
		}
		if (UserManagerCloud.Instance.CurrentUser.SnowBall > 0)
		{
			itemNames.Add(ItemModel.SNOW_BALL);
			itemNums.Add(UserManagerCloud.Instance.CurrentUser.SnowBall);
		}
		if (UserManagerCloud.Instance.CurrentUser.Hourglass > 0)
		{
			itemNames.Add(ItemModel.HOUR_GLASS);
			itemNums.Add(UserManagerCloud.Instance.CurrentUser.Hourglass);
		}
		
		for (int i = 0; i < itemNames.Count; i++)
		{
			string itenName = itemNames[i];
			int itemNum = itemNums[i];
			
			ItemData itemData = ItemModel.Instance.itemDataDict[itenName];
			
			GameObject cellObj = transform.GetChild(i).gameObject;
			DiamondCell cellCom = cellObj.GetComponent<DiamondCell>();
			
			// item name 
			GameObject nameLabelObj = cellCom.transform.Find("NameLabel").gameObject;
			UILabel nameLabelCom = nameLabelObj.GetComponent<UILabel>();
			nameLabelCom.text = Language.Get(itemData.textKey);
			
			// item sprite 
			GameObject itemSpriteObj = cellCom.transform.Find("ItemSprite").gameObject;
			UISprite itemSpriteCom = itemSpriteObj.GetComponent<UISprite>();
			itemSpriteCom.spriteName = itemData.spriteName;
			
			if (itemData.name == ItemModel.MAGIC_POWER)
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
			itemNumCom.text = itemNum.ToString();
			itemNumCom.transform.localScale = new Vector3(40, 40, 0);
			itemNumCom.transform.localPosition = new Vector3(40, -30, 0);
			
			packageCells[i].SetActive(true);
		}
		
		for (int i = itemNames.Count; i < packageCells.Count; i++)
		{
			packageCells[i].SetActive(false);
		}
		
		// Re-position
		UIGrid gridCom = this.GetComponent<UIGrid>();
		gridCom.Reposition();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
