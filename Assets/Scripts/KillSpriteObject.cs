using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSpriteObject : MonoBehaviour {
	public void Kill() {
		Destroy (this.transform.parent.gameObject);
	}
}
