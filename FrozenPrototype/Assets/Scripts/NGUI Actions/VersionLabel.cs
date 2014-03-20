using UnityEngine;
using System.Collections;

public class VersionLabel : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		gameObject.GetComponent<UILabel>().text = Language.Get("ABOUT_VERSION").Replace("1.0.0", "1.1.0");
	}
}
