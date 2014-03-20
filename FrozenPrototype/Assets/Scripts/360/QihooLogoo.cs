using UnityEngine;
using System.Collections;

public class QihooLogoo : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
#if UNITY_IOS || DIANXIN_ZIYOU || LIANTONG
		gameObject.SetActive(false);
#else
		StartCoroutine(Hide360Logoo());
#endif
	}
	
	IEnumerator Hide360Logoo()
	{
		yield return new WaitForSeconds(1.0f);
		gameObject.SetActive(false);
	}
}
