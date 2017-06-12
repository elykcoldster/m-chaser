using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyController : PlayerControllable {

	public Transform headCheck;
	public Transform straightProjectileSpawn, straightCrawlProjectileSpawn, straightCrouchProjectileSpawn;

	float fireTimer;

	// Use this for initialization
	void Start () {
		base.Start ();
		fireTimer = 1f;
	}

	void Update() {
		base.Update ();
		Crouch ();
		Shoot ();
	}

	void FixedUpdate() {
		base.FixedUpdate ();
	}

	void LateUpdate () {
		base.LateUpdate ();
		HeadCheckUpdate ();
	}

	void Crouch() {
		if (grounded && Input.GetButton ("Crouch_Runner")) {
			anim.SetBool ("crouch", true);
			crouchMultiplier = 0.5f;
		} else {
			int layerMask = ~(1 << LayerMask.NameToLayer ("Player"));
			bool canStand = !Physics2D.OverlapBox (headCheck.position, new Vector2 (2 * sr.bounds.extents.x, 1f - 2 * sr.bounds.extents.y), 0.0f, layerMask);
			if (canStand) {
				anim.SetBool ("crouch", false);
				crouchMultiplier = 1.0f;
			}
		}
	}

	void Shoot() {
		if (Input.GetButtonDown ("Fire Runner") && fireTimer >= (1 / fireRate)) {
			anim.SetBool ("spin_jump", false);
			forwardJump = false;

			Quaternion rot = sr.flipX ? Quaternion.Euler (0f, 0f, 180f) : Quaternion.identity;
			Vector3 spawnLoc = GetSpawnLocation ();
			BeamController beam = ((GameObject)Instantiate (Global.instance.beam, spawnLoc, rot)).GetComponent<BeamController> ();

			beam.parent = transform;
			beam.speed = 20f;
			beam.distance = 10f;
			fireTimer = 0f;
		}
		if (fireTimer < 1 / fireRate) {
			fireTimer += Time.deltaTime;
		}
	}

	void Jump() {
		if (grounded && Input.GetButtonDown("Vertical_Runner") && Input.GetAxis("Vertical_Runner") > 0f) {
			rb.AddForce (Vector2.up * jumpSpeed, ForceMode2D.Impulse);
			if (Mathf.Abs (rb.velocity.x) > 0f) {
				forwardJump = true;
				StartCoroutine (FJTimeOut (0.1f));
			} else {
				forwardJump = false;
			}
		}
	}

	void HeadCheckUpdate() {
		headCheck.localPosition = Vector3.up * sr.bounds.extents.y * 2;
	}

	Vector3 GetSpawnLocation() {
		Transform referencePosition;
		if (anim.GetBool ("crouch") && anim.GetBool ("run")) {
			referencePosition = straightCrawlProjectileSpawn;
		} else if (anim.GetBool ("crouch")) {
			referencePosition = straightCrouchProjectileSpawn;
		} else {
			referencePosition = straightProjectileSpawn;
		}
		return sr.flipX ? referencePosition.position + 2 * Vector3.left * referencePosition.localPosition.x : referencePosition.position;
	}

	IEnumerator FJTimeOut(float time) {
		yield return new WaitForSeconds (time);
		forwardJump = false;
	}
}
