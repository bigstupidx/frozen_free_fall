using UnityEngine;
using System.Collections;

public class BIInitService : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		BiService.login(QihooSnsModel.Instance.Using360Login ? "qihoo" : "anonymous");
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

