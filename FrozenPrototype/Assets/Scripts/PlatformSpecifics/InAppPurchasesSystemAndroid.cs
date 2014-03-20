using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;
using System.Text.RegularExpressions;

public class InAppPurchasesSystemAndroid : InAppPurchasesSystem
{
	protected static string mobilityKey = 
		"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2j9qJwLzr2+YVNlrV9/iy" +
		"M0lWUDvUCfYx/VaJ5rBs70hz3gVhp/DYQmS4fcGWvsS2/k+k1Ga9vhWLTjdPP2+JV" +
		"WeQ44csxBzLVtxmCNKdb7/jRXJfdd1+eH2WzrxbzH0/Cd0VCQaiHldMyTB6i0e1c7" +
		"DvDfOX3gCi3RjhSdNiILhGSMMNpo7rTZ6kTmQFmzyk/gfAwn8uV0ciSLmDLGJo3uX" +
		"A8uqgjrpNNnPBYyPsjj+0TbqgbJ3Itd07lPOaQ51KqIr3LtHFLI59lcLzGfjDEl+t" +
		"t/jVJ8EiEDDEdue0GRll7d1IH6+2V+5RVS1wPLR2XQV3QzSvWIA8d97Y+MovwIDAQAB";
	
	protected static string disneyKey = 
		"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAii9xn0a6AqBV0ku7/p4q8" +
		"6PUEioqA34xB6xgGnIMmVwyCP1zjC4BKBs8aRVV2tCUz2NEhl1Rns01PcTZ9ipBBw" +
		"RHJwd5Z015m4ffI6eqYAyI39zhTdSy6C4rd1EeX9dsryYUESkKTBTeWmiqZe5ibd7" +
		"li+d4Py3u6oIUXBlktgaOfzJ2OIjl4UZtTtebTgg7Bm4FgOqxLr64n7n3oXfBUE7E" +
		"i9MW9Af/xyAwKQlc29Upjw+DZVNygwAi/MnBLWemoI8ut3uRLxdQ8PZHjJ3QlKvlh" +
		"Kgg8h6ujAqVnsSSYsmHwfp8puhbZH17fLGd2qq/r/lak3M2Ys3p/J5CqvHw0wIDAQAB";
	
	protected static string[] productExtraIds = new string[] {
		".2208941",
		".2208942",
		".2208943",
		".2208944",
		".2208945",
		".2208946",
		".2208947",
		".2208948",
		".2208949",
		".2208950",
		".2208951",
		".2208952",
		".2208953",
	};
	
	protected string key = disneyKey;
	
	protected bool billingSupported = false;
	
#if UNITY_ANDROID	
	protected List<GoogleSkuInfo> productList;
#endif
	
	protected Dictionary<string, string> currencies;
	
	protected override void Awake() 
	{
		if (distributionBuild) 
		{
			key = disneyKey;
		}
		else {
			key = mobilityKey;
		}
		
		currencies = new Dictionary<string, string>();
		currencies.Add("$", "USD");
		currencies.Add("Lek", "ALL");
//		currencies.Add("ƒ", "AWG");
//		currencies.Add("ман", "AZN");
		currencies.Add("KM", "BAM");
		currencies.Add("лв", "BGN");
		currencies.Add("лв.", "BGN");
		currencies.Add("$b", "BOB");
		currencies.Add("R$", "BRL");
		currencies.Add("BZ$", "BZD");
		currencies.Add("fr.", "CHF");
//		currencies.Add("¥", "JPY");
//		currencies.Add("₡", "CRC");
		currencies.Add("Din.", "CSD");
		currencies.Add("Kč", "CZK");
		currencies.Add("kr.", "DKK");
		currencies.Add("kr", "SEK");
		currencies.Add("RD$", "DOP");
		currencies.Add("€", "EUR");
		currencies.Add("£", "GBP");
		currencies.Add("Lari", "GEL");
		currencies.Add("Q", "GTQ");
		currencies.Add("HK$", "HKD");
		currencies.Add("L.", "HNL");
		currencies.Add("L", "HNL");
		currencies.Add("kn", "HRK");
		currencies.Add("Ft", "HUF");
		currencies.Add("Rp", "IDR");
		currencies.Add("J$", "JMD");
		//尽快尽快 
		currencies.Add("S", "KES");
//		currencies.Add("сом", "KGS");
//		currencies.Add("₩", "KRW");
//		currencies.Add("Т", "KZT");
//		Debug.Log(currencies["₭"]);
//		currencies.Add("₭", "LAK"); eur
		currencies.Add("Lt", "LTL");
		currencies.Add("Ls", "LVL");
		currencies.Add("ден.", "MKD");
//		currencies.Add("ден", "MKD"); kgs
//		currencies.Add("₮", "MNT"); eur
//		currencies.Add("₨", "MUR"); eur
		currencies.Add("RM", "MYR");
		//Debug.Log(currencies["N"]); //// the given key was not present in the dictionary
		//currencies.Add("N", "NIO");
		currencies.Add("C$", "NIO");
		currencies.Add("B/.", "PAB");
		currencies.Add("S/.", "PEN");
		currencies.Add("PhP", "PHP");
//		Debug.Log(currencies["₱"]);
//		currencies.Add("₱", "PHP");  eur
		currencies.Add("Rs", "PKR");
		currencies.Add("zł", "PLN");
		currencies.Add("Gs", "PYG");
		currencies.Add("lei", "RON");
//		Debug.Log(currencies["Дин."]);
//		currencies.Add("Дин.", "RSD"); mkd
		currencies.Add("р.", "RUB");
//		Debug.Log(currencies["руб"]);
		currencies.Add("руб", "RUB");
		currencies.Add("т.р.", "TJS");
		currencies.Add("m.", "TMT");
		currencies.Add("TL", "TRY");
		currencies.Add("TT$", "TTD");
		currencies.Add("NT$", "TWD");
//		Debug.Log(currencies["₴"]);
//		currencies.Add("₴", "UAH"); eur
		currencies.Add("$U", "UYU");
		currencies.Add("Bs.", "VEF");
		currencies.Add("Bs", "VEF");
//		Debug.Log(currencies["₫"]);
//		currencies.Add("₫", "VND"); eur
		currencies.Add("R", "ZAR");
		currencies.Add("Z$", "ZWL");
//		};
				
		base.Awake();
	}
	
	protected override void PlatformRequestProductList(string[] products)
	{
#if UNITY_ANDROID
		GoogleIABManager.billingSupportedEvent += OnBillingSupported;
		GoogleIABManager.billingNotSupportedEvent += OnBillingNotSupported;
		
		GoogleIAB.init(key);
#endif

	}
	
#if UNITY_ANDROID
	protected void OnBillingSupported()
	{
		GoogleIABManager.billingSupportedEvent -= OnBillingSupported;
		GoogleIABManager.billingNotSupportedEvent -= OnBillingNotSupported;
		
		billingSupported = true;
		
		QueryInventory();
	}
	
	protected void OnBillingNotSupported(string error)
	{
		GoogleIABManager.billingSupportedEvent -= OnBillingSupported;
		GoogleIABManager.billingNotSupportedEvent -= OnBillingNotSupported;
		
		billingSupported = false;
	}
	
	protected void QueryInventory()
	{
		GoogleIABManager.queryInventorySucceededEvent += OnQueryInventorySucceeded;
		GoogleIABManager.queryInventoryFailedEvent += OnQueryInventoryFailed;
		
		GoogleIAB.queryInventory(GetProducts());
	}
	
	protected void OnQueryInventorySucceeded(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		GoogleIABManager.queryInventorySucceededEvent -= OnQueryInventorySucceeded;
		GoogleIABManager.queryInventoryFailedEvent -= OnQueryInventoryFailed;
		
		if (skus != null && skus.Count > 0) 
		{
			Debug.Log("Product list received with " + skus.Count + " items.");
			
			productList = skus;
			receivedProductList = true;
		}
		
		if (purchases != null && purchases.Count > 0) 
		{
			foreach (GooglePurchase purchase in purchases) 
			{
				if (purchase.purchaseState == GooglePurchase.GooglePurchaseState.Purchased) {
					GoogleIAB.consumeProduct(purchase.productId);
				}
			}
		} 
	}
	
	protected void OnQueryInventoryFailed(string error)
	{
		GoogleIABManager.queryInventorySucceededEvent -= OnQueryInventorySucceeded;
		GoogleIABManager.queryInventoryFailedEvent -= OnQueryInventoryFailed;
	}
	
	public override InAppProduct GetProduct(string productId)
	{
		if (productList == null) {
			return base.GetProduct(productId);
		}
		
		foreach (GoogleSkuInfo product in productList) 
		{
			if (product.productId == productId) 
			{
				InAppProduct myProduct = new InAppProduct();
				
				myProduct.id = productId;

		        Regex regex = new Regex("(?<price>([0-9]*[.,]?[0-9]+)+)");
		        Match match = regex.Match(product.price);
		
				Debug.Log("Product price: " + product.price);
		        if (match.Success)
		        {
		            myProduct.price = match.Groups["price"].Value;
					myProduct.currencyCode = product.price.Replace(myProduct.price ,"").Trim();
					if (currencies.ContainsKey(myProduct.currencyCode)) {
						myProduct.currencyCode = currencies[myProduct.currencyCode];
					}
					else if (myProduct.currencyCode.Length != 3) {
						myProduct.currencyCode = "NUL"; //fallback
					}
					Debug.Log("Product currency code: " + myProduct.currencyCode);
		        }
				else {
					myProduct.price = product.price;
					myProduct.currencyCode = "NUL"; //fallback
				}
				
				return myProduct;
			}
		}
		
		return base.GetProduct(productId);
	}
#endif
	
	public override string GetPurchaseId(InAppPurchase purchase)
	{
		return prefix + productIds[(int)purchase] + productExtraIds[(int)purchase];
	}
	
	public override void PurchaseProduct(InAppPurchase purchase)
	{
#if UNITY_EDITOR
		base.PurchaseProduct(purchase);
#elif UNITY_ANDROID
		if (!billingSupported) 
		{
			RequestProductList(); //try to check again
			
			OnInAppDisabled(purchase);
		}
		else {
			purchasingProduct = purchase;
			
			RequestProductList();
			
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += OnProductAwaitingVerificationAndroid;
			GoogleIABManager.purchaseSucceededEvent += OnVerificationSuccess;
			GoogleIABManager.purchaseFailedEvent += OnProductFailed;

			Debug.Log("Purchasing product: " + GetPurchaseId(purchase));
			GoogleIAB.purchaseProduct(GetPurchaseId(purchase));
		}
#endif
	}

	protected void OnProductAwaitingVerificationAndroid(string purchaseData, string signature)
	{
#if UNITY_ANDROID
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= OnProductAwaitingVerificationAndroid;
#endif				
		OnProductAwaitingVerification();
	}


#if UNITY_ANDROID
	protected void OnVerificationSuccess(GooglePurchase purchase)
	{
#if UNITY_ANDROID
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= OnProductAwaitingVerificationAndroid;
		GoogleIABManager.purchaseSucceededEvent -= OnVerificationSuccess;
		GoogleIABManager.purchaseFailedEvent -= OnProductFailed;
		
		GoogleIAB.consumeProduct(purchase.productId);
#endif				

		OnVerificationSuccess();
	}
#endif
	
	protected override void OnProductFailed(string error)
	{
		if (error.ToLower().Contains("user canceled") || error.ToLower().Contains("user cancelled"))
		{
			OnProductCanceled(error);
			return;
		}
		
#if UNITY_ANDROID
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= OnProductAwaitingVerificationAndroid;
		GoogleIABManager.purchaseSucceededEvent -= OnVerificationSuccess;
		GoogleIABManager.purchaseFailedEvent -= OnProductFailed;
#endif
		
		base.OnProductFailed(error);
	}
	
	protected override void OnProductCanceled(string error)
	{
#if UNITY_ANDROID
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= OnProductAwaitingVerificationAndroid;
		GoogleIABManager.purchaseSucceededEvent -= OnVerificationSuccess;
		GoogleIABManager.purchaseFailedEvent -= OnProductFailed;
#endif
		
		base.OnProductCanceled(error);
	}
	
	protected void OnLevelWasLoaded()
	{
#if UNITY_ANDROID
		QueryInventory();
#endif
	}
}
