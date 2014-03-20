package com.mfp.receiver;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.os.Build;
import android.provider.Settings;
import android.provider.Settings.SettingNotFoundException;
import android.telephony.TelephonyManager;
import android.util.Log;


public class TimeChangeReceiver  extends BroadcastReceiver {
	private static final String ModifyTimeKey = "TimeModify";
	private static final String TAG = "TimeChangeReceiver";
	@Override
	public void onReceive(Context context, Intent intent) {
		
		try{
			TelephonyManager telManager = (TelephonyManager)context.getSystemService(Context.TELEPHONY_SERVICE); 
			String operator = telManager.getSimOperator();
			if(operator.equals("46003")){//电信手机不做处理
				Log.d(TAG, "Telecom phone.Ignore the modify flag.");
				return;
			}
		}catch(Exception e){
			Log.e(TAG, e.toString());
		}
		
		if (isAutoTimeSet(context)){// 移动和联通手机判断是否自动同步时间标识
			Log.d(TAG, "Time is modified by AutoTime.Ignore the modify flag.");
			return;
		}
		
		if (Intent.ACTION_TIME_CHANGED.equals(intent.getAction())){
			Log.d(TAG, "Catched user modified the system's time.");
			SharedPreferences purchaseSP = context.getSharedPreferences(context.getPackageName(), Context.MODE_PRIVATE);
        	Editor editor = purchaseSP.edit();
        	editor.putInt(ModifyTimeKey, 1);
        	editor.commit();
		}
	}

	
    private boolean isAutoTimeSet(Context context)
    {
        try {
        	if(android.os.Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR1) 
        	{
        		return Settings.Global.getInt(context.getContentResolver(), Settings.Global.AUTO_TIME) > 0;
        	}
        	else
        	{
        		return Settings.System.getInt(context.getContentResolver(), Settings.System.AUTO_TIME) > 0;
        	}
        } catch (SettingNotFoundException snfe) {
			Log.e(TAG, "Cann't find AUTO_TIME in Settings.");
            return false;
        } catch(Exception ex){
			Log.e(TAG, "Unexpected exception: " + ex.toString());
        }
        return false;
    }

}
