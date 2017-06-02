using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Global_Variable : MonoBehaviour {
	public Sprite[] toddlerSprites;
	private string stageID;
	// Use this for initialization
	void Start () {
		stageID = PlayerPrefs.GetString ("selectedStage");
		string level = stageID.Substring(0,2);
		int stageNum = int.Parse(stageID.Substring(3,2));
		if (Equals (level, "ts")) {
			GameObject.Find ("Image").GetComponent<Image> ().sprite = toddlerSprites[stageNum];
		}

	}

}
