using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour {

	public Transform parent;
	public float speed;
	public float distance;

	Rigidbody2D rb;
	float ttl; // time to live

	// Use this for initialization
	void Start () {
		ttl = distance / speed;
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = transform.right * speed;
		Destroy (gameObject, ttl);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.transform != parent) {
			if (c.tag == "Monster") {
				Monster m = c.GetComponent<Monster> ();
				m.TakeDamage (GetPC ().AttackDamage (), parent);
				if (m.health <= 0f) {
					m.Death ();
				}
			}
			Destroy (gameObject);
		}
	}

	public PlayerControllable GetPC() {
		return this.parent.GetComponent<PlayerControllable> ();
	}
}
