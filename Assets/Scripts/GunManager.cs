using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour {

	private float maxY = 120f;
	private float minY = 0f;
	private float maxX = 60f;
	private float minX = 0f;

	private float shootTime = 1f;
	private float shootTimer = 0f;

	public GameObject bulletGo;
	public Transform firePosition;
	public float bulletSpeed = 2000f;

	private AudioSource gunAudio;

	void Awake() {
		gunAudio = gameObject.GetComponent<AudioSource> ();
	}

	void Update() {
		if (!GameManager.single.isPaused) {
			shootTimer += Time.deltaTime;
			if (shootTimer >= shootTime) {
				if (Input.GetMouseButtonDown (0)) {
					GameObject newBullet = GameObject.Instantiate (bulletGo, firePosition.position, Quaternion.identity);
					newBullet.GetComponent<Rigidbody> ().AddForce (transform.forward * bulletSpeed);
					gameObject.GetComponent<Animation> ().Play ();
					shootTimer = 0f;

					gunAudio.Play ();

					UIManager.single.addShootNum ();
					UIManager.single.showNum ();
				}
			}

			float xPos = Input.mousePosition.x / Screen.width;
			float yPos = Input.mousePosition.y / Screen.height;
			float xAngle = -Mathf.Clamp (yPos * maxX, minX, maxX)+15;
			float yAngle = Mathf.Clamp (xPos * maxY, minY, maxY)-60;

			transform.eulerAngles = new Vector3 (xAngle, yAngle, 0);
		}

	}

}
