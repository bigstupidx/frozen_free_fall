using UnityEngine;
using System.Collections;

public class MallTable : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{		
		for (int i = 0; i < transform.childCount; i++)
		{
			DiamondCell cellCom = transform.GetChild(i).gameObject.GetComponent<DiamondCell>();
			cellCom.diamondProductID = i;
			
			DiamondProductData diamondProduct = ItemModel.Instance.diamondProducts[i];
			
			// item name 
			GameObject nameLabelObj = cellCom.transform.Find("NameLabel").gameObject;
			UILabel nameLabelCom = nameLabelObj.GetComponent<UILabel>();
			nameLabelCom.text = Language.Get(diamondProduct.name + "_NAME"); // itemData.name;
			
			// item sprite 
			GameObject itemSpriteObj = cellCom.transform.Find("ItemSprite").gameObject;
			UISprite itemSpriteCom = itemSpriteObj.GetComponent<UISprite>();
			itemSpriteCom.spriteName = diamondProduct.name + "_PIC";
			
			// item introduction 
			GameObject introLabelObj = cellCom.transform.Find("IntroLabel").gameObject;
			UILabel introLabelCom = introLabelObj.GetComponent<UILabel>();
#if UNITY_IPHONE			
			introLabelCom.text = Language.Get(diamondProduct.name + "_DESC_IOS");
#else
			introLabelCom.text = Language.Get(diamondProduct.name + "_DESC");
#endif
			
			// RMB number 
			GameObject diamondNumObj = cellCom.transform.Find("ConfirmButton/NumLabel").gameObject;
			UILabel diamondNumCom = diamondNumObj.GetComponent<UILabel>();
			if (diamondProduct.displayPrice == "")
			{
			//	Debug.Log("productID: " + diamondProduct.productID + " has no localPrice");
				diamondNumCom.text = diamondProduct.price + Language.Get("CHINESE_YUAN");
			}
			else
			{
			//	Debug.Log("productID: " + diamondProduct.productID + " has localPrice: " + diamondProduct.displayPrice);
				diamondNumCom.text = diamondProduct.displayPrice;
			}
		}
		
	}
	
	public void UpdateContent()
	{
		Debug.Log("DiamondTable update content!");
		for (int i = 0; i < transform.childCount; i++)
		{
			DiamondCell cellCom = transform.GetChild(i).gameObject.GetComponent<DiamondCell>();
			cellCom.diamondProductID = i;
			
			DiamondProductData diamondProduct = ItemModel.Instance.diamondProducts[i];
			Debug.Log("productID:" + diamondProduct.productID);
			InAppProduct product = InAppPurchasesSystem.Instance.GetProduct(diamondProduct.productID);
			
			if (product != null)
			{
				// RMB number 
				Debug.Log("text: " + product.currencyCode.ToString() + " " + product.price.ToString());
				GameObject diamondNumObj = cellCom.transform.Find("ConfirmButton/NumLabel").gameObject;
				UILabel diamondNumCom = diamondNumObj.GetComponent<UILabel>();
				diamondNumCom.text = product.currencyCode.ToString() + " " + product.price.ToString();
			}
		}
	}
	

	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
