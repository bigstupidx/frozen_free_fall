using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompanionsManager : MonoBehaviour
{
	private static CompanionsManager instance;
	
	public CompanionSelect[] companionsLandscape;
	public CompanionSelect[] companionsPortrait;
	
	public UILabel chooseLabelLandscape;
	public UILabel chooseLabelPortrait;
		
	public UILabel targetLabelLandscape;
	public UILabel targetLabelPortrait;
	public UILabel objectiveLabelLandscape;
	public UILabel objectiveLabelPortrait;
	
	public TokensLabel tokensLabelLandscape;
	public TokensLabel tokensLabelPortrait;
	
	public LevelDestroyTargets[] destroyTargets;
	
	protected Vector3[] positionsLandscape;
	protected Vector3[] positionsPortrait;
	
	[System.NonSerialized]
	public Vector3 avatarPosition;
	[System.NonSerialized]
	public Vector3 avatarScale;
	[System.NonSerialized]
	public Vector3 avatarButtonPosition;
	
	[System.NonSerialized]
	public Vector3 avatarOldPosition;
	
	public GameObject friendAvatarPrefab;
	
//	[System.NonSerialized]
	public LoadLevelButton[] levelButtons;
	
	public PlayMakerFSM gameEndFsm;
	
	public static CompanionsManager Instance {
		get {
			return instance;
		}
	}	
	
	// Use this for initialization
	void Awake()
	{
		instance = this;
		
		// store the level buttons in an array double the size of all available levels.
		// the 2 buttons for one level (landscape and portrait) will be stored at level * 2 and level * 2 + 1 (doesn't matter the order as we shouldn't need it)
		levelButtons = new LoadLevelButton[(LoadLevelButton.maxLevels + 1) * 2]; //to keep indexing from one to make code more readable
		
		positionsLandscape = new Vector3[companionsLandscape.Length];
		positionsPortrait = new Vector3[companionsPortrait.Length];
		
		for (int i = 0; i < companionsLandscape.Length; ++i) {
			positionsLandscape[i] = companionsLandscape[i].transform.localPosition;
			positionsPortrait[i] = companionsPortrait[i].transform.localPosition;
		}
		
	}
	
	void Start()
	{
		// load the first level prefab to have the resources in memory and make the other levels load faster
		//GameObject levelPrefab = Resources.Load("Game/Levels/Level1") as GameObject;
		
		//Debug.Log("app rater - last level: " + LoadLevelButton.lastUnlockedLevel);
		// wenming modify 10 级提示玩家评分
#if UNITY_IOS
		if (LoadLevelButton.lastUnlockedLevel > 10) 
		{
			AppRater.instance.TryShowAppRater(Language.Get("APP_RATE_TITLE"), Language.Get("APP_RATE_MESSAGE"), 
				Language.Get("APP_RATE_BUTTON_RATE"), Language.Get("APP_RATE_BUTTON_NEVER"), Language.Get("APP_RATE_BUTTON_CANCEL"));
		}
#endif
	}
	
//	void Start()
//	{
//		if (UserManager.Instance != null) {
//			UserManager.Instance.GetFacebookFriends(true, FriendsReceived/*(args) => {
//				friendsArgs = args;
//			}*/);
//	//		StartCoroutine(CheckFriendsReceived());
//		}
//	}
	
//	UserFriendsDelegateEventArgs friendsArgs = null;
//	IEnumerator CheckFriendsReceived()
//	{
//		Debug.Log("Start checking friends received");
//		while (friendsArgs == null) {
//			yield return null;
//		}
//		Debug.Log("Friends received, calling function");
//		FriendsReceived(friendsArgs);
//		friendsArgs = null;
//	}
	
//	void FriendsReceived(UserFriendsDelegateEventArgs args) 
//	{
//		foreach (LoadLevelButton button in levelButtons) {
//			if (button != null && button.friend != null) {
//				Destroy(button.friend.gameObject);
//				button.friend = null;
//			}
//		}
//#if UNITY_EDITOR
//		CreateFriendAvatar(null);
//#endif
//		
//		if (args.FriendsList == null || args.FriendsList.Count == 0) {
//			return;
//		}
//		
//		foreach (User user in args.FriendsList) {
//			CreateFriendAvatar(user);
//		}
//	}
//	
//	void CreateFriendAvatar(User friend)
//	{
//		LoadLevelButton button1 = null;
//		LoadLevelButton button2 = null;
//
//#if UNITY_EDITOR
//		button1 = levelButtons[18 * 2];
//		button2 = levelButtons[18 * 2 + 1];
//#else	
//		Debug.Log("Creating friend avatar: " + friend.FBName + " level: " + friend.LastFinishedLvl);
//		
//		if (friend.LastFinishedLvl < LoadLevelButton.maxLevels) {
//			button1 = levelButtons[(friend.LastFinishedLvl + 1) * 2]; //last unlocked level is +1
//			button2 = levelButtons[(friend.LastFinishedLvl + 1) * 2 + 1];
//		}
//		else if (friend.LastFinishedLvl == LoadLevelButton.maxLevels) {
//			button1 = levelButtons[friend.LastFinishedLvl * 2]; //all levels finished so last unlocked level is the last finished level
//			button2 = levelButtons[friend.LastFinishedLvl * 2 + 1];
//		}
//#endif
//
//		if (button1 != null && button1.friend == null) {
//			SetFriendAvatar(button1, friend);
//			SetFriendAvatar(button2, friend);
//		}
//	}
//	
//	void SetFriendAvatar(LoadLevelButton button, User friend) 
//	{
//		GameObject friendAvatarObj = GameObject.Instantiate(friendAvatarPrefab) as GameObject;
//		Transform friendTransform = friendAvatarObj.transform;
//		friendTransform.parent = button.transform;
//		friendTransform.localPosition = GetPositionOnButton(button.friendAvatarPos);
//		friendTransform.localScale = Vector3.one;
//	
//		button.friend = friendAvatarObj.GetComponent<FriendAvatar>();
//		button.friend.friend = friend;
//		button.friend.Refresh();
//	}
	
	Vector3 GetPositionOnButton(LoadLevelButton.AvatarPos avatarPos)
	{
		if (avatarPos == LoadLevelButton.AvatarPos.Left) {
			return new Vector3(-73f, 0f, -2f);
		}
		else if (avatarPos == LoadLevelButton.AvatarPos.Right) {
			return new Vector3(73f, 0f, -2f);
		}
		else if (avatarPos == LoadLevelButton.AvatarPos.Top) {
			return new Vector3(0f, 57f, -2f);
		}
		else if (avatarPos == LoadLevelButton.AvatarPos.Bottom) {
			return new Vector3(0f, -50f, -2f);
		}	
		
		return Vector3.zero;
	}
	
	public void UpdateCenterButtonPosition(Vector3 pos)
	{
		avatarButtonPosition = pos;
	}
	
	public void UpdateAvatarPosition(Vector3 pos, LoadLevelButton.AvatarPos avatarPos)
	{
		avatarPosition = pos + GetPositionOnButton(avatarPos);
		avatarScale = avatarPos == LoadLevelButton.AvatarPos.Right ? new Vector3(-1f, 1f, 1f) : Vector3.one;
	}
	
	public void UpdateOldAvatarPosition(Vector3 pos, LoadLevelButton.AvatarPos avatarPos)
	{
		avatarOldPosition = pos + GetPositionOnButton(avatarPos);
	}
	
	public void UpdateCompanions(int[] indexes) 
	{
		if (indexes.Length > 1) {
			chooseLabelLandscape.GetComponent<UILabel>().text = Language.Get("LEVEL_CHOOSE_COMPANION");
			chooseLabelPortrait.GetComponent<UILabel>().text = Language.Get("LEVEL_CHOOSE_COMPANION");
		}
		else {
			chooseLabelLandscape.GetComponent<UILabel>().text = Language.Get("LEVEL_COMPANION");
			chooseLabelPortrait.GetComponent<UILabel>().text = Language.Get("LEVEL_COMPANION");
		}
		
		CharacterSpecialAnimations.characterIndex = -1;
		
		for (int i = 0; i < 4; ++i) 
		{
			companionsLandscape[i].gameObject.SetActive(i < indexes.Length);
			companionsPortrait[i].gameObject.SetActive(i < indexes.Length);
			
			companionsLandscape[i].selected = i == 0;
			companionsPortrait[i].selected = i == 0;
			
			companionsLandscape[i].transform.localPosition = positionsLandscape[i] + (positionsLandscape[1] - positionsLandscape[0]) * (4 - indexes.Length) * 0.5f;
			companionsPortrait[i].transform.localPosition = positionsPortrait[i] + (positionsPortrait[1] - positionsPortrait[0]) * (4 - indexes.Length) * 0.5f;
			
			if (i < indexes.Length) {
				companionsLandscape[i].UpdateCompanion(indexes[i]);
				companionsPortrait[i].UpdateCompanion(indexes[i]);
			}
		}
		
		tokensLabelLandscape.UpdateStatus();
		tokensLabelPortrait.UpdateStatus();
		
		GameObject levelPrefab = Resources.Load("Game/Levels/Level" + Match3BoardRenderer.levelIdx) as GameObject;
		
		if (levelPrefab != null) 
		{
			Match3BoardRenderer levelData = levelPrefab.GetComponent<Match3BoardRenderer>();
		
			if (levelData != null) {
				string key = "Level" + Match3BoardRenderer.levelIdx + "Star1";
				if (TweaksSystem.Instance.intValues.ContainsKey(key)) {
					targetLabelLandscape.text = Language.Get("LEVEL_TARGET") + " " + ScoreSystem.FormatScore(TweaksSystem.Instance.intValues[key]);
				}
				else {
					targetLabelLandscape.text = Language.Get("LEVEL_TARGET") + " " + ScoreSystem.FormatScore((levelData.winConditions as WinScore).targetScore);
				}
				targetLabelPortrait.text = targetLabelLandscape.text;
				//objectiveLabelLandscape.text = Language.Get("LEVEL_OBJECTIVE") + "\n" + levelData.winConditions.GetObjectiveString();
				objectiveLabelLandscape.text = levelData.winConditions.GetShortObjectiveString(levelData.loseConditions);
				objectiveLabelPortrait.text = objectiveLabelLandscape.text;
				
				foreach (LevelDestroyTargets target in destroyTargets) {
					target.UpdateValues(levelData.winConditions);
				}
				
				Vector3 newPos = targetLabelPortrait.transform.localPosition;
				if (levelData.winConditions.GetType() != typeof(WinDestroyTiles) && levelData.winConditions.GetType() != typeof(WinDestroyTilesDrop)) 
				{
					newPos.y = 112f;
				}
				else {
					newPos.y = 86f;
				}
					
				targetLabelPortrait.transform.localPosition = newPos;
				targetLabelLandscape.transform.localPosition = targetLabelPortrait.transform.localPosition;
			}
			
//			levelPrefab = null;
//			Resources.UnloadUnusedAssets();
		}
	}
	
	public void RegisterButton(LoadLevelButton button, int levelIndex) 
	{
		if (levelButtons[levelIndex * 2] == null) {
			levelButtons[levelIndex * 2] = button;
		}
		else {
			levelButtons[levelIndex * 2 + 1] = button;
		}
	}
	
	public void RefreshLevelButtons()
	{
		foreach (LoadLevelButton button in levelButtons) {
			if (button) {
				button.UpdateButtonStatus();
			}
		}
	}
}

