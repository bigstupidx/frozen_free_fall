using UnityEngine;
using System.Collections;

public class DiamondCellButton : MonoBehaviour {
	protected static bool purchasing = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/**
	 * diamondProductID  return 0,1,2,3
	 * paynum: 1,2,3,4 
	 *   1  2元  20钻
	 * 2  6元  65钻
	 * 3  10元 110钻
	 * 4  15元  170钻
	 */ 
	public void OnClick()
	{
		Transform parentTrans = transform.parent;
		GameObject diamondCellObj = parentTrans.gameObject;
		DiamondCell diamondCellCom = diamondCellObj.GetComponent<DiamondCell>();
		int diamondProductID = diamondCellCom.diamondProductID;
#if UNITY_ANDROID
        int paynum = diamondProductID + 1;
		MFPBillingAndroid.Instance.pay (paynum.ToString());
#elif UNITY_IPHONE
		if (purchasing) {
			return;
		}
		purchasing = true;
        DiamondProductData diamondProduct = ItemModel.Instance.diamondProducts[diamondProductID];
		Debug.Log("purchase ProductID:" + diamondProductID);

		InAppPurchasesSystem.OnPurchaseSuccess += OnPurchaseSuccess;
		InAppPurchasesSystem.OnPurchaseFail += OnPurchaseFail;
		InAppPurchasesSystem.OnPurchaseCancel += OnPurchaseFail;
		InAppPurchasesSystem.Instance.PurchaseProduct((InAppPurchasesSystem.InAppPurchase)diamondProductID);
#endif	
		Debug.Log("Click diamond " + diamondProductID);
	}
	
	void OnPurchaseFail (string id)
	{
		purchasing = false;
		
		InAppPurchasesSystem.OnPurchaseSuccess -= OnPurchaseSuccess;
		InAppPurchasesSystem.OnPurchaseFail -= OnPurchaseFail;
		InAppPurchasesSystem.OnPurchaseCancel -= OnPurchaseFail;
	}

	void OnPurchaseSuccess (string id)
	{
		purchasing = false;
		
		InAppPurchasesSystem.OnPurchaseSuccess -= OnPurchaseSuccess;
		InAppPurchasesSystem.OnPurchaseFail -= OnPurchaseFail;
		InAppPurchasesSystem.OnPurchaseCancel -= OnPurchaseFail;
		
		/*
		foreach (DiamondProductData diamondProduct in ItemModel.Instance.diamondProducts) 
		{
			if(diamondProduct.productID == id)
			{
				UserManagerCloud.Instance.CurrentUser.UserGoldCoins += diamondProduct.diamondNum;
				// Update user.data
				UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);
				
				Debug.Log("Purchase has success, diamond added!");
				return;
			}
		}
		Debug.LogError("Cannot find productID: " + id +" in productList.");
		*/
	}
	
	void OnDestroy()
	{
		InAppPurchasesSystem.OnPurchaseSuccess -= OnPurchaseSuccess;
		InAppPurchasesSystem.OnPurchaseFail -= OnPurchaseFail;
		InAppPurchasesSystem.OnPurchaseCancel -= OnPurchaseFail;
	}
	
}
