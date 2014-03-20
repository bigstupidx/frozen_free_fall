package com.mfp.android;

import java.math.BigInteger;
import java.security.SecureRandom;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Build;
import android.provider.Settings.Secure;
import android.telephony.TelephonyManager;
import android.util.Log;

public class DeviceManager {
	public void DeviceManager()
	{
		
	}

	static Activity mActivity;
/**移动 */
	public static int PROVIDER_MOBILE = 1;
	
	/**联通 */
	public static int PROVIDER_UNICON = 2;
	
	/**电信 */
	public static int PROVIDER_TELECOM = 3;
	
	private static final String TAG = "DeviceManager";
	
	/**
	 * 得到运营商信息
	 * 1：移动
	 * 2：联通
	 * 3：电信
	 * 
	 */ 
	public static int getProvider(Activity _activity)
	{
		Log.d(TAG, "start init getProvider:" );
		mActivity = _activity;
		try{
			TelephonyManager telManager = (TelephonyManager)mActivity.getSystemService(Context.TELEPHONY_SERVICE); 
			String operator = telManager.getSimOperator();
			Log.d(TAG, "  getProvider,operator:"+operator );
			if(operator!=null){ if(operator.equals("46000") 
					|| operator.equals("46002")
					|| operator.equals("46007")){
	
			//移动
				return 1;
			}else if(operator.equals("46001")){
	
			//联通
	              return 2;
			}else if(operator.equals("46003")){
	
			//电信
				return 3;
	
			} }
		}catch(Exception e){
			Log.e(TAG, e.toString());
		}

		return 1;
		
	}
	
	/**
	 * 得到设备的联网状态
	 * 网络连接： 1、 wifi 开着或者 mobile 开着
	 * 网络无连接：2、两者都没有开，或者都么有
	 * 
	 */ 
	private static ConnectivityManager cm;
	public static int getNetWorkState(Activity _activity)
	{
		Log.d(TAG, "start init getNetWorkState:" );
		mActivity = _activity;
		try{
			ConnectivityManager cm = (ConnectivityManager) mActivity
            .getSystemService(Context.CONNECTIVITY_SERVICE);
		    NetworkInfo info = cm.getActiveNetworkInfo();
		    boolean isOK = info != null && info.isConnected() && info.isAvailable();
		    if (isOK) {
		    	Log.d(TAG, "start init getNetWorkState:"+1 );
		        return 1;
		    } else {
		    	Log.d(TAG, "start init getNetWorkState:"+2 );
		       return 2;
		    }
		}catch(Exception e){
			Log.e(TAG, e.toString());
		}
		return 2;

	}
	/**
	 * 
	 * 得到设备的型号
	 * @param _activity
	 * @return
	 */
	public static String getDeviceMode(Activity _activity)
	{
		Log.d(TAG, "start init getNetWorkState:" );
		mActivity = _activity;
		try{
			Build bd = new Build(); 
	    	String model = bd.MODEL;
	    	Log.e(TAG, "model:"+model);
	    	return model;
	    	
		}catch(Exception e){
			Log.e(TAG, e.toString());
		}
		return "default model";

	}
	
	/**
	 * 获取设备id
	 * 
	 */ 
	public static  String getVendorID(Activity _activity)
	{
		Log.d(TAG, "start init getVendorID:" );
		mActivity = _activity;
		
		//Try to get the ANDROID_ID
		String deviceID = Secure.getString(_activity.getContentResolver(), Secure.ANDROID_ID);
		if (deviceID == null || deviceID.equals("9774d56d682e549c") || deviceID.length() < 15 ) {
			//if ANDROID_ID is null, or it's equals to the GalaxyTab generic ANDROID_ID or bad, generates a new one
			final SecureRandom random = new SecureRandom();
			deviceID = new BigInteger(64, random).toString(16);
		}

		return deviceID;
//		try{
//		 TelephonyManager tManager = (TelephonyManager)mActivity.getSystemService(Context.TELEPHONY_SERVICE);
//		 if(tManager.getDeviceId()==null) return "null";
//		 return tManager.getDeviceId(); 
//		}catch(Exception e){
//			Log.e(TAG, e.toString());
//		}
//		return "null";
	}
	
	public static String getDataPath(Activity _activity)
	{
		String path = _activity.getFilesDir().getAbsolutePath();
		Log.d("Unity", "[java][DataPath]: " + path);

		return path;
	}
	
}
