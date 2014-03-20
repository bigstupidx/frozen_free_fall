using UnityEngine;
using System.Collections;

public class LivesLabel : MonoBehaviour 
{
	protected UILabel myLabel;
	
	void Awake() 
	{
		myLabel = GetComponent<UILabel>();
	}
	
	void Start()
	{
		UpdateText();
		LivesSystem.OnLivesUpdate += UpdateText;
	}
	
	void UpdateText() 
	{
		myLabel.text = LivesSystem.lives.ToString();
	}
	
	void OnDestroy()
	{
		LivesSystem.OnLivesUpdate -= UpdateText;
	}
}
