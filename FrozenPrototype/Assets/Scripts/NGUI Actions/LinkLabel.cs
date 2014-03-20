using UnityEngine;
using System.Collections;

public class LinkLabel : MonoBehaviour 
{
	public string link;
	public PlayMakerFSM confirmFsm;
	public string confirmEvent = "Confirm";
	
	void OnClick() 
	{
		NoticeCellButton cellBtnObj = transform.gameObject.GetComponent<NoticeCellButton>();
		if (cellBtnObj)
		{
			string url = NoticeModel.Instance.notices[cellBtnObj.noticeIndex].btn_url;
			link = url;

			OpenLinkButton.linkLabel = this;
			confirmFsm.SendEvent(confirmEvent);
			Debug.Log(link);
		}
		else
		{
			OpenLinkButton.linkLabel = this;
			confirmFsm.SendEvent(confirmEvent);
			Debug.Log(link);
		}
	}
	
	public void OpenURL()
	{
		Debug.Log ("OpenURL " + link);
		Application.OpenURL(link);
	}
}
