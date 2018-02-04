using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour {

	private Animation anim;

	public AnimationClip idleClip;
	public AnimationClip dieClip;

	public AudioSource kickAudio;
	public int monsterType;

	void Awake() {
		anim = gameObject.GetComponent<Animation> ();
		anim.clip = idleClip;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Bullet") {
			Destroy (collision.gameObject);
			kickAudio.Play ();
			anim.clip = dieClip;
			anim.Play ();
			anim.GetComponent<BoxCollider> ().enabled = false;
			StartCoroutine (beActive());

			UIManager.single.addScoreNum ();
			UIManager.single.showNum ();
		}
	}

	IEnumerator beActive() {
		yield return new WaitForSeconds (0.8f);
		gameObject.GetComponentInParent<TargetManager> ().updateMonster ();
	}

	void OnDisable() {
		anim.clip = idleClip;
	}
}
