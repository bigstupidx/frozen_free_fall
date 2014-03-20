using UnityEngine;
using System.Collections;

public class BasicItem : MonoBehaviour
{
	public delegate void UsingItemEvent(BasicItem item);
	
	public static event UsingItemEvent OnActuallyUsingAnyItem;
	public static event UsingItemEvent OnStartUsingAnyItem;
	
	public event UsingItemEvent OnFinishUsingItem;
	public event UsingItemEvent OnActuallyUsingItem;
	
	public GameObject effectPrefab;
	public Transform effectPosition;
	public Vector3 effectOffset;
	
	public string nameSingularKey;
	public string namePluralKey;
	
	public string iconName;
	
	protected Match3BoardGameLogic boardLogic;
	
	protected DestroyEffect destroyEffect;
	
	
	protected virtual void Awake() { }
	protected virtual void Start() { }
	
	public string NameSingular {
		get {
			return nameSingularKey;
		}
	}
	
	public virtual string ItemName 
	{
		get {
			return "";
		}
	}
	
	public string NamePlural {
		get {
			return namePluralKey;
		}
	}
	
	public virtual void StartUsingItem(Match3BoardGameLogic _boardLogic)
	{
		boardLogic = _boardLogic;		
		
		if (effectPrefab) {
			destroyEffect = effectPrefab.GetComponent<DestroyEffect>();
		}
		
		if (OnStartUsingAnyItem != null)
		{
			OnStartUsingAnyItem(this);
		}
	}
	
	public virtual void FinishUsingItem()
	{
		boardLogic.unstableLock--;
		
		if (OnFinishUsingItem != null)
		{
			OnFinishUsingItem(this);
		}
	}
	
	public virtual void ActuallyUsingItem()
	{
		boardLogic.unstableLock++;
		
//		AnalyticsBinding.LogEventGameAction(Match3BoardGameLogic.Instance.characterUsed, "use_powerup", ItemName, 
//			Match3BoardGameLogic.Instance.GetLevelType(), Match3BoardRenderer.levelIdx);
		
		if (OnActuallyUsingItem != null)
		{
			OnActuallyUsingItem(this);
		}
		
		if (OnActuallyUsingAnyItem != null)
		{
			OnActuallyUsingAnyItem(this);
		}
	}
	
	public virtual void CancelUsingItem()
	{
		DoDestroy();
	}
	
	public virtual void StartItemEffects()
	{
		ActuallyUsingItem();
		
		if (effectPrefab) {
			SpawnEffect(effectPosition.position, effectPrefab);
		}
		
		if (destroyEffect) {
			Invoke("DoItem", destroyEffect.destroyTileTime);
			Invoke("DoDestroy", destroyEffect.lifeTime);
		}
		else {
			DoItem();
			DoDestroy();
		}
	}
	
	protected Transform SpawnEffect(Vector3 initPosition, GameObject prefab) 
	{
		Transform effectInstance = (Instantiate(prefab) as GameObject).transform;
		effectInstance.position = initPosition + effectOffset;
		
		Destroy(effectInstance.gameObject, destroyEffect.lifeTime);
		
		return effectInstance;
	}
	
	protected virtual void DoItem()
	{
		FinishUsingItem();
		boardLogic.IsBoardStable = false;
		boardLogic.TryCheckStableBoard();
	}
	
	protected virtual void DoDestroy()
	{	

		Destroy(gameObject);
	}
	
	protected virtual void OnDestroy() {
		
	}
}

