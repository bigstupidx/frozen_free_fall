using UnityEngine;
using System.Collections;

public class LivesTimer : MonoBehaviour 
{
	public UILabel livesLabel;
	public UISprite livesTexture;
	
	protected UILabel myLabel;
	
	void Awake() 
	{
		myLabel = GetComponent<UILabel>();
	}
	
	// Use this for initialization
	void Start () {
		UpdateLives();
		
		LivesSystem.OnLivesUpdate += UpdateLives;
	}
	
	void UpdateLives()
	{
		myLabel.enabled = (LivesSystem.lives < 5);
		//livesLabel.enabled = (LivesSystem.lives > 0);
		livesTexture.enabled = (LivesSystem.lives < 5);
		StopAllCoroutines();
		
		if (LivesSystem.lives < 5) {
			StartCoroutine(UpdateTimer());
		}
	}
	
	IEnumerator UpdateTimer()
	{
		WaitForSeconds waiter = new WaitForSeconds(0.05f);
		
		while (LivesSystem.lives < 5) {
			myLabel.text = LivesSystem.GetTimerString();
			yield return waiter;
		}
	}
	
	void OnDestroy()
	{
		LivesSystem.OnLivesUpdate -= UpdateLives;
	}
}
