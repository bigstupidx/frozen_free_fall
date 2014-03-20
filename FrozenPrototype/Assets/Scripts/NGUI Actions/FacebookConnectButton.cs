using UnityEngine;
using System.Collections;

public class FacebookConnectButton : MonoBehaviour 
{
//	protected static bool dirty = false;
//	
//	public UILabel label;
//	
//	// Use this for initialization
//	void Start () 
//	{
//		if (UserManager.Instance == null) {
//			return;
//		}
//		
//		UserManager.Instance.UserFBInit += FacebookStatusChanged;
//		UserManager.Instance.UserLogged += FacebookStatusChanged;
//		UserManager.Instance.UserLogout += FacebookStatusChanged;
//		
//		UpdateText();
//	}
//	
//	void FacebookStatusChanged(object sender, UserLoginEventDelegateEventArgs e)
//	{
//		dirty = false;
//		UpdateText();
//	}
//	
//	void UpdateText() 
//	{
//		if (User.CurrentUser.IsLogged) {
//			label.text = Language.Get("FACEBOOK_LOGOUT");
//		}
//		else {
//			label.text = Language.Get("FACEBOOK_LOGIN");
//		}
//	}
//	
//	void OnClick()
//	{
//		if (dirty) {
//			return;
//		}
//		
//		dirty = true;
//		
//		if (User.CurrentUser.IsLogged) {
//			UserManager.Instance.Logout();
//		}
//		else {
//			UserManager.Instance.Login();
//		}
//	}
//	
//	void OnDestroy()
//	{
//		dirty = false;
//		
//		if (UserManager.Instance) {
//			UserManager.Instance.UserFBInit -= FacebookStatusChanged;
//			UserManager.Instance.UserLogged -= FacebookStatusChanged;
//			UserManager.Instance.UserLogout -= FacebookStatusChanged;
//		}
//	}
}
