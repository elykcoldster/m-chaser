using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSpriteObject : MonoBehaviour {

	public int canvasIndex = 0;

	Transform parent;

	// Use this for initialization
	void Start () {
		string canvasTag = canvasIndex == 0 ? "Left UI" : "Right UI";
		parent = transform.parent;
	}

	public void GameOver() {
		Global.instance.GameOver (parent.gameObject, GetComponentInParent<Screen>().UIObject);
	}
}
