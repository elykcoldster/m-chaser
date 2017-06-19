using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserController : PlayerControllable {

	public Transform straightProjectileSpawn, straightCrouchProjectileSpawn, straightJumpProjectSpawn;

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
	}

	void Crouch() {
		if (grounded && Input.GetButton ("Crouch_Chaser")) {
			anim.SetBool ("crouch", true);
			crouchMultiplier = 0f;
		} else {
			anim.SetBool ("crouch", false);
			crouchMultiplier = 1.0f;
		}
	}

	void Shoot() {
		if (Input.GetButtonDown ("Fire Chaser") && fireTimer >= (1 / fireRate)) {
			anim.SetBool ("spin_jump", false);
			forwardJump = false;

			Quaternion rot = sr.flipX ? Quaternion.Euler (0f, 0f, 180f) : Quaternion.identity;
			Vector3 spawnLoc = GetSpawnLocation ();
			BeamController beam = ((GameObject)Instantiate (beamObject, spawnLoc, rot)).GetComponent<BeamController> ();

			beam.parent = transform;
			beam.speed = fireSpeed;
			beam.distance = fireDistance;
			fireTimer = 0f;
		}
		if (fireTimer < 1 / fireRate) {
			fireTimer += Time.deltaTime;
		}
	}

	void Jump() {
		if (grounded && Input.GetButtonDown("Vertical_Chaser") && Input.GetAxis("Vertical_Chaser") > 0f) {
			rb.AddForce (Vector2.up * jumpSpeed, ForceMode2D.Impulse);
			if (Mathf.Abs(Input.GetAxis("Horizontal_Chaser")) > 0f) {
				forwardJump = true;
				StartCoroutine (FJTimeOut (0.1f));
			} else {
				forwardJump = false;
			}
		}
	}
		
	Vector3 GetSpawnLocation() {
		Transform referencePosition;
		if (anim.GetBool ("crouch")) {
			referencePosition = straightCrouchProjectileSpawn;
		} else if (anim.GetBool ("floating")) {
			referencePosition = straightJumpProjectSpawn;
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
