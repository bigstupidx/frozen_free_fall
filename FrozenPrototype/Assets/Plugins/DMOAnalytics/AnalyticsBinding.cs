using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.InteropServices;

public class AnalyticsBinding 
{
	#region Interface with the Android Analytics Android Control
	#if UNITY_ANDROID
	
	//These settings have been moved in the native code for initialization (in the class FrozenProxyInterface.java in the AndroidNativeInterfaceProxyProject).
	public static string API_KEY_ANDROID 	= "AE734CA0-0D09-47E6-A486-55289FD83B19";
	public static string SECRET_KEY_ANDROID = "0AFE2463-C93A-4220-9C41-C216C3E70AA1";
	
	public static string DEVICE_ID = "";
	public static string PLAYER_ID = "";
	
	public static AndroidJavaClass 	unityPlayer = null;
	public static AndroidJavaObject activity = null;
	public static AndroidJavaObject app = null;
	public static AndroidJavaObject objDMOAnalytics = null;
	public static AndroidJavaClass 	classDMOAnalytics = null;
	
		#endif	
	#endregion	
//	
//	/// <summary>
//	/// Converts a dictionary to an AndroidJavaObject for sending
//	/// </summary>
//	/// <param name='_dict'>
//	/// Dictionary to convert
//	/// </param>
//	public static AndroidJavaObject Dictionary2AndroidJavaObject(Dictionary<string, string> _dict) {
//		return new AndroidJavaObject("org.json.JSONObject", JsonFx.Json.JsonWriter.Serialize(_dict));
//	}
//
//	/// <summary>
//	/// Converts a dictionary to an AndroidJavaObject for sending
//	/// </summary>
//	/// <param name='_dict'>
//	/// Dictionary to convert
//	/// </param>
//	public static AndroidJavaObject Dictionary2AndroidJavaObject(Dictionary<string, object> _dict) {
//		return new AndroidJavaObject("org.json.JSONObject", JsonFx.Json.JsonWriter.Serialize(_dict));
//	}
//

//	
	#region Interface with the iOS Analytics iOS Control
	#if !UNITY_EDITOR && UNITY_IPHONE
//	/* Interface to native implementation */
	[DllImport ("__Internal")]
	private static extern void Analytics_Init(string appID, string appSecret);
//	
	[DllImport ("__Internal")]
	private static extern void Analytics_LogEventUserInfo();
	
	[DllImport ("__Internal")]
	private static extern void Analytics_LogEventPlayerInfo();
//	
	[DllImport ("__Internal")]
	private static extern void Analytics_LogEventGameAction(string context, string action, string type, string message, int level);
//	
	[DllImport ("__Internal")]
	private static extern void Analytics_LogEventNavigationAction(string button_pressed, string from_location, string to_location, string target_url);
//	
	[DllImport ("__Internal")]
	private static extern void Analytics_LogEventTimingAction(string location, float elapsed_time);
//	
	[DllImport ("__Internal")]
	private static extern void Analytics_LogEventPageView(string location, string pageUrl, string message);
//	
	[DllImport ("__Internal")]
	private static extern void Analytics_LogEventPaymentAction(string currency, string locale, float amountPaid, string itemId, int itemCount,
                                     string type, string subtype, string context, int level);
//	
	[DllImport ("__Internal")]
	private static extern void Analytics_LogEvent(string eventName);
//	
//
	#endif	
	#endregion
//	
//	/* Wrapper methods to native implementation */
//	
//	/// <summary>
//	/// Init DMOAnalytics with the specified appID and secretKey.
//	/// </summary>
//	/// <param name='appID'>
//	/// App ID.
//	/// </param>
//	/// <param name='secretKey'>
//	/// Secret key.
//	/// </param>
//	public static void Init(string appID, string appSecret)
//	{
//	
//		#if !UNITY_EDITOR && UNITY_IPHONE
//			Analytics_Init(appID, appSecret);
//		#elif !UNITY_EDITOR && UNITY_ANDROID
//			Debug.Log("[Init]Starting Analytics Binding init!");
//		
//			// Core setup
//			appID = API_KEY_ANDROID;
//			appSecret = SECRET_KEY_ANDROID;
//			DEVICE_ID = SystemInfo.deviceUniqueIdentifier;
//			PLAYER_ID = SystemInfo.deviceUniqueIdentifier;
//					
//			unityPlayer 		= new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//       	activity			= unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
//        	app 				= activity.Call<AndroidJavaObject>("getApplicationContext");	
//			classDMOAnalytics 	= new AndroidJavaClass("com.disneymobile.analytics.DMOAnalytics");
//		objDMOAnalytics 	= new AndroidJavaObject("com.disneymobile.analytics.DMOAnalytics", app, appID, appSecret);
//			
//			// The "sharedAnalyticsManager()" getter should return the native initialized instance of DMOAnalytics.
//			objDMOAnalytics = classDMOAnalytics.CallStatic<AndroidJavaObject>("sharedAnalyticsManager");
//
//			if(unityPlayer == null)
//			{
//				Debug.Log("[Analytics][Init][null]UnityPlayer is null!");
//			}
//			    if(app == null)
//			{
//				Debug.Log("[Analytics][Init][null]App is null!");
//			}
//				if(objDMOAnalytics == null)
//			{
//				Debug.Log("[Analytics][Init][null]objDMOAnalytics is null!");
//			}
//
//     		objDMOAnalytics.Call("setDebugLogging", true);
//		#endif
//		
//		Debug.Log("[AnalyticsBinding] Analytics_Init(appID, appSecret): " + appID + ", " + appSecret);
//		
//	}
// 
//	/// <summary>
//	/// Logs the event.
//	/// </summary>
//	/// <param name='eventName'>
//	/// Event name.
//	/// </param>
//	public static void LogEvent(string eventName)
//	{
//		#if !UNITY_EDITOR && UNITY_IPHONE	
//			Analytics_LogEvent(eventName);
//		#elif !UNITY_EDITOR && UNITY_ANDROID
////			objDMOAnalytics.Call("logEvent", eventName);
//		#endif
//		
//		Debug.Log("[AnalyticsBinding] Analytics_LogEvent(eventName): " + eventName);
//	}
//	
//	
//	
//	/// <summary>
//	/// Logs UserInfo
//	/// </summary>
//	public static void LogEventUserInfo() 
//	{
//		#if !UNITY_EDITOR && UNITY_IPHONE
//			Analytics_LogEventUserInfo();
//		#elif !UNITY_EDITOR && UNITY_ANDROID
//		
//			Dictionary<string, string> dict = new Dictionary<string, string>();
//			dict["device_id"] = DEVICE_ID;
//			dict["player_id"] = PLAYER_ID;
//			dict["user_id_domain"] = "Disney";
//			
////			objDMOAnalytics.Call("logEventWithContext", "user_info", Dictionary2AndroidJavaObject(dict));
//		#endif
//		
//		Debug.Log("[AnalyticsBinding] Analytics_LogEventUserInfo()");
//	}
//	
//	
//	
//	/// <summary>
//	/// Logs Player Info
//	/// </summary>
//	public static void LogEventPlayerInfo()
//	{
//		#if !UNITY_EDITOR && UNITY_IPHONE
//			Analytics_LogEventPlayerInfo();
//		#elif !UNITY_EDITOR && UNITY_ANDROID
//		
//			Dictionary<string, string> dict = new Dictionary<string, string>();
//			dict["device_id"] = DEVICE_ID;
//			dict["player_id"] = PLAYER_ID;
//			dict["user_id_domain"] = "Disney";
//			
////			objDMOAnalytics.Call("logEventWithContext", "player_info", Dictionary2AndroidJavaObject(dict));
//		#endif
//		
////		Debug.Log("[AnalyticsBinding] Analytics_LogEventPlayerInfo()");
//	}
//		
//		
//		
//		
//	/// <summary>
//	/// Logs a Game Action event.
//	/// </summary>
//	/// <param name='context'>
//	/// Context
//	/// </param>
//	/// <param name='action'>
//	/// action
//	/// </param>
//	/// <param name='type'>
//	/// type
//	/// </param>
//	/// <param name='message'>
//	/// message
//	/// </param>
//	/// <param name='level'>
//	/// level
//	/// </param>
	public static void LogEventGameAction(string context, string action, string type, string message, int level)
	{
		return;
		/*
		#if !UNITY_EDITOR && UNITY_IPHONE
			Analytics_LogEventGameAction(context, action, type, message, level);
		#elif !UNITY_EDITOR && UNITY_ANDROID
		
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict["device_id"] = DEVICE_ID;
			dict["player_id"] = PLAYER_ID;
			dict["context"] = context;
			dict["action"] = action;
			dict["type"] = type;
			dict["message"] = message;
			
			if (level > -1)
				dict["level"] = ""+level;
			
			//objDMOAnalytics.Call("logGameAction", Dictionary2AndroidJavaObject(dict));
		#endif
		
		Debug.Log("[AnalyticsBinding] Analytics_LogEventGameAction(context, action, type, message, level): " + context + ", " + action + ", " + type + ", " + message + 
			", " + level);
			*/
	}
//	
//	
//	
//		
//	/// <summary>
//	/// Logs a Navitagion Action event.
//	/// </summary>
//	/// <param name='button_pressed'>
//	/// button_pressed
//	/// </param>
//	/// <param name='from_location'>
//	/// from_location
//	/// </param>
//	/// <param name='to_location'>
//	/// to_location
//	/// </param>
//	/// <param name='target_url'>
//	/// target_url
//	/// </param>
	public static void LogEventNavigationAction(string button_pressed, string from_location, string to_location, string target_url)
	{
		return;
		#if !UNITY_EDITOR && UNITY_IPHONE
			Analytics_LogEventNavigationAction(button_pressed, from_location, to_location, target_url);
		#elif !UNITY_EDITOR && UNITY_ANDROID
			
//			if(objDMOAnalytics == null)
//			{
//				Debug.Log("objDMOAnalytics is null!");
//			}
		
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict["device_id"] = DEVICE_ID;
			dict["player_id"] = PLAYER_ID;
			dict["user_id_domain"] = "Disney";
			
			dict["button_pressed"] = button_pressed;
			dict["from_location"] = from_location;
			dict["to_location"] = to_location;
			
			if ( (target_url != null) && (target_url.Length > 0) )
				dict["target_url"] = target_url;

	//		objDMOAnalytics.Call("logEventWithContext", "navigation_action", Dictionary2AndroidJavaObject(dict));
		#endif
		
		Debug.Log("[AnalyticsBinding] Analytics_LogEventNavigationAction(button_pressed, from_location, to_location, target_url): " + 
			button_pressed + ", " + from_location + ", " + to_location + ", " + target_url);
	}
//	
//	
//	
//	
//		
//	/// <summary>
//	/// Logs a Timing Action Event.
//	/// </summary>
//	/// <param name='location'>
//	/// location
//	/// </param>
//	/// <param name='elapsed_time'>
//	/// elapsed_time
//	/// </param>
	public static void LogEventTimingAction(string location, float elapsed_time)
	{
		return;
		#if !UNITY_EDITOR && UNITY_IPHONE
			Analytics_LogEventTimingAction(location, elapsed_time);
		#elif !UNITY_EDITOR && UNITY_ANDROID
		
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict["device_id"] = DEVICE_ID;
			dict["player_id"] = PLAYER_ID;
			dict["location"] = location;
			dict["elapsed_time"] = ""+elapsed_time;
			
//			objDMOAnalytics.Call("logEventWithContext", "timing", Dictionary2AndroidJavaObject(dict));
		#endif
		
		Debug.Log("[AnalyticsBinding] Analytics_LogEventTimingAction(location, elapsed_time):" + location + ", " + elapsed_time);
	}
//	
//	
//	
//		
//	/// <summary>
//	/// Logs a Page View event.
//	/// </summary>
//	/// <param name='location'>
//	/// location
//	/// </param>
//	/// <param name='pageUrl'>
//	/// pageUrl
//	/// </param>
//	/// <param name='message'>
//	/// message
//	/// </param>
	public static void LogEventPageView(string location, string pageUrl, string message)
	{
		return;
		Debug.Log ("LogEventPageView");
	
		#if !UNITY_EDITOR && UNITY_IPHONE
			Analytics_LogEventPageView(location, pageUrl, message);
		#elif !UNITY_EDITOR && UNITY_ANDROID
			if(objDMOAnalytics == null)
			{
				Debug.Log("[LogEventPageView]objDMOAnalytics is null!");
			}
			else
			{
				Debug.Log("[LogEventPageView]objDMOAnalytics SUCCESS!");
			}
		
			
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict["device_id"] = DEVICE_ID;
			dict["player_id"] = PLAYER_ID;
			dict["location"] = location;
			
			if ( (pageUrl!=null) && (pageUrl.Length>0) )
				dict["page_url"] = pageUrl;
			
			if ( (message!=null) && (message.Length>0) )
				dict["message"] = message;
			
//			objDMOAnalytics.Call("logEventWithContext", "page_view", Dictionary2AndroidJavaObject(dict));
		#endif
		
		Debug.Log("[AnalyticsBinding] Analytics_LogEventPageView(location, pageUrl, message): " + location + ", " + pageUrl + ". " + message);
	}
//	
//	
//	
//		
//	/// <summary>
//	/// Logs a Payment Action Event
//	/// </summary>
//	/// <param name='currency'>
//	/// currency
//	/// </param>
//	/// <param name='locale'>
//	/// locale
//	/// </param>
//	/// <param name='amountPaid'>
//	/// amountPaid
//	/// </param>
//	/// <param name='itemId'>
//	/// itemId
//	/// </param>
//	/// <param name='itemCount'>
//	/// itemCount
//	/// </param>
//	/// <param name='type'>
//	/// type
//	/// </param>
//	/// <param name='subtype'>
//	/// subtype
//	/// </param>
//	/// <param name='context'>
//	/// context
//	/// </param>
//	/// <param name='level'>
//	/// level
//	/// </param>
	public static void LogEventPaymentAction(string currency, string locale, float amountPaid, string itemId, int itemCount,
                                     string type, string subtype, string context, int level)
	{
		return;
		#if !UNITY_EDITOR && UNITY_IPHONE
			Analytics_LogEventPaymentAction(currency, locale, amountPaid, itemId, itemCount, type, subtype, context, level);
		#endif
		
		Debug.Log("[AnalyticsBinding] Analytics_LogEventPaymentAction(currency, locale, amountPaid, itemId, itemCount, type, subtype, context, level): " +
			currency + ", " + locale + ", " + amountPaid + ", " + itemId + ", " + itemCount + ", " + type + ", " + subtype + ", " + context + ", " + level);
	}
//	
	public static void LogEventPaymentAction(string currency, string locale, string amountPaid, string itemId, int itemCount,
                                     string type, string subtype, string context, int level)
	{
		return;
		#if !UNITY_EDITOR && UNITY_ANDROID
			Dictionary<string, string> itemInfo = new Dictionary<string, string>();
			itemInfo["item_id"] = itemId;
			itemInfo["item_count"] = itemCount.ToString();
		
			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict["device_id"] = (object)DEVICE_ID;
			dict["player_id"] = (object) PLAYER_ID;
			dict["currency"] = (object) currency;
			dict["locale"] = (object) locale;
			dict["amount_paid"] = (object)amountPaid;
			dict["item"] = (object) itemInfo;
			dict["type"] = (object)type;
						
			if ( (subtype!=null) && (subtype.Length>0) )
				dict["subtype"] = subtype;
						
			if ( (context!=null) && (context.Length>0) )
				dict["context"] = context;
						
			if (level>=0)
				dict["level"] = (object)(""+level);
				
//			objDMOAnalytics.Call("logEventWithContext", "payment_action", Dictionary2AndroidJavaObject(dict));
		#endif
		
		Debug.Log("[AnalyticsBinding] Analytics_LogEventPaymentAction(currency, locale, amountPaid, itemId, itemCount, type, subtype, context, level): " +
			currency + ", " + locale + ", " + amountPaid + ", " + itemId + ", " + itemCount + ", " + type + ", " + subtype + ", " + context + ", " + level);
	}	
}






		                                                                              