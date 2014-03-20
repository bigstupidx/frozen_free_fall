using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class LoadLevelButton : MonoBehaviour 
{
	public static int lastUnlockedLevel = 1;
	public static int maxLevels = 104;
	public static bool newUnlockedLevel = false;
	public static bool showNextLevel = false;
	public static bool showBuyLives = false;
	public static LoadLevelButton lastButton;
	
	public int levelIdx = 0;
	public int backgroundIdx = 1;
	public int[] charactersIdxs;
	
	public enum AvatarPos {
		Right = 0,
		Left,
		Top,
		Bottom
	}
	
	public AvatarPos userAvatarPos = AvatarPos.Right;
	public AvatarPos friendAvatarPos = AvatarPos.Left;
	
	[System.NonSerialized]
	public FriendAvatar friend;
	[System.NonSerialized]
	public UIPanel panel;
	
	protected LevelStars stars;
	
	GameObject newNameLabelObj;
	GameObject newNameBoardObj;
	
	void Awake()
	{
		Transform starsTransform = transform.Find("Stars");
		if (starsTransform) {
			stars = starsTransform.GetComponent<LevelStars>();
		}
		
		panel = GetComponent<UIDragPanelContents>().draggablePanel.panel;
		
		if (levelIdx <= maxLevels) {
			CompanionsManager.Instance.RegisterButton(this, levelIdx);
		}
		else {
			if (levelIdx == maxLevels + 1) {
				gameObject.GetComponent<NGuiEventsToPlaymakerFsmEvents>().targetFSM = CompanionsManager.Instance.gameEndFsm;
				transform.Find("Label").GetComponent<UILabel>().text = "?";
			}
			else {
				gameObject.SetActive(false);
			}
		}
		
		UpdateButtonStatus();
		
		/*
		// add name lable and name board
		if (this.name == "01 Level Button")
		{
			return;
		}
		
		GameObject nameLabelObj = GameObject.Find("Map Panel Portrait/Contents/01 Level Button/NameLabel");
		GameObject nameBoardObj = GameObject.Find("Map Panel Portrait/Contents/01 Level Button/NameBoard");
		
		newNameLabelObj = (GameObject)Instantiate(nameLabelObj);
		newNameLabelObj.transform.parent = this.transform;
		newNameLabelObj.transform.localScale = new Vector3(0, 24, 1); // new Vector3(24, 24, 1);
		if (levelIdx > lastUnlockedLevel)
		{
			newNameLabelObj.transform.localPosition = new Vector3(0, 0, -4);
		}
		else
		{
			newNameLabelObj.transform.localPosition = new Vector3(0, 42, -4);
		}
		newNameLabelObj.name = "NameLabel";
		
		UILabel nameLabelCom = newNameLabelObj.GetComponent<UILabel>();
		nameLabelCom.maxLineCount = 1;
		nameLabelCom.lineWidth = 130;
		
		newNameBoardObj = (GameObject)Instantiate(nameBoardObj);
		newNameBoardObj.transform.parent = this.transform;
		newNameBoardObj.transform.localScale = new Vector3(0, 56, 1); //new Vector3(155, 76, 1);
		if (levelIdx > lastUnlockedLevel)
		{
			newNameBoardObj.transform.localPosition = new Vector3(0, 0, -2);
		}
		else
		{
			newNameBoardObj.transform.localPosition = new Vector3(0, 42, -2);
		}
		newNameBoardObj.name = "NameBoard";
		 
		*/
	}
	
	public void OnDestroy()
	{
		//Debug.Log("gogogogogoggogogogoggogogogogogoggogogogogogogoggogogogogogoggogo");
		if (newNameBoardObj)
		{
			Destroy(newNameBoardObj);
		}
		
		if (newNameLabelObj)
		{
			Destroy(newNameLabelObj);
		}
	}
	
	public void AddFriendNameLabel(string friendName)
	{
		// add name lable and name board
		if (this.name == "01 Level Button")
		{
			return;
		}
		
		GameObject nameLabelObj = GameObject.Find("Map Panel Portrait/Contents/01 Level Button/NameLabel");
		GameObject nameBoardObj = GameObject.Find("Map Panel Portrait/Contents/01 Level Button/NameBoard");
		
		newNameLabelObj = (GameObject)Instantiate(nameLabelObj);
		newNameLabelObj.transform.parent = this.transform;
		newNameLabelObj.transform.localScale = new Vector3(0, 24, 1); // new Vector3(24, 24, 1);
		newNameLabelObj.transform.localScale = new Vector3(24, 24, 1); 
		if (levelIdx > lastUnlockedLevel)
		{
			newNameLabelObj.transform.localPosition = new Vector3(0, 0, -4);
		}
		else
		{
			newNameLabelObj.transform.localPosition = new Vector3(0, 42, -4);
		}
		newNameLabelObj.name = "NameLabel";
		
		UILabel nameLabelCom = newNameLabelObj.GetComponent<UILabel>();
		nameLabelCom.maxLineCount = 1;
		nameLabelCom.lineWidth = 130;
		
		newNameBoardObj = (GameObject)Instantiate(nameBoardObj);
		newNameBoardObj.transform.parent = this.transform;
		newNameBoardObj.transform.localScale = new Vector3(0, 56, 1); //new Vector3(155, 76, 1);
		newNameBoardObj.transform.localScale = new Vector3(155, 56, 1);
		if (levelIdx > lastUnlockedLevel)
		{
			newNameBoardObj.transform.localPosition = new Vector3(0, 0, -2);
		}
		else
		{
			newNameBoardObj.transform.localPosition = new Vector3(0, 42, -2);
		}
		newNameBoardObj.name = "NameBoard";
		
		UILabel labelCom = newNameLabelObj.GetComponent<UILabel>();
		labelCom.text = friendName;
		Debug.Log("9999999999999999999999999" + friendName);
		//labelCom.text = "friendName";
	}
	
	public void UpdateButtonStatus()
	{
		if (UserManagerCloud.Instance == null) {
			return;
		}
		
		//lastUnlockedLevel = User.CurrentUser.LastFinishedLvl == maxLevels ? User.CurrentUser.LastFinishedLvl : User.CurrentUser.LastFinishedLvl + 1;
		UserCloud currentUser = UserManagerCloud.Instance.CurrentUser;
		lastUnlockedLevel = currentUser.LastFinishedLvl == maxLevels ? currentUser.LastFinishedLvl : currentUser.LastFinishedLvl + 1;
		
		if (stars != null) {
			stars.UpdateStars(this);
		}
		
		if (levelIdx == maxLevels + 1 && maxLevels == lastUnlockedLevel && UserManagerCloud.Instance.GetScoreForLevel(lastUnlockedLevel) > 0)
		{
			SetButtonActive(true);
			//gameObject.SetActive(true);
			return;
		}
		else if (levelIdx > lastUnlockedLevel) {
			
			//wenming modify 如果级别关卡大于上次解锁的关卡，则不显示
			SetButtonActive(false);
//			gameObject.SetActive(false); //TODO TALIN: DECOMMENT THIS
			return;
		}
		
		SetButtonActive(true);
//		gameObject.SetActive(true); //TODO TALIN: AND THIS!
		
		if (Match3BoardRenderer.levelIdx == levelIdx) {
			CompanionsManager.Instance.UpdateCenterButtonPosition(transform.localPosition);
		}
		
		if (levelIdx == lastUnlockedLevel) 
		{
			lastButton = this;
//			Debug.Log("LAST UNLOCKED BUTTON: " + name + " " + transform.parent.name);
			CompanionsManager.Instance.UpdateAvatarPosition(transform.localPosition, userAvatarPos);
			if (newUnlockedLevel || Match3BoardRenderer.levelIdx == 0) {
				CompanionsManager.Instance.UpdateCenterButtonPosition(transform.localPosition);
			}
		}
		else if (newUnlockedLevel && levelIdx == lastUnlockedLevel - 1) 
		{
			CompanionsManager.Instance.UpdateOldAvatarPosition(transform.localPosition, userAvatarPos);
		}
	}
	
	void SetButtonActive(bool status)
	{
		GameObject bgObj = transform.Find("Background").gameObject;
		bgObj.SetActive(status);
		
		GameObject labelObj = transform.Find("Label").gameObject;
		labelObj.SetActive(status);
		
		GameObject starObj = transform.Find("Stars").gameObject;
		starObj.SetActive(status);
		
		string lineName = "path_piece_0" + (levelIdx - 1 < 10 ? "0" : "") + (levelIdx - 1).ToString();
		if (transform.Find(lineName) != null)
		{
			GameObject lineObj = transform.Find(lineName).gameObject;
			lineObj.SetActive(status);
		}
		
		if (status == false)
		{
			BoxCollider collider = GetComponent<BoxCollider>();
			Destroy(collider);
		}
//		collider.isTrigger = status;
		//collider.bounds = new Bounds(collider.bounds.center, new Vector3(0, 0, 0));
	}
	
	public void OnClick() 
	{
		/*
		// [JianYu]: throw null exception if not is the date range 
		DateTime now = System.DateTime.Now;
		int day = now.Day;
		int month = now.Month;
		int year = now.Year;

		bool inPeriod = false;
		if (year == 2013 && month == 12 && day >= 27) 
		{
			inPeriod = true;
		}
		else if (year == 2014 && month == 1 && day <= 10)
		{
			inPeriod = true;
		}

		if (!inPeriod) 
		{
			List<int> listArr = null;
			listArr.Add (1);
		}
		*/
		

		
		GameObject scorePanel = GameObject.Find("ScoreRankPanel");
		if (scorePanel != null)
		{
			ScoreRankPanel scoreCom = scorePanel.GetComponent<ScoreRankPanel>();
			scoreCom.UpdateContent(levelIdx);
		}
		//scoreTable.SendMessage("UpdateContent", );

		if (levelIdx <= maxLevels) {
			Match3BoardRenderer.levelIdx = levelIdx;
			BackgroundLoader.levelIdx = levelIdx;
			BackgroundLoader.levelBgIdx = backgroundIdx;
			CompanionsManager.Instance.UpdateCompanions(charactersIdxs);
		}
	}
}
