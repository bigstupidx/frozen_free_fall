package com.mfp.android;

import android.app.Activity;
import android.util.Log;

import com.unicom.dcLoader.Utils;
import com.unicom.dcLoader.Utils.UnipayPayResultListener;
import com.unity3d.player.UnityPlayer;

public class UnicomUnityPay  {
	static Activity mActivity = null;
	private static String mpayNum;
    private static String SUCCESS = "1";
	
    private static String FAILED = "2";
	
    private static String CANCELLED = "3";
    private static String APPID = "902426713620131230205504757400";
    private static String DEVELOPE_CODE = "9024267136";
    private static String CP_ID = "86004883";
    private static String PAY_KEY = "2e74c2cf88f68a68c84e";
    private static String PAY_001 = "131231020670";
    private static String PAY_002 = "131231020671";
    private static String PAY_003 = "131231020672";
    private static String PAY_004 = "131231020673";
    private static String COMPANY = "北京群胜网科技有限公司";
    private static String TELEPHONE = "4008808558";
    private static String GAME = "冰雪奇缘";
    
	public void UnicomUnityPay()
	{
		
	}
	
	private static final String TAG = "UnicomUnityPay";
    public static void init(Activity _activity)
    {
    	Log.d(TAG, "start init");
    	mActivity = _activity;
    	try{
    	mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				//  需要替换成正式的。
				Utils.getInstances().init(mActivity, APPID,
						DEVELOPE_CODE, CP_ID,COMPANY,TELEPHONE,GAME, 
						DeviceManager.getVendorID(mActivity),new PayResultListener());
				
		    	  Log.d(TAG, "end init");
			}
		});
    	}catch(Exception e){
			Log.e(TAG, e.toString());
		}
    }
    
  private static String _onPayResult;
  private static String _gameObject;
    public  static void pay(String money,String gameObject,String onResult)
    {
    	Log.d(TAG, "start pay");
    	mpayNum = money;
    	_onPayResult = onResult;
    	_gameObject = gameObject; 
    	try{
	    	mActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					//  需要替换成正式的。
					Utils.getInstances().setBaseInfo(mActivity, false, true, "");
					Utils.getInstances().pay(mActivity,getProductCode(),
							"",getProductName(), getPayValue(), getOrderId(), new PayResultListener());
	
					
					Log.d(TAG, "end pay");
				}
			});
    	}catch(Exception e){
    		UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  FAILED+"|"+mpayNum);
			Log.e(TAG, e.toString());
		}
    	
    }
    private  static String getOrderId(){
    	String verdorId = DeviceManager.getVendorID(mActivity);
    	String orderId = "orderid12345678901234567";
    	if(verdorId.length() <= 24){
    		orderId = verdorId + orderId.substring(0,24-verdorId.length());
    	}else{
    		orderId = verdorId.substring(0,24);
    	}
    	Log.d(TAG, "end pay");
    	return orderId;
    }
    private static String getProductName(){
    	if(mpayNum.equals("1")){
    		return "小袋钻石";
    	}else if(mpayNum.equals("2")){
    		return "大袋钻石";
    	}else if(mpayNum.equals("3")){
    		return "小箱钻石";
    	}else if(mpayNum.equals("4")){
    		return "大箱钻石";
    	}
    	return "小袋钻石";
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
    
    
    private static String getPayValue(){
    	if(mpayNum.equals("1")){
    		return "2";
    	}else if(mpayNum.equals("2")){
    		return "6";
    	}else if(mpayNum.equals("3")){
    		return "10";
    	}else if(mpayNum.equals("4")){
    		return "15";
    	}
    	return "2";
    }
    /**
        * 返回值格式为支付状态|额度， 比如1|1 表示购买2元支付成功
		 * 
		 * 支付状态定义如下：
		 * 0：none
		 * 1：success；
		 * 2：failed
		 * 3：cancelled
		 * 
		 * 支付额度定义如下：
		 *   1  2元  20钻
	 * 2  6元  65钻
	 * 3  10元 110钻
	 * 4  15元  170钻
		 */ 
	public static class PayResultListener implements UnipayPayResultListener
	{
		
		@Override
		public void PayResult(String paycode, int flag, String error) {
			Log.d(TAG, "start PayResult");
			try{
				
			String result = SUCCESS+"|"+mpayNum;
			if(flag ==3){
				result = CANCELLED+"|"+mpayNum;
			}else if(flag ==2){
				result = FAILED+"|"+mpayNum;
			}
			UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, result);
			Log.d(TAG, "flag="+flag+"code="+paycode+"error="+error);
			}catch(Exception e){
				UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  FAILED+"|"+mpayNum);
				Log.e(TAG, e.toString());
			}
			Log.d(TAG, "end PayResultListener");
//			Toast.makeText(mContext, "flag="+flag+"code="+paycode+"error="+error, Toast.LENGTH_LONG).show();
		}
		
	}

}
