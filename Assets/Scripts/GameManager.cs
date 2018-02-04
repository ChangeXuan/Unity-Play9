using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;
using System.Xml;

public class GameManager : MonoBehaviour {

	private static GameManager _single;
	public static GameManager single {
		get {
			return _single;
		}
	}

	public bool isPaused = true;
	public GameObject menuGo;
	public GameObject[] targetsGo;

	void Awake() {
		_single = this;
		pause ();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			pause ();
		}
	}

	void play() {
		isPaused = false;
		menuGo.SetActive (false);
		Time.timeScale = 1;
		Cursor.visible = false;
	}

	void pause() {
		isPaused = true;
		menuGo.SetActive (true);
		Time.timeScale = 0;
		Cursor.visible = true;
	}

	Save createSaveGo() {
		Save save = new Save ();
		foreach (var targetGo in targetsGo) {
			TargetManager targetManager = targetGo.GetComponent<TargetManager> ();
			if (targetManager.acMonster != null) {
				save.livingTargetPos.Add (targetManager.targetPos);
				int type = targetManager.acMonster.GetComponent<MonsterManager> ().monsterType;
				save.livingMonsterType.Add (type);
			}
		}
		save.shootNum = UIManager.single.shootInt;
		save.score = UIManager.single.scoreInt;
		return save;
	}

	void setGame(Save save) {
		foreach (var targetGo in targetsGo) {
			targetGo.GetComponent<TargetManager> ().updateMonster ();
		}

		for (int i = 0; i < save.livingTargetPos.Count; i++) {
			int pos = save.livingTargetPos [i];
			int type = save.livingMonsterType [i];
			targetsGo [pos].GetComponent<TargetManager> ().activeMonsterByType (type);
		}
		UIManager.single.shootInt = save.shootNum;
		UIManager.single.scoreInt = save.score;
		UIManager.single.showNum ();

		play ();
		UIManager.single.showMsg ("");
	}

	bool saveByBin() {
		Save save = createSaveGo ();
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fileStream = File.Create (Application.dataPath + "/StreamFile"+"/byBin.txt");
		bf.Serialize (fileStream, save);
		fileStream.Close ();
		if (File.Exists (Application.dataPath + "/StreamFile" + "/byBin.txt")) {
			return true;
		}
		return false;
	}

	void loadByBin() {
		if (File.Exists (Application.dataPath + "/StreamFile"+"/byBin.txt")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fileStream = File.Open (Application.dataPath + "/StreamFile"+"/byBin.txt",FileMode.Open);
			Save save = (Save)bf.Deserialize (fileStream);
			fileStream.Close ();
			UIManager.single.showMsg ("加载成功");
			setGame (save);

		}
		UIManager.single.showMsg ("没有存档");

	}

	bool saveByXml() {
		Save save = createSaveGo ();
		string filePath = Application.dataPath + "/StreamFile"+"/byXML.txt";
		XmlDocument xmlDoc = new XmlDocument ();
		XmlElement root = xmlDoc.CreateElement ("save");
		root.SetAttribute ("name", "saveFile");
		XmlElement target;
		XmlElement targetPos;
		XmlElement monsterType;
		for (int i = 0; i < save.livingTargetPos.Count; i++) {
			target = xmlDoc.CreateElement ("target");
			targetPos = xmlDoc.CreateElement ("targetPos");
			targetPos.InnerText = save.livingTargetPos [i].ToString ();
			monsterType = xmlDoc.CreateElement ("monsterType");
			monsterType.InnerText = save.livingMonsterType [i].ToString ();

			target.AppendChild (targetPos);
			target.AppendChild (monsterType);
			root.AppendChild (target);
		}

		XmlElement shootNum = xmlDoc.CreateElement ("shootNum");
		shootNum.InnerText = save.shootNum.ToString ();
		root.AppendChild (shootNum);

		XmlElement score = xmlDoc.CreateElement ("score");
		score.InnerText = save.score.ToString();
		root.AppendChild (score);

		xmlDoc.AppendChild (root);
		xmlDoc.Save (filePath);
		if (File.Exists (filePath)) {
			return true;
		}
		return false;
	}

	void loadByXml() {
		string filePath = Application.dataPath+"/StreamFile"+"/byXML.txt";
		if (File.Exists (filePath)) {
			Save save = new Save ();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load (filePath);

			XmlNodeList targets = xmlDoc.GetElementsByTagName ("target");
			if (targets.Count != 0) {
				foreach (XmlNode target in targets) {
					
					XmlNode posNode = target.ChildNodes [0];
					int targetPosIndex = int.Parse(posNode.InnerText);
					save.livingTargetPos.Add (targetPosIndex);

					XmlNode typeNode = target.ChildNodes [1];
					int monsterIndex = int.Parse (typeNode.InnerText);
					save.livingMonsterType.Add (monsterIndex);
				}
			}
			XmlNodeList shootNum = xmlDoc.GetElementsByTagName ("shootNum");
			int shootNumCount = int.Parse(shootNum[0].InnerText);
			save.shootNum = shootNumCount;

			XmlNodeList score = xmlDoc.GetElementsByTagName ("score");
			int scoreCount = int.Parse (score [0].InnerText);
			save.score = scoreCount;
			setGame (save);

		} else {
			UIManager.single.showMsg ("没有存档");
		}
	}

	bool saveByJson() {
		Save save = createSaveGo ();
		string filePath = Application.dataPath + "/StreamFile"+"/byJson.json";
		string saveJsonStr = JsonMapper.ToJson (save);
		StreamWriter sw = new StreamWriter (filePath);
		sw.Write (saveJsonStr);
		sw.Close ();
		if (File.Exists (filePath)) {
			return true;
		}
		return false;

	}

	void loadByJson() {
		string filePath = Application.dataPath + "/StreamFile"+"/byJson.json";
		if (File.Exists (filePath)) {
			StreamReader sr = new StreamReader (filePath);
			string jsonStr = sr.ReadToEnd ();
			sr.Close ();
			Save save = JsonMapper.ToObject<Save> (jsonStr);
			setGame (save);
		} else {
			UIManager.single.showMsg ("没有存档");
		}
	}

	public void continueGame() {
		play ();
		UIManager.single.showMsg ("");
	}

	public void newGame() {
		foreach (var go in targetsGo) {
			go.GetComponent<TargetManager> ().updateMonster ();
		}
		UIManager.single.shootInt = 0;
		UIManager.single.scoreInt = 0;
		UIManager.single.showNum ();
		UIManager.single.showMsg ("");
		play ();
	}

	public void quitGame() {
		Application.Quit ();
	}

	public void saveGame() {
		bool isSave = false;
//		isSave = saveByBin ();
		isSave = saveByXml ();
//		isSave = saveByJson ();
		if (isSave) {
			UIManager.single.showMsg ("保存成功");
		}

	}

	public void loadGame() {
//		loadByBin ();
//		loadByJson();
		loadByXml();
	}
}
