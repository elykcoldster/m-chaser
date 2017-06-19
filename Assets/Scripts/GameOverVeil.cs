using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverVeil : MonoBehaviour {

	public GameObject gameOverText;

	float fadeTime = -1f;
	Image img;

	// Use this for initialization
	void Start () {
		img = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeTime > 0f) {
			Color c = img.color;
			c.a += Time.deltaTime * (1 / fadeTime);
			c.a = Mathf.Min (1f, c.a);
			img.color = c;
		}
	}

	public void Fade(float t) {
		fadeTime = t;
		StartCoroutine (GameOverTextAppear (fadeTime / 2));
	}

	IEnumerator GameOverTextAppear(float t) {
		yield return new WaitForSeconds (t);
		GameOverText got = ((GameObject)Instantiate (gameOverText, transform)).GetComponent<GameOverText> ();
		got.GetComponent<RectTransform> ().localPosition = new Vector3 (0.5f, 0.5f, 0f);
		got.Fade (t);
	}
}
