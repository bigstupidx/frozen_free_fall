using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InAppPurchasesSystemIOS : InAppPurchasesSystem
{	
#if UNITY_IPHONE	
	protected List<StoreKitProduct> productList;
#endif
	
	protected override void Awake() 
	{
		base.Awake();
		
#if UNITY_IPHONE
		StoreKitUtilsBinding.Instance.OnProductLocaleReceived += ProductLocaleReceived;
		StoreKitUtilsBinding.Instance.RequestProductInformation(GetPurchaseId(InAppPurchase.Lives));
#endif
	}

	void ProductLocaleReceived (string _locale)
	{
		locale = _locale;
	}
		
	protected override void PlatformRequestProductList(string[] products)
	{
#if UNITY_IPHONE
		StoreKitManager.productListReceivedEvent -= OnProductListReceived;
		StoreKitManager.productListReceivedEvent += OnProductListReceived;
		
		StoreKitBinding.requestProductData(products);
#endif
	}
	
#if UNITY_IPHONE	
	protected void OnProductListReceived(List<StoreKitProduct> list)
	{
		StoreKitManager.productListReceivedEvent -= OnProductListReceived;
		
		if (list != null && list.Count > 0) 
		{
			Debug.Log("Product list received with " + list.Count + " items.");
			
			productList = list;
			receivedProductList = true;
			
			Debug.Log("Diamonds with " + ItemModel.Instance.diamondProducts.Count + " items.");
			for (int i = 0; i < ItemModel.Instance.diamondProducts.Count; i++)
			{
				string productID = ItemModel.Instance.diamondProducts[i].productID;
				foreach (StoreKitProduct product in productList) 
				{
					if (product.productIdentifier == productID) 
					{
						ItemModel.Instance.diamondProducts[i].UpdateLocalPrice(product.formattedPrice);
						break;
					}
				}
				/*if (product != null)
				{
					Debug.Log("update productID:" + ItemModel.Instance.diamondProducts[i].productID);
					ItemModel.Instance.diamondProducts[i].UpdateLocalPrice(product.currencyCode + " " + product.price);
				}*/
			}
		}
		
		
	}
	
	public override InAppProduct GetProduct(string productId)
	{
		if (productList == null) {
			return base.GetProduct(productId);
		}
		
		foreach (StoreKitProduct product in productList) 
		{
			if (product.productIdentifier == productId) 
			{
				InAppProduct myProduct = new InAppProduct();
				
				myProduct.id = productId;
				myProduct.price = product.price;
				myProduct.currencyCode = product.currencyCode;
				
				return myProduct;
			}
		}
		
		return base.GetProduct(productId);
	}
#endif
		
	public override void PurchaseProduct(InAppPurchase purchase)
	{
#if UNITY_EDITOR
		base.PurchaseProduct(purchase);
#elif UNITY_IPHONE
		if (StoreKitBinding.canMakePayments()) 
		{			
			purchasingProduct = purchase;
			
			RequestProductList();
			
			StoreKitManager.productPurchaseAwaitingConfirmationEvent += OnProductAwaitingVerification;
			StoreKitManager.purchaseFailedEvent += OnProductFailed;
			StoreKitManager.purchaseCancelledEvent += OnProductCanceled;
			
			Debug.Log("Purchasing product: " + productIds[(int)purchasingProduct]);
			StoreKitBinding.purchaseProduct(GetPurchaseId(purchase), 1);
		}
		else {
			OnInAppDisabled(purchase);
		}
#endif
	}
	
#if UNITY_IPHONE
	protected void OnProductAwaitingVerification(StoreKitTransaction transaction)
	{
		StoreKitManager.productPurchaseAwaitingConfirmationEvent -= OnProductAwaitingVerification;
		
		StoreKitManager.purchaseSuccessfulEvent += OnVerificationSuccess;
		
		OnProductAwaitingVerification();
	}
#endif		

#if UNITY_IPHONE	
	protected void OnVerificationSuccess(StoreKitTransaction transaction)
	{
		StoreKitManager.purchaseSuccessfulEvent -= OnVerificationSuccess;
		StoreKitManager.purchaseFailedEvent -= OnProductFailed;
		StoreKitManager.purchaseCancelledEvent -= OnProductCanceled;

		OnVerificationSuccess();
	}
#endif
	
	protected override void OnProductFailed(string error)
	{
#if UNITY_IPHONE
		StoreKitManager.productPurchaseAwaitingConfirmationEvent -= OnProductAwaitingVerification;
		StoreKitManager.purchaseSuccessfulEvent -= OnVerificationSuccess;
		StoreKitManager.purchaseFailedEvent -= OnProductFailed;
		StoreKitManager.purchaseCancelledEvent -= OnProductCanceled;
#endif
		
		base.OnProductFailed(error);
	}
	
	protected override void OnProductCanceled(string error)
	{
#if UNITY_IPHONE
		StoreKitManager.productPurchaseAwaitingConfirmationEvent -= OnProductAwaitingVerification;
		StoreKitManager.purchaseSuccessfulEvent -= OnVerificationSuccess;
		StoreKitManager.purchaseFailedEvent -= OnProductFailed;
		StoreKitManager.purchaseCancelledEvent -= OnProductCanceled;
#endif
		
		base.OnProductCanceled(error);
	}
}

