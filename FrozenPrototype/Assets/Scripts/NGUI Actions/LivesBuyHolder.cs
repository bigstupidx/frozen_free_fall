using UnityEngine;
using System.Collections;

public class LivesBuyHolder : MonoBehaviour 
{
	public PlayMakerFSM fsm;
	public PlayMakerFSM buyLivePanelFsm;
	public PlayMakerFSM mallFsm;
	
	public string sendEvent = "BuyLives";
	
	protected bool purchasing = false;

	void OnClick()
	{
		if (LivesSystem.instance.Lives < LivesSystem.maxLives)
		{
			if (UserManagerCloud.Instance.CurrentUser.UserGoldCoins >= 20)
			{
				// refill energy
				UserManagerCloud.Instance.CurrentUser.UserGoldCoins -= 20;
				UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);
		
//				PlayerPrefs.SetInt(LivesSystem.livesKey, LivesSystem.maxLives);
//				PlayerPrefs.Save();
				UserManagerCloud.Instance.CurrentUser.NumsLiveLeft = LivesSystem.maxLives;
				UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);
				LivesSystem.instance.Lives = LivesSystem.maxLives;
		
				BIModel.Instance.addPurchaseData("Lives", LivesSystem.maxLives, 20);
				fsm.SendEvent(sendEvent);
				buyLivePanelFsm.SendEvent("NGUI / ON CLICK");
			}
			else
			{
				// go to mall
				
				MallTab mallTab = GameObject.Find("MallTabButton2").gameObject.GetComponent<MallTab>();
				mallTab.OnClick();
				
				buyLivePanelFsm.SendEvent("Other Panel");
				mallFsm.SendEvent("Lives");
			}
		}
		/*
		if (purchasing) {
			return;
		}
		
		purchasing = true;
		
		InAppPurchasesSystem.OnPurchaseSuccess += OnPurchaseSuccess;
		InAppPurchasesSystem.OnPurchaseFail += OnPurchaseFail;
		InAppPurchasesSystem.OnPurchaseCancel += OnPurchaseFail;
		InAppPurchasesSystem.Instance.PurchaseProduct(InAppPurchasesSystem.InAppPurchase.Lives);
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
		if (product != null) 
		{
			#if UNITY_ANDROID
			AnalyticsBinding.LogEventPaymentAction(product.currencyCode, InAppPurchasesSystem.locale, "-" + product.price, id, 1, "lives_refill", 
				"consumable", "", LoadLevelButton.lastUnlockedLevel);
			
			#else
			float price;
			if (!float.TryParse(product.price , out price)) {
				price = 0.99f;
			}
			
			AnalyticsBinding.LogEventPaymentAction(product.currencyCode, InAppPurchasesSystem.locale, -price, id, 1, "lives_refill", "consumable", "", LoadLevelButton.lastUnlockedLevel);
			#endif
		}

		
//		PlayerPrefs.SetInt(LivesSystem.livesKey, LivesSystem.maxLives);
//		PlayerPrefs.Save();
		UserManagerCloud.Instance.CurrentUser.NumsLiveLeft = LivesSystem.maxLives;
		UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);
		LivesSystem.instance.Lives = LivesSystem.maxLives;
		
		fsm.SendEvent(sendEvent);
	}
	
	void OnDestroy()
	{
		InAppPurchasesSystem.OnPurchaseSuccess -= OnPurchaseSuccess;
		InAppPurchasesSystem.OnPurchaseFail -= OnPurchaseFail;
		InAppPurchasesSystem.OnPurchaseCancel -= OnPurchaseFail;
	}
}
