using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class Carrot : BasicItem {
	
	protected Transform shakeTarget;
	
	public int nrOfTilesPerTurn = 3;
	public int nrOfTurns = 3;
	public float shakeDuration = 0.3f;
	
	public float delayBetweenTurns = 1f;
	
	protected List<NormalTile> tileList;

	
	public override string ItemName {
		get {
			return "Carrot";
		}
	}

	public override void StartUsingItem(Match3BoardGameLogic _boardLogic)
	{
		base.StartUsingItem(_boardLogic);
		
		tileList = new List<NormalTile>(10);
		shakeTarget = Match3BoardGameLogic.Instance.match3Board;
		
//		TileSwitchInput.Instance.gameObject.SetActive(false);
		TileSwitchInput.Instance.DisableInput();
		
		if(tileList.Count != 0)
		{
			tileList.Clear();
		}
		
		PopulateTileList();
		DoItem();
	}
	
	public override void CancelUsingItem()
	{
//		TileSwitchInput.Instance.gameObject.SetActive(true);
		TileSwitchInput.Instance.EnableInput();
		
		base.CancelUsingItem();
	}
	
	protected override void DoItem()
	{
		ActuallyUsingItem();
		StartCoroutine(CarrotBehaviour());
	}
	
	protected IEnumerator CarrotBehaviour()
	{	
		for(int turnIndex = 0; turnIndex < nrOfTurns; turnIndex++)
		{
			yield return new WaitForSeconds(delayBetweenTurns);
			
//			Debug.LogError("EffectPosition: " + effectPosition.name);
			HOTween.Shake(shakeTarget, shakeDuration, new TweenParms().Prop("position", shakeTarget.position + Vector3.down * 0.25f), 1f, 1f);
			
			for(int i = 0 ; i < effectPosition.childCount; i++)
			{
				Transform t = effectPosition.GetChild(i);
				HOTween.Shake(t, shakeDuration, new TweenParms().Prop("position", t.position + Vector3.down * 0.06f), 0.5f, 1f);
			}
			
			for(int tileIndex = 0; tileIndex < nrOfTilesPerTurn; tileIndex++)
			{
				int randomTileIndex = Random.Range(0, tileList.Count);
				tileList[randomTileIndex].Destroy();
				tileList.RemoveAt(randomTileIndex);
			}	
		}
		
		base.DoItem();
		
//		TileSwitchInput.Instance.gameObject.SetActive(true);
		TileSwitchInput.Instance.EnableInput();
		
		DoDestroy();
	}
	
	protected void PopulateTileList()
	{
		Match3BoardGameLogic.Instance.boardData.ApplyActionToAll((boardPiece) => {
			NormalTile tile = boardPiece.Tile as  NormalTile;
			
			if(tile && tile.GetType() == typeof(NormalTile))
			{
				tileList.Add(tile);
			}
		});
	}
}
