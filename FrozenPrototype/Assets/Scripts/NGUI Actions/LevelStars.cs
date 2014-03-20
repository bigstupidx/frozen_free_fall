using UnityEngine;
using System.Collections;

public class LevelStars : MonoBehaviour 
{
	protected UISprite mySprite;
	
	public void UpdateStars(LoadLevelButton button) 
	{
		if (mySprite == null) {
			mySprite = GetComponent<UISprite>();
		}
		
		if (button.levelIdx > LoadLevelButton.lastUnlockedLevel) {
			mySprite.enabled = false;
			return;
		}
		
		int count = UserManagerCloud.Instance.GetStarsForLevel(button.levelIdx);
		
		mySprite.enabled = count > 0;
		
		if (count == 1) {
			mySprite.spriteName = "star_map_01";
		}
		else if (count == 2) {
			mySprite.spriteName = "star_map_02";
		}
		else if (count == 3) {
			mySprite.spriteName = "star_map_03";
		}
	}
}
