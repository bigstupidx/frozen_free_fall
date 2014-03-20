using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class UserAvatar : MonoBehaviour
{
//	public Texture defaultAvatar;
//	
//	protected UITexture mySprite;
	public PlayMakerFSM levelFsm;
	public string showEvent = "AutoShow";
	
	protected MapPanelLimit myMap;
	
	// Use this for initialization
	void Start ()
	{
//		mySprite = GetComponent<UITexture>();
//		mySprite.mainTexture = defaultAvatar;
		
		UpdateAvatar(true);
		
//		if (UserManager.Instance == null) {
//			return;
//		}
//		
//		UserManager.Instance.UserFBInit += FacebookStatusChanged;
//		UserManager.Instance.UserLogged += FacebookStatusChanged;
//		UserManager.Instance.UserLogout += FacebookStatusChanged;

		if (UserManagerCloud.Instance != null) {
			UserManagerCloud.Instance.UserHasBeenDownloadedFromCloud += DownloadedFromCloud;
		}
	}
	
//	void FacebookStatusChanged(object sender, UserLoginEventDelegateEventArgs e)
//	{
//		UpdateAvatar();
//	}

	void DownloadedFromCloud(object sender, UserCloudDownloadDelegateEventArgs e)
	{
		CompanionsManager.Instance.RefreshLevelButtons();
		UpdateAvatar();
	}

	 void UpdateAvatar(bool centerMap = false)
	{
//		if (UserManager.Instance != null && User.CurrentUser.IsLogged) 
//		{
//			UserManager.Instance.GetUserPictureProfile((avatar) => 
//			{
//				if (avatar != null) {
//					mySprite.mainTexture = avatar;
//				}
//				else {
//					mySprite.mainTexture = defaultAvatar;
//				}
//				// TODO set texture on map
//				//TODO move texture on map
//				//User.CurrentUser.LastFinishedLvl;
//				
//				//TODO set score for finished level
//				//UserManager.Instance.SetScoreForLevel();
//			});
//			
//			CompanionsManager.Instance.RefreshLevelButtons();
//		}
//		else {
//			mySprite.mainTexture = defaultAvatar;
//		}
		
		transform.parent.localPosition = CompanionsManager.Instance.avatarPosition;
		transform.parent.localScale = CompanionsManager.Instance.avatarScale;
		
		if (centerMap) {
			if (myMap == null) {
				myMap = transform.parent.parent.parent.GetComponent<MapPanelLimit>();
			}
			
			Vector3 mapPos = - CompanionsManager.Instance.avatarButtonPosition * myMap.contents.localScale.x;
			mapPos.z = myMap.transform.localPosition.z;
			myMap.SetPosition(mapPos);
		}
		
		if (LoadLevelButton.newUnlockedLevel) 
		{
			StartCoroutine(ResetNewUnlocked());			
			
			if (LoadLevelButton.maxLevels > LoadLevelButton.lastUnlockedLevel || (LoadLevelButton.maxLevels == LoadLevelButton.lastUnlockedLevel && 
				UserManagerCloud.Instance.GetScoreForLevel(LoadLevelButton.lastUnlockedLevel) == 0)) 
			{
				float duration = (CompanionsManager.Instance.avatarPosition - CompanionsManager.Instance.avatarOldPosition).magnitude / 100f;
	
				HOTween.From(transform.parent, duration, 
							new TweenParms().Prop("localPosition", CompanionsManager.Instance.avatarOldPosition)
							.Ease(EaseType.Linear)
							.OnComplete(ActionOnMoveComplete)
				);
			}
			else 
			{
				LoadLevelButton.showNextLevel = false;
				CompanionsManager.Instance.gameEndFsm.SendEvent("AutoShow");
			}
		}
		else if (LoadLevelButton.showBuyLives) 
		{
			LoadLevelButton.showBuyLives = false;
			StartCoroutine(ShowBuyLives());
		}
	}
	
	IEnumerator ShowBuyLives()
	{
		yield return null;
		
		if (Match3BoardRenderer.levelIdx * 2 > 0) {
			CompanionsManager.Instance.levelButtons[Match3BoardRenderer.levelIdx * 2].OnClick();
		}
		else {
			CompanionsManager.Instance.levelButtons[2].OnClick();
		}
			
		levelFsm.SendEvent(showEvent);
	}
	
	void ActionOnMoveComplete()
	{
//		Debug.LogWarning("showNextLevel: " + LoadLevelButton.showNextLevel);
		if (LoadLevelButton.showNextLevel) 
		{
			LoadLevelButton.showNextLevel = false;
			if (LoadLevelButton.lastButton != null) {
//				Debug.LogWarning("last button: " + LoadLevelButton.lastButton);
				LoadLevelButton.lastButton.OnClick();
				levelFsm.SendEvent(showEvent);
			}
		}
	}
	
	IEnumerator ResetNewUnlocked()
	{
		yield return new WaitForEndOfFrame();
		LoadLevelButton.newUnlockedLevel = false;
	}
	
	void OnDestroy()
	{
//		if (UserManager.Instance) {
//			UserManager.Instance.UserFBInit -= FacebookStatusChanged;
//			UserManager.Instance.UserLogged -= FacebookStatusChanged;
//			UserManager.Instance.UserLogout -= FacebookStatusChanged;
//		}
		
		if (UserManagerCloud.Instance != null) {
			UserManagerCloud.Instance.UserHasBeenDownloadedFromCloud -= DownloadedFromCloud;
		}
	}
}

