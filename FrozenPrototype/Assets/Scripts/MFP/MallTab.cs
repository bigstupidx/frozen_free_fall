using UnityEngine;
using System.Collections;

public class MallTab : MonoBehaviour {

	public string relatedTableName = "ItemTable";
	
	public int tabIndex = 0;
	
//	private GameObject tabSprite;
	private UISprite tabSprite;
	
	private GameObject diamondObj;
	private GameObject itemObj;
	private GameObject fragmentObj;
	private GameObject packageObj;
	
	private GameObject mallDragPanelObj;
	
	// Use this for initialization
	void Start () {
		mallDragPanelObj = GameObject.Find("MallDragPanel");
		diamondObj = GameObject.Find("DiamondTable");
		itemObj = GameObject.Find("ItemTable");
		fragmentObj = GameObject.Find("FragmentTable");
		packageObj = GameObject.Find("PackageTable");
		
		GameObject tabSpriteObject = GameObject.Find("TabSprite");
		tabSprite = tabSpriteObject.GetComponent<UISprite>();
		
		string[] tabKeys = new string[] { "MFP_MALL_TAB_ITEM", "MFP_MALL_TAB_DIAMOND",/*"MFP_MALL_TAB_FRAGMENT",*/ "MFP_MALL_TAB_PACKAGE"};
		if (tabIndex < 0 || tabIndex >= tabKeys.Length)
		{
			return;
		}
		
		GameObject labelObj = transform.Find("Label").gameObject;
		UILabel labelCom = labelObj.GetComponent<UILabel>();
		labelCom.text = Language.Get(tabKeys[tabIndex]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick()
	{
		// Back to initial state. The tableview's offset is stored in Panel object 
		mallDragPanelObj.transform.localPosition = new Vector3(-150, 0, 0);
		UIPanel dragPanelCom = mallDragPanelObj.GetComponent<UIPanel>();
		dragPanelCom.clipRange = new Vector4(150, 0, 540, 420);
		
		GameObject[] tableObjects = new GameObject[] {itemObj, diamondObj,/*fragmentObj,*/ packageObj};
		string[] tableNames = new string[] {"ItemTable", "DiamondTable", /*"FragmentTable",*/ "PackageTable"};
		for (int i = 0; i < tableNames.Length; i++) 
		{
			/*
			GameObject table = GameObject.Find(tableNames[i]);
			if (table == null)
			{
				continue;
			}

			if (i == tabIndex)
			{
				table.transform.localPosition = new Vector3(0, 0, -10);
			}
			else
			{
				table.transform.localPosition = new Vector3(0, 5000, -10);
			}
			*/
			
			if (i == tabIndex)
			{
				
				tableObjects[i].transform.localPosition = new Vector3(0, 0, -10);
				tableObjects[i].SetActive(true);
			}
			else 
			{
				
				tableObjects[i].transform.localPosition = new Vector3(0, 5000, -10);
				tableObjects[i].SetActive(false);
			}
		}
		
		string[] tabSpriteNames = new string[] {"mfp_mall_tab1", "mfp_mall_tab2", "mfp_mall_tab3", "mfp_mall_tab4"};
		
		if (tabIndex < tabSpriteNames.Length)
		{
			tabSprite.spriteName = tabSpriteNames[tabIndex];
		}
	}
}
