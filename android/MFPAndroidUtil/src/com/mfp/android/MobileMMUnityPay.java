package com.mfp.android;
import java.util.HashMap;

import mm.sms.purchasesdk.OnSMSPurchaseListener;
import mm.sms.purchasesdk.PurchaseCode;
import mm.sms.purchasesdk.SMSPurchase;
import android.app.Activity;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

public class MobileMMUnityPay   {
	
	public static final String PLATFORM = "mobile mm";
	
	private static final String TAG = "MobileMMUnityPay";
	
    private static SMSPurchase purchase;
    
    static Activity mActivity = null;
    
    private static IAPListener iapListener;
    
    static MobileMMUnityPay m_instance = null;
    
    private static String SUCCESS = "1";
	
    private static String FAILED = "2";
	
    private static String CANCELLED = "3";
 // 计费信息
	// 计费信息 (现网环境) todo 替换正式的
	private static final String APPID = "300008030501";
	private static final String APPKEY = "A6A5379900BAE206";
	// 计费点信�?
	private static final String PAY_001 = "30000803050101";
	private static final String PAY_002 = "30000803050102";
	private static final String PAY_003 = "30000803050103";
	private static final String PAY_004 = "30000803050104";
	
    
    private MobileMMUnityPay(Activity ctx) {
    	mActivity = ctx;
	}

	public static MobileMMUnityPay getInstance(Activity ctx){
    	if(m_instance == null && ctx != null){
    		m_instance = new MobileMMUnityPay(ctx);
    	}
    	return m_instance;
    }
	
//	public void queryPayment(String payCode, String tradeId){
//		_queryPaycode = payCode;
//		if(tradeId == null || tradeId.trim().length() == 0){
//			purchase.query(activity, payCode, iapListener);
//		}else{
//			purchase.query(activity, payCode, tradeId, iapListener);
//		}
//	}
	
	public static void init(Activity _activity) {
		
		Log.d(TAG, "start init Mobile Market purchase!!!\nAppID:" + APPID + "\nAppKey:" + APPKEY);
		
	  	mActivity = _activity;
		/**
		 * IAP组件初始�?包括下面3步�?
		 */
		/**
		 * step1.实例化PurchaseListener。实例化传入的参数与您实现PurchaseListener接口的对象有关�?
		 */
		iapListener = new IAPListener();
		/**
		 * step2.获取Purchase实例�?
		 */
		purchase = SMSPurchase.getInstance();
		/**
		 * step3.向Purhase传入应用信息。APPID，APPKEY�?�?��传入参数APPID，APPKEY�?APPID，见�?��者文�?
		 * APPKEY，见�?��者文�?
		 */
		try {
			purchase.setAppInfo(APPID, APPKEY);
		} catch (Exception e1) {
			e1.printStackTrace();
		}
		/**
		 * step4. IAP组件初始化开始， 参数PurchaseListener，初始化函数�?��入step1时实例化�?
		 * PurchaseListener�?
		 */
		mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				try {
					//设置超时时间
//					purchase.setTimeout(1000, 1000);
					purchase.smsInit(mActivity, iapListener);
				} catch (Exception e) {
					e.printStackTrace();
					Log.e(TAG, e.toString());
				}
			}
		});

	}

	private static String _payCode = null;
//	private int quantity = 0;
	 private static String mpayNum;
	  private static String _onPayResult;
	  private static String _gameObject;
	    public  static void pay(String payNum,String gameObject,String onResult)
	    {
	    	
	    	Log.d(TAG, "start pay Mobile Market purchase!!! payCode:" + payNum );
	    	mpayNum = payNum;
	    	_payCode = getProductCode();
	    	_onPayResult = onResult;
	    	_gameObject = gameObject; 
	    	
			mActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					try{
						purchase.smsOrder(mActivity, _payCode,  iapListener,"test");
						return;
					}catch(Exception e){
						UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  FAILED+"|"+mpayNum+"|"+"order error");
						Log.e(TAG, e.toString());
					}
				}
			});
			
			Log.d(TAG, "end pay Mobile Market purchase!!! _payCode:" + _payCode );
//		quantity = jsonObject.optInt("quantity");
//		if(quantity < 1){
//			quantity = 1;
//		}
		//TODO: call native code;
	}
		   private static String getProductCode(){
		    	if(mpayNum.equals("1")){
		    		return PAY_001;
		    	}else if(mpayNum.equals("2")){
		    		return PAY_002;
		    	}else if(mpayNum.equals("3")){
		    		return PAY_003;
		    	}else if(mpayNum.equals("4")){
		    		return PAY_004;
		    	}
		    	return PAY_001;
		    }
		    

	
	private static class IAPListener implements OnSMSPurchaseListener{
		private final static String TAG = "com.mfp.android.IAPListener";
		
		public IAPListener(){
			Log.i(TAG, "Invoked " + IAPListener.class.getName());
		}


		@SuppressWarnings("rawtypes")
		@Override
		public void onBillingFinish(int code, HashMap map) {
			Log.d(TAG, "billing finish, status code = " + code);
			
			try{
				String result = "";
				// 商品信息
				String paycode = null;
				// 商品的交易ID，用户可以根据这个交易ID，查询商品是否已经交易
				String tradeID = null;
	
				if (code == PurchaseCode.ORDER_OK) {
					result =  SUCCESS+"|"+mpayNum+"|"+code;
					/**
					 * 商品购买成功或�?已经购买,此时会返回商品的paycode，orderID,以及剩余时间(租赁类型商品)
					 */
					if (map != null) {
						paycode = (String) map.get(OnSMSPurchaseListener.PAYCODE);
						if (paycode != null && paycode.trim().length() != 0) {
							result = result + ",Paycode:" + paycode;
						}
						tradeID = (String) map.get(OnSMSPurchaseListener.TRADEID);
						if (tradeID != null && tradeID.trim().length() != 0) {
							result = result + ",tradeid:" + tradeID;
						}
						
					}
					
				} else {
					/**
					 * 表示订购失败
					 */
					result = FAILED+"|"+mpayNum+"|"+code;
				}
	//			String result = flag+"|"+paycode;
				UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, result);
				Log.d(TAG, "billing finish end, status result = " + result);
			}catch(Exception e){
				UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  FAILED+"|"+mpayNum+"|"+code);
				Log.e(TAG, e.toString());
			}
		
		}


		@Override
		public void onInitFinish(int code) {
			// TODO Auto-generated method stub
	//	UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, "init successfully");
			Log.d(TAG, "Init finish, status code = " + code);
		}


		
	}

}
