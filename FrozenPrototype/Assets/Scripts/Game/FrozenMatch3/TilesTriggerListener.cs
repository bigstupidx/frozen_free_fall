using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class TilesTriggerListener : MonoBehaviour {
	public System.Action<NormalTile> OnTileEntered;
	public System.Action<Match3BoardPiece> OnBoardPieceEntered;
	
	private NormalTile tileComponent;
	private Match3BoardPiece boardPieceComponent;
	
	void Start() { }
	
	/// <summary>
	/// Raises the trigger enter Unity event.
	/// </summary>
	/// <param name='other'>
	/// The collider that entered this trigger.
	/// </param>
	void OnTriggerEnter(Collider other)
	{
		if (!enabled)  //|| other.gameObject.layer != Match3Globals.Instance.layerBoardTile
		{
			return;
		}
		
		tileComponent = other.GetComponent<NormalTile>();
		
		if (OnTileEntered != null && tileComponent != null)
		{
			OnTileEntered(tileComponent);
			return;
		}
		
		boardPieceComponent = other.GetComponent<Match3BoardPiece>();
		
		if (OnBoardPieceEntered != null && boardPieceComponent != null)
		{
			OnBoardPieceEntered(boardPieceComponent);
		}
	}
}
