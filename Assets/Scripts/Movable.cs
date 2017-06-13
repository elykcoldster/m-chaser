using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour {

	public float moveSpeed = 1.0f;
	public bool flippable;

	protected Animator anim;
	protected Rigidbody2D rb;
	protected SpriteRenderer sr;
	protected float xvel;
	protected bool dead, moving;

	float timeToMove, moveTimer;

	// Use this for initialization
	protected void Start () {
		if (GetComponentInChildren<Animator> ()) {
			anim = GetComponentInChildren<Animator>();
		}
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponentInChildren<SpriteRenderer> ();
		moving = false;
		moveTimer = 0f;
		timeToMove = 0f;
	}
	
	// Update is called once per frame
	protected void Update () {
		if (anim) {
			anim.SetBool ("walk", moving);
		}

		if (moving) {
			moveTimer += Time.deltaTime;

			if (moveTimer >= timeToMove) {
				moveTimer = 0f;
				Stop ();
			}
		}
	}

	public void Move(Vector3 destination) {
		if (!dead) {
			moving = true;
			float time = Mathf.Abs ((destination - transform.position).x) / moveSpeed;

			xvel = Mathf.Sign ((destination - transform.position).x) * moveSpeed;
			rb.velocity = new Vector2 (xvel, rb.velocity.y);
			sr.flipX = flippable ? (destination - transform.position).x > 0f : false;

			timeToMove = time;
		}
	}

	void Stop() {
		rb.velocity = Vector3.zero;
		moving = false;
	}
}
