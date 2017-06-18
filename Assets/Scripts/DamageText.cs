using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

	public float timeOut = 1f;
	public float floatSpeed = 1f;

	Camera cam;
	LRScreen screen;
	Transform target;
	Text text;
	Vector3 position;

	RectTransform rect;

	float damage, displacement;

	// Use this for initialization
	void Start () {
		displacement = 0f;

		text = GetComponent<Text> ();
		text.text = ((int)damage).ToString ();
		rect = text.GetComponent<RectTransform> ();

		StartCoroutine (DestroyAfterSeconds (timeOut));
	}
	
	// Update is called once per frame
	void Update () {
		// Spatial Position
		displacement += Time.deltaTime * floatSpeed;
		if (target != null) {
			position = target.position + target.GetComponentInChildren<SpriteRenderer> ().bounds.extents.y * 2f * Vector3.up;
			rect.position = position;
			rect.position += Vector3.up * displacement;
		} else {
			rect.position = position;
			rect.position += Vector3.up * displacement;
		}

		// Color alpha
		Color c = text.color;
		c.a -= 1 / timeOut * Time.deltaTime;
		text.color = c;
	}

	public void Initialize(Transform target, GameObject screen, float damage) {
		this.cam = screen.GetComponent<LRScreen> ().camera;
		this.screen = screen.GetComponent<LRScreen> ();
		this.target = target;

		this.damage = damage;
	}

	IEnumerator DestroyAfterSeconds(float time) {
		yield return new WaitForSeconds (time);
		Destroy (gameObject);
	}
}
