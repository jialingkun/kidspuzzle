using UnityEngine;
using System.Collections;

[System.Serializable]
public class StageData{
	public bool completed;

	public StageData(bool status){
		completed = status;
	}
}
