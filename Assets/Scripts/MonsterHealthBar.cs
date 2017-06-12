using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour {

	Monster monster;
	Slider slider;
	Transform target;

	bool targeted;

	void Start() {
		slider = GetComponent<Slider> ();
	}

	// Update is called once per frame
	void Update () {
		if (targeted && target == null) {
			Destroy (gameObject);
		} else {
			transform.position = target.position + Vector3.up * target.GetComponentInChildren<SpriteRenderer> ().bounds.extents.y * 2f;
			slider.value = Mathf.Lerp(slider.value, monster.health / monster.maxHealth, Time.deltaTime * 5f);
		}
	}

	public void SetTarget(Transform target) {
		this.monster = target.GetComponent<Monster> ();
		this.target = target;
		this.targeted = true;
	}
}
