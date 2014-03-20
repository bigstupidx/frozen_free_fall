using UnityEngine;
using System.Collections;

public class SpecialCharacterCameraTransform : MonoBehaviour 
{
	public Transform[] transforms;
	
	// Use this for initialization
	void Awake () 
	{
		int index = CharacterSpecialAnimations.characterIndex;
		
		if (index >= 0 && transforms.Length > index && transforms[index] != null)
		{
			transform.position = transforms[index].position;
			transform.rotation = transforms[index].rotation;
			transform.localScale = transforms[index].localScale;
		}
	}
}
