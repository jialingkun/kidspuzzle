using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage : MonoBehaviour {
	public int unlockLevel2Condition;
	public int unlockLevel3Condition;

	private Sprite[] ToodlerStageButton;
	private Sprite[] PreschoolStageButton;
	private Sprite[] KindergartenStageButton;

	private GameObject levelButton;
	private GameObject backButton;
	private GameObject stageToodler;
	private GameObject stagePreschool;
	private GameObject stageKindergarten;
	private GameObject buttonToodler;
	private GameObject buttonPreschool;
	private GameObject buttonKindergarten;
	private GameObject PreschoolLock;
	private GameObject KindergartenLock;

	private bool levelSelect;

	private int countLevel1Complete;
	private int countLevel2Complete;


	// Use this for initialization
	void Start () {

		levelSelect = true;

		countLevel1Complete = 0;
		countLevel2Complete = 0;

		//get permanent data
		Game_Data permanentData = SaveLoad.getPermanentData ();

		//get stage button permanent variable
		ToodlerStageButton = permanentData.ToodlerStageButton;
		PreschoolStageButton = permanentData.PreschoolStageButton;
		KindergartenStageButton = permanentData.KindergartenStageButton;

		//find gameobject
		levelButton = GameObject.Find ("Level");
		backButton = GameObject.Find ("Back");
		stageToodler = GameObject.Find ("StageToodler");
		stagePreschool = GameObject.Find ("StagePreschool");
		stageKindergarten = GameObject.Find ("StageKindergarten");
		buttonToodler = GameObject.Find ("Toddler");
		buttonPreschool = GameObject.Find ("Preschool");
		buttonKindergarten = GameObject.Find ("Kindergarten");
		PreschoolLock = GameObject.Find ("PreschoolLock");
		KindergartenLock = GameObject.Find ("KindergartenLock");

		//put all sprite asset initialize by permanent variable
		GameObject.Find ("Title").GetComponent<Image> ().sprite = permanentData.titleImage;
		GameObject.Find ("Background").GetComponent<Image> ().sprite = permanentData.backgroundImage;
		backButton.GetComponent<Image> ().sprite = permanentData.backImage;
		buttonToodler.GetComponent<Image> ().sprite = permanentData.ToodlerImage;
		buttonPreschool.GetComponent<Image> ().sprite = permanentData.PreschoolImage;
		buttonKindergarten.GetComponent<Image> ().sprite = permanentData.KindergartenImage;
		PreschoolLock.GetComponent<Image> ().sprite = permanentData.lockImage;
		KindergartenLock.GetComponent<Image> ().sprite = permanentData.lockImage;


		//Stage Button Sprite
		int ToodlerStagesCount = 10;
		int PreschoolStagesCount = 6;
		int KindergartenStagesCount = 10;
		for (int i = 0; i < ToodlerStagesCount; i++) {
			GameObject.Find ("ts_" + i).GetComponent<Image> ().sprite = ToodlerStageButton [i];
		}
		for (int i = 0; i < PreschoolStagesCount; i++) {
			GameObject.Find ("ps_" + i).GetComponent<Image> ().sprite = PreschoolStageButton [i];
		}
		for (int i = 0; i < KindergartenStagesCount; i++) {
			GameObject.Find ("ks_" + i).GetComponent<Image> ().sprite = KindergartenStageButton [i];
		}
			
		//disable object
		stageToodler.SetActive (false);
		stagePreschool.SetActive (false);
		stageKindergarten.SetActive (false);
		backButton.SetActive (false);




		//load database
		SaveLoad.Load ();

		//count level complete
		foreach (StageData data in SaveLoad.savedLevel1) {
			if (data.completed == true) {
				countLevel1Complete++;
			}
		}

		foreach (StageData data in SaveLoad.savedLevel2) {
			if (data.completed == true) {
				countLevel2Complete++;
			}
		}

		//unlock level
		if (countLevel1Complete < unlockLevel2Condition) {
			buttonPreschool.GetComponent<Button> ().enabled = false;
		} else {
			PreschoolLock.SetActive (false);
		}
		if (countLevel2Complete < unlockLevel3Condition) {
			buttonKindergarten.GetComponent<Button> ().enabled = false;
		} else {
			KindergartenLock.SetActive (false);
		}

		//back to stage select from gameplay
		if (Equals (PlayerPrefs.GetString ("lastLevel"), "ts")) {
			clickToodler ();
		} else if (Equals (PlayerPrefs.GetString ("lastLevel"), "ps")) {
			clickPreschool ();
		} else if (Equals (PlayerPrefs.GetString ("lastLevel"), "ks")) {
			clickKindergarten ();
		}

	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (levelSelect == false) {
				clickBack ();
			} else {
				Application.Quit ();
			}
		}
	}

	public void clickToodler(){
		levelSelect = false;
		levelButton.SetActive (false);
		stageToodler.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stageToodler.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void clickPreschool(){
		levelSelect = false;
		levelButton.SetActive (false);
		stagePreschool.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stagePreschool.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void clickKindergarten(){
		levelSelect = false;
		levelButton.SetActive (false);
		stageKindergarten.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stageKindergarten.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void clickBack(){
		levelSelect = true;
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
