using UnityEngine;
using System.Collections;

public class DiamondButton : MonoBehaviour {

	public PlayMakerFSM fsm;
	public string sendEvent = "Lives";
	public string tabButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{
		//MallInitialization 
		//if (mySprite.enabled) {
		// "MallTabButton2"
		MallTab mallTab = GameObject.Find(tabButton).GetComponent<MallTab>();
		mallTab.OnClick();
		
		fsm.SendEvent(sendEvent);
		//}
	}
}
