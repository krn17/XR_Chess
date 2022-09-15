using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public static class SaveManager
{
	public static readonly string savePath = Application.persistentDataPath + "/save.json";
	public static void Save(SaveObject saveObject) {
		Debug.Log("Saving to: " + savePath);
		
		if(File.Exists(savePath)) {
			SaveData data = Load();
			data.saveObject = saveObject;

			string json = JsonUtility.ToJson(data);
			Debug.Log(json);
			File.WriteAllText(savePath, json);
		}
		else {
			SaveData data = new SaveData();
			data.saveObject = saveObject;

			string json = JsonUtility.ToJson(data);
			Debug.Log(json);
			File.WriteAllText(savePath, json);
		}
	}

	public static void SaveName(string name) {
		Debug.Log("Saving to: " + savePath);
		
		if(File.Exists(savePath)) {
			SaveData data = Load();
			data.saveObject.playerName = name;

			string json = JsonUtility.ToJson(data);
			Debug.Log(json);
			File.WriteAllText(savePath, json);
		}
		else {
			SaveData data = new SaveData();
			data.saveObject.playerName = name;

			string json = JsonUtility.ToJson(data);
			Debug.Log(json);
			File.WriteAllText(savePath, json);
		}
	}
	public static void SaveHighscore(int highscore) {
		Debug.Log("Saving to: " + savePath);
		
		if(File.Exists(savePath)) {
			SaveData data = Load();
			data.saveObject.highscore = highscore;

			string json = JsonUtility.ToJson(data);
			Debug.Log(json);
			File.WriteAllText(savePath, json);
		}
		else {
			SaveData data = new SaveData();
			data.saveObject.highscore = highscore;

			string json = JsonUtility.ToJson(data);
			Debug.Log(json);
			File.WriteAllText(savePath, json);
		}
	}
	public static void SaveLevelStats(int currentLevel, int currentXpGained, int currentXpRequired) {
		Debug.Log("Saving to: " + savePath);
		
		if(File.Exists(savePath)) {
			SaveData data = Load();
			data.saveObject.currentLevel = currentLevel;
			data.saveObject.currentXpGained = currentXpGained;
			data.saveObject.currentXpRequired = currentXpRequired;

			string json = JsonUtility.ToJson(data);
			Debug.Log(json);
			File.WriteAllText(savePath, json);
		}
		else {
			SaveData data = new SaveData();
			data.saveObject.currentLevel = currentLevel;
			data.saveObject.currentXpGained = currentXpGained;
			data.saveObject.currentXpRequired = currentXpRequired;

			string json = JsonUtility.ToJson(data);
			Debug.Log(json);
			File.WriteAllText(savePath, json);
		}
	}

	// public static void Remove(SaveObject saveObject) {
		
		
	// 	if(File.Exists(savePath)) {
	// 		SaveData data = Load();
	// 		for(int i = 0; i<data.saveItems.Count; i++) {
	// 			if(data.saveItems[i].currentXpGained == saveObject.currentXpGained) {
	// 				Debug.Log("Deleting: " + saveObject.playerName);
	// 				data.saveItems.RemoveAt(i);
	// 			}
	// 		}
			

	// 		string json = JsonUtility.ToJson(data);
	// 		Debug.Log(json);
	// 		File.WriteAllText(savePath, json);
	// 	}
	// 	else {
	// 		Debug.LogError("Failed to Remove: Save Object " + saveObject.playerName + " not found.");
	// 	}
	// }

	public static SaveData Load() {
		Debug.Log("Loading from: " + savePath);
		
		if(File.Exists(savePath)) {
			string jsonString = File.ReadAllText(savePath);
			Debug.Log("Loaded: " + jsonString);
			SaveData data = JsonUtility.FromJson<SaveData>(jsonString);
			return data;
		}
		else {
			return new SaveData();
		}
	}
	public static string GetTimeString(DateTime value)
	{
		return value.ToString("O");
	}

	public static DateTime GetDateTimeFromTimeString(string timeString) {
		return DateTime.ParseExact(timeString, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
	}
}
