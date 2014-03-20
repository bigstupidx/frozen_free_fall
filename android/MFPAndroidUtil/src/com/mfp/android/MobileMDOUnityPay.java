package com.mfp.android;

import android.app.Activity;
import android.util.Log;

import com.enorbus.android.mdosdk.service.BillingCallBack;
import com.enorbus.android.mdosdk.service.BillingManager;
import com.unity3d.player.UnityPlayer;

public class MobileMDOUnityPay {

public static final String PLATFORM = "mobile mm";
	
	private static final String TAG = "MobileMDOUnityPay";
	
    
    static Activity mActivity = null;
    
    
    
    private static String SUCCESS = "1";
	
    private static String FAILED = "2";
	
    private static String CANCELLED = "3";
    
    private static String PAY_001 = "FROZEN001";
    private static String PAY_002 = "FROZEN002";
    private static String PAY_003 = "FROZEN003";
    private static String PAY_004 = "FROZEN004";
    
    
public static void init(Activity _activity) {
		
		Log.d(TAG, "start init Mobile mdo purchase!!!");
		
	  	mActivity = _activity;
		mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				try{
					BillingManager.init(mActivity,"北京群胜网科技有限公司","010-58731393","冰雪奇缘");
				}catch (Exception e1) {
					Log.d(TAG, e1.toString());
					e1.printStackTrace();
				}
			}
			
		});
		
	}

//private int quantity = 0;
 private static String mpayNum;
  private static String _onPayResult;
  private static String _gameObject;
    public  static void pay(String payNum,String gameObject,String onResult)
    {
    	
    	Log.d(TAG, "start pay mobile mdo  purchase!!! payCode:" + payNum );
    	mpayNum = payNum;
//    	_payCode = getPayCode(mpayNum);
    	_onPayResult = onResult;
    	_gameObject = gameObject; 
    	
		mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				try{
//					Log.d(TAG, "start init");
//					 BillingManager.init(mActivity,"北京群胜网","4006661551","冰雪奇缘");
					 Log.d(TAG, "start dobilling");
					BillingManager.getInstance().doBilling(getProductCode(), new BillingCallBack() {
						@Override
						public void onUserOperCancel() {
							Log.e(TAG, "cancel pay"+",num:"+mpayNum);
							UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, CANCELLED+"|"+mpayNum);
						}
						@Override
						public void onCanntSupport() {
							Log.e(TAG, "can not support pay"+",num:"+mpayNum);
							UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, FAILED+"|"+mpayNum+"|not support");
						}
						@Override
						public void onBillingSuccess() {
							Log.e(TAG, "success pay"+",num:"+mpayNum);
							UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, SUCCESS+"|"+mpayNum);
						}
						@Override
						public void onBillingFail(String message) {
							Log.e(TAG, "fail pay"+message+",num:"+mpayNum);
							UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, FAILED+"|"+mpayNum+"|"+message);
						}
					});
					Log.d(TAG, "end dobilling");
					return;
				}catch(Exception e){
					e.printStackTrace();
					UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  FAILED+"|"+mpayNum+"|"+"order error");
					Log.e(TAG, e.toString()+",num:"+mpayNum);
				}
			}
		});
		
		Log.d(TAG, "end pay mobile mdo  purchase!!! _payCode:" + mpayNum );
//	quantity = jsonObject.optInt("quantity");
//	if(quantity < 1){
//		quantity = 1;
//	}
	//TODO: call native code;
}
    
    
    private static String getProductCode(){
    	
    	if(mpayNum.equals("1")){
    		Log.e(TAG, "getProductCode"+",num:"+PAY_001);
    		return PAY_001;
    	}else if(mpayNum.equals("2")){
    		Log.e(TAG, "getProductCode"+",num:"+PAY_002);
    		return PAY_002;
    	}else if(mpayNum.equals("3")){
    		Log.e(TAG, "getProductCode"+",num:"+PAY_003);
    		return PAY_003;
    	}else if(mpayNum.equals("4")){
    		Log.e(TAG, "getProductCode"+",num:"+PAY_004);
    		return PAY_004;
    	}
    	return PAY_001;
    }
}
