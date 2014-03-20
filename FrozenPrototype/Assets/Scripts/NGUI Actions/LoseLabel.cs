using UnityEngine;
using System.Collections;

public class LoseLabel : MonoBehaviour
{
	public Match3BoardGameLogic gameLogic;
	public UILabel questionLabel;
	public UILabel itemsLabel;
	public UISprite itemsBg;
	public UISprite itemsIcon;
	public ItemHolder movesTimeItem;
	
	public PlayMakerFSM fsm;
	public string acceptEvent = "AcceptOffer";
	public string acceptPovertyEvent = "AcceptOfferAsAPoorMF";
	
	UILabel label;
	
	void Start () 
	{
		label = GetComponent<UILabel>();
		label.text = gameLogic.loseConditions.GetLoseString();
		
		if (questionLabel != null) {
			if (gameLogic.loseConditions is LoseMoves) {
				questionLabel.text = Language.Get("LOSE_CONTINUE_MOVES");
				itemsIcon.spriteName = "gui_powerup_icon2";
			}
			else {
				questionLabel.text = Language.Get("LOSE_CONTINUE_TIME");
				itemsIcon.spriteName = "gui_powerup_icon10";
			}
		}
	}
	
	public void ShowOffer()
	{
		UpdateItem();
	}
	
	public void UpdateItem()
	{
		itemsLabel.text = movesTimeItem.itemCount.ToString();
		itemsBg.spriteName = movesTimeItem.itemCount > 0 ? "gui_powerup_bg" : "gui_powerup_bgon";
	}
	
	public void AcceptOffer()
	{
		if (movesTimeItem.itemCount > 0)
		{
			movesTimeItem.OnClick();
			//movesTimeItem.AddItems(-1);
			UpdateItem();
			
//			if (gameLogic.loseConditions is LoseMoves) {
//				(gameLogic.loseConditions as LoseMoves).RemainingMoves = movesTimeItem.itemPrefab.GetComponent<Snowball>().extraMoves;
//			}
//			else {
//				(gameLogic.loseConditions as LoseTimer).RemainingTime = movesTimeItem.itemPrefab.GetComponent<Hourglass>().extraTime;
//			}
			
			gameLogic.IsGameOver = false;
			gameLogic.TryCheckStableBoard();
			
			fsm.SendEvent(acceptEvent);
		}
		else {
			BuyItemHolder.SetSelectedItem(movesTimeItem, true);
			fsm.SendEvent(acceptPovertyEvent);
		}
	}
}

