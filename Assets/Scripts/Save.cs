﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {

	public List<int> livingTargetPos = new List<int>();
	public List<int> livingMonsterType = new List<int> ();

	public int shootNum = 0;
	public int score = 0;

}
