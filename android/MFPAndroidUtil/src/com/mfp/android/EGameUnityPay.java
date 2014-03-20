package com.mfp.android;

import android.app.Activity;
import android.util.Log;
import android.widget.Toast;

import cn.egame.terminal.smspay.EgamePay;
import cn.egame.terminal.smspay.EgamePayListener;

import com.mfp.android.UnicomUnityPay.PayResultListener;
import com.unicom.dcLoader.Utils;
import com.unicom.dcLoader.Utils.UnipayPayResultListener;
import com.unity3d.player.UnityPlayer;

public class EGameUnityPay {

	
	static Activity mActivity = null;
	private static String mpayNum;
	private static String payAlias = "";
    private static String SUCCESS = "1";
	
    private static String FAILED = "2";
	
    private static String CANCELLED = "3";
    
    private static final int CHANNEL_CODE = 41000001;
    // 是否外放的 sdk ，需要导出另外的jar
    private static final Boolean isChannel = true;
    
    
	// 计费外放计费点
	private static final String PAY_001 = "117978";
	private static final String PAY_002 = "117979";
	private static final String PAY_003 = "117980";
	private static final String PAY_004 = "117981";
	
	// 计费自有计费点
	private static final String SELF_PAY_001 = "117098";
	private static final String SELF_PAY_002 = "117099";
	private static final String SELF_PAY_003 = "117100";
	private static final String SELF_PAY_004 = "117101";
	
	
	public void EGameUnityPay()
	{
		
	}
	
	private static final String TAG = "EGameUnityPay";
	// 电信爱游戏无初始化工作
    public static void init(Activity _activity)
    {
    	Log.e(TAG, "start init");
    	mActivity = _activity;
    	try{
//    	mActivity.runOnUiThread(new Runnable() {
//			@Override
//			public void run() {
//				// todo 需要替换成正式的。
//							
//		    	  Log.d("unicompay", "end init");
//			}
//		});
    	}catch(Exception e){
			Log.e(TAG, e.toString());
		}
    }
    
  private static String _onPayResult;
  private static String _gameObject;
	private static String _result = "";
	
	/**
	 * 
	 * 支付方法，直接返回值
	 * @param money
	 * @param gameObject
	 * @param onResult
	 * @param 
	 */
	private static String mPayWay;
    public  static void pay(String money,String gameObject,String onResult)
    {
    	Log.e(TAG, "start pay");
    	mpayNum = money;
    	_onPayResult = onResult;
    	_gameObject = gameObject; 
    	try{
	    	mActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					// todo 需要替换成正式的。
					payAlias= getProductCode();
					//软计费点，复活，2元，主角死亡后主动点击
				
					if(isChannel){
						
						EgamePay.pay(mActivity, payAlias,CHANNEL_CODE, new EgamePayListener() {
							
							@Override
							public void paySuccess(String alias) {
								Log.e(TAG, "success alias:"+alias);
								_result = SUCCESS+"|"+mpayNum;
								UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, _result);
							}
							
	
							@Override
							public void payFailed(String alias, int errorInt) {
								Log.e(TAG, "alias:"+alias+",errorInt:"+errorInt);
								_result = FAILED+"|"+mpayNum+"|"+errorInt;
								UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, _result);
							}
	
							@Override
							public void payCancel(String alias) {
								_result = CANCELLED+"|"+mpayNum;
								Log.e(TAG, "cancel alias:"+alias);
								UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, _result);
								
							}
							
						});
					}else{
//							EgamePay.pay(mActivity, payAlias, new EgamePayListener() {
//							
//							@Override
//							public void paySuccess(String alias) {
//								Log.e(TAG, "success alias:"+alias);
//								_result = SUCCESS+"|"+mpayNum;
//								UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, _result);
//							}
//							
//	
//							@Override
//							public void payFailed(String alias, int errorInt) {
//								Log.e(TAG, "alias:"+alias+",errorInt:"+errorInt);
//								_result = FAILED+"|"+mpayNum+"|"+errorInt;
//								UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, _result);
//							}
//	
//							@Override
//							public void payCancel(String alias) {
//								_result = CANCELLED+"|"+mpayNum;
//								Log.e(TAG, "cancel alias:"+alias);
//								UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, _result);
//								
//							}
//							
//						});
					}
					Log.e(TAG, "end pay");
				}
			});
    	}catch(Exception e){
    		UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  FAILED+"|"+mpayNum);
			Log.e(TAG, e.toString());
		}
    	
    }
    
	   private static String getProductCode(){
		   if(isChannel){
			 	if(mpayNum.equals("1")){
		    		return PAY_001;
		    	}else if(mpayNum.equals("2")){
		    		return PAY_002;
		    	}else if(mpayNum.equals("3")){
		    		return PAY_003;
		    	}else if(mpayNum.equals("4")){
		    		return PAY_004;
		    	}
		   }else{
				if(mpayNum.equals("1")){
		    		return SELF_PAY_001;
		    	}else if(mpayNum.equals("2")){
		    		return SELF_PAY_002;
		    	}else if(mpayNum.equals("3")){
		    		return SELF_PAY_003;
		    	}else if(mpayNum.equals("4")){
		    		return SELF_PAY_004;
		    	}
		   }
	   
	    	return PAY_001;
	    }
	    

}
