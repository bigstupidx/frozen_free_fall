package com.mfp.android;

import android.app.Activity;
import android.util.Log;
import cn.cmgame.billing.api.BillingResult;
import cn.cmgame.billing.api.GameInterface;
import cn.cmgame.billing.api.GameInterface.IPayCallback;

import com.unity3d.player.UnityPlayer;

public class MobileGameUnityPay {
private static final String TAG = "MobileGameUnityPay";
	
    
    static Activity mActivity = null;
    
    
    private static String SUCCESS = "1";
	
    private static String FAILED = "2";
	
    private static String CANCELLED = "3";
    
public static void init(Activity _activity) {
		
		Log.d(TAG, "start init Mobile game purchase!!!");
		
	  	mActivity = _activity;
		try {
			// SDK初始化接口，必须首先调用，且在主线程调用
		    GameInterface.initializeApp(mActivity);
		}
		catch (Exception e1) {
			e1.printStackTrace();
		}
	}

private static String _payCode = null;
//private int quantity = 0;
 private static String mpayNum;
  private static String _onPayResult;
  private static String _gameObject;
    public  static void pay(String payNum,String gameObject,String onResult)
    {
    	
    	Log.d(TAG, "start pay Mobile game Market purchase!!! payCode:" + payNum );
    	mpayNum = payNum;
    	_onPayResult = onResult;
    	_gameObject = gameObject; 
    	
		mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				try{
					String billingIndex = getBillingIndex(mpayNum);
				    // 支付或购买道具回调结果
				    final IPayCallback payCallback = new IPayCallback() {
				      @Override
				      public void onResult(int resultCode, String billingIndex, Object obj) {
				        String result = "";
				        switch (resultCode) {
				          case BillingResult.SUCCESS:
				        	  result =  SUCCESS+"|"+mpayNum;
				            UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, result);
				            break;
				          case BillingResult.FAILED:
				        	  result =  FAILED+"|"+mpayNum;
					            UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, result);
				            break;
				          default:
				        	  result =  CANCELLED+"|"+mpayNum;
					            UnityPlayer.UnitySendMessage(_gameObject, _onPayResult, result);
				            break;
				        }
				      }
				    };
				    GameInterface.doBilling(mActivity, true, true, billingIndex, null, payCallback);
					return;
				
				}catch(Exception e){
					UnityPlayer.UnitySendMessage(_gameObject, _onPayResult,  FAILED+"|"+mpayNum+"|"+"order error");
					Log.e(TAG, e.toString());
				}
			}
		});
		
		Log.d(TAG, "end pay Mobile Market purchase!!! _payCode:" + payNum );
//	quantity = jsonObject.optInt("quantity");
//	if(quantity < 1){
//		quantity = 1;
//	}
	//TODO: call native code;
}
    
    /**
     * 
     * @param i
     * 默认4档
     * @return
     */
    private static String getBillingIndex(String i) {
          return "00" + i;
      }
    /**
	    * 
	    * 退出游戏
	    */
	   public static void exitGame(Activity _activity)
	    {
	    	Log.e(TAG, "start exitGame");
	    	mActivity = _activity;
	    	try{
	    	mActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					GameInterface.exit(mActivity);
				}
			});
	    	}catch(Exception e){
				Log.e(TAG, e.toString());
			}
	    	Log.e(TAG, "end exitGame");
	    }
	   /**
	    * 
	    * 音效是否打开；
	    */
	   static int isOn = 0; 
	   public static int isMusicEnabled(Activity _activity)
	    {
	    	Log.e(TAG, "start isMusicEnabled");
	    	mActivity = _activity;
	    	try{
	    	mActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					if(GameInterface.isMusicEnabled() == true){
						isOn =  1;
					}else{
						isOn = 0;
					}
				}
			});
	    	}catch(Exception e){
				Log.e(TAG, e.toString());
			}
	    	Log.e(TAG, "end isMusicEnabled");
	    	return isOn;
	    }
	   
	    /**
		    * 
		    * 更多游戏
		    */
		   public static void viewMoreGames(Activity _activity)
		    {
		    	Log.e(TAG, "start viewMoreGames");
		    	mActivity = _activity;
		    	try{
		    	mActivity.runOnUiThread(new Runnable() {
					@Override
					public void run() {
						GameInterface.viewMoreGames(mActivity);
					}
				});
		    	}catch(Exception e){
					Log.e(TAG, e.toString());
				}
		    	Log.e(TAG, "end viewMoreGames");
		    }
}
