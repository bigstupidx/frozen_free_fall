using UnityEngine;
using System.Collections;

public class PlayAnimation : MonoBehaviour
{
	public void PlayTheAnimation()
	{
		//Debug.LogWarning("CANTA FUTU-TI MORTII MATII!");
		animation.Stop();
		animation.clip = animation["PauseFadeOut"].clip;
		animation["PauseFadeOut"].normalizedSpeed = 1f;
		animation.Play("PauseFadeOut");
		StartCoroutine(CEMORTIIMAAAAATIIIIIII());
	}
	
	IEnumerator CEMORTIIMAAAAATIIIIIII()
	{
		while (animation["PauseFadeOut"].time < animation["PauseFadeOut"].length) {
			animation["PauseFadeOut"].time += Time.deltaTime;
			animation.Sample();
//			Debug.Log(animation["PauseFadeOut"].normalizedTime);
//			Debug.Log(animation["PauseFadeOut"].normalizedSpeed);
			yield return null;
		}
	}
}

