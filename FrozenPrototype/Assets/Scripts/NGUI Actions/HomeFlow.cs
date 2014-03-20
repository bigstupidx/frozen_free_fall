using UnityEngine;
using System.Collections;

public class HomeFlow : MonoBehaviour
{
	public PlayMakerFSM flowFSM;
	public string okEvent = "MessageOk";
	
	// Use this for initialization
	IEnumerator Start ()
	{
		if (PlayerPrefs.GetInt("PURCHASES_WARNING", 0) == 0) 
		{
			PlayerPrefs.SetInt("PURCHASES_WARNING", 1);
			
			yield return null;

			flowFSM.SendEvent(okEvent);
			/*
			string message = Language.Get("PURCHASES_WARNING");
#if UNITY_ANDROID
			message = message.Replace("iTunes", "Google");
#endif
			
			NativeMessagesSystem.OnButtonPressed += OnButtonPressed;
			NativeMessagesSystem.Instance.ShowMessage(Language.Get("PURCHASES_WARNING_TITLE"), 
				message, Language.Get("BUTTON_OK"));
				*/
		}
		else {
			flowFSM.SendEvent(okEvent);
		}
	}
	
	void OnButtonPressed(int index)
	{
		NativeMessagesSystem.OnButtonPressed -= OnButtonPressed;
		
		flowFSM.SendEvent(okEvent);
	}
	
	void OnDestroy()
	{
		NativeMessagesSystem.OnButtonPressed -= OnButtonPressed;
	}
}

