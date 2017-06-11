using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Global_Variable : MonoBehaviour {
	public float pieceSpeed;
	public float waitingTime;
	public float waitingSoundTime;

	private AudioClip winSound;
	private Template_Component[] ToodlerTemplates;
	private Template_Component[] PreschoolTemplates;
	private Template_Component[] KindergartenTemplates;
	private Stage_Component[] ToodlerStages;
	private Stage_Component[] PreschoolStages;
	private Stage_Component[] KindergartenStages;

	private Template_Component[] selectedTemplates;
	private Stage_Component[] selectedStages;
	private string stageID;
	private string level;
	private int levelID;
	private int stageNum;
	private AudioSource audioSource;
	private GameObject selectedTemplate;

	private GameObject nextButton;
	private GameObject nameImage;
	private GameObject soundTrigger;
	private List<GameObject> templates;
	private GameObject[] pieceObject;
	private RectTransform[] piecePos;
	private RectTransform[] pieceTargetPos;

	private int pieceCount;
	private int countArrived;
	private int pieceCompleted;
	private bool[] pieceArrived;
	private bool waiting;
	private bool reachTarget;
	private bool isWin;

	// Use this for initialization
	void Start () {

		//get permanent data
		Game_Data permanentData = SaveLoad.getPermanentData ();

		//put all permanent data
		winSound = permanentData.winSound;
		ToodlerTemplates = permanentData.ToodlerTemplate;
		PreschoolTemplates = permanentData.PreschoolTemplate;
		KindergartenTemplates = permanentData.KindergartenTemplate;
		ToodlerStages = permanentData.ToodlerStages;
		PreschoolStages = permanentData.PreschoolStages;
		KindergartenStages = permanentData.KindergartenStages;

		//permanent data sprite
		GameObject.Find ("Background").GetComponent<Image> ().sprite = permanentData.backgroundImage;
		GameObject.Find ("next").GetComponent<Image> ().sprite = permanentData.nextImage;
		GameObject.Find ("home").GetComponent<Image> ().sprite = permanentData.backImage;

		//next button
		nextButton = GameObject.Find ("next");
		nextButton.SetActive (false);

		//sound button
		soundTrigger = GameObject.Find ("soundTrigger");
		soundTrigger.SetActive (false);

		//audio
		audioSource = this.GetComponent<AudioSource> ();

		//initialize
		pieceCompleted = 0;
		waiting = true;
		reachTarget = false;
		isWin = false;
		countArrived = 0;

		//get data from main menu
		stageID = PlayerPrefs.GetString ("selectedStage");
		level = stageID.Substring(0,2);
		stageNum = int.Parse(stageID.Substring(3,2));



		//random template to use
		int templateNum = Random.Range (1, 5); //number of template +1
		string templateName = level + "Template" + templateNum;
		//select template from random result
		templates = new List<GameObject> ();
		selectedTemplate = GameObject.Find (templateName);
		foreach (GameObject template in GameObject.FindGameObjectsWithTag("template")) {
			templates.Add (template);
			template.SetActive (false);
		}
		selectedTemplate.SetActive (true);

		//initialize depend on level
		if (Equals (level, "ts")) {
			pieceCount = 4;
			levelID = 1;
			selectedStages = ToodlerStages;
			selectedTemplates = ToodlerTemplates;
		} else if (Equals (level, "ps")) {
			pieceCount = 6;
			levelID = 2;
			selectedStages = PreschoolStages;
			selectedTemplates = PreschoolTemplates;
		} else if (Equals (level, "ks")) {
			pieceCount = 9;
			levelID = 3;
			selectedStages = KindergartenStages;
			selectedTemplates = KindergartenTemplates;
		}

		//fullpiece mask sprite
		GameObject.Find("fullPiece").GetComponent<Image>().sprite = selectedTemplates[templateNum-1].fullPiece;

		//stage name
		nameImage = GameObject.Find ("name");
		nameImage.GetComponent<Text> ().text = selectedStages[stageNum].Name;
		nameImage.SetActive (false);

		//stage image
		foreach (GameObject stageSprite in GameObject.FindGameObjectsWithTag("stageImage")) {
			stageSprite.GetComponent<Image> ().sprite = selectedStages[stageNum].Image;
		}

		//initialize array size
		pieceArrived = new bool[pieceCount];
		pieceObject = new GameObject[pieceCount];
		piecePos = new RectTransform[pieceCount];
		pieceTargetPos = new RectTransform[pieceCount];

		//initialize variable
		string pieceName;
		string pieceTargetPosName;
		int numberFromOne;

		for (int i = 0; i < pieceCount; i++) {


			//piece arrived
			pieceArrived [i] = false;

			//piece object
			numberFromOne = i + 1;
			pieceName = "piece" + numberFromOne;
			pieceObject [i] = GameObject.Find (pieceName);

			//piece mask sprite
			pieceObject[i].GetComponent<Image>().sprite = selectedTemplates[templateNum-1].piece[i];

			//piece pos
			piecePos[i] = pieceObject[i].GetComponent<RectTransform> ();

			//piece target pos
			pieceTargetPosName = level+"PiecePos"+numberFromOne;
			pieceTargetPos [i] = GameObject.Find (pieceTargetPosName).GetComponent<RectTransform> ();
		}

		//initialize for random target position
		RectTransform tempTargetPos;
		int swapIndex;
		//random target position (swapping)
		for (int i = 0; i < pieceCount; i++) {
			swapIndex = Random.Range (0, pieceCount);
			tempTargetPos = pieceTargetPos [swapIndex];
			pieceTargetPos [swapIndex] = pieceTargetPos [i];
			pieceTargetPos [i] = tempTargetPos;
		}
			
		//scramble delay coroutine
		StartCoroutine (scrambleDelay ());

	}

	void Update(){
		if (waiting == false) {
			for (int i = 0; i < pieceCount; i++) {
				if (Vector2.Distance (piecePos [i].position, pieceTargetPos [i].position) < 0.5f && pieceArrived [i] == false) {
					//if piece arrive on stand by position below
					pieceArrived [i] = true;
					countArrived++;
				} else if(pieceObject[i].GetComponent<Piece_Properties>().getDrag() == false && pieceObject[i].GetComponent<Piece_Properties>().getCompleted() == false) {
					//piece move back to stand by position below
					piecePos [i].position = Vector2.MoveTowards (piecePos [i].position, pieceTargetPos [i].position, pieceSpeed * Time.deltaTime * Screen.currentResolution.height);

				}
			}

			if (countArrived>=pieceCount && reachTarget == false) {
				//if all piece reach stand by position below
				reachTarget = true;
				//resize piece to small
				foreach (GameObject piece in GameObject.FindGameObjectsWithTag("piece")) {
					piece.GetComponent<Piece_Properties>().toSmall();
				}
			}

			if (pieceCompleted >= pieceCount && isWin==false) { 
				//if you win the stage
				SaveLoad.Save(levelID,stageNum);
				nameImage.SetActive (true);
				soundTrigger.SetActive (true);
				StartCoroutine (waitWinSound ());
				isWin = true;
			}
		}

		//on back phone button
		if (Input.GetKeyDown(KeyCode.Escape)) {
			clickHome ();
		}
	}

	IEnumerator scrambleDelay(){
		yield return new WaitForSeconds(waitingTime);
		waiting = false;
	}

	public string getLevel(){
		return level;
	}

	public bool getWaitingStatus(){
		return reachTarget;
	}

	public void addPieceCompleted(){
		pieceCompleted++;
	}

	public void clickHome(){
		PlayerPrefs.SetString ("lastLevel", level);
		SceneManager.LoadScene (1);
	}

	public void playSound(){
		if (audioSource.isPlaying==false) {
			audioSource.PlayOneShot (selectedStages[stageNum].Sound);
		}
	}

	IEnumerator waitWinSound(){
		audioSource.PlayOneShot (winSound);
		yield return new WaitForSeconds(winSound.length);
		audioSource.PlayOneShot (selectedStages[stageNum].Sound);
		yield return new WaitForSeconds(selectedStages[stageNum].Sound.length);
		nextButton.SetActive (true);
	}

	public void clickNext(){
		int stageCount = selectedStages.Length;
		if (stageNum >= stageCount - 1) {
			stageNum = 0;

			if (Equals (level, "ts")) {
				level = "ps";
				pieceCount = 6;
				levelID = 2;
				selectedStages = PreschoolStages;
				selectedTemplates = PreschoolTemplates;
			} else if (Equals (level, "ps")) {
				level = "ks";
				pieceCount = 9;
				levelID = 3;
				selectedStages = KindergartenStages;
				selectedTemplates = KindergartenTemplates;
			} else if (Equals (level, "ks")) {
				level = "ts";
				pieceCount = 4;
				levelID = 1;
				selectedStages = ToodlerStages;
				selectedTemplates = ToodlerTemplates;
			}

		} else {
			stageNum++;
		}
		refresh ();
	}

	public void refresh(){

		//next button
		nextButton.SetActive (false);

		//sound button
		soundTrigger.SetActive (false);

		//initialize
		pieceCompleted = 0;
		waiting = true;
		reachTarget = false;
		isWin = false;
		countArrived = 0;

		//random template to use
		int templateNum = Random.Range (1, 5); //number of template +1
		string templateName = level + "Template" + templateNum;
		//select template from random result
		foreach (GameObject template in templates) {
			template.SetActive (true);
		}
		selectedTemplate = GameObject.Find (templateName);
		foreach (GameObject template in templates) {
			template.SetActive (false);
		}
		selectedTemplate.SetActive (true);

		//fullpiece mask sprite
		GameObject.Find("fullPiece").GetComponent<Image>().sprite = selectedTemplates[templateNum-1].fullPiece;


		//stage name
		nameImage.GetComponent<Text> ().text = selectedStages[stageNum].Name;
		nameImage.SetActive (false);

		//stage image
		foreach (GameObject stageSprite in GameObject.FindGameObjectsWithTag("stageImage")) {
			stageSprite.GetComponent<Image> ().sprite = selectedStages[stageNum].Image;
		}

		//initialize array size
		pieceArrived = new bool[pieceCount];
		pieceObject = new GameObject[pieceCount];
		piecePos = new RectTransform[pieceCount];
		pieceTargetPos = new RectTransform[pieceCount];

		//initialize variable
		string pieceName;
		string pieceTargetPosName;
		int numberFromOne;

		for (int i = 0; i < pieceCount; i++) {

			//piece arrived
			pieceArrived [i] = false;

			//piece object
			numberFromOne = i + 1;
			pieceName = "piece" + numberFromOne;
			pieceObject [i] = GameObject.Find (pieceName);
			pieceObject [i].GetComponent<Piece_Properties> ().refresh ();

			//piece mask sprite
			pieceObject[i].GetComponent<Image>().sprite = selectedTemplates[templateNum-1].piece[i];

			//piece pos
			piecePos[i] = pieceObject[i].GetComponent<RectTransform> ();

			//piece target pos
			pieceTargetPosName = level+"PiecePos"+numberFromOne;
			pieceTargetPos [i] = GameObject.Find (pieceTargetPosName).GetComponent<RectTransform> ();
		}

		//initialize for random target position
		RectTransform tempTargetPos;
		int swapIndex;
		//random target position (swapping)
		for (int i = 0; i < pieceCount; i++) {
			swapIndex = Random.Range (0, pieceCount);
			tempTargetPos = pieceTargetPos [swapIndex];
			pieceTargetPos [swapIndex] = pieceTargetPos [i];
			pieceTargetPos [i] = tempTargetPos;
		}

		//scramble delay coroutine
		StartCoroutine (scrambleDelay ());

	}

}
