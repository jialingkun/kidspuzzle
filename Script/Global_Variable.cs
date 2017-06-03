using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Global_Variable : MonoBehaviour {
	public Sprite[] stageImages;
	private string stageID;
	private GameObject selectedTemplate;
	// Use this for initialization
	void Start () {
		stageID = PlayerPrefs.GetString ("selectedStage");
		string level = stageID.Substring(0,2);
		int stageNum = int.Parse(stageID.Substring(3,2));

		int templateNum = Random.Range (1, 3);
		string templateName = level + "Template" + templateNum;
		Debug.Log (templateName);

		selectedTemplate = GameObject.Find (templateName);
		GameObject.Find ("tsTemplate1").SetActive (false);
		GameObject.Find ("tsTemplate2").SetActive (false);
		selectedTemplate.SetActive (true);


		foreach (GameObject stageSprite in GameObject.FindGameObjectsWithTag("stageImage")) {
			stageSprite.GetComponent<Image> ().sprite = stageImages [stageNum];
		}
		/*if (Equals (level, "ts")) {
			GameObject.Find ("Image").GetComponent<Image> ().sprite = ;
		}*/

	}

}
