using UnityEngine;
using System.Collections;

public class BuyItemHolder : MonoBehaviour
{
	public delegate void SelectedItemChanged();
	
	public static event SelectedItemChanged OnSelectedItemChanged;
	
	protected static ItemHolder item;
	protected static bool purchasing = false;
	protected static bool endGame = false;
	
	public UISprite icon;
	public UISprite iconBg;
	public UISprite buttonBg;
	public UILabel priceLabel;
	public UILabel packLabel;
	public UILabel itemCountLabel;
	public UILabel itemDescriptionLabel;
	public UILabel titleLabel;
	public int packIndex;
	
	public PlayMakerFSM buyFSM;
	public PlayMakerFSM mallFSM;
	
	protected int itemCount;
	protected int freeItemCount;
	protected InAppPurchasesSystem.InAppPurchase purchaseId;
	
	public static void SetSelectedItem(ItemHolder newItem, bool _endGame = false)
	{
		item = newItem;
		endGame = _endGame;
		
		if (OnSelectedItemChanged != null) {
			OnSelectedItemChanged();
		}
	}
	
	void Start()
	{
		OnSelectedItemChanged += UpdateStats;
	}
	
	void ChangeScaleX(Transform xForm, float newX)
	{
		Vector3 newScale = xForm.localScale;
		newScale.x = newX;
		xForm.localScale = newScale;
	}
	
	void UpdateStats()
	{
		BasicItem itemComponent = item.itemPrefab.GetComponent<BasicItem>();
		
		ItemProductData productData = ItemModel.Instance.getItemProductData(itemComponent.ItemName, packIndex);
		
		icon.enabled = true;
		iconBg.enabled = true;
		icon.spriteName = itemComponent.iconName;
		icon.transform.localScale = new Vector3(100, 100, 0);
		ChangeScaleX(buttonBg.transform, 352f);
				
		if (packIndex == 0) {
			itemDescriptionLabel.text = Language.Get(itemComponent.NameSingular.Replace(" ", "_").ToUpper() + "_DESCRIPTION");
			titleLabel.text = Language.Get("BUY_ITEMS");
		}
		
		if (itemComponent.GetType() == typeof(IcePick)) 
		{
			//itemCount = TweaksSystem.Instance.intValues["IcePickPack" + packIndex];
			itemCount = productData.itemNum;
			freeItemCount = 0;
		}
		else if (itemComponent.GetType() == typeof(Snowball)) 
		{
//			itemCount = TweaksSystem.Instance.intValues["SnowballPack" + packIndex];
//			freeItemCount = endGame ? 0 : TweaksSystem.Instance.intValues["SnowballFreePack" + packIndex];
			itemCount = productData.itemNum;
			freeItemCount = 0;
		}
		else if (itemComponent.GetType() == typeof(Hourglass)) 
		{
//			itemCount = TweaksSystem.Instance.intValues["HourglassPack" + packIndex];
//			freeItemCount = endGame ? 0 : TweaksSystem.Instance.intValues["HourglassFreePack" + packIndex];
			itemCount = productData.itemNum;
			freeItemCount = 0;
		}
		else {
			//itemCount = TweaksSystem.Instance.intValues["ItemsPack" + packIndex];
			itemCount = productData.itemNum;
			freeItemCount = 0;
			
			//icon.enabled = false;
			//iconBg.enabled = false;
			icon.spriteName = "magic_potion";
			icon.transform.localScale = new Vector3(54, 86, 0);
			
			ChangeScaleX(buttonBg.transform, 310f);
			
			if (packIndex == 0) {
				itemDescriptionLabel.text = Language.Get("ITEM_TOKEN_DESCRIPTION");
				titleLabel.text = Language.Get("GET_MORE_TOKENS");
			}
		}
		
		itemCountLabel.text = (itemCount + freeItemCount).ToString();// + " " + ((itemCount == 1) ? itemComponent.NameSingular : itemComponent.NamePlural);
		packLabel.text = productData.diamondNum.ToString();	// packLabel actually is diamond number label
		
		if (itemComponent is Snowball) {
			purchaseId = (InAppPurchasesSystem.InAppPurchase)((int)InAppPurchasesSystem.InAppPurchase.SnowballSmallPack + packIndex);
		}
		else if (itemComponent is Hourglass) {
			purchaseId = (InAppPurchasesSystem.InAppPurchase)((int)InAppPurchasesSystem.InAppPurchase.HourglassSmallPack + packIndex);
		}
		else if (itemComponent is IcePick) {
			purchaseId = (InAppPurchasesSystem.InAppPurchase)((int)InAppPurchasesSystem.InAppPurchase.IcePickSmallPack + packIndex);
		}
		else {
			purchaseId = (InAppPurchasesSystem.InAppPurchase)((int)InAppPurchasesSystem.InAppPurchase.TokenSmallPack + packIndex);
		}
	}
	
	void OnClick()
	{		
		if (purchasing || buyFSM.ActiveStateName != "In") {
			return;
		}
		
		//purchasing = true;
		
		BasicItem itemComponent = item.itemPrefab.GetComponent<BasicItem>();
		ItemProductData productData = ItemModel.Instance.getItemProductData(itemComponent.ItemName, packIndex);
		
		bool purchaseSuccess = ItemModel.Instance.buyItem(productData);
		if (purchaseSuccess)
		{
			Debug.Log("Purchase success");
			item.AddItems(itemCount + freeItemCount);
			buyFSM.SendEvent("BuyFinished");
		}
		else
		{
			Debug.Log("Not enough diamond");
		
			
			// Popup mall window, with "mall" tab enabled.
			MallTab mallTab = GameObject.Find("MallTabButton2").gameObject.GetComponent<MallTab>();
			mallTab.OnClick();
			mallFSM.SendEvent("Show");
			
		}
		
		/*
		InAppPurchasesSystem.OnPurchaseSuccess += OnPurchaseSuccess;
		InAppPurchasesSystem.OnPurchaseFail += OnPurchaseFail;
		InAppPurchasesSystem.OnPurchaseCancel += OnPurchaseFail;
		InAppPurchasesSystem.Instance.PurchaseProduct(purchaseId);
		*/
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
		
		InAppProduct product = InAppPurchasesSystem.Instance.GetProduct(id);
		if (product != null) {
			BasicItem itemComponent = item.itemPrefab.GetComponent<BasicItem>();
			
			string subtype = "small";
			if (packIndex == 1) {
				subtype = "medium";
			}
			else if (packIndex == 2) {
				subtype = "large";
			}
			
			#if UNITY_ANDROID
			AnalyticsBinding.LogEventPaymentAction(product.currencyCode, InAppPurchasesSystem.locale, "-" + product.price, id, 1, itemComponent.ItemName, 
				"consumable", endGame ? "postgame" : "ingame", Match3BoardRenderer.levelIdx);
			
			#else
			float price;
			if (!float.TryParse(product.price , out price)) {
				price = 0.99f + packIndex * 1f;
			}
			
			AnalyticsBinding.LogEventPaymentAction(product.currencyCode, InAppPurchasesSystem.locale, -price, id, 1, itemComponent.ItemName, 
				"consumable", endGame ? "postgame" : "ingame", Match3BoardRenderer.levelIdx);
			#endif
		}
		
		item.AddItems(itemCount + freeItemCount);
		buyFSM.SendEvent("BuyFinished");
	}
	
	void OnDestroy()
	{
		OnSelectedItemChanged -= UpdateStats;
		
		InAppPurchasesSystem.OnPurchaseSuccess -= OnPurchaseSuccess;
		InAppPurchasesSystem.OnPurchaseFail -= OnPurchaseFail;
		InAppPurchasesSystem.OnPurchaseCancel -= OnPurchaseFail;
	}
}

