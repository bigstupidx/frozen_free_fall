using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class SlideInAnimation : MonoBehaviour
{
	public Transform targetContainerLandscape;
	public Transform targetContainerPortrait;
	protected Transform cachedTransform;
	
	public Camera myCamera;
	public Camera landscapeCamera;
	public Camera portraitCamera;
	
	protected Vector3 initPos;
	protected Vector3 landscapePos;
	protected Vector3 portraitPos;
	
	protected Vector3 initLocalPos;
	protected Vector3 landscapeLocalPos;
	protected Vector3 portraitLocalPos;
	
	public float animationTime = 1f;
	public int startPos = 2000;
	
	void Start()
	{
		startPos = Mathf.CeilToInt(startPos * (Mathf.Max(Screen.width, Screen.height) / 1024f));
		cachedTransform = transform;
		
		initLocalPos = cachedTransform.localPosition;
		landscapeLocalPos = targetContainerLandscape.localPosition;
		portraitLocalPos = targetContainerPortrait.localPosition;
		
		initPos = cachedTransform.position;
		landscapePos = targetContainerLandscape.position;
		portraitPos = targetContainerPortrait.position;
		
		float initViewY = myCamera.WorldToViewportPoint(cachedTransform.position).y;
		float landscapeViewY = landscapeCamera.WorldToViewportPoint(targetContainerLandscape.position).y;
		float portraitViewY = portraitCamera.WorldToViewportPoint(targetContainerPortrait.position).y;
		
		Vector3 pos = new Vector3((float)startPos / Screen.width, initViewY, 0f);
		Vector3 posL = new Vector3((float)startPos / Screen.width, landscapeViewY, 0f);
		Vector3 posP = new Vector3((float)startPos / Screen.width, portraitViewY, 0f);
		
		cachedTransform.position = myCamera.ViewportToWorldPoint(pos);
		cachedTransform.position = SetZ(cachedTransform.position, initPos.z);
		targetContainerLandscape.position = landscapeCamera.ViewportToWorldPoint(posL);
		targetContainerLandscape.position = SetZ(targetContainerLandscape.position, landscapePos.z);
		targetContainerPortrait.position = portraitCamera.ViewportToWorldPoint(posP);
		targetContainerPortrait.position = SetZ(targetContainerPortrait.position, portraitPos.z);
	}
	
	Vector3 SetZ(Vector3 v, float z)
	{
		v.z = z;
		return v;
	}
	
	void SlideIn()
	{
//		HOTween.To(cachedTransform, animationTime, new TweenParms().Prop("position", initPos).Ease(EaseType.Linear));
//		HOTween.To(targetContainerLandscape, animationTime, new TweenParms().Prop("position", landscapePos).Ease(EaseType.Linear));
//		HOTween.To(targetContainerPortrait, animationTime, new TweenParms().Prop("position", portraitPos).Ease(EaseType.Linear));
		cachedTransform.localPosition = initLocalPos;
		targetContainerLandscape.localPosition = landscapeLocalPos;
		targetContainerPortrait.localPosition = portraitLocalPos;
	}
}

