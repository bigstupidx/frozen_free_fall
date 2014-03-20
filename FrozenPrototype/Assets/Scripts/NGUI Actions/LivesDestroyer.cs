using UnityEngine;
using System.Collections;

public class LivesDestroyer : MonoBehaviour 
{
	public Match3BoardGameLogic boardLogic;
	
	protected bool canDestroyLife = false;
	protected bool pauseLifeDestroyed = false;
	
	public bool CanDestroyLife {
		get {
			return canDestroyLife;
		}
	}
	
	public int Lives {
		get {
			return UpdateLives(false);
		}
	}
	
	void Start()
	{
		boardLogic.loseConditions.OnNewMove += ThouShallObliterateLife;
		boardLogic.winConditions.OnWinChecked += ThouShallProtectLife;
		BasicItem.OnActuallyUsingAnyItem += OnActuallyUsingAnyItem;
	}

	void OnActuallyUsingAnyItem (BasicItem item)
	{
		ThouShallObliterateLife();
	}
	
	void ThouShallObliterateLife()
	{
		canDestroyLife = true;
		boardLogic.loseConditions.OnNewMove -= ThouShallObliterateLife;
		BasicItem.OnActuallyUsingAnyItem -= OnActuallyUsingAnyItem;
	}
	
	void ThouShallProtectLife()
	{
		canDestroyLife = false;
	}
	
	int UpdateLives(bool save, bool updateNotifications = false)
	{
		LivesSystem.lifeRefillTime = TweaksSystem.Instance.intValues["LifeRefillTime"];
		
		long time = LivesSystem.TimeSeconds();
//		int lives = PlayerPrefs.GetInt(LivesSystem.livesKey, LivesSystem.maxLives);
		int lives = UserManagerCloud.Instance.CurrentUser.NumsLiveLeft;
//		long waitTime = lives < LivesSystem.maxLives ? long.Parse(PlayerPrefs.GetString(LivesSystem.livesTimeKey, time.ToString())) : time;
		long waitTime = lives < LivesSystem.maxLives ? UserManagerCloud.Instance.CurrentUser.LivesTime : time;
		
		int newLives = (int)(time - waitTime) / (int)LivesSystem.lifeRefillTime;
		if (PlayerPrefs.HasKey(LivesSystem.timeModifyKey) && PlayerPrefs.GetInt(LivesSystem.timeModifyKey, 0) != 0 && PlayerPrefs.GetInt("cheat", 0) == 0)
		{
			newLives = 0;
		}
		
		if (newLives + lives >= LivesSystem.maxLives) {
			waitTime = time;
			lives = LivesSystem.maxLives;
		}
		else {
			lives += newLives;
			waitTime += newLives * LivesSystem.lifeRefillTime;
		}
		
		LivesSystem.SaveLivesAndNotify(lives, waitTime, false);
		
		if (save) {
			PlayerPrefs.Save();
		}
		
		return lives;
	}
	
	public void DestroyLife()
	{
		if (!canDestroyLife) {
			return;
		}
		
		if (Match3BoardRenderer.levelIdx >= LoadLevelButton.lastUnlockedLevel)
		{
			int times = PlayerPrefs.GetInt(BIModel.ChallengeTimesKey, 0) + 1;
			PlayerPrefs.SetInt(BIModel.ChallengeTimesKey, times);
		}
	
		canDestroyLife = false;
		
		int lives = UpdateLives(false);
		lives = Mathf.Max(0, lives - 1);
		
//		LivesSystem.SaveLivesAndNotify(lives, long.Parse(PlayerPrefs.GetString(LivesSystem.livesTimeKey, LivesSystem.TimeSeconds().ToString())));
		LivesSystem.SaveLivesAndNotify(lives, UserManagerCloud.Instance.CurrentUser.LivesTime != 0 ? UserManagerCloud.Instance.CurrentUser.LivesTime : LivesSystem.TimeSeconds());
		
		PlayerPrefs.Save();
	}
	
	public void RetryLivesCheck()
	{
		if (Lives == 0) {
			Debug.LogWarning("SHOW BUY LIVES TRUE");
			LoadLevelButton.showBuyLives = true;
		}
	}
	
//	void OnApplicationQuit() 
//	{
//		DestroyLife();
//	}
	
	void OnApplicationPause(bool pause) 
	{
		int lives = UpdateLives(false);
		int oldLives = lives;
		
		//TODO TALIN - !!!
		if (pause) {
			if (canDestroyLife) {
				Debug.Log("Destroying life");
				lives = Mathf.Max(0, lives - 1);
				pauseLifeDestroyed = true;
			}
		}
		else if (pauseLifeDestroyed) {
			Debug.Log("Restoring life");
			lives = Mathf.Min(LivesSystem.maxLives, lives + 1);
			pauseLifeDestroyed = false;
		}
		
		if (lives != oldLives) {
			Debug.Log("Saving new lives");
//			LivesSystem.SaveLivesAndNotify(lives, long.Parse(PlayerPrefs.GetString(LivesSystem.livesTimeKey, LivesSystem.TimeSeconds().ToString())));
			LivesSystem.SaveLivesAndNotify(lives, UserManagerCloud.Instance.CurrentUser.LivesTime != 0 ? UserManagerCloud.Instance.CurrentUser.LivesTime : LivesSystem.TimeSeconds());
			PlayerPrefs.Save();
		}
	}
	
	void OnDestroy()
	{
		if (boardLogic) {
			if (boardLogic.loseConditions) {
				boardLogic.loseConditions.OnNewMove -= ThouShallObliterateLife;
			}
		}
		
		BasicItem.OnActuallyUsingAnyItem -= OnActuallyUsingAnyItem;
	}
}
