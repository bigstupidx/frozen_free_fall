using UnityEngine;
using System.Collections;
using Prime31;


#if UNITY_IPHONE || UNITY_ANDROID
public class GPGAchievementMetadata : DTOBase
{
	public string formattedCompletedSteps;
	public string achievementId;
	public string achievementDescription;
	public double numberOfSteps;
	public double type;
	public double lastUpdatedTimestamp;
	public string formattedNumberOfSteps;
	public double state;
	public double progress;
	public string revealedIconUrl;
	public string unlockedIconUrl;
	public string name;
	public double completedSteps;
}
#endif
