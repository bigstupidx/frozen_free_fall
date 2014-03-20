using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class NoticeData : DTOBase
{
	/*
	public int id;
	public int type;
	
	public string platform;
	public string uid;
	
	[{"id":1,"type":1,"desc":"冰雪奇緣電影的同名遊戲終於和大家見面啦！準備好迎接上百個“冷酷”關卡的挑戰了嗎？滑動你的手指，戰勝嚴寒，解開冰雪謎團！","platform":"android,ios","uid":"notice1","start_date":1388764800000,"end_date":1404144000000,"btn_name":null,"title":"寒意來襲","btn_url":null,"unlock_level":5,"notice_order":1,"auto_show":1}]
	[{"desc":"desc","title":"title"}]
	
	*/
	public double id;
	public string title;
	public string desc;
	
	public double type;
	public string btn_name;
	public string btn_url;
	
	public double unlock_level;
	public double auto_show;
	
	public string descWithLineChange
	{
		get
		{
			return desc;
			
			/*
			Debug.Log(desc.Length);
			int lineLength = 18;
			int lineNum = (desc.Length) / lineLength;

			string res = "";
			for (int i = 0; i < lineNum; i++)
			{
				res += desc.Substring(i * lineLength, lineLength) + "\n"; 
			}
			
			if ( desc.Length % lineLength != 0)
			{
				res += desc.Substring(lineLength * lineNum);
				Debug.Log(res);
			}
			return res;
			*/
		}
	}
};


public class NoticeModel
{
	public bool hasGotNotice = false;
	public List<NoticeData> notices = new List<NoticeData>();
	
	protected static NoticeModel instance;
	
	public static NoticeModel Instance {
		get 
		{
			if (instance == null) 
			{		
				instance = new NoticeModel();
			}
			
			return instance;
		}
	}
	
	public void filterByLevel()
	{
		List<NoticeData> filterResult = new List<NoticeData>();
		
		for (int i = 0; i < notices.Count; i++)
		{
			if (UserManagerCloud.Instance.CurrentUser.LastFinishedLvl >= notices[i].unlock_level)
			{
				filterResult.Add(notices[i]);
			}
		}
		
		notices = filterResult;
	}
	
	public void PopupIfNecessary()
	{
		for (int i = 0; i < notices.Count; i++)
		{
			if (notices[i].auto_show == 2)
			{
				string key = "notice_auto" + notices[i].id.ToString();
				int flag = PlayerPrefs.GetInt(key, 0);
				if (flag == 0)
				{
					PlayerPrefs.SetInt(key, 1);
					
					GameObject noticeObj = GameObject.Find("MFP Notice Anchor/Sprite");
					NoticeButton noticeCom = noticeObj.GetComponent<NoticeButton>();
					noticeCom.OnClick();
				}
			}
		}
	}
};

public class NoticeService : MonoBehaviour {
	
	void Awake()
	{
	}
	
	// Use this for initialization
	void Start () 
	{		
		
		
		getNoticeService();
	}
	
	


	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public void getNoticeService()
	{
		if (NoticeModel.Instance.hasGotNotice == false)
		{
			Dictionary<string, object> data = new Dictionary<string, object> ();
			data["cmd"] = "getNotice";
			data["lang"] = "zh_cn";
			
#if UNITY_IPHONE		
			string systemLang = PlayerPrefs.GetString("iOS_systemLanguage");
			if (systemLang != "zh-Hans")
			{
				data["lang"] = "zh_tw";
			}
#endif
			
			HttpRequestService.sendRequest(data, new HttpRequestService.RequestSuccessDelegate(onGetNoticesSuccess), new HttpRequestService.RequestFailDelegate(onGetNoticesFail));
		}
/*		else
		{
			NoticeModel.Instance.PopupIfNecessary();
		}
*/
	}

	void onGetNoticesSuccess (string jsonData)
	{
		NoticeModel.Instance.hasGotNotice = true;
		Debug.Log ("onGetNoticesSuccess");
		
		Dictionary<string, object> dataDict = jsonData.dictionaryFromJson();
		if (dataDict == null || !dataDict.ContainsKey("notice"))
		{
			return;
		}
		
		List<object> noticeObjList = dataDict["notice"] as List<object>;
		if (noticeObjList == null)
		{
			return;
		}
		
		string arrayJson = noticeObjList.toJson();
		NoticeModel.Instance.notices = NoticeData.listFromJson<NoticeData>(arrayJson);
		Debug.Log ("Get " + NoticeModel.Instance.notices.Count + " Notices.");
		
		NoticeModel.Instance.filterByLevel();
		
		if (NoticeModel.Instance.notices.Count > 0 && HttpRequestService.getUserID() != -1)
		{
			string userIdStr = Language.Get("USER_ID_KEY") + HttpRequestService.getUserID().ToString();
			NoticeModel.Instance.notices[NoticeModel.Instance.notices.Count - 1].desc += "\n" + userIdStr;
		}
		
		// popup automatically
		//NoticeModel.Instance.PopupIfNecessary();
	}

	void onGetNoticesFail()
	{
		Debug.Log("onGetNoticesFail");
	}
	
}
