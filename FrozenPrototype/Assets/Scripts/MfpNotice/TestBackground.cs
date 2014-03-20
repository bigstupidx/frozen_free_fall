using UnityEngine;
using System.Collections;

public class TestBackground : MonoBehaviour {

	// Use this for initialization
	void Start () {
	//	Debug.Log("background start");
		
		Debug.Log(gameObject.name + "start");
	}
	
	// Update is called once per frame
	void Awake () {
	//	Debug.Log("background Awake");
		
		Debug.Log(gameObject.name + "Awake");
	}
}
