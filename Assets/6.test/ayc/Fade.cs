using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Fade : MonoBehaviour {

	public Image img;
	public AnimationCurve curve;
	public GameObject fadePrefab;

	void Start ()
	{
		StartCoroutine(FadeIn());
	}

	public void FadeTo (string scene)
	{
		StartCoroutine(FadeOut(scene));
	}

	IEnumerator FadeIn()
	{
		float t = 1f;

		while (t > 0f)
		{
			t -= Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color (0f, 0f, 0f, a);
			if(a == 0) {
				fadePrefab.SetActive(false);
			}
			yield return 0;
		}
	}

	IEnumerator FadeOut(string scene)
	{
		fadePrefab.SetActive(true);

		float t = 0f;

		while (t < 1f)
		{
			t += Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}

		SceneManager.LoadScene(scene);
	}
}