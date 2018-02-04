using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

	void Start () {
		StartCoroutine (destorySelf());
	}

	IEnumerator destorySelf() {
		yield return new WaitForSeconds (2f);
		Destroy (gameObject);
	}
}
