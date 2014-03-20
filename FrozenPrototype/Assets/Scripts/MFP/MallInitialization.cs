using UnityEngine;
using System.Collections;

public class MallInitialization : MonoBehaviour {
	
	public static int ITEM_INDEX = 0;
	public static int DIAMOND_INDEX = 0;
	public static int PACKAGE_INDEX = 0;
	
	// Use this for initialization
	void Start () {
		//setActiveTab(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/*
	public void setActiveTab(int index)
	{
		if (index == 0)
		{
			GameObject tabObj = transform.Find("TabButton1").gameObject;
			MallTab tabCom = tabObj.GetComponent<MallTab>();
			
			tabCom.OnClick();
		}
	}
	*/
	
	// tabIndex: 0, 1, 2
	public void setActiveTable(int tabIndex)
	{
		string[] tabButtonNames = {"MallTabButton1", "MallTabButton2", "MallTabButton3"};
		
		GameObject tabObj = transform.Find(tabButtonNames[tabIndex]).gameObject;
		MallTab tabCom = tabObj.GetComponent<MallTab>();
			
		tabCom.OnClick();
	}
	
	public void addDiamond(string productId)
	{
		foreach (DiamondProductData diamondProduct in ItemModel.Instance.diamondProducts) 
		{
			if(diamondProduct.productID == productId)
			{
				UserManagerCloud.Instance.CurrentUser.UserGoldCoins += diamondProduct.diamondNum;
				// Update user.data
				UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);
				Debug.Log("Purchase has success,"+ diamondProduct.diamondNum +" diamonds added!");
				BIModel.Instance.addOrderData(diamondProduct.price, diamondProduct.diamondNum);
			}
		}
	}
}
