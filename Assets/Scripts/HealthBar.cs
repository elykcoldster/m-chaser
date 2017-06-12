using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Slider))]
public class HealthBar : MonoBehaviour {

	public PlayerControllable pc;

	Slider slider;

	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		float healthValue = Mathf.Clamp(pc.health / pc.maxHealth, 0f, 1f);
		slider.value = Mathf.Lerp (slider.value, healthValue, 5f * Time.deltaTime);
	}
}
