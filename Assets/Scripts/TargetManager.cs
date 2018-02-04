using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

	public GameObject[] monsters;
	public GameObject acMonster = null;

	public int targetPos;

	void Awake() {

	}

	void Start() {
		foreach (GameObject monster in monsters) {
			monster.GetComponent<BoxCollider> ().enabled = false;
			monster.SetActive (false);
		}
//		activeMonster ();
		StartCoroutine(aliveTimer());
	}

	private void activeMonster() {
		int index = Random.Range (0, monsters.Length);
		acMonster = monsters [index];
		acMonster.SetActive (true);
		acMonster.GetComponent<BoxCollider> ().enabled = true;
		StartCoroutine (deTimer ());
	}

	IEnumerator aliveTimer() {
		yield return new WaitForSeconds (Random.Range (1, 5));
		activeMonster ();

	}

	private void deActiveMonster() {
		if (acMonster != null) {
			acMonster.GetComponent<BoxCollider> ().enabled = false;
			acMonster.SetActive (false);
			acMonster = null;
		}
		StartCoroutine(aliveTimer());
	}

	IEnumerator deTimer() {
		yield return new WaitForSeconds (Random.Range (3, 8));
		deActiveMonster ();
	}

	public void updateMonster() {
		StopAllCoroutines ();
		if (acMonster != null) {
			acMonster.GetComponent<BoxCollider> ().enabled = false;
			acMonster.SetActive (false);
			acMonster = null;
		}
		StartCoroutine(aliveTimer());
	}

	public void activeMonsterByType(int type) {
		StopAllCoroutines ();
		if (acMonster != null) {
			acMonster.GetComponent<BoxCollider> ().enabled = false;
			acMonster.SetActive (false);
			acMonster = null;
		}
		acMonster = monsters [type];
		acMonster.SetActive (true);
		acMonster.GetComponent<BoxCollider> ().enabled = true;
		StartCoroutine (deTimer());
	}
}
