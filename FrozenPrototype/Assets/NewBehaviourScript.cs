using UnityEngine;
using System.Collections;
using Prime31;
using System;

public class NewBehaviourScript : MonoBehaviour {

	

	void OnGUI(){
//				if (GUI.Button (new Rect (50, 50, 300, 100), "login")) {
//				Debug.Log ("login");
//				MFPBillingAndroid.Instance.init();
//
//				}
//				if (GUI.Button (new Rect (50, 170, 300, 100), "get game friends")) {
//					MFPBillingAndroid.Instance.pay("1");
//			Debug.Log ("get game friends");
//
//		
//
//		}
//		if (GUI.Button (new Rect (50, 300, 300, 100), "upload score")) {
//			MFPBillingAndroid.Instance.pay("1");
//			Debug.Log ("upload score");
//			
//			
//			
//		}
	}


	/**
		 * 支付完成后 回调的
		 * 根据返回的码来确定支付是否成功：
		 * 返回值格式为支付状态|额度|其他信息， 比如1|1| 表示购买2元支付成功
		 * 
		 * 支付状态定义如下：
		 * 0：none
		 * 1：success；
		 * 2：failed
		 * 3：cancelled
		 * 
		 * 支付额度定义如下：
	 * 1  2元  20钻
	 * 2  6元  65钻
	 * 3  10元 110钻
	 * 4  15元  170钻
		 */ 
	public	void onBillingResult(string result)
	{ 
		Debug.Log ("#################  BillingResult=" + result);
		Debug.Log("onBillingResult="+result);
		string[] results = result.Split('|');

		string paynum = results [1].ToString();
		int coin = getGoldCoinByNum (paynum);
		if (BillingResult.CANCELLED.Equals(results [0])) {
			//	ExitWithUI ();
			// todo 给予玩家反馈
			PopupMessage.Show(Language.Get("IAP_CANCEL"));
			//MFPBillingAndroid.Instance.ExitWithUI();
			Debug.Log(" wenming MFPBillingAndroid onBillingResult CANCEL:"+paynum.ToString());
			BiService.log("MFPBillingAndroid onBillingResult CANCEL:" + paynum.ToString());
		} else if (BillingResult.SUCCESS.Equals(results [0])) {
			// todo 给予玩家反馈
			
			UserManagerCloud.Instance.CurrentUser.UserGoldCoins += coin;
			UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);

			PopupMessage.Show(Language.Get("IAP_SUCCESS"));
			Debug.Log(" wenming MFPBillingAndroid onBillingResult SUCCESS:"+paynum.ToString());
			
			BIModel.Instance.addOrderData(getPriceByNum(paynum), coin);
		} else if (BillingResult.FAILED.Equals(results [0])) {
			// todo 给予玩家反馈
			PopupMessage.Show(Language.Get("IAP_FAILURE"));
			Debug.Log(" wenming MFPBillingAndroid onBillingResult FAILED:"+paynum.ToString());
			
			BiService.log("MFPBillingAndroid onBillingResult FAILED:" + paynum.ToString());
		} else {
			// todo 给予玩家反馈
			PopupMessage.Show(Language.Get("IAP_FAILURE"));
			Debug.Log(" wenming MFPBillingAndroid onBillingResult EXCEPTION:"+paynum.ToString());
			BiService.log("MFPBillingAndroid onBillingResult EXCEPTION:" + paynum.ToString());
		}
	}



	/**
	 * 1  2元  20钻
	 * 2  6元  65钻
	 * 3  10元 110钻
	 * 4  15元  170钻
	 */ 
	private int getGoldCoinByNum(string paynum){
		Debug.Log ("get gold coin by num : pay num:"+paynum);
		if (paynum.Equals("1")) {
			return 20;
		} else if (paynum.Equals("2")) {
			return 65;
		} else if (paynum.Equals("3")) {
			return 110;
		} else if (paynum.Equals("4")) {
			return 170;
		} else {
			return 20;		
		}
		return 20;
	}
			
	private int getPriceByNum(string paynum)
	{		
		if (paynum.Equals("1")) {
			return 2;
		} else if (paynum.Equals("2")) {
			return 6;
		} else if (paynum.Equals("3")) {
			return 10;
		} else if (paynum.Equals("4")) {
			return 15;
		}
		return 2;		
	}

	/**
	 * Result of billing action.
	 */
	public class BillingResult
	{
		/** No billing action */
		public const string NONE = "0";
		
		/** Billing success */
		public const string SUCCESS = "1";
		
		/** Billing failed, such as sim card is not ready */
		public const string FAILED = "2";
		
		/** Billing canceled, such as use cancel to purchase it in billing ui.*/
		public const string CANCELLED = "3";
	}


	// Use this for initialization
	void Awake () 
	{
		long start = LivesSystem.TimeSeconds();
		Resources.UnloadUnusedAssets();
		GC.Collect();
		long end = LivesSystem.TimeSeconds();
		Debug.Log(" gc time:"+(end - start).ToString());
	//	CmBillingAndroid.Instance.test2 ();
	}
	
	private static long lastFreeMemoryTime = 0;
	
	void Start()
	{
		long curTime = LivesSystem.TimeSeconds();
		Debug.Log("CurTime = " + curTime.ToString() + ", lastFreeMemoryTime = " + lastFreeMemoryTime.ToString());
		if (curTime - lastFreeMemoryTime > 5 * 60)
		{
			if (lastFreeMemoryTime != 0)
			{
				Debug.Log("YU JIAN: start free memory");
				StartCoroutine(FreeMemory());
			}
			lastFreeMemoryTime = curTime;			
		}
	}
	
		public IEnumerator FreeMemory()
	{
		long start = LivesSystem.TimeSeconds();
		GameObject obj1 = GameObject.Find("MFPFreeMemory");
		PlayMakerFSM fsm = obj1.GetComponent<PlayMakerFSM>();
		fsm.SendEvent("FreeMemory");
		long end = LivesSystem.TimeSeconds();
		Debug.Log("YU JIAN Freee memory = " + (end - start).ToString());
		yield return "";
	}

	
	// Update is called once per frame
	void Update () {
	
	}

}
