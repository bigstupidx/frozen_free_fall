using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

[ActionCategory(ActionCategory.GameObject)]
[Tooltip("Destroys a Game Object immediately (optional).")]
public class DestroyObjectImmediate : DestroyObject 
{
	[Tooltip("Destroy the object immediately.")]
	public FsmBool immediate;
	
	public override void OnEnter()
	{
		if (immediate.Value) {
			GameObject.DestroyImmediate(gameObject.Value);
		}
		else {
			base.OnEnter();
		}
	}
}
