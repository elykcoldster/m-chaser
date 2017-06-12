using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {

	public Transform target;
	public Vector3 offset = new Vector3(0f, 2f, -10f);

	// Update is called once per frame
	void Update () {
		transform.position = target.position + offset;
	}
}
