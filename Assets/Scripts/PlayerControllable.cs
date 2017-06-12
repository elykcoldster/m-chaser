using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllable : MonoBehaviour {

	public float maxHealth = 100f, health, armor = 25f;
	public float moveSpeed = 4.0f;
	public float jumpSpeed = 5.0f;
	public float fireRate = 20.0f;
	public float crouchMultiplier = 0f;
	public float attackDamage = 10f;
	public Transform groundCheck;

	protected Animator anim;
	protected BoxCollider2D bc;
	protected Rigidbody2D rb;
	protected SpriteRenderer sr;

	protected bool damaged, grounded, forwardJump, invulnerable, regainControl, death;
	protected int groundLayer;

	// Use this for initialization
	protected void Start () {
		anim = GetComponentInChildren<Animator> ();
		health = maxHealth;
		bc = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponentInChildren<SpriteRenderer> ();

		groundLayer = 1 << LayerMask.NameToLayer ("Ground");
	}
	
	// Update is called once per frame
	protected void Update () {
		Move (transform.tag);
		Jump (transform.tag);
		DamageCheck ();
	}

	protected void FixedUpdate() {
		GroundCheck ();
	}

	protected void LateUpdate() {
		BCUpdate ();
	}

	void Move(string tag) {
		string axis = "";
		if (tag == "Runner") {
			axis = "Horizontal_Runner";
		} else if (tag == "Chaser") {
			axis = "Horizontal_Chaser";
		}
		float h = Input.GetAxis (axis);
		if (!damaged) {
			if (h > 0f) {
				sr.flipX = false;
			} else if (h < 0f) {
				sr.flipX = true;
			}
		}

		if (!damaged) {
			rb.velocity = new Vector2 (h * moveSpeed * crouchMultiplier, rb.velocity.y);
		}
		anim.SetBool ("run", Mathf.Abs(rb.velocity.x) > 0f);
	}

	void Jump(string tag) {
		string axis = "";
		if (tag == "Runner") {
			axis = "Vertical_Runner";
		} else if (tag == "Chaser") {
			axis = "Vertical_Chaser";
		}
		if (grounded && Input.GetButtonDown(axis) && Input.GetAxis(axis) > 0f && !damaged) {
			rb.AddForce (Vector2.up * jumpSpeed, ForceMode2D.Impulse);
			if (Mathf.Abs(Input.GetAxis(axis)) > 0f) {
				forwardJump = true;
				StartCoroutine (FJTimeOut (0.1f));
			} else {
				forwardJump = false;
			}
		}
	}

	void DamageCheck() {
		if (!death) {
			anim.SetBool ("damaged", damaged);
		}
		if (regainControl && grounded) {
			damaged = false;
		}
	}

	void GroundCheck() {
		grounded = Physics2D.OverlapBox (groundCheck.position, new Vector2 (2 * sr.bounds.extents.x, 0.01f), 0.0f, groundLayer);

		if (grounded) {
			anim.SetBool ("floating", false);
			anim.SetBool ("spin_jump", false);
		} else {
			anim.SetBool ("floating", true);
			if (forwardJump) {
				anim.SetBool ("spin_jump", true);
			}
		}
	}

	void BCUpdate() {
		bc.offset = Vector2.up * sr.bounds.extents.y;
		bc.size = new Vector2 (2 * sr.bounds.extents.x, 2 * sr.bounds.extents.y);
	}

	public void Damage(float damage) {
		this.damaged = true;
		this.regainControl = false;

		float damageTaken = Mathf.Round (damage * (1 - armor / (100 + armor)));
		health -= damageTaken;
		if (health > 0f) {
			StartCoroutine (RegainControl (0.1f));
			StartCoroutine (Invulnerable (Global.instance.invulerableTime));
		} else {
			death = true;
			StartCoroutine (DeathAnimation (0.1f));
		}
	}

	public void KnockBack(Vector2 force) {
		rb.velocity = Vector2.zero;
		rb.AddForce (force, ForceMode2D.Impulse);
		sr.flipX = force.x < 0f ? false: true;
	}

	public float AttackDamage() {
		return this.attackDamage;
	}

	IEnumerator RegainControl(float t) {
		yield return new WaitForSeconds (t);
		this.regainControl = true;
	}

	IEnumerator FJTimeOut(float time) {
		yield return new WaitForSeconds (time);
		forwardJump = false;
	}

	IEnumerator DeathAnimation(float t) {
		yield return new WaitForSeconds (t);
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;
		bc.enabled = false;
		anim.SetTrigger ("death");
		anim.SetBool ("damaged", false);
	}

	IEnumerator Invulnerable(float t) {
		invulnerable = true;
		int monsterLayer = LayerMask.NameToLayer ("Monster");

		StartCoroutine (FlashSprite (Global.instance.invulnerableFlashingInterval));

		Physics2D.IgnoreLayerCollision (gameObject.layer, gameObject.layer, true);
		Physics2D.IgnoreLayerCollision (gameObject.layer, monsterLayer, true);

		yield return new WaitForSeconds (t);

		invulnerable = false;

		Physics2D.IgnoreLayerCollision (gameObject.layer, gameObject.layer, false);
		Physics2D.IgnoreLayerCollision (gameObject.layer, monsterLayer, false);
	}

	IEnumerator FlashSprite(float t) {
		if (invulnerable) {
			sr.enabled = !sr.enabled;
			yield return new WaitForSeconds (t);
			StartCoroutine (FlashSprite (t));
		} else {
			sr.enabled = true;
		}
	}
}
