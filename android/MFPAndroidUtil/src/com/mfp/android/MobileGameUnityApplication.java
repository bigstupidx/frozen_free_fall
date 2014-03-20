package com.mfp.android;

import android.app.Application;


/**
 * 
 * @author Administrator
 * 移动游戏基地入口
 *  移动游戏基地的登录界面必须是游戏基地logo 相关，所以先显示此入口，初始化完毕，会自动跳转到unity游戏里面
 */
public class MobileGameUnityApplication extends Application{
	 
	public void onCreate() {
		    System.loadLibrary("megjb");
		  }
}
