using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TextBlink : MonoBehaviour {

	public Text anyText;
    public string textMassage;

	void Start()
    {
		anyText = GetComponent<Text>();
		StartCoroutine(BlinkText());
	}

	void Update()
	{
		if(FakeScript.firstLoading) {
			return;
		}
		else {
			if(Input.GetMouseButtonDown(0)) {
				SceneManager.LoadScene("2.Title");
			}
		}
	}

	public IEnumerator BlinkText(){
		while(true) {
			anyText.text = "";
			yield return new WaitForSeconds (0.5f);
			anyText.text = textMassage;
			yield return new WaitForSeconds (0.5f);
		}
	}
}