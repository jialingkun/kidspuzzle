using UnityEngine;
using System.Collections;

public class Clear : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetString ("selectedStage", "");
		PlayerPrefs.SetString ("lastLevel", "");
	}
}
