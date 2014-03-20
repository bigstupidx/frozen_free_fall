using UnityEngine;
using System.Collections;
using System;

public class LivesSystem : MonoBehaviour 
{
	public static System.DateTime baseDate = new System.DateTime(2013, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	
	public static LivesSystem instance;
	
//	public static string livesKey = "Lives";
//	public static string livesTimeKey = "LivesTime";
	public static  string timeModifyKey = "TimeModify";
	
	public static int lives;
	public static int maxLives = 5;
	public static int lifeRefillTime = 60;//1800;
	
	public static event System.Action OnLivesUpdate;
	
	public static long waitTime = 0;
	
	
	public int Lives {
		get {
			return lives;
		}
		set {
			Debug.Log("[LivesSystem] Live value is set!Old Live: " + lives + ", New Lives: " + value);
			lives = value;
			if (OnLivesUpdate != null) {
				OnLivesUpdate();
			}
		}
	}
	
	// Use this for initialization
	void Awake () 
	{
		instance = this;
		
		lifeRefillTime = TweaksSystem.Instance.intValues["LifeRefillTime"];
		
		//TODO TALIN: very easy to modify this property by the user if it's stored in playerprefs (save to binary file?)
//		Lives = PlayerPrefs.GetInt(livesKey, maxLives);
		
		Lives = UserManagerCloud.Instance.CurrentUser.NumsLiveLeft;
		
		Debug.LogWarning("[LivesSystem]Loaded lives: " + lives);
		long time = TimeSeconds();
//		waitTime = System.Math.Min(long.Parse(PlayerPrefs.GetString(livesTimeKey, time.ToString())), time);
		waitTime = UserManagerCloud.Instance.CurrentUser.LivesTime == 0 ? time : UserManagerCloud.Instance.CurrentUser.LivesTime;
		
		//TODO: user modify time to get live. will not charge live.
		if(PlayerPrefs.HasKey(timeModifyKey))
		{
			Debug.Log("[LivesSystem]The time of system has been modified!!" + PlayerPrefs.GetInt(timeModifyKey));
		}
		else
		{
			Debug.Log("[LivesSystem]Cannot get the timeModify flag!");
		}
		
		if(PlayerPrefs.GetInt(timeModifyKey, 0) != 0 && PlayerPrefs.GetInt("cheat", 0) == 0)
		{
			waitTime = TimeSeconds();
			UserManagerCloud.Instance.CurrentUser.LivesTime = waitTime;
			// Update user.data
			UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);
			PlayerPrefs.SetInt(timeModifyKey, 0);
		}
	}
	
	public static long TimeSeconds()
	{
		return (long)System.DateTime.Now.Subtract(baseDate).TotalSeconds;
	}
	
	// Update is called once per frame
	private long lastUpdateTime = 0;
	void Update () 
	{
		if (lives >= maxLives) {
			return;
		}
		long time = TimeSeconds();
		/*if (lastUpdateTime != 0 && (time - lastUpdateTime) > 60)
		{
			waitTime = time;
			return;
		}
		*/
		if(PlayerPrefs.GetInt(timeModifyKey, 0) != 0 && PlayerPrefs.GetInt("cheat", 0) == 0)
		{
			Debug.Log("[LivesSystem] time changed. Reset waitTime!");
			waitTime = time;
			UserManagerCloud.Instance.CurrentUser.LivesTime = waitTime;
			UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);
			PlayerPrefs.SetInt(timeModifyKey, 0);
			return;
		}
		
//		Debug.Log("[LivesSystem] time: " + time+", lastUpdateTime: " + lastUpdateTime);
		lastUpdateTime = time;
		if (time - waitTime >= lifeRefillTime) {
			while (lives < maxLives && time - waitTime >= lifeRefillTime) {
				waitTime += lifeRefillTime;
				Lives++;
				Debug.Log("[LivesSystem] Lives added!");
			}
		}
		else if (time - waitTime < 0)
		{
			waitTime = time;
		}
	}
	
	public static string GetTimerString()
	{
		long time = lifeRefillTime - (TimeSeconds() - waitTime);
		long minutes = (time / 60);
		long seconds = (time % 60);
		return "" + minutes.ToString("00") + ":" + seconds.ToString("00");
	}
	
	public static void SaveLivesAndNotify(int _lives, long _waitTime, bool notifications = true)
	{
		Exception e = new Exception();
		Debug.Log("[LivesSystem] " + e.ToString());
		Debug.Log("[LivesSystem] set UserCloud numsLiveLeft: " + _lives);
		UserManagerCloud.Instance.CurrentUser.NumsLiveLeft = _lives;
		UserManagerCloud.Instance.CurrentUser.LivesTime = _waitTime;
//		PlayerPrefs.SetInt(livesKey, _lives);
//		PlayerPrefs.SetString(livesTimeKey, _waitTime.ToString());
		UserCloud.Serialize(UserManagerCloud.FILE_NAME_LOCAL);
		if (notifications) 
		{
			NativeMessagesSystem.CancelNotifications(Language.Get("LIVES_NOTIFICATION_MESSAGE"));
			
			if (_lives < maxLives) {
				long showTime = lifeRefillTime * (maxLives - _lives) - (TimeSeconds() - _waitTime);
				NativeMessagesSystem.ScheduleNotification(Language.Get("LIVES_NOTIFICATION_TITLE"), Language.Get("LIVES_NOTIFICATION_MESSAGE"), showTime);
			}
		}
	}
	
	void OnDestroy()
	{
		SaveLivesAndNotify(lives, waitTime);
		PlayerPrefs.Save();
	}
}
