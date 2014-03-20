using UnityEngine;
using System.Collections;

public class PopupMessage : MonoBehaviour {
	
	public PlayMakerFSM messageFsm;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	static public void Show(string content)
	{
		GameObject messageObj = GameObject.Find("Popup Message Panel/Message Label");
		UILabel labelCom = messageObj.GetComponent<UILabel>();
		labelCom.text = content;
		
		PopupMessage messageCom = messageObj.GetComponent<PopupMessage>();
		messageCom.messageFsm.SendEvent("ShowMessage");
		
	}
}
