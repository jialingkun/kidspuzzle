using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Click_Stage : MonoBehaviour {
	private GameObject levelButton;
	private GameObject backButton;
	private GameObject stageToodler;
	private GameObject stagePreschool;
	private GameObject stageKindergarten;
	// Use this for initialization
	void Start () {
		levelButton = GameObject.Find ("Level");
		backButton = GameObject.Find ("Back");
		stageToodler = GameObject.Find ("StageToodler");
		stagePreschool = GameObject.Find ("StagePreschool");
		stageKindergarten = GameObject.Find ("StageKindergarten");
		stageToodler.SetActive (false);
		stagePreschool.SetActive (false);
		stageKindergarten.SetActive (false);
		backButton.SetActive (false);

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
