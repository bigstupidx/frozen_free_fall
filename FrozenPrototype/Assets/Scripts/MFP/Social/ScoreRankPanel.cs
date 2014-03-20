using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ScoreRankPanel : MonoBehaviour {
	
	GameObject FriendListPanel;
	GameObject LoginPanel;
	GameObject NoNetPanel;
	GameObject BackgroundObj;
	GameObject TitleLabelObj;
	
	void Awake()
	{
		Debug.Log("ScoreRankPanel.Awake");
	}
	
	// Use this for initialization
	void Start () 
	{
#if UNITY_IPHONE
		gameObject.SetActive(false);
		return;
#endif
		
		Debug.Log("ScoreRankPanel.Start");
		
		FriendListPanel = GameObject.Find("ScoreRankPanel/FriendListPanel");
		LoginPanel = GameObject.Find("ScoreRankPanel/LoginPanel");
		NoNetPanel = GameObject.Find("ScoreRankPanel/NoNetPanel");
		BackgroundObj = GameObject.Find("ScoreRankPanel/Background");
		TitleLabelObj = GameObject.Find("ScoreRankPanel/TitleLabel");
		
		GameObject scoreTable = GameObject.Find("FriendScoreTable").gameObject;
		GameObject cellObj = GameObject.Find("FriendScoreTable/ScoreCell");
		
		int MAX_CELL_NUM = 20;
		for (int i = 1; i < MAX_CELL_NUM; i++)
		{
			GameObject newCell = (GameObject)Instantiate(cellObj);
			newCell.transform.parent = scoreTable.transform;
			newCell.transform.localScale = new Vector3(1, 1, 1);
		}
		
		UIGrid gridCom = scoreTable.GetComponent<UIGrid>();
		gridCom.Reposition();
		
		Debug.Log("ScoreRankPanel.Start End");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void UpdateContentForCurrentLevel()
	{
		UpdateContent(Match3BoardRenderer.levelIdx);
	}
	
	public void UpdateContent(int levelIdx)
	{
		const int MIN_LEVEL_SHOW = 4;
		
		if (QihooSnsModel.Instance.Using360Login ||
			(AppSettings.Is360Platform && 
			UserManagerCloud.Instance.CurrentUser.LastFinishedLvl >= MIN_LEVEL_SHOW))
		{
			FriendListPanel.SetActive(true);
			LoginPanel.SetActive(true);
			NoNetPanel.SetActive(true);
			BackgroundObj.SetActive(true);
			TitleLabelObj.SetActive(true);
		}
		else
		{
			FriendListPanel.SetActive(false);
			LoginPanel.SetActive(false);
			NoNetPanel.SetActive(false);
			BackgroundObj.SetActive(false);
			TitleLabelObj.SetActive(false);
			return;
		}
		
		//PlayerPrefs.SetInt("key_360", 1);
		
		int score = 0;
		Dictionary<string, object> FinishedLevels = (Dictionary<string, object>)UserManagerCloud.Instance.CurrentUser.FinishedLevels;
		if (FinishedLevels.ContainsKey(levelIdx.ToString()))
		{
			Dictionary<string, object> levelInfo = FinishedLevels[levelIdx.ToString()] as Dictionary<string, object>;
			if (levelInfo.ContainsKey("score"))
			{
				score = Convert.ToInt32(levelInfo["score"]);
			}
		}
				
		if (!QihooSnsModel.Instance.Using360Login && MFPDeviceAndroid.Instance.getNetWorkState() == MFPDeviceAndroid.NETWORK_STATE_NOT_CONNECTED)
		{
			NoNetPanel.SetActive(true);
			FriendListPanel.SetActive(false);
			LoginPanel.SetActive(false);
			
			GameObject scoreLabel = GameObject.Find("NoNetPanel/MyScoreCell/ScoreLabel");
			UILabel scoreUILabel = scoreLabel.GetComponent<UILabel>();
			scoreUILabel.text = score.ToString();
		}
		else if (QihooSnsModel.Instance.Using360Login)
		{
			NoNetPanel.SetActive(false);
			FriendListPanel.SetActive(true);
			LoginPanel.SetActive(false);
			
			UpdateScoreTable(levelIdx);
		}
		else
		{
			NoNetPanel.SetActive(false);
			FriendListPanel.SetActive(false);
			LoginPanel.SetActive(true);
			
			GameObject scoreLabel = GameObject.Find("LoginPanel/MyScoreCell/ScoreLabel");
			UILabel scoreUILabel = scoreLabel.GetComponent<UILabel>();
			scoreUILabel.text = score.ToString();
			
			/*
			Dictionary<string, object> FinishedLevels = (Dictionary<string, object>)UserManagerCloud.Instance.CurrentUser.FinishedLevels;
			if (FinishedLevels.ContainsKey(levelIdx.ToString()))
			{
				Dictionary<string, object> levelInfo = FinishedLevels[levelIdx.ToString()] as Dictionary<string, object>;
				if (levelInfo.ContainsKey("score"))
				{
					int score = Convert.ToInt32(levelInfo["score"]);
					scoreUILabel.text = score.ToString();
				}
			}
			*/
		}
	}
	
	public void UpdateScoreTable(int levelIdx)
	{
		// set panel to initial position
		GameObject panelObj = GameObject.Find("FriendListPanel");
		panelObj.transform.localPosition = new Vector3(-250, 110, 6);
		UIPanel panelCom = panelObj.GetComponent<UIPanel>();
		panelCom.clipRange = new Vector4(250, 0, 768, 420);
		
		// get friend score data
		List<FriendScoreData> scoreDataList = HighScoreModel.Instance.getFriendScoresForLevel(levelIdx);
		
		GameObject scoreTable = GameObject.Find("FriendScoreTable").gameObject;
		if (scoreTable == null)
		{
			return;
		}
		
		// set showed cells' number
		for (int i = 0; i < scoreTable.transform.childCount; i++)
		{
			GameObject childObj = scoreTable.transform.GetChild(i).gameObject;
			childObj.SetActive(i < scoreDataList.Count);
		}
		
		// update content
		for (int i = 0; i < scoreDataList.Count; i++)
		{
			FriendScoreData scoreData = scoreDataList[i];
			
			GameObject childCellObj = scoreTable.transform.GetChild(i).gameObject;
			
			GameObject backgroundObj = childCellObj.transform.Find("background").gameObject;
			UISprite backgroundSprite = backgroundObj.GetComponent<UISprite>();
			if (scoreData.isMe)
			{
				backgroundSprite.spriteName = "me_score_board";
			}
			else
			{
				backgroundSprite.spriteName = "score_board";
			}
			
			GameObject nameObj = childCellObj.transform.Find("NameLabel").gameObject;
			UILabel nameLabel = nameObj.GetComponent<UILabel>();
			nameLabel.text = scoreData.name;
			//nameLabel.text = "11111111111111111111111111";
			
			GameObject scoreObj = childCellObj.transform.Find("ScoreLabel").gameObject;
			UILabel scoreLabel = scoreObj.GetComponent<UILabel>();
			scoreLabel.text = scoreData.score.ToString();
			
			GameObject rankObj = childCellObj.transform.Find("RankLabel").gameObject;
			UILabel rankLabel = rankObj.GetComponent<UILabel>();
			rankLabel.text = (i + 1).ToString();
			
			int id = 0;
			int.TryParse(scoreData.platformID, out id);
			int mod = id % 10;
			string spriteName = "avatar_00" + mod.ToString();
			spriteName = mod != 0 ? spriteName : "avatar_010";
		
			GameObject iconObj = childCellObj.transform.Find("Icon").gameObject;
			UISprite iconSprite = iconObj.GetComponent<UISprite>();
			iconSprite.spriteName = spriteName;
		}
	}
}
