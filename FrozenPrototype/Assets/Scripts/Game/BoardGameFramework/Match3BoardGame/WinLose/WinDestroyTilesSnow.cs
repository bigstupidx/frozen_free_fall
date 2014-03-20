using UnityEngine;
using System.Collections;

public class WinDestroyTilesSnow : WinDestroyTiles {
	
	[System.NonSerialized]
	public int nrOfSnowTiles;
	
	protected override void Awake()
	{
		base.Awake();
//		nrOfSnowTiles = 0;
		
		destroyTiles[0].current = 0;
		destroyTiles[0].number = 0;
		
		SnowTile.OnSnowTileInit += ActionOnSnowTileInit;
		SnowTile.OnSnowWinDestroyCondition += ActionOnSnowWinDestroyCondition;
	}
	
//	public override bool Check()
//	{
//		return base.Check() && SnowDestroyed();
//	}
//	
//	protected bool SnowDestroyed()
//	{
//		return nrOfSnowTiles == 0 ? true : false;
//	}
	
	protected void ActionOnSnowTileInit(SnowTile sender) 
	{
		destroyTiles[0].number += sender.numberOfSnowLayers;
	}
	
	protected void ActionOnSnowWinDestroyCondition(SnowTile sender) 
	{
		destroyTiles[0].current++;
		
		CheckWin();
	}
	
	public override string GetLoseReason()
	{
//		if (SnowDestroyed()) {
		if (AllDestroyed()) {
			return base.GetLoseReason();
		}
		else {
			return Language.Get("LOSE_SNOW");
			//return "You didn't destroy\nall the snow tiles.";
		}
	}
	
	public override string GetObjectiveString()
	{
		return Language.Get("GAME_OBJECTIVE_SNOW");
		//return "You have to destroy\nall the snow tiles.";
	}
	
	public override string GetShortObjectiveString(AbstractLoseCondition loseConditions)
	{
		return Language.Get("MAP_OBJECTIVE_SNOW");
	}
	
	public override string GetLevelType (AbstractLoseCondition loseConditions)
	{
		return "Snow";
	}
	
	protected override void OnDestroy() 
	{
		SnowTile.OnSnowTileInit -= ActionOnSnowTileInit;
		SnowTile.OnSnowWinDestroyCondition -= ActionOnSnowWinDestroyCondition;
//		SnowTile.OnSnowTileDestroyed -= ActionOnSnowTileDestroyed;
		base.OnDestroy();
	}
}
