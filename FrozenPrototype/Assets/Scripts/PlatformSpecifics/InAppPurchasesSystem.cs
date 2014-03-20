using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class InAppPurchasesSystem : MonoBehaviour
{
	public static bool distributionBuild = true; //TODO TALIN - set this to true when making distribution builds
	
	public static string locale = "en_US";
	
	protected static InAppPurchasesSystem instance;
	
	protected static string[] productIds = new string[] {
		
		"cash_30",
		"cash_65",
		"cash_180",
		"cash_600",
		"cash_30",
		"cash_65",
		"cash_180",
		"cash_600",
		"cash_30",
		"cash_65",
		"cash_180",
		"cash_600",
		"cash_30",
	};
	
	protected static string mobilityId = "com.mfp.frozen.";
	protected static string disneyId = "com.mfp.frozen.";
		
	public enum InAppPurchase {
		Lives = 0,
		IcePickSmallPack,
		IcePickMediumPack,
		IcePickLargePack,
		SnowballSmallPack,
		SnowballMediumPack,
		SnowballLargePack,
		HourglassSmallPack,
		HourglassMediumPack,
		HourglassLargePack,
		TokenSmallPack,
		TokenMediumPack,
		TokenLargePack,
	}
	
	public delegate void ProductPurchased(string id);
	
	public static event ProductPurchased OnPurchaseSuccess;
	public static event ProductPurchased OnPurchaseFail;
	public static event ProductPurchased OnPurchaseCancel;
	
	public bool showDialogs = true;
	public bool logAnalytics = true;
	
	protected bool receivedProductList = false;
	
	protected string prefix = disneyId;
	
	protected InAppPurchase purchasingProduct;
		
	public static InAppPurchasesSystem Instance {
		get {
			if (instance == null) {
				GameObject container = new GameObject("InAppPurchasesSystem");
				instance = container.AddComponent<InAppPurchasesSystem>();
				DontDestroyOnLoad(container);
			}
			
			return instance;
		}
	}
	
	protected virtual void Awake() 
	{
		if (instance != null)
		{
			Destroy(gameObject);
			return;
		}
		
		if (this.GetType() == typeof(InAppPurchasesSystem))
		{
#if UNITY_IPHONE
			InAppPurchasesSystemIOS newSystem = gameObject.AddComponent<InAppPurchasesSystemIOS>();
			newSystem.showDialogs = showDialogs;
			newSystem.logAnalytics = logAnalytics;
#elif UNITY_ANDROID
			InAppPurchasesSystemAndroid newSystem = gameObject.AddComponent<InAppPurchasesSystemAndroid>();
			newSystem.showDialogs = showDialogs;
			newSystem.logAnalytics = logAnalytics;
#endif
			
#if (UNITY_IPHONE || UNITY_ANDROID)
			Destroy(this);
			return;
#endif
		}
		
		instance = this;
		DontDestroyOnLoad(gameObject);
		
		if (distributionBuild) 
		{
			prefix = disneyId;
		}
		else {
			prefix = mobilityId;
		}
		
		Debug.LogWarning("In app purchases prefix: " + prefix);
		
		RequestProductList();
	}
		
	protected void RequestProductList()
	{
		if (receivedProductList) {
			return;
		}
		
		PlatformRequestProductList(GetProducts());
	}
	
	protected string[] GetProducts()
	{
		string[] products = new string[productIds.Length];
		for (int i = 0; i < products.Length; ++i) {
			products[i] = GetPurchaseId((InAppPurchase)i);
		}
		
		return products;
	}
	
	protected virtual void PlatformRequestProductList(string[] products)
	{
		receivedProductList = true;
	}
		
	public virtual string GetPurchaseId(InAppPurchase purchase)
	{
		return prefix + productIds[(int)purchase];
	}
	
	public virtual InAppProduct GetProduct(string productId)
	{
		return null;
	}
	
	public virtual void PurchaseProduct(InAppPurchase purchase)
	{
		purchasingProduct = purchase;
		
		if (OnPurchaseSuccess != null) {
			OnPurchaseSuccess(GetPurchaseId(purchasingProduct));
		}
	}
	
	protected void OnInAppDisabled(InAppPurchase purchase)
	{
		if (OnPurchaseFail != null) {
			OnPurchaseFail(GetPurchaseId(purchase));
		}
		
		ShowInAppDisabledWindow();
	}
	
	protected void OnProductAwaitingVerification()
	{
		Debug.Log("Purchase successful: " + productIds[(int)purchasingProduct]);
		ShowWaitingStoreWindow();
	}
	
	protected void OnVerificationSuccess()
	{
		Debug.Log("Purchase and verification successful: " + productIds[(int)purchasingProduct]);
		if (OnPurchaseSuccess != null) {
			OnPurchaseSuccess(GetPurchaseId(purchasingProduct));
		}
		
		ShowPurchasedWindow();
	}
	
	protected virtual void OnProductFailed(string error)
	{
		Debug.Log("Purchase failed: " + productIds[(int)purchasingProduct] + " Error: " + error);
		if (OnPurchaseFail != null) {
			OnPurchaseFail(GetPurchaseId(purchasingProduct));
		}
		
		ShowFailedPurchaseWindow();
	}
	
	protected virtual void OnProductCanceled(string error)
	{
		Debug.Log("Purchase canceled: " + productIds[(int)purchasingProduct] + " Error: " + error);
		if (OnPurchaseCancel != null) {
			OnPurchaseCancel(GetPurchaseId(purchasingProduct));
		}
	}
		
	protected void ShowWaitingStoreWindow()
	{
		if (showDialogs) {
			NativeMessagesSystem.Instance.ShowMessage(Language.Get("STORE_WAITING_TITLE"), 
				Language.Get("STORE_WAITING_TEXT"), "");
		}
	}
	
	protected void ShowInAppDisabledWindow()
	{
		if (showDialogs) {
			NativeMessagesSystem.Instance.ShowMessage(Language.Get("STORE_DISABLED_TITLE"), 
				Language.Get("STORE_DISABLED_TEXT"), Language.Get("BUTTON_OK"));
		}
	}
	
	protected void ShowPurchasedWindow()
	{
		if (showDialogs) {
			NativeMessagesSystem.Instance.ShowMessage(Language.Get("STORE_PURCHASED_TITLE"), 
				Language.Get("STORE_PURCHASED_TEXT"), Language.Get("BUTTON_OK"));
		}
	}
	
	protected void ShowFailedPurchaseWindow()
	{
		if (showDialogs) {
			NativeMessagesSystem.Instance.ShowMessage(Language.Get("STORE_FAILED_TITLE"), 
				Language.Get("STORE_FAILED_TEXT"), Language.Get("BUTTON_OK"));
		}
	}
}

public class InAppProduct 
{
	public string id;
	public string currencyCode;
	public string price;
}