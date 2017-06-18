using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stage : MonoBehaviour {
	private Select_Stage_Component[] ToodlerStageButton;
	private Select_Stage_Component[] PreschoolStageButton;
	private Select_Stage_Component[] KindergartenStageButton;

	private GameObject levelButton;
	private GameObject muteButton;
	private GameObject backButton;
	private GameObject stageToodler;
	private GameObject stagePreschool;
	private GameObject stageKindergarten;
	private GameObject buttonToodler;
	private GameObject buttonPreschool;
	private GameObject buttonKindergarten;
	private GameObject PreschoolLock;
	private GameObject KindergartenLock;

	private AudioSource BGMaudioSource;
	private AudioSource audioSource;

	private bool levelSelect;

	private int countLevel1Complete;
	private int countLevel2Complete;

	private Game_Data permanentData;


	// Use this for initialization
	void Start () {
		levelSelect = true;

		countLevel1Complete = 0;
		countLevel2Complete = 0;

		//show banner ads
		SaveLoad.getAdsComponent ().showBannerBottom ();

		//get permanent data
		permanentData = SaveLoad.getPermanentData ();

		//get local audio source
		audioSource = permanentData.getSEaudioSource();

		//get stage button permanent variable
		ToodlerStageButton = permanentData.ToodlerStageButton;
		PreschoolStageButton = permanentData.PreschoolStageButton;
		KindergartenStageButton = permanentData.KindergartenStageButton;

		//find gameobject
		levelButton = GameObject.Find ("Level");
		muteButton = GameObject.Find ("Mute");
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
		GameObject.Find ("ToodlerLabel").GetComponent<Image> ().sprite = permanentData.level1Label;
		GameObject.Find ("PreschoolLabel").GetComponent<Image> ().sprite = permanentData.level2Label;
		GameObject.Find ("KindergartenLabel").GetComponent<Image> ().sprite = permanentData.level3Label;
		backButton.GetComponent<Image> ().sprite = permanentData.backImage;
		buttonToodler.GetComponent<Image> ().sprite = permanentData.ToodlerImage;
		buttonPreschool.GetComponent<Image> ().sprite = permanentData.PreschoolImage;
		buttonKindergarten.GetComponent<Image> ().sprite = permanentData.KindergartenImage;
		PreschoolLock.GetComponent<Image> ().sprite = permanentData.lockImageLevel2;
		KindergartenLock.GetComponent<Image> ().sprite = permanentData.lockImageLevel3;

		//load database
		SaveLoad.Load ();

		//mute sprite
		if (SaveLoad.getMuteState () == true) {
			muteButton.GetComponent<Image> ().sprite = permanentData.muteOnImage;
		} else {
			muteButton.GetComponent<Image> ().sprite = permanentData.muteOffImage;
		}

		//Main menu play state
		BGMaudioSource = SaveLoad.getPermanentAudio ();
		if (permanentData.getBGMStatus() == false) {
			BGMaudioSource.clip = permanentData.BGM;
			permanentData.BGMPlayed ();

			//play depend on mute state
			if (SaveLoad.getMuteState () == false) {
				BGMaudioSource.Play ();
			}
		}



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
		if (countLevel1Complete < permanentData.unlockLevel2Condition) {
			buttonPreschool.GetComponent<Button> ().enabled = false;
		} else {
			PreschoolLock.SetActive (false);
		}
		if (countLevel2Complete < permanentData.unlockLevel3Condition) {
			buttonKindergarten.GetComponent<Button> ().enabled = false;
		} else {
			KindergartenLock.SetActive (false);
		}

		//Stage Button Sprite
		int ToodlerStagesCount = 10;
		int PreschoolStagesCount = 6;
		int KindergartenStagesCount = 10;
		for (int i = 0; i < ToodlerStagesCount; i++) {
			if (i < SaveLoad.savedLevel1.Count) {
				if (SaveLoad.savedLevel1 [i].completed == true) {
					GameObject.Find ("ts_" + i).GetComponent<Image> ().sprite = ToodlerStageButton [i].stageCompleted;
				} else {
					GameObject.Find ("ts_" + i).GetComponent<Image> ().sprite = ToodlerStageButton [i].stageUncompleted;
				}
			} else {
				GameObject.Find ("ts_" + i).GetComponent<Image> ().sprite = ToodlerStageButton [i].stageUncompleted;
			}

		}
		for (int i = 0; i < PreschoolStagesCount; i++) {
			if (i < SaveLoad.savedLevel2.Count) {
				if (SaveLoad.savedLevel2 [i].completed == true) {
					GameObject.Find ("ps_" + i).GetComponent<Image> ().sprite = PreschoolStageButton [i].stageCompleted;
				} else {
					GameObject.Find ("ps_" + i).GetComponent<Image> ().sprite = PreschoolStageButton [i].stageUncompleted;
				}
			} else {
				GameObject.Find ("ps_" + i).GetComponent<Image> ().sprite = PreschoolStageButton [i].stageUncompleted;
			}
		}
		for (int i = 0; i < KindergartenStagesCount; i++) {
			if (i < SaveLoad.savedLevel3.Count) {
				if (SaveLoad.savedLevel3 [i].completed == true) {
					GameObject.Find ("ks_" + i).GetComponent<Image> ().sprite = KindergartenStageButton [i].stageCompleted;
				} else {
					GameObject.Find ("ks_" + i).GetComponent<Image> ().sprite = KindergartenStageButton [i].stageUncompleted;
				}
			} else {
				GameObject.Find ("ks_" + i).GetComponent<Image> ().sprite = KindergartenStageButton [i].stageUncompleted;
			}
		}

		//disable object
		stageToodler.SetActive (false);
		stagePreschool.SetActive (false);
		stageKindergarten.SetActive (false);
		backButton.SetActive (false);


		//back to stage select from gameplay
		if (Equals (PlayerPrefs.GetString ("lastLevel"), "ts")) {
			backToodler ();
		} else if (Equals (PlayerPrefs.GetString ("lastLevel"), "ps")) {
			backPreschool ();
		} else if (Equals (PlayerPrefs.GetString ("lastLevel"), "ks")) {
			backKindergarten ();
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
		audioSource.PlayOneShot (permanentData.selectSound);
		levelSelect = false;
		levelButton.SetActive (false);
		stageToodler.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stageToodler.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void clickPreschool(){
		audioSource.PlayOneShot (permanentData.selectSound);
		levelSelect = false;
		levelButton.SetActive (false);
		stagePreschool.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stagePreschool.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void clickKindergarten(){
		audioSource.PlayOneShot (permanentData.selectSound);
		levelSelect = false;
		levelButton.SetActive (false);
		stageKindergarten.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stageKindergarten.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void backToodler(){
		levelSelect = false;
		levelButton.SetActive (false);
		stageToodler.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stageToodler.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void backPreschool(){
		levelSelect = false;
		levelButton.SetActive (false);
		stagePreschool.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stagePreschool.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void backKindergarten(){
		levelSelect = false;
		levelButton.SetActive (false);
		stageKindergarten.SetActive (true);
		backButton.SetActive (true);
		RectTransform contentTransform;
		contentTransform = stageKindergarten.transform.Find ("Viewport").transform.Find ("Content").gameObject.GetComponent<RectTransform>();
		contentTransform.localPosition = new Vector2 (0, contentTransform.localPosition.y);
	}

	public void clickBack(){
		audioSource.PlayOneShot (permanentData.selectSound);
		levelSelect = true;
		stageToodler.SetActive (false);
		stagePreschool.SetActive (false);
		stageKindergarten.SetActive (false);
		backButton.SetActive (false);
		levelButton.SetActive (true);
	}

	public void clickMute(){
		bool muteState = SaveLoad.changeMuteState();
		if (muteState == true) {
			BGMaudioSource.Stop ();
			muteButton.GetComponent<Image> ().sprite = permanentData.muteOnImage;
		} else {
			BGMaudioSource.Play ();
			muteButton.GetComponent<Image> ().sprite = permanentData.muteOffImage;
		}
	}

	public void clickStage(string stageID){
		audioSource.PlayOneShot (permanentData.selectSound);
		PlayerPrefs.SetString ("selectedStage", stageID);
		SceneManager.LoadScene (2);
	}
}
