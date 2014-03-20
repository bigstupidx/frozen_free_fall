using UnityEngine;
using System.Collections;

public class ItemCellButton : MonoBehaviour {
	
	private PackageTable packageTableCom;
	
	// Use this for initialization
	void Start () {
		packageTableCom = GameObject.Find("PackageTable").GetComponent<PackageTable>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		/*
		GameObject mallTableObj = GameObject.Find("MallDragPanel");
		//MallInitialization tableCom = mallTableObj.GetComponent<MallInitialization>();
		//tableCom.addDiamond("com.mfp.frozen.cash_30");
		mallTableObj.SendMessage("addDiamond", "com.mfp.frozen.cash_30");
		
		return;
		*/
		
		GameObject diamondCellObj = transform.parent.gameObject;
		DiamondCell diamondCellCom = diamondCellObj.GetComponent<DiamondCell>();
		int productID = diamondCellCom.diamondProductID;
		
		Debug.Log("Press item cell" + productID);
		
		bool purchaseSuccess = ItemModel.Instance.buyItem(productID);
		if (purchaseSuccess)
		{
			Debug.Log("Purchase Success!");
			PopupMessage.Show(Language.Get("PURCHASE_ITEM_SUCCESS"));
			
			packageTableCom.UpdateContent();
		}
		else 
		{
			Debug.Log("Not enough diamond. Then jump to mall tab.");
			PopupMessage.Show(Language.Get("NOT_ENOUGH_DIAMOND"));
			MallTab mallTab = GameObject.Find("MallTabButton2").gameObject.GetComponent<MallTab>();
			mallTab.OnClick();
		}
	}
}
