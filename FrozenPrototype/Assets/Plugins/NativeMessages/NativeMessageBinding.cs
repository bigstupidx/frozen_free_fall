using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class NativeMessageBinding 
{
	#if !UNITY_EDITOR && UNITY_IPHONE
	[DllImport ("__Internal")]
	public static extern void Native_ShowMessage(string title, string message, string button1, string button2, string button3);
	
	[DllImport ("__Internal")]
	public static extern void Native_ScheduleNotification(string title, string message, long showTime);
	
	[DllImport ("__Internal")]
	public static extern void Native_CancelNotifications(string message);
	
	[DllImport ("__Internal")]
	public static extern string Native_GetRateLink(string appId);
	#endif
	
	#if !UNITY_EDITOR && UNITY_ANDROID
	public static AndroidJavaClass  javaBinding = null;
	
	static NativeMessageBinding()
	{
		javaBinding = new AndroidJavaClass("com.mobilitygames.messagescontroller.AndroidMessagesController");
	}
	#endif
}
