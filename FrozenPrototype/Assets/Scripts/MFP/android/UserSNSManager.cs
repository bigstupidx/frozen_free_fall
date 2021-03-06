//------------------------------------------------------------------------------
// <auto-generated>
// 调用 360 接口
//获取用户信息 GetUserInfo 和好友 信息 GetAppFriends
//上传用户关卡信息 UploadData
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections.Generic;

		public class UserSNSManager
		{

	private String TOP_ID = "1";

		#if UNITY_ANDROID && !UNITY_EDITOR
		private  AndroidJavaClass klass = new AndroidJavaClass("com.mfp.android.sns.QiHooSocialManager");	
		#endif

				private static UserSNSManager _instance;
				public static UserSNSManager Instance
				{
					get
					{
						if(_instance==null){
							_instance = new UserSNSManager();
						}
						return _instance;
					}

				}
		/**
	 * 
	 * cmd:UploadData    上传玩家等级数据到360 ；
	 * 参数：
	 * platformId
	 * level

	*/
		
	public void UploadData(String platformId,String score)
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			using (AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
				using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
					Debug.Log(" wenming UploadData ");
					klass.CallStatic("uploadScore",curActivity, "BackgroundCamera", "onUploadScoreResult",TOP_ID,score);	
				}
			}
			#endif	
		}
		
		/***
		 * cmd:GetAppFriends  得到 360 玩游戏的好友

		 * 
		 * 
		 */
		public void GetAppFriends(String platformId)
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			using (AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
				using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
					Debug.Log(" wenming GetAppFriends ");
					klass.CallStatic("getGameFriends",curActivity, "BackgroundCamera", "onGetGameFriendsResult");	
				}
			}
			#endif	
			
		}

		public void getContactContent()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			using (AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
				using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
					Debug.Log(" wenming UploadData ");
					klass.CallStatic("sdkGetContactContent",curActivity, "BackgroundCamera", "onSdkGetContactContentResult");	
				}
			}
			#endif	
		}
		public void showFloatWnd()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			using (AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
				using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
					Debug.Log(" wenming showFloatWnd ");
					klass.CallStatic("showFloatWnd",curActivity, "BackgroundCamera", "onShowFloatWndResult");	
				}
			}
			#endif	
		}
		/***
		 * cmd: 登录，获取玩家信息

		 * 
		 * 
		 */

		public void snsLogin()

		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			using (AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
				using (AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
					Debug.Log(" wenming UserSNSManager 44444444444444444444");
					klass.CallStatic("login",curActivity, "BackgroundCamera", "onSnsLoginResult");	
					
				}
			}
			#endif
			
		}

	}


