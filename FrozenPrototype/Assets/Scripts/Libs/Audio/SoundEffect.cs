using UnityEngine;
using System.Collections;

[System.Serializable]
public class SoundEffect
{
	public string id;
	public AudioClip audioClip;
	
	//TODO:quick hack to support random audio clip playing for same sound effect
	public AudioClip[] randomAudioClips;
	
	/// <summary>
	/// The max audio sources. You can also set this to zero in which case this sound effect won't allocate it's own AudioSource buffer
	/// and could only be used with the sound managers PlayOneShot methods. (useful for reducing the number of allocated AudioSources but with less control
	/// over the sound effect).
	/// </summary>
	public int maxAudioSources = 1;
	
	public bool loop = false;
	public int priority = 128;
	public float volume = 1f;
	public float pitch = 1f;
	public float pan = 0f;
	
	/// <summary>
	/// The delay when playing the same sound in the same frame.
	/// </summary>
	public float multiplePlayDelay = 0.05f;
	
	public float delayUntilNextPlay = 0f;

	[System.NonSerialized]
	public float defaultVolume = 1f;
	
	[System.NonSerialized]
	public float defaultPitch = 1f;
	
	[System.NonSerialized]
	public float defaultPan = 0f;

	
	public SoundEffect()
	{
		maxAudioSources = 1;

		priority = 128;
		volume = 1f;
		pitch = 1f;
		pan = 0f;
	}

	public SoundEffect(AudioClip _audioClip) : this() 
	{
		audioClip = _audioClip;
		
		if (audioClip != null) {
			id = audioClip.name;
		}
	}
	
	public SoundEffect(AudioClip _audioClip, string _id) : this(_audioClip)
	{
		id = _id;
	}
	
	public void ResetDefaults()
	{
		defaultVolume = volume;
		defaultPitch = pitch;
		defaultPan = pan;
	}
}
