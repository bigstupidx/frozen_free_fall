using UnityEngine;
using System.Collections;

public class UnlockSystem : MonoBehaviour
{
	protected static UnlockSystem instance;
	
	public string iconName;
	public string textKey;
	
	public static UnlockSystem Instance {
		get {
			return instance;
		}
	}
	
	void Awake()
	{
		if (UserManagerCloud.Instance.GetScoreForLevel(Match3BoardRenderer.levelIdx) > 0) { //level already finished
			Destroy(this);
		}
		else {
			instance = this;
		}
	}
}

