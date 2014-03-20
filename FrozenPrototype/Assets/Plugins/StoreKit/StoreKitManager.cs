using UnityEngine;
using System;
using System.Collections.Generic;
using Prime31;
using System.Collections;


#if UNITY_IPHONE
public class StoreKitManager : AbstractManager
{
	public static bool autoConfirmTransactions = true;
	
	private static int tryTimes = 2;

	// Fired when the product list your required returns.  Automatically serializes the productString into StoreKitProduct's.
	public static event Action<List<StoreKitProduct>> productListReceivedEvent;

	// Fired when requesting product data fails
	public static event Action<string> productListRequestFailedEvent;

	// Fired when a product purchase has returned from Apple's servers and is awaiting completion. By default the plugin will finish transactions for you.
	// You can change that behaviour by setting autoConfirmTransactions to false which then requires that you call StoreKitBinding.finishPendingTransaction
	// to complete a purchase.
	public static event Action<StoreKitTransaction> productPurchaseAwaitingConfirmationEvent;

	// Fired when a product is successfully paid for. The event will provide a StoreKitTransaction object that holds the productIdentifer and receipt of the purchased product.
	public static event Action<StoreKitTransaction> purchaseSuccessfulEvent;

	// Fired when a product purchase fails
	public static event Action<string> purchaseFailedEvent;

	// Fired when a product purchase is cancelled by the user or system
	public static event Action<string> purchaseCancelledEvent;

	// Fired when an error is encountered while adding transactions from the user's purchase history back to the queue
	public static event Action<string> restoreTransactionsFailedEvent;

	// Fired when all transactions from the user's purchase history have successfully been added back to the queue
	public static event Action restoreTransactionsFinishedEvent;

	// Fired when any SKDownload objects are updated by iOS. If using hosted content you should not be confirming the transaction until all downloads are complete.
	public static event Action<List<StoreKitDownload>> paymentQueueUpdatedDownloadsEvent;



    static StoreKitManager()
    {
		
		AbstractManager.initialize( typeof( StoreKitManager ) );
    }


	public void productPurchaseAwaitingConfirmation( string json )
	{
		StoreKitTransaction transaction = StoreKitTransaction.transactionFromJson( json ) ;
		if( productPurchaseAwaitingConfirmationEvent != null )
			productPurchaseAwaitingConfirmationEvent( transaction );
		
		Debug.Log("purchase json:" + json);
		
		startServerVerify(transaction);
		
/*		if( autoConfirmTransactions )
			StoreKitBinding.finishPendingTransactions();
			*/
	}
	
	private void startServerVerify(StoreKitTransaction transaction)
	{
		string url = "http://frozen.microfunplus.com/v101/net";
 
        WWWForm form = new WWWForm();
		form.AddField ("cmd", "inAppPurchase");  
		form.AddField ("data", transaction.base64EncodedTransactionReceipt); 
        WWW www = new WWW(url, form);
 
        StartCoroutine(WaitForRequest(www, transaction));
	}
	
	IEnumerator WaitForRequest(WWW www, StoreKitTransaction transaction)
    {
        yield return www;
 
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.data);
			if (www.text == "0")
			{
				GameObject mallTableObj = GameObject.Find("MallDragPanel");
				mallTableObj.SendMessage("addDiamond", transaction.productIdentifier);
//				foreach (DiamondProductData diamondProduct in ItemModel.Instance.diamondProducts) 
//				{
//					if(diamondProduct.productID == transaction.productIdentifier)
//					{
//						UserManagerCloud.Instance.CurrentUser.UserGoldCoins += diamondProduct.diamondNum;
//						// Update user.data
//						UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);
//						Debug.Log("Purchase has success,"+ diamondProduct.diamondNum +" diamonds added!");
//					}
//				}
			}
        } else {
			GameObject mallTableObj = GameObject.Find("MallDragPanel");
			mallTableObj.SendMessage("addDiamond", transaction.productIdentifier);
//            Debug.Log("WWW Error: "+ www.error);
//			int times = PlayerPrefs.GetInt(transaction.transactionIdentifier, 0);
//			Debug.Log("restore transaction times:" + times);
//			if(times > tryTimes)
//			{
//				PlayerPrefs.DeleteKey(transaction.transactionIdentifier);
//				StoreKitBinding.finishPendingTransaction(transaction.transactionIdentifier);
//			}
//			else
//			{
//				times += 1;
//				PlayerPrefs.SetInt(transaction.transactionIdentifier, times);
//				productPurchaseFailed("Server Verify Failed!!");
//			}
//			PlayerPrefs.Save();
        }
		
		StoreKitBinding.finishPendingTransaction(transaction.transactionIdentifier);
    } 
	

	public void productPurchased( string json )
	{
		if( purchaseSuccessfulEvent != null )
			purchaseSuccessfulEvent( StoreKitTransaction.transactionFromJson( json ) );
	}


	public void productPurchaseFailed( string error )
	{
		if( purchaseFailedEvent != null )
			purchaseFailedEvent( error );
	}


	public void productPurchaseCancelled( string error )
	{
		if( purchaseCancelledEvent != null )
			purchaseCancelledEvent( error );
	}


	public void productsReceived( string json )
	{
		if( productListReceivedEvent != null )
			productListReceivedEvent( StoreKitProduct.productsFromJson( json ) );
	}


	public void productsRequestDidFail( string error )
	{
		if( productListRequestFailedEvent != null )
			productListRequestFailedEvent( error );
	}


	public void restoreCompletedTransactionsFailed( string error )
	{
		if( restoreTransactionsFailedEvent != null )
			restoreTransactionsFailedEvent( error );
	}


	public void restoreCompletedTransactionsFinished( string empty )
	{
		if( restoreTransactionsFinishedEvent != null )
			restoreTransactionsFinishedEvent();
	}


	public void paymentQueueUpdatedDownloads( string json )
	{
		if( paymentQueueUpdatedDownloadsEvent != null )
			paymentQueueUpdatedDownloadsEvent( StoreKitDownload.downloadsFromJson( json ) );

	}

}
#else
public class StoreKitManager : MonoBehaviour
{
	
}
#endif
