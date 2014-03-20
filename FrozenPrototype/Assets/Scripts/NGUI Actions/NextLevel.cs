using UnityEngine;
using System.Collections;

public class NextLevel : MonoBehaviour 
{
	public PlayMakerFSM fsm;
	public string sendEvent = "NextLevel";
	public GameObject replayButton;
	
	public void UpdateButton()
	{
		if (Match3BoardRenderer.levelIdx < LoadLevelButton.lastUnlockedLevel || 
			(Match3BoardRenderer.levelIdx == LoadLevelButton.maxLevels && 
			UserManagerCloud.Instance.GetStarsForLevel(Match3BoardRenderer.levelIdx) > 0)) 
		{
			gameObject.SetActive(false);
			replayButton.SetActive(true);
		}
	}
	
	void OnClick()
	{
		LoadLevelButton.showNextLevel = true;
		fsm.SendEvent(sendEvent);
	}
}
