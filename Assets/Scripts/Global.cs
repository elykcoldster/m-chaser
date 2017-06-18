using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

	public static Global instance;

	public GameObject beam, gameOverVeil, damageText, monsterHealthBar;
	public float gameOverFadeTime = 2.5f;
	public float invulerableTime = 2f, invulnerableFlashingInterval = 0.2f;
	public float updateRate = 0.5f;

	bool gameOver = false;

	void Awake() {
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}
		
	public void GameOver(GameObject loser, GameObject canvas) {
		GameObject veil = (GameObject)(Instantiate (gameOverVeil, canvas.transform));
		veil.GetComponent<GameOverVeil> ().Fade (gameOverFadeTime);
	}

	public void SpawnDamageText(float damage, Transform target, GameObject screen) {
		GameObject dmgTxt = (GameObject)Instantiate (damageText, screen.GetComponent<LRScreen>().UIObject.transform);
		DamageText DTObj = dmgTxt.GetComponent<DamageText> ();
		DTObj.Initialize (target, screen, damage);
	}
}
