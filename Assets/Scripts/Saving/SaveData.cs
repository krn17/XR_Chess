using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
	public SaveObject saveObject;

	public SaveData() {
		saveObject = new SaveObject("", 0, 1, 0, 0);
	}
	public SaveData(SaveObject initialSaveObject) {
		saveObject = initialSaveObject;
	}
}

[Serializable]
public class SaveObject {
	public string playerName;
	public int highscore;
	public int currentLevel;
	public int currentXpGained;
	public int currentXpRequired;

	public SaveObject(string playerName, int highscore, int currentLevel, int currentXpGained, int currentXpRequired) {
		this.playerName = playerName;
		this.highscore = highscore;
		this.currentLevel = currentLevel;
		this.currentXpGained = currentXpGained;
		this.currentXpRequired = currentXpRequired;
	}
}
