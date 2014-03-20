using UnityEngine;
using System.Collections;

public class LivesButton : MonoBehaviour 
{
	public PlayMakerFSM fsm;
	public string sendEvent = "Lives";
	
	UISprite mySprite;
	
	void Awake()
	{
		mySprite = gameObject.GetComponent<UISprite>();
	}
	
	void OnClick()
	{
		if (mySprite.enabled) {
			fsm.SendEvent(sendEvent);
		}
	}
}
