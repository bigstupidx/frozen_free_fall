using UnityEngine;
using System.Collections;

public class HudDiamondLabel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UILabel labelCom = GetComponent<UILabel>();
		labelCom.text = UserManagerCloud.Instance.CurrentUser.UserGoldCoins.ToString();
	}
}
