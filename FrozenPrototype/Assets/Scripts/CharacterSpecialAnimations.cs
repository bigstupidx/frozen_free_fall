using UnityEngine;
using System.Collections;

public class CharacterSpecialAnimations : MonoBehaviour 
{
	public static int characterIndex = -1;
	
	public bool NewSystem {
		get {
			return newSystem;
		}
		set {
			newSystem = value;
		}
	}
	
	public Vector2 timeBetweenPowerLooks = new Vector2(5f, 8f);
	public Vector2 timeBetweenWaves = new Vector2(10f, 13f);
	public float minTimeBetweenLooks = 10f;
	public float maxTimeBetweenLooks = 20f;
	public float minTimeBetweenHappy = 10f;
	public float minTimeLooksAfterHappy = 5f;
	public float minTimeBetweenHappyBig = 10f;
	public float minTimeBetweenLooksAndWave = 3f;
	
	public PlayMakerFSM characterFSM;
	
	public string pauseEventName = "PauseAnimation";
	public string lookEventName = "Look at items";
	public string happyEventName = "Happy";
	public string happyBigEventName = "Happy Big";
	public string waveEventName = "Wave";
	
	protected float timeNextLook = 0f;
	protected float timeNextWave = 0f;
	protected float timeNextHappy = 0f;
	protected float timeNextHappyBig = 0f;
	
	protected bool newSystem = false;
	
	public int CharIdx {
		get {
			return characterIndex;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		if (characterFSM == null) {
			characterFSM = GetComponent<PlayMakerFSM>();
		}
		
		timeNextLook = Random.Range(timeBetweenPowerLooks.x, timeBetweenPowerLooks.y);
		timeNextWave = Random.Range(timeBetweenWaves.x, timeBetweenWaves.y);
		
//		Debug.Log("time next look: "  + timeNextLook);
//		Debug.Log("time next wave: "  + timeNextWave);
		
		timeNextHappy = 0f;
		timeNextHappyBig = 0f;
		
		Match3Tile.OnAnyTileDestroyed += OnTileDestroyed;
		ScoreSystem.Instance.OnScoreUpdated += OnScoreUpdated;
//		WinScore.OnNewStarReached += OnNewStarReached;
	}
	
	void OnTileDestroyed(Match3Tile tile) 
	{
		timeNextLook = Random.Range(timeBetweenPowerLooks.x, timeBetweenPowerLooks.y);
		timeNextWave = Random.Range(timeBetweenWaves.x, timeBetweenWaves.y);
		
		if ((tile is ColorBombTile) && timeNextHappyBig <= 0) {
			if (NewSystem) {
				HappyFaceBig();
			}
			else {
				HappyFace();
			}
		}
		else if (((tile is BombTile) || (tile is DirectionalDestroyTile)) && timeNextHappy <= 0f) {
			HappyFace();
		}
	}
	
	void OnScoreUpdated()
	{
		if (ScoreSystem.Instance.multiplier >= 5 && timeNextHappyBig <= 0f) {
			if (NewSystem) {
				HappyFaceBig();
			}
			else {
				HappyFace();
			}
		}
	}
	
//	void OnNewStarReached(int count)
//	{
//		if (timeNextHappy <= 0f) {
//			HappyFace();
//		}
//	}
	
	void HappyFace()
	{
		characterFSM.SendEvent(pauseEventName);
		characterFSM.SendEvent(happyEventName);
		
		timeNextHappy = minTimeBetweenHappy;
	}
	
	void HappyFaceBig()
	{
		characterFSM.SendEvent(pauseEventName);
		characterFSM.SendEvent(happyBigEventName);
		
		timeNextHappy = minTimeBetweenHappy;
		timeNextHappy = minTimeBetweenHappyBig;
	}
		
	// Update is called once per frame
	void Update () 
	{
		if (NewSystem) {
			UpdateNewSystem();
			return;
		}
		
		timeNextLook -= Time.deltaTime;
		timeNextHappy -= Time.deltaTime;
		timeNextHappyBig = timeNextHappy;
		
		if (timeNextLook <= 0f && minTimeBetweenHappy - timeNextHappy >= minTimeLooksAfterHappy) {
			characterFSM.SendEvent(pauseEventName);
			characterFSM.SendEvent(lookEventName);
			
			timeNextLook = Random.Range(timeBetweenPowerLooks.x, timeBetweenPowerLooks.y);
		}
	}
	
	void UpdateNewSystem () 
	{
		timeNextLook -= Time.deltaTime;
		timeNextWave -= Time.deltaTime;
		timeNextHappy -= Time.deltaTime;
		timeNextHappyBig -= Time.deltaTime;
		
		if (timeNextLook <= 0f) {
			characterFSM.SendEvent(pauseEventName);
			characterFSM.SendEvent(lookEventName);
			
			timeNextLook = Random.Range(timeBetweenPowerLooks.x, timeBetweenPowerLooks.y);
			timeNextWave = Mathf.Max(timeNextWave, minTimeBetweenLooksAndWave);
			
//			Debug.Log("LOOK time next look: "  + timeNextLook);
//			Debug.Log("time next wave: "  + timeNextWave);
		}
		
		if (timeNextWave <= 0f) {
			characterFSM.SendEvent(pauseEventName);
			characterFSM.SendEvent(waveEventName);
			
			timeNextWave = Random.Range(timeBetweenWaves.x, timeBetweenWaves.y);
			timeNextLook = Mathf.Max(timeNextLook, minTimeBetweenLooksAndWave);
			
//			Debug.Log("time next look: "  + timeNextLook);
//			Debug.Log("WAVE time next wave: "  + timeNextWave);
		}
	}
	
	/// <summary>
	/// Destroy event raised by Unity.
	/// </summary>
	void OnDestroy() {
		Match3Tile.OnAnyTileDestroyed -= OnTileDestroyed;
//		WinScore.OnNewStarReached -= OnNewStarReached;
	}
}
