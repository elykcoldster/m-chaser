using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverText : MonoBehaviour {

	float fadeTime = -1f;
	Text txt;

	// Use this for initialization
	void Start () {
		txt = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		if (fadeTime > 0f) {
			Color c = txt.color;
			c.a += Time.deltaTime * (1 / fadeTime);
			c.a = Mathf.Min (1f, c.a);
			txt.color = c;
		}
	}

	public void Fade(float t) {
		fadeTime = t;
	}
}
