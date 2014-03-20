using UnityEngine;
using System.Collections;

public class GiftIconInMap : MonoBehaviour {
	
	public PlayMakerFSM fsm;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnClick()
	{
		GameObject TipObj = GameObject.Find("MFP Gift Panel Portrait/Tip1");
		UILabel tipLabel = TipObj.GetComponent<UILabel>();
		tipLabel.text = "";
		
		GameObject labelObj = GameObject.Find("MFP Gift Panel Portrait/Input/Label");
		UILabel labelCom = labelObj.GetComponent<UILabel>();
		labelCom.text = "";
		
		fsm.SendEvent("Lives");
	}
}
