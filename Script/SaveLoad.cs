using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {
	
	public static List<StageData> savedLevel1 = new List<StageData>();
	public static List<StageData> savedLevel2 = new List<StageData>();
	public static GameObject permanentObject;

	//it's static so we can call it from anywhere
	public static void Store(GameObject dataObject){
		if (permanentObject != null) {
			GameObject.Destroy(permanentObject);
		}
		permanentObject = dataObject;
	}

	public static void Clear(){
		if (permanentObject != null) {
			permanentObject = null;
		}
	}

	public static Game_Data getPermanentData(){
		return permanentObject.GetComponent<Game_Data> ();
	}

	public static void Save(int level, int stageNum) {

		savedLevel1.Clear();
		savedLevel2.Clear();
		Load();

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
		}
		if ( level == 1 && savedLevel1 [stageNum].completed == false) {
			savedLevel1[stageNum]=completedData;
		}else if( level == 2 && savedLevel2 [stageNum].completed == false) {
			savedLevel2[stageNum]=completedData;
		}
			

		BinaryFormatter bf = new BinaryFormatter();
		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Create (Application.persistentDataPath + "/savedLevel1.gd"); //you can call it anything you want
		bf.Serialize(file, savedLevel1);
		file.Close();

		file = File.Create (Application.persistentDataPath + "/savedLevel2.gd"); //you can call it anything you want
		bf.Serialize(file, savedLevel2);
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
	}
}