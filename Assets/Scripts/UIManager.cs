using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	private static UIManager _single;
	public static UIManager single {
		get {
			return _single;
		}
	}

	public Text shootNum;
	public Text scoreNum;

	public int shootInt;
	public int scoreInt;

	public Toggle musicToggle;
	public AudioSource musicBg;

	public Text msgText;

	void Awake() {
		_single = this;
		if (PlayerPrefs.HasKey ("MusicOn")) {
			if (PlayerPrefs.GetInt ("MusicOn") == 1) {
				musicToggle.isOn = true;
				musicBg.enabled = true;
			} else {
				musicToggle.isOn = false;
				musicBg.enabled = false;
			}
		} else {
			musicToggle.isOn = true;
			musicBg.enabled = true;
		}
	}

	void Update() {
	}

	public void musicSwitch() {
		if (!musicToggle.isOn) {
			musicBg.enabled = false;
			PlayerPrefs.SetInt ("MusicOn", 0);
		} else {
			musicBg.enabled = true;
			PlayerPrefs.SetInt ("MusicOn", 1);
		}
		PlayerPrefs.Save ();
	}

	public void addShootNum () {
		shootInt++;
	}

	public void addScoreNum() {
		scoreInt++;
	}

	public void showNum() {
		shootNum.text = "射击数："+shootInt.ToString ();
		scoreNum.text = "得分："+scoreInt.ToString ();
	}

	public void showMsg(string msg) {
		msgText.text = msg;
	}

}
