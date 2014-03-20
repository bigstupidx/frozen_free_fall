using UnityEngine;
using System.Collections;

public class ScoreSystem : MonoBehaviour
{
	public delegate void ScoreUpdated();
	
	protected static ScoreSystem instance;
	
	protected float multiplierWait = 0.5f;
	protected int maxMultiplier = 15;
	
	protected int score;
	public int multiplier;
	
	public event ScoreUpdated OnScoreUpdated;
	
	protected float timeToWait = 0f;
	
	public int Score {
		get {
			return score;
		}
		set {
			score = value;
			if (OnScoreUpdated != null) {
				OnScoreUpdated();
			}
		}
	}
	
	public int Multiplier {
		get {
			return multiplier;
		}
	}
	
	public static ScoreSystem Instance {
		get {
//			if (instance == null) {
//				Debug.LogError("ScoreSystem hasn't been initialized");
//			}
			
			return instance;
		}
	}
	
	void Awake() 
	{
		Debug.Log("ScoreSystem awake");
		instance = this;
		
		Match3Tile.OnAnyTileDestroyed += OnAnyTileDestroyed;
		
		Reset();
	}

	void OnAnyTileDestroyed (Match3Tile tile)
	{
		timeToWait = multiplierWait;
	}
	
	public void Reset()
	{
		score = 0;
		multiplierWait = TweaksSystem.Instance.floatValues["MultiplierWait"];
		maxMultiplier = TweaksSystem.Instance.intValues["MaxMultiplier"];
		timeToWait = 0f;
		
		ResetMultiplier();
	}
	
	public void IncreaseMultiplier()
	{
		multiplier = Mathf.Min(multiplier + 1, maxMultiplier);
		
		StopAllCoroutines();
		StartCoroutine(WaitForMultiplierReset());
	}
	
	public void ResetMultiplier() 
	{
		multiplier = 1;
		
		StopAllCoroutines();
	}
	
	IEnumerator WaitForMultiplierReset()
	{
		timeToWait = multiplierWait;
		
		while (timeToWait > 0f) 
		{
			// done this way so that i can easily reset this time when 
			// a tile is destroyed, without restarting the coroutine
			timeToWait -= Time.deltaTime;
			
			yield return null;
		}
		
		multiplier = 1;
	}
	
	public string GetScoreString()
	{
		return ScoreSystem.FormatScore(score);
	}
	
	public int AddScore(int newScore, bool multiplied = true)
	{
		Score += newScore + (multiplied ? TweaksSystem.Instance.intValues["MultipliedScore"] * (multiplier - 1) : 0);
		
		return score;
	}
	
	public static string FormatScore(int score)
	{
		return score >= 1000 ? score.ToString("0,0").Replace(",", Language.Get("SCORE_SEPARATOR").Replace("<SPACE>"," ")) : score.ToString();
	}
	
	void OnDestroy()
	{
		Match3Tile.OnAnyTileDestroyed -= OnAnyTileDestroyed;
	}
}

