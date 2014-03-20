using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;


public class NoticeButton : MonoBehaviour {
	
	public PlayMakerFSM fsm;
	
	List<GameObject> noticeCellObjects = new List<GameObject>();
	List<GameObject> buttonObjects = new List<GameObject>();
	
	GameObject contentObj;
	GameObject noContentObj;
	
	// Use this for initialization
	void Start () 
	{		
#if DIANXIN_ZIYOU || LIANTONG
		gameObject.SetActive(false);
		return;
#endif
		
		GameObject noticeTableObj = GameObject.Find("NoticeTable");
		for (int i = 0; i < noticeTableObj.transform.childCount; i++)
		{
			GameObject cellObj = noticeTableObj.transform.GetChild(i).gameObject;
			noticeCellObjects.Add(cellObj);
			
			GameObject buttonObj = cellObj.transform.Find("LinkButton").gameObject;
			buttonObjects.Add(buttonObj);

			NoticeCellButton cellBtnObj = buttonObj.GetComponent<NoticeCellButton>();
			cellBtnObj.noticeIndex = i;
		}
		
		contentObj = GameObject.Find("MFP Notice Panel Portrait/NoticeDragPanel");
		noContentObj = GameObject.Find("MFP Notice Panel Portrait/No net Label");
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnClick()
	{	
		
		
		if (NoticeModel.Instance.notices.Count == 0 && MFPDeviceAndroid.Instance.getNetWorkState() == MFPDeviceAndroid.NETWORK_STATE_NOT_CONNECTED)
		{
			fsm.SendEvent("Lives");
			noContentObj.SetActive(true);
			contentObj.SetActive(false);
			return;
		}
		
		noContentObj.SetActive(false);
		contentObj.SetActive(true);
		
		// Once click notice button, will send get notice service
		NoticeModel.Instance.hasGotNotice = false;
		
		GameObject noticeServiceObj = GameObject.Find("NoticeService");
		noticeServiceObj.SendMessage("getNoticeService");
		/*
		Dictionary<string, object> data = new Dictionary<string, object> ();
		data["cmd"] = "getNotice";
		data["lang"] = "zh_cn";
		HttpRequestService.sendRequest(data, new HttpRequestService.RequestSuccessDelegate(onGetNoticesSuccess), new HttpRequestService.RequestFailDelegate(onGetNoticesFail));
		*/
		
		fsm.SendEvent("Lives");
		
		foreach (GameObject obj in noticeCellObjects)
		{
			obj.SetActive(false);
		}
		
		foreach (GameObject obj in buttonObjects)
		{
			obj.SetActive(false);
		}
		
		int i = 0;
		for (; NoticeModel.Instance.notices != null && i < NoticeModel.Instance.notices.Count; i++)
		{
			NoticeData oneNotice = NoticeModel.Instance.notices[i];
			
			if (i < noticeCellObjects.Count)
			{
				GameObject cellObj = noticeCellObjects[i];
				cellObj.SetActive(true);
			
				GameObject titleObj = cellObj.transform.Find("TitleLabel").gameObject;
				UILabel titleLabelCom = titleObj.GetComponent<UILabel>();
				titleLabelCom.text = oneNotice.title;
				
				GameObject infoObj = cellObj.transform.Find("InfoLabel").gameObject;
				UILabel infoLabelCom = infoObj.GetComponent<UILabel>();
				infoLabelCom.text = oneNotice.descWithLineChange;
				
				if (oneNotice.type == 2)
				{
					buttonObjects[i].SetActive(true);
					
					GameObject buttonLabelObj = buttonObjects[i].transform.Find("ButtonLabel").gameObject;
					UILabel buttonLabelCom = buttonLabelObj.GetComponent<UILabel>();
					buttonLabelCom.text = oneNotice.btn_name;
				}
			}
		}
		
		for (; i < noticeCellObjects.Count; i++)
		{
			GameObject cellObj = noticeCellObjects[i];
			cellObj.SetActive(false);
		}
		
		/*
		if (HttpRequestService.getUserID() != -1)
		{
			GameObject uidLabelObj = GameObject.Find("MFP Notice Panel Portrait/UID Label");
			UILabel uiLabel = uidLabelObj.GetComponent<UILabel>();
			uiLabel.text = Language.Get("USER_ID_KEY") + HttpRequestService.getUserID().ToString();
		}
		*/
	}
	
	private IEnumerator DoWWW()
	{

		WWWForm form = new WWWForm ();  
		form.AddField ("cmd", "getNotice");  
		form.AddField ("lang", "zh");  
	
		WWW getData = new WWW (AppSettings.gameServerUrl, form);  
		yield return getData;  

		if (getData.error == null) 
		{  
			string jsonStr = getData.text;
			List<NoticeData> notices = NoticeData.listFromJson<NoticeData>(jsonStr);
			
			NoticeData data = notices[0];
			//var oneNotice = noticeList[0];
			//List<object> x = noticeList;
			
			/*
			foreach (Dictionary<string, object> dict in noticeList)
			{
				Debug.Log ("fail");
			}
			*/
			
			/*
			foreach (var oneNotice in noticeList)
			{
				Debug.Log(oneNotice.ToString());
			}
			*/
			
			Debug.Log (data.desc);
			Debug.Log (getData.error);  

			//newVersionPanelFsm.SendEvent("Has New Version");
		} 
		else 
		{  
			Debug.Log ("success");
			Debug.Log (getData.text);
			/*
			if (getData.text != "1")
			{
				newVersionPanelFsm.SendEvent("Has New Version");
				appUrl = getData.text;
			}
			*/
		}
	}
}
