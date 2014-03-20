using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendScoreTable : MonoBehaviour {
	
	GameObject cellObj;
	
	// Use this for initialization
	void Start () {
		//cellObj = transform.Find("NoticeCell").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void UpdateContent(int curLevel)
	{
		List<FriendScoreData> scoreDataList = HighScoreModel.Instance.getFriendScoresForLevel(curLevel);
		
		for (int i = 1; i < scoreDataList.Count; i++)
		{
			GameObject newCell = (GameObject)Instantiate(cellObj);
			newCell.transform.parent = this.transform;
			newCell.transform.localScale = new Vector3(1, 1, 1);
		}
		
		UIGrid gridCom = this.GetComponent<UIGrid>();
		gridCom.Reposition();
		
		for (int i = 0; i < scoreDataList.Count; i++)
		{
			FriendScoreData scoreData = scoreDataList[i];
			
			GameObject childCellObj = transform.GetChild(i).gameObject;
			
			GameObject nameObj = childCellObj.transform.Find("NameLabel").gameObject;
			UILabel nameLabel = nameObj.GetComponent<UILabel>();
			nameLabel.text = scoreData.name;
			
			GameObject scoreObj = childCellObj.transform.Find("ScoreLabel").gameObject;
			UILabel scoreLabel = scoreObj.GetComponent<UILabel>();
			scoreLabel.text = scoreData.score.ToString();
			
			GameObject rankObj = childCellObj.transform.Find("RankLabel").gameObject;
			UILabel rankLabel = rankObj.GetComponent<UILabel>();
			rankLabel.text = (i + 1).ToString();
		}
	}
}
