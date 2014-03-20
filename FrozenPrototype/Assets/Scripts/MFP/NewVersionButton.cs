using UnityEngine;
using System.Collections;

public class NewVersionButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{
		LoginRequest request = GameObject.Find ("LoginRequest").GetComponent<LoginRequest> ();

		Application.OpenURL(request.appUrl);
	}
}
