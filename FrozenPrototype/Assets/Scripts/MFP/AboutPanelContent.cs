using UnityEngine;
using System.Collections;

public class AboutPanelContent : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{		
#if DIANXIN_ZIYOU
		SetActiveChildObject("_DianXin1", true);
		SetActiveChildObject("_DianXin2", true);
		SetActiveChildObject("_DianXin3", true);
		SetActiveChildObject("_VisitLabel", false);
#else
		SetActiveChildObject("_DianXin1", false);
		SetActiveChildObject("_DianXin2", false);
		SetActiveChildObject("_DianXin3", false);
		SetActiveChildObject("_VisitLabel", true);
#endif	
	}
	
	void SetActiveChildObject(string objName, bool inActive)
	{
		GameObject obj = transform.Find(objName).gameObject;
		obj.SetActive(inActive);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
