using UnityEngine;
using System.Collections;

public class RenderBufferSize : MonoBehaviour 
{
	protected static RenderTexture landscapeBuffer;
	protected static RenderTexture portraitBuffer;
	
	public Camera[] cameras;
	public Renderer[] renderers;
	public Transform[] sizes;
	

	void Awake () 
	{
		UpdateBuffers();
	}
	
	void Start()
	{
		UpdateSize();
		
		OrientationListener.Instance.OnOrientationChanged += OrientationChanged;
	}
	
	void UpdateBuffers()
	{
		int width = Mathf.Max(Screen.width, Screen.height);
		int height = Mathf.Min(Screen.width, Screen.height);
		
		if (landscapeBuffer == null || width != landscapeBuffer.width || height != landscapeBuffer.height) {
			if (landscapeBuffer != null) {
				Destroy(landscapeBuffer);
			}
			landscapeBuffer = new RenderTexture(width, height, 32, RenderTextureFormat.ARGB32);
			landscapeBuffer.Create();
		}
		if (portraitBuffer == null || height != portraitBuffer.width || width != portraitBuffer.height) {
			if (portraitBuffer != null) {
				Destroy(portraitBuffer);
			}
			portraitBuffer = new RenderTexture(height, width, 32, RenderTextureFormat.ARGB32);
			portraitBuffer.Create();
		}
	}
	
	public void OrientationChanged(ScreenOrientation newOrientation) 
	{
		//StartCoroutine(UpdateSizeNextFrame());
		UpdateSize();
	}
	
	IEnumerator UpdateSizeNextFrame() 
	{
		// Wait for a frame to pass to make sure the cameras have been updated
		yield return null;
		UpdateSize();
	}
	
	// Update is called once per frame
	void UpdateSize () 
	{			
		UpdateBuffers();
		
		RenderTexture bufferToUse = (Screen.width > Screen.height) ? landscapeBuffer : portraitBuffer;
		
		float aspectRatio = (float)Screen.width / (float)Screen.height;
		
		foreach (Camera cam in cameras) {
			cam.targetTexture = bufferToUse;
			if (cam.rect == new Rect(0f, 0f, 1f, 1f)) {
				cam.aspect = aspectRatio;
			}
			else {
				cam.ResetAspect();
				cam.ResetProjectionMatrix();
			}
		}
		
		foreach (Renderer render in renderers) {
			render.material.mainTexture = bufferToUse;
		}
		
		foreach (Transform xForm in sizes) {
			Camera renderCam = null;
			foreach (Camera cam in cameras) {
				if ((cam.cullingMask | xForm.gameObject.layer) != 0) {
					renderCam = cam;
					break;
				}
			}
			
			if (renderCam != null) {
				Vector3 newPos = renderCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
				newPos.z = xForm.position.z;
				xForm.position = newPos;
				
				Vector3 newSize = xForm.localScale;
				newSize.z = (renderCam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y - 
					renderCam.ViewportToWorldPoint(Vector3.zero).y) / renderCam.transform.localScale.y;
				if (renderCam.orthographic) {
					newSize.z /= renderCam.orthographicSize / 10f;
				}
				newSize.x = newSize.z * Screen.width / (float)Screen.height;
				xForm.localScale = newSize;
			}
		}
	}
	
	public void DeactivateCameras()
	{
		for (int i = 0; i < cameras.Length; ++i) {
			cameras[i].gameObject.SetActive(false);
		}
	}
}
