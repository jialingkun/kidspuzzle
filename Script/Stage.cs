using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage : MonoBehaviour {
	public int unlockLevel2Condition;
	public int unlockLevel3Condition;
	public Sprite lockImage;
	private GameObject levelButton;
	private GameObject backButton;
	private GameObject stageToodler;
	private GameObject stagePreschool;
	private GameObject stageKindergarten;
	private GameObject buttonPreschool;
	private GameObject buttonKindergarten;

	private bool preschoolLock;
	private bool KindergartenLock;
	private int countLevel1Complete;
	private int countLevel2Complete;
	// Use this for initialization
	void Start () {
		preschoolLock = true;
		KindergartenLock = true;
		countLevel1Complete = 0;
		countLevel2Complete = 0;

		levelButton = GameObject.Find ("Level");
		backButton = GameObject.Find ("Back");
		stageToodler = GameObject.Find ("StageToodler");
		stagePreschool = GameObject.Find ("StagePreschool");
		stageKindergarten = GameObject.Find ("StageKindergarten");
		buttonPreschool = GameObject.Find ("Preschool");
		buttonKindergarten = GameObject.Find ("Kindergarten");
		stageToodler.SetActive (false);
		stagePreschool.SetActive (false);
		stageKindergarten.SetActive (false);
		backButton.SetActive (false);

		SaveLoad.Load ();

		foreach (StageData data in SaveLoad.savedLevel1) {
			Debug.Log ("data:" + data.completed);
			if (data.completed == true) {
				countLevel1Complete++;
			}
		}

		foreach (StageData data in SaveLoad.savedLevel2) {
			if (data.completed == true) {
				countLevel2Complete++;
			}
		}

		if (countLevel1Complete<unlockLevel2Condition) {
			buttonPreschool.GetComponent<Image> ().sprite = lockImage;
			buttonPreschool.GetComponent<Button> ().enabled = false;
		}
		if (countLevel2Complete<unlockLevel3Condition) {
			buttonKindergarten.GetComponent<Image> ().sprite = lockImage;
			buttonKindergarten.GetComponent<Button> ().enabled = false;
		}


	}

	public void clickToodler(){
		levelButton.SetActive (false);
		stageToodler.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stageToodler.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void clickPreschool(){
		
		levelButton.SetActive (false);
		stagePreschool.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stagePreschool.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void clickKindergarten(){
		levelButton.SetActive (false);
		stageKindergarten.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stageKindergarten.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void clickBack(){
		stageToodler.SetActive (false);
		stagePreschool.SetActive (false);
		stageKindergarten.SetActive (false);
		backButton.SetActive (false);
		levelButton.SetActive (true);
	}

	public void clickStage(string stageID){
		PlayerPrefs.SetString ("selectedStage", stageID);
		SceneManager.LoadScene (2);
	}
}
