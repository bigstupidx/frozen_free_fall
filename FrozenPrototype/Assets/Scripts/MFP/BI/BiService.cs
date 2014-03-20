using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BiService : MonoBehaviour
{
	private const string BI_FORM_KEY = "bi";

	// Use this for initialization
	void Start ()
	{
		login(QihooSnsModel.Instance.Using360Login ? "qihoo" : "anonymous");
		// load dat.
		BIModel.Instance.Deserialize();
		
		// send order data
		syncOrderData();
		// send purchase data
		syncPurchaseData();
		// send consume data
		syncConsumeData();
		// send score data
		syncScoreData();
	}
	
	void syncOrderData()
	{
		if (BIModel.Instance.BIOrderDataList.Count > 0)
		{
			Dictionary<string, object> data = new Dictionary<string, object> ();
			data["cmd"] = "order";
			data["bi"] = BIModel.Instance.BIOrderDataList;
			HttpRequestService.sendRequest(data, new HttpRequestService.RequestSuccessDelegate(onSyncOrderSuccess), null, BI_FORM_KEY);
		}
	}
	
	void onSyncOrderSuccess(string jsonData)
	{
		Debug.Log("Sync OrderData Success!");
		
		BIModel.Instance.BIOrderDataList.Clear();
		BIModel.Instance.DeleteCache(BIModel.ORDER_CACAH_DATA);
	}
	
	void syncPurchaseData()
	{
		if (BIModel.Instance.BIPurchaseDataList.Count > 0)
		{
			Dictionary<string, object> data = new Dictionary<string, object> ();
			data["cmd"] = "purchase";
			data["bi"] = BIModel.Instance.BIPurchaseDataList;
			HttpRequestService.sendRequest(data, new HttpRequestService.RequestSuccessDelegate(onSyncPurchaseSuccess), null, BI_FORM_KEY);
		}
	}
	
	void onSyncPurchaseSuccess(string jsonData)
	{
		Debug.Log("Sync Purchase Success!");
		
		BIModel.Instance.BIPurchaseDataList.Clear();
		BIModel.Instance.DeleteCache(BIModel.PURCHASE_CACHE_DATA);
	}
	
	void syncConsumeData()
	{
		if (BIModel.Instance.BIConsumeDataList.Count > 0)
		{
			Dictionary<string, object> data = new Dictionary<string, object> ();
			data["cmd"] = "consume";
			data["bi"] = BIModel.Instance.BIConsumeDataList;
			HttpRequestService.sendRequest(data, new HttpRequestService.RequestSuccessDelegate(onSyncConsumeSuccess), null, BI_FORM_KEY);
		}
	}
	
	void onSyncConsumeSuccess(string jsonData)
	{
		Debug.Log("Sync Consume Success!");
		
		BIModel.Instance.BIConsumeDataList.Clear();
		BIModel.Instance.DeleteCache(BIModel.CONSUME_CACHE_DATA);
	}
	
	void syncScoreData()
	{
		if (BIModel.Instance.BIScoreDataList.Count > 0)
		{
			Dictionary<string, object> data = new Dictionary<string, object> ();
			data["cmd"] = "level_up";
			data["v"] = AppSettings.frontEndVersion.ToString();
			data["bi"] = BIModel.Instance.BIScoreDataList;
			HttpRequestService.sendRequest(data, new HttpRequestService.RequestSuccessDelegate(onSyncScoreSuccess), null, BI_FORM_KEY);
		}
	}
	
	void onSyncScoreSuccess(string jsonData)
	{
		Debug.Log("Sync Consume Success!");
		
		BIModel.Instance.BIScoreDataList.Clear();
		BIModel.Instance.DeleteCache(BIModel.SCORE_CACHE_DATA);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnDestroy()
	{
		BIModel.Instance.Serialize();
	}
	
	public static void init()
	{
		Dictionary<string, object> data = new Dictionary<string, object> ();
		data["cmd"] = "init";
		HttpRequestService.sendRequest(data, null, null, BI_FORM_KEY);
	}
	
	public static void login(string account = "anonymous")
	{
		Dictionary<string, object> data = new Dictionary<string, object> ();
		data["cmd"] = "biLogin";
		Dictionary<string, object> biData = new Dictionary<string, object> ();
		biData["account"] = account;
		data["bi"] = biData;
		HttpRequestService.sendRequest(data, null, null, BI_FORM_KEY);
	}
	
	public static void log(string message)
	{
		Dictionary<string, object> data = new Dictionary<string, object> ();
		data["cmd"] = "client_log";
		Dictionary<string, object> biData = new Dictionary<string, object> ();
		biData["message"] = message;
		data["bi"] = biData;
		HttpRequestService.sendRequest(data, null, null, BI_FORM_KEY);
	}
}

