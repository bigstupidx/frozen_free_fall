using UnityEngine;
using System.Collections;

public class SaveScore : MonoBehaviour 
{
	public Match3BoardGameLogic boardLogic;
	
	public void SyncScores()
	{
		WinScore winCondition = boardLogic.winConditions as WinScore;
		int score = ScoreSystem.Instance.Score;
		int stars = 0;
		
		if (score >= winCondition.targetScore3Stars) {
			stars = 3;
		}
		else if (score >= winCondition.targetScore2Stars) {
			stars = 2;
		}
		else if (score >= winCondition.targetScore) {
			stars = 1;
		}
		
		// Each 6 level, Call 360 friend interface if already using 360 login
		if (Match3BoardRenderer.levelIdx % 6 == 0)
		{
			int flag = PlayerPrefs.GetInt("GetContactContent", 0);
			if (flag == 0 && QihooSnsModel.Instance.Using360Login)
			{
				Debug.Log("getContactContent");
				PlayerPrefs.SetInt("GetContactContent", 1);
				UserSNSManager.Instance.getContactContent();
			}
		}
		
		//UserManager.Instance.SetScoreForLevel(Match3BoardRenderer.levelIdx, score, stars);
		
		if (stars > 0) {
			LoadLevelButton.newUnlockedLevel = (Match3BoardRenderer.levelIdx >= LoadLevelButton.lastUnlockedLevel);
			UserManagerCloud.Instance.SetScoreForLevel(Match3BoardRenderer.levelIdx, score, stars);
			//Debug.LogError("NEW LAST FINISHED LEVEL: " + UserManagerCloud.Instance.CurrentUser.LastFinishedLvl);
			if (Match3BoardRenderer.levelIdx >= LoadLevelButton.lastUnlockedLevel)
			{
				BIModel.Instance.addScoreData(Match3BoardRenderer.levelIdx, score, PlayerPrefs.GetInt(BIModel.ChallengeTimesKey, 0));
				PlayerPrefs.SetInt(BIModel.ChallengeTimesKey, 0);
			}
		}
		else {
			//Debug.LogError("SYNCING");
			if (Match3BoardRenderer.levelIdx >= LoadLevelButton.lastUnlockedLevel)
			{
				int times = PlayerPrefs.GetInt(BIModel.ChallengeTimesKey, 0) + 1;
				PlayerPrefs.SetInt(BIModel.ChallengeTimesKey, times);
			}
			UserManagerCloud.Instance.LoadUserFromCloud();
		}
		PlayerPrefs.Save();
	}
}
