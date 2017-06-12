using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Movable {

	public Transform groundCheck;
	public float maxHealth = 100f, health = 100f, armor = 25f;
	public float attackDamage = 10f;
	public float invulnerableTime = 1f;
	public Vector2 moveTime;
	public Vector2 moveDistance;
	public bool canJump;
	public float jumpSpeed = 5.0f;

	BoxCollider2D bc;
	Transform target;

	bool grounded;
	float invulnTimer, findTargetTimer;

	void Start () {
		base.Start ();

		if (moveTime.y == 0f) {
			moveTime = Vector2.one * Mathf.Infinity;
		}

		bc = GetComponent<BoxCollider2D> ();
		health = maxHealth;
		invulnTimer = invulnerableTime;
		findTargetTimer = 0f;

		RandomWalk ();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();

		FollowTarget ();

		invulnTimer += Time.deltaTime;
		invulnTimer = Mathf.Min (invulnTimer, invulnerableTime);
	}

	void LateUpdate() {
		DetectGround ();
		DetectWall ();
	}

	void FollowTarget() {
		findTargetTimer += Time.deltaTime;
		if (target != null && findTargetTimer > Global.instance.updateRate) {
			Move (target.position);
			findTargetTimer = 0f;
		}
	}

	void DetectGround() {
		int groundLayer = 1 << LayerMask.NameToLayer ("Ground");
		grounded = Physics2D.OverlapBox (groundCheck.position, new Vector2 (2 * sr.bounds.extents.x, 0.01f), 0.0f, groundLayer);
	}

	void DetectWall() {
		int layerMask = 1 << LayerMask.NameToLayer ("Ground");
		Vector2 boxPos = new Vector2 (transform.position.x + (sr.flipX ? moveSpeed / 4 : -moveSpeed / 4), transform.position.y + sr.bounds.extents.y);
		Collider2D c = Physics2D.OverlapBox(boxPos, new Vector2 (1.5f, 0.5f), 0f, layerMask);
		if (c && c.tag == "Platform") {
			if (grounded) {
				Jump ();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D c) {
		if (c.transform.tag == "Chaser" || c.transform.tag == "Runner") {
			Vector2 dir = (c.transform.position - transform.position + Vector3.up).normalized;

			PlayerControllable pc = c.gameObject.GetComponent<PlayerControllable> ();
			pc.Damage (attackDamage);
			pc.KnockBack(dir * 10f);
		}
	}

	public void RandomWalk() {
		if (target == null) {
			float time = moveTime.x + Random.value * (moveTime.y - moveTime.x);
			StartCoroutine (MoveAfterTime (time));
		}
	}

	public void Jump() {
		if (!dead && grounded) {
			rb.velocity = new Vector2 (xvel, jumpSpeed);
		}
	}

	public void TakeDamage(float damage, Transform source) {
		if (health == maxHealth && damage > 0f) {
			MonsterHealthBar mhb = ((GameObject)Instantiate (Global.instance.monsterHealthBar, transform.GetComponentInParent<Screen> ().UIObject.transform)).GetComponent<MonsterHealthBar> ();
			mhb.SetTarget (transform);
		}
		if (invulnTimer >= invulnerableTime) {
			float damageTaken = Mathf.Round (damage * (1 - armor / (100 + armor)));
			invulnTimer = 0f;

			if (health > 0f) {
				this.health -= damageTaken;
				Global.instance.SpawnDamageText (damageTaken, transform, GetComponentInParent<Screen> ().gameObject);
			}

			if (target == null) {
				target = source;
			}
		}
	}

	public void Death() {
		if (!dead) {
			anim.SetTrigger ("death");
			bc.enabled = false;
			dead = true;
			rb.isKinematic = true;
			rb.velocity = Vector2.zero;
		}
	}

	IEnumerator MoveAfterTime(float t) {
		yield return new WaitForSeconds(t);
		float distance = moveDistance.x + Random.value * (moveDistance.y - moveDistance.x);
		float dir = Mathf.Sign (Random.value - 0.5f);

		if (!dead) {
			Move (transform.position + Vector3.right * distance * dir);
		}
		RandomWalk ();
	}
}
