using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LeaderBoardData {
	public int[] highScores;
	public LeaderBoardData(int[] highScores) {
		this.highScores = highScores;
	}
}
