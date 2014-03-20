using UnityEngine;
using System.Collections;

/// <summary>
/// LayeredBoardPiece
/// 
/// Describes a board piece that has multiple layers of resistance before it can be considered a normal board piece.
/// </summary>
public class LayeredBoardPiece : Match3BoardPiece {
	public delegate void LayeredBoardPieceHandler(LayeredBoardPiece sender);
	
	public static event LayeredBoardPieceHandler OnNumLayersDecreased;
	public static event LayeredBoardPieceHandler OnNumLayersIncreased;
	
	public static event LayeredBoardPieceHandler OnLayeredBoardPieceInit;
	public static event LayeredBoardPieceHandler OnAllLayersDestroyed;
	
	protected Material[] materialArray;
	protected string resourcePath = "Game/Materials/FrostedBoardPiece/tile_frosted";
	protected int materialArraySize = 3;
	
	protected float destroyEffectTime = 2f;
	
	//Particle Effect lifetime in seconds.
	public float particleLifeTime = 1f;
	
	public GameObject FrostDestroyEffectPrefab;
	public GameObject backModel;
	
	protected Transform frostDestroyEffectInstance;
	
	[SerializeField]
	private int numLayers = 1;
	
	private int frostTextureIndex = 0;
	
	public int NumLayers {
		get { 
			return numLayers;
		}
		set {
			int lastNumLayers = numLayers;
			numLayers = value;
			
			if (lastNumLayers != numLayers) {
				RaiseNumLayersChangedEvent(lastNumLayers);
			}
		}
	}
	
	public override void Awake ()
	{
		base.Awake ();
		
		//Populate material array
		materialArray = new Material[materialArraySize];
		
		for(int i = 0; i < materialArraySize; i++)
		{
			materialArray[i] = Resources.Load(resourcePath + (materialArraySize-i)) as Material;
		}
		
		gameObject.AddComponent<BoxCollider>().center = Vector3.forward * 0.5f;
		gameObject.layer = LayerMask.NameToLayer("BoardPiece");
	}
	
	public override void InitComponent (AbstractBoardRenderer boardRenderer) {
		base.InitComponent (boardRenderer);
		
		if(OnLayeredBoardPieceInit != null) {
			OnLayeredBoardPieceInit(this);
		}
		
		frostTextureIndex = numLayers-1;

		backModel.renderer.material = materialArray[frostTextureIndex];
	
		OnTileDestroyed += OnTileDestroyedAction;
	}
	
	protected void OnTileDestroyedAction(AbstractBoardPiece sender, AbstractTile destroyedTile) {
		
		//Early out for those scenarios in witch you would not want to pop a frost tile
		if (destroyedTile is  LockedTile || destroyedTile is SnowTile || destroyedTile is FreezerTile)
		{
			return;
		}
		
		NumLayers--;
	}
	
	/// <summary>
	/// This event is called every time the <see cref="NumLayers"/> property is changed.
	/// This is where the renderer can start various specific effects for this board piece to change its appearance when
	/// the number of layers is reduced or increased.
	/// </summary>
	public virtual void RaiseNumLayersChangedEvent(int oldNumLayers) {
		
		//Shut Down LayaeredBoardPiece behaviour
		if(NumLayers < 0) {
			OnTileDestroyed -= OnTileDestroyedAction;
			return;
		}
		
		//Finished LayeredBoardPiece behaviour
		if(NumLayers == 0) {
			if(OnAllLayersDestroyed != null) {
				OnAllLayersDestroyed(this);
			}
		}
		
		if (NumLayers < oldNumLayers) {
			if (OnNumLayersDecreased != null) {
				OnNumLayersDecreased(this);
			}
			
			animation.PlayQueued("effect_frosted_destroy", QueueMode.CompleteOthers);
//			frontModel.animation.Play("tile_frosted_destroyed", PlayMode.StopAll);
		} else {
			if (OnNumLayersIncreased != null) {
				OnNumLayersIncreased(this);
			}
		}
	}
	
	//AnimationEvent called at the start of the fade out anim
	public void OnAnimationStarted() {
		PlayFrostDestroyEffectAnim(materialArray[frostTextureIndex]);
		
		if(frostTextureIndex > 0) {
			backModel.renderer.material = materialArray[frostTextureIndex-1];
		}
		
		if(frostTextureIndex == 0) {
			backModel.renderer.enabled = false;
			GameObject.Destroy(collider);
		}
	}
	
	//AnimationEvent called at the end of the fade out anim.
	public void OnAnimationFinished() {
		frostTextureIndex--;
		
		if(frostTextureIndex > 0) {
			backModel.renderer.material = materialArray[frostTextureIndex];
		}
	}
	
	protected void PlayFrostDestroyEffectAnim(Material matParam) {
		frostDestroyEffectInstance = (Instantiate(FrostDestroyEffectPrefab) as GameObject).transform;
		frostDestroyEffectInstance.parent = cachedTransform;
		frostDestroyEffectInstance.localPosition = Vector3.forward * -3f;
		GameObject.Destroy(frostDestroyEffectInstance.gameObject, destroyEffectTime);
	}
}
