using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {
	
	public static List<StageData> savedLevel1 = new List<StageData>();
	public static List<StageData> savedLevel2 = new List<StageData>();
	public static List<StageData> savedLevel3 = new List<StageData>();
	public static GameObject permanentObject;
	public static bool notificationUnlockLevel2;
	public static bool notificationUnlockLevel3;
	public static bool muteState;

	//it's static so we can call it from anywhere
	public static void Store(GameObject dataObject){
		if (permanentObject != null) {
			GameObject.Destroy(permanentObject);
		}
		permanentObject = dataObject;
		notificationUnlockLevel2 = false;
		notificationUnlockLevel3 = false;
		muteState = false;
	}

	public static void Clear(){
		if (permanentObject != null) {
			permanentObject = null;
		}
	}

	public static Game_Data getPermanentData(){
		return permanentObject.GetComponent<Game_Data> ();
	}

	public static AudioSource getPermanentAudio(){
		return permanentObject.GetComponent<AudioSource> ();
	}

	public static bool getNotificationLevel2Status(){
		return notificationUnlockLevel2;
	}

	public static void notificationLevel2Displayed(){
		notificationUnlockLevel2 = true;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/notificationLevel2.gd"); //you can call it anything you want
		bf.Serialize(file, notificationUnlockLevel2);
		file.Close();
	}

	public static bool getNotificationLevel3Status(){
		return notificationUnlockLevel3;
	}

	public static void notificationLevel3Displayed(){
		notificationUnlockLevel3 = true;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/notificationLevel3.gd"); //you can call it anything you want
		bf.Serialize(file, notificationUnlockLevel3);
		file.Close();
	}

	public static bool changeMuteState(){
		if (muteState == true) {
			muteState = false;
		} else {
			muteState = true;
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/muteState.gd");
		bf.Serialize(file, muteState);
		file.Close();
		return muteState;
	}

	public static bool getMuteState(){
		return muteState;
	}

	public static Ads getAdsComponent(){
		return permanentObject.GetComponent<Ads> ();
	}

	public static void Save(int level, int stageNum) {
		Load();

		//savedLevel1.Clear();
		//savedLevel2.Clear();
		//savedLevel3.Clear();
		//notificationUnlockLevel2 = false;
		//notificationUnlockLevel3 = false;

		StageData completedData = new StageData (true);
		StageData defaultData = new StageData (false);
		if (level == 1 && savedLevel1.Count <= stageNum) { //Initialize
			while (savedLevel1.Count <= stageNum) {
				savedLevel1.Add (defaultData);
			}
		} else if(level == 2 && savedLevel2.Count <= stageNum) { //Initialize
			while (savedLevel2.Count <= stageNum) {
				savedLevel2.Add (defaultData);
			}
		}else if(level == 3 && savedLevel3.Count <= stageNum) { //Initialize
			while (savedLevel3.Count <= stageNum) {
				savedLevel3.Add (defaultData);
			}
		}



		if ( level == 1 && savedLevel1 [stageNum].completed == false) {
			savedLevel1[stageNum]=completedData;
		}else if( level == 2 && savedLevel2 [stageNum].completed == false) {
			savedLevel2[stageNum]=completedData;
		}else if( level == 3 && savedLevel3 [stageNum].completed == false) {
			savedLevel3[stageNum]=completedData;
		}
			

		BinaryFormatter bf = new BinaryFormatter();
		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Create (Application.persistentDataPath + "/savedLevel1.gd"); //you can call it anything you want
		bf.Serialize(file, savedLevel1);
		file.Close();

		file = File.Create (Application.persistentDataPath + "/savedLevel2.gd"); //you can call it anything you want
		bf.Serialize(file, savedLevel2);
		file.Close();

		file = File.Create (Application.persistentDataPath + "/savedLevel3.gd"); //you can call it anything you want
		bf.Serialize(file, savedLevel3);
		file.Close();
	}   

	public static void Load() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file;
		if(File.Exists(Application.persistentDataPath + "/savedLevel1.gd")) {
			file = File.Open(Application.persistentDataPath + "/savedLevel1.gd", FileMode.Open);
			savedLevel1 = (List<StageData>)bf.Deserialize(file);
			file.Close();
		}
		if(File.Exists(Application.persistentDataPath + "/savedLevel2.gd")) {
			file = File.Open(Application.persistentDataPath + "/savedLevel2.gd", FileMode.Open);
			savedLevel2 = (List<StageData>)bf.Deserialize(file);
			file.Close();
		}
		if(File.Exists(Application.persistentDataPath + "/savedLevel3.gd")) {
			file = File.Open(Application.persistentDataPath + "/savedLevel3.gd", FileMode.Open);
			savedLevel3 = (List<StageData>)bf.Deserialize(file);
			file.Close();
		}

		if(File.Exists(Application.persistentDataPath + "/notificationLevel2.gd")) {
			file = File.Open(Application.persistentDataPath + "/notificationLevel2.gd", FileMode.Open);
			notificationUnlockLevel2 = (bool)bf.Deserialize(file);
			file.Close();
		}
		if(File.Exists(Application.persistentDataPath + "/notificationLevel3.gd")) {
			file = File.Open(Application.persistentDataPath + "/notificationLevel3.gd", FileMode.Open);
			notificationUnlockLevel3 = (bool)bf.Deserialize(file);
			file.Close();
		}
		if(File.Exists(Application.persistentDataPath + "/muteState.gd")) {
			file = File.Open(Application.persistentDataPath + "/muteState.gd", FileMode.Open);
			muteState = (bool)bf.Deserialize(file);
			file.Close();
		}
	}
}