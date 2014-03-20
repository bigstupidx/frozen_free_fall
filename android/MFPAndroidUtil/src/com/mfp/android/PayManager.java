package com.mfp.android;

import android.app.Activity;
import android.util.Log;
import android.view.WindowManager;

/**
 * 
 * 
 * @author wenming
 * init如果为空，支付时需要返回错误码
 */
public class PayManager {

	private static Activity mActivity;
	private static  String mMobileWay;
	private static final String TAG = "PayManager";
	private static String MOBILE_WAY_MM = "1";
	private static String MOBILE_WAY_MDO = "2";
	private static String MOBILE_WAY_GAME = "3";
	private static String MOBILE_WAY_OTHER = "0";
	public PayManager(){
		
	}
	/**
	 * 初始化 android 支付
	 * 具体根据运营商来决定初始化哪个支付的sdk ，在java 代码中处理；
	 * 非 移动的手机，参数设置成0;
	 * 移动的手机，根据后台参数来决定用哪个sdk；
	 * mobile way: 移动mm（1），mdo（2），游戏基地（3），,其他（0）；
	 */ 
	public static void init(Activity _activity,String _mobileway)
	{
		mActivity = _activity;
		mMobileWay = _mobileway;
		//移动运营商 
		Log.d(TAG, "start init :" );
		try{
		int operator = DeviceManager.getProvider(mActivity);
		if(operator == 1){
			if(_mobileway.equals(MOBILE_WAY_MM)){
				MobileMMUnityPay.init(mActivity);
			}else if(_mobileway.equals(MOBILE_WAY_MDO)){
//				 todo 调用mdo 初始化
				MobileMDOUnityPay.init(mActivity);
			}else if(_mobileway.equals(MOBILE_WAY_GAME)){
				
				//todo 调用 游戏基地初始化
				MobileGameUnityPay.init(mActivity);
			}else{
				MobileMMUnityPay.init(mActivity);
			}
		}else if(operator == DeviceManager.PROVIDER_UNICON){//联通
			UnicomUnityPay.init(mActivity);
		}else if(operator == DeviceManager.PROVIDER_TELECOM){//电信
			EGameUnityPay.init(mActivity);
		}else{
			// MobileMMUnityPay.init(mActivity);
			MobileMMUnityPay.init(mActivity);
		}
		}catch(Exception e){
			Log.e(TAG, e.toString());
		}
		Log.d(TAG, "end init :" );
		
		mActivity.runOnUiThread(new Runnable(){
			@Override
			public void run() {
				mActivity.getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
			}
		});
	}
	
	/**
	 * 支付接口
	 * 具体根据运营商来决定初始化哪个支付的sdk ，在java 代码中处理；
	 * 2元：1
	 * 10元：2
	 * 20元：3
	 * 30元：4
	 */ 
	public static void pay(String num,String gameObject,String onResult)
	{
		//移动运营商 
		Log.e(TAG, "start pay ,mobile way:" +mMobileWay+",num:"+num);
		try{
		int operator = DeviceManager.getProvider(mActivity);
		if(operator == 1){
			if(mMobileWay.equals(MOBILE_WAY_MM)){
				MobileMMUnityPay.pay(num,gameObject,onResult);
			}else if(mMobileWay.equals(MOBILE_WAY_MDO)){
				// todo 调用mdo 初始化
				MobileMDOUnityPay.pay(num,gameObject,onResult);
			}else if(mMobileWay.equals(MOBILE_WAY_GAME)){
				//todo 调用 游戏基地初始化
				MobileGameUnityPay.pay(num,gameObject,onResult);
			}else{
				MobileMMUnityPay.pay(num,gameObject,onResult);
			}
		}else if(operator == DeviceManager.PROVIDER_UNICON){//联通
			UnicomUnityPay.pay(num,gameObject,onResult);
		}else if(operator == DeviceManager.PROVIDER_TELECOM){//电信
			EGameUnityPay.pay(num,gameObject,onResult);
		}else{
			// MobileMMUnityPay.init(mActivity);
			MobileMMUnityPay.pay(num,gameObject,onResult);
		}
		}catch(Exception e){
			Log.e(TAG, e.toString());
		}
		Log.e(TAG, "end pay :" );
	}
}
