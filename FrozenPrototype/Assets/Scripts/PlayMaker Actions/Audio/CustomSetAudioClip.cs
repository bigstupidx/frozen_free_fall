// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.Audio)]
[Tooltip("Sets the Audio Clip played by the AudioSource component on a Game Object. If that audio clip is already set it won't set it again, thus avoiding to stop the audio source.")]
public class CustomSetAudioClip : FsmStateAction
{
	[RequiredField]
	[CheckForComponent(typeof(AudioSource))]
	[Tooltip("The GameObject with the AudioSource component.")]
	public FsmOwnerDefault gameObject;

	[ObjectType(typeof(AudioClip))]
	[Tooltip("The AudioClip to set.")]
	public FsmObject audioClip;

	public override void Reset()
	{
		gameObject = null;
		audioClip = null;
	}

	public override void OnEnter()
	{
		var go = Fsm.GetOwnerDefaultTarget(gameObject);
		if (go != null)
		{
			var audio = go.audio;
			if (audio != null && audio.clip != audioClip.Value)
			{
				audio.clip = audioClip.Value as AudioClip;
			}
		}
		
		Finish();
	}
}
