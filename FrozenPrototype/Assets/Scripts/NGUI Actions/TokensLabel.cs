using UnityEngine;
using System.Collections;

public class TokensLabel : MonoBehaviour
{
	UILabel myLabel;
	
	void Awake()
	{
		myLabel = gameObject.GetComponent<UILabel>();
//		myLabel.text = Language.Get("REMAINING_TOKENS").Replace("<SPACE>", " ") + ": " + TokensSystem.Instance.itemTokens.ToString();
//		myLabel.text = Language.Get("REMAINING_TOKENS").Replace("<SPACE>", " ") + ": " + UserManagerCloud.Instance.CurrentUser.MagicPower.ToString();
	}
	
	void Update()
	{
		myLabel.text = Language.Get("REMAINING_TOKENS").Replace("<SPACE>", " ") + ": " + UserManagerCloud.Instance.CurrentUser.MagicPower.ToString();
	}
	
	public void UpdateStatus()
	{
		myLabel.enabled = (Match3BoardRenderer.levelIdx >= 8);
	}
}

