using UnityEngine;
using System.Collections;

public class ProbabilisticSpawnController : MonoBehaviour {
	
	private static ProbabilisticSpawnController instance = null;
	
	protected float tileTotalCount;
	protected int[] tileColorCount;

	protected float[] colorProbability;
	protected float colorProbabilitySum;
	
	protected int[] genericColorsIndexes;
	
	bool isEnabled = false;
	
	void Awake() {
		instance = this;
		
		NormalTile.OnAnyTileDestroyed += OnAnyTileDestroyedEvent;
		NormalTile.OnTileInitAfterAttachedToBoard += OnTileAttachedToBoardEvent;
		Match3BoardRenderer.OnRandomGenericColorInitialized += OnBoardRendererInitEvent;
		Match3BoardRenderer.OnBoardFinishedSetup += OnBoardFinishedSetupEvent;
	}
	
	public void OnBoardRendererInitEvent()
	{
//		Debug.LogError("[ActionOnBoardRendererInit]");
//		Debug.LogError("[ActionOnBoardRendererInit] Max Count: " + Match3BoardRenderer.maxNumBoardColors);
		
		tileColorCount = new int[(int)TileColorType.Count];
		colorProbability = new float[Match3BoardRenderer.maxNumBoardColors];
		genericColorsIndexes = new int[(int)TileColorType.Count];
		
		for(int i = 0; i < Match3BoardRenderer.maxNumBoardColors; i++)
		{
			genericColorsIndexes[(int)RuleEntry.genericColors[i]] = i;
			colorProbability[i] = 1f;
		}
	}
	
	public static ProbabilisticSpawnController Instance {
		get
		{
			return instance;
		}
	}
	
	protected void OnTileAttachedToBoardEvent(Match3Tile tile)
	{
//		Debug.LogError("[Spawner]++");
		if(tile.GetType() == typeof(NormalTile))
		{	
			tileTotalCount++;
			tileColorCount[(int)tile.TileColor]++;
			colorProbability[genericColorsIndexes[(int)tile.TileColor]] = 1f - (tileTotalCount == 0 ? 0f : tileColorCount[(int)tile.TileColor] / tileTotalCount);
		}
	}
	
	protected void OnAnyTileDestroyedEvent(Match3Tile tile)
	{
		if(tile.GetType() == typeof(NormalTile))
		{
			tileColorCount[(int)tile.TileColor]--;
			tileTotalCount--;
			colorProbability[genericColorsIndexes[(int)tile.TileColor]] = 1f - (tileTotalCount == 0 ? 0f : tileColorCount[(int)tile.TileColor] / tileTotalCount);
		}
	}
	
	protected void OnDestroy()
	{
		instance = null;
		
		NormalTile.OnTileInitAfterAttachedToBoard -= OnTileAttachedToBoardEvent;
		NormalTile.OnAnyTileDestroyed -= OnAnyTileDestroyedEvent;
		Match3BoardRenderer.OnRandomGenericColorInitialized -= OnBoardRendererInitEvent;
		Match3BoardRenderer.OnBoardFinishedSetup -= OnBoardFinishedSetupEvent;
	}
	
	protected void OnBoardFinishedSetupEvent(AbstractBoardRenderer sender)
	{
		isEnabled = true;
	}
	
	public int GetRandomTileColor()
	{	
		if(!isEnabled)
		{
			return Random.Range(0, Match3BoardRenderer.maxNumBoardColors);
		}
		
		colorProbabilitySum = 0f;
//		Debug.LogError("[GetRandomColor] maxNumBoardColors: " + Match3BoardRenderer.maxNumBoardColors);
		
		for(int i = 0; i < Match3BoardRenderer.maxNumBoardColors; i++)
		{
			colorProbabilitySum += colorProbability[i];
		}
		
		float randomPoint = Random.value * colorProbabilitySum;
//		Debug.LogError("[GetRandomColor]ProbabilitySum: " + randomPoint);
		
		for(int i = 0; i < Match3BoardRenderer.maxNumBoardColors; i++)
		{
			if(randomPoint < colorProbability[i])
			{
//				Debug.LogError("Returning: " + RuleEntry.genericColors[i]);
				return i;
			}
			else
			{
				randomPoint -= colorProbability[i];
			}
		}
		
//		Debug.LogError("--Fallback--");
		
		//This will never get called
		return 0;
	}
}
