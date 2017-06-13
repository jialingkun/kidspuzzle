using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game_Data : MonoBehaviour {

	public AudioClip winSound;

	public Sprite splashImage;
	public Sprite backgroundImage;
	public Sprite titleImage;
	public Sprite backImage;
	public Sprite nextImage;
	public Sprite ToodlerImage;
	public Sprite PreschoolImage;
	public Sprite KindergartenImage;
	public Sprite lockImageLevel2;
	public Sprite lockImageLevel3;
	public Sprite level1Label;
	public Sprite level2Label;
	public Sprite level3Label;

	public Select_Stage_Component[] ToodlerStageButton;
	public Select_Stage_Component[] PreschoolStageButton;
	public Select_Stage_Component[] KindergartenStageButton;

	public Template_Component[] ToodlerTemplate;
	public Template_Component[] PreschoolTemplate;
	public Template_Component[] KindergartenTemplate;

	public Stage_Component[] ToodlerStages;
	public Stage_Component[] PreschoolStages;
	public Stage_Component[] KindergartenStages;


	void Awake() {
		GameObject.DontDestroyOnLoad (gameObject); 
	}
	void OnDestroy() {
		SaveLoad.Clear ();
	}

	// Use this for initialization
	void Start () {
		GameObject.Find ("splash").GetComponent<Image> ().sprite = splashImage;
		PlayerPrefs.SetString ("selectedStage", "");
		PlayerPrefs.SetString ("lastLevel", "");
		SaveLoad.Store (gameObject);
	}
}
