using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Global_Variable : MonoBehaviour {
	public float pieceSpeed;
	public float waitingTime;
	public float waitingSoundTime;
	public AudioClip winSound;
	public Sprite[] stageImages;
	public AudioClip[] stageSounds;
	public string[] stageNames;

	private string stageID;
	private string level;
	private int levelID;
	private int stageNum;
	private AudioSource audioSource;
	private GameObject selectedTemplate;
	private GameObject nextButton;
	private GameObject nameImage;
	private GameObject soundTrigger;
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

		nextButton = GameObject.Find ("next");
		nextButton.SetActive (false);

		soundTrigger = GameObject.Find ("soundTrigger");
		soundTrigger.SetActive (false);

		audioSource = this.GetComponent<AudioSource> ();

		pieceCompleted = 0;
		waiting = true;
		reachTarget = false;
		isWin = false;
		countArrived = 0;
		stageID = PlayerPrefs.GetString ("selectedStage");
		level = stageID.Substring(0,2);
		stageNum = int.Parse(stageID.Substring(3,2));

		nameImage = GameObject.Find ("name");
		nameImage.GetComponent<Text> ().text = stageNames [stageNum];
		nameImage.SetActive (false);

		int templateNum = Random.Range (1, 2); //number of template +1
		string templateName = level + "Template" + templateNum;

		selectedTemplate = GameObject.Find (templateName);
		GameObject.Find ("tsTemplate1").SetActive (false);
		GameObject.Find ("tsTemplate2").SetActive (false);
		selectedTemplate.SetActive (true);

		foreach (GameObject stageSprite in GameObject.FindGameObjectsWithTag("stageImage")) {
			stageSprite.GetComponent<Image> ().sprite = stageImages [stageNum];
		}

		RectTransform tempTargetPos;
		int swapIndex;
		if (Equals (level, "ts")) {
			pieceCount = 4;
			levelID = 1;
			pieceArrived = new bool[4];
			for (int i = 0; i < 4; i++) {
				pieceArrived [i] = false;
			}

			pieceObject = new GameObject[4];
			piecePos = new RectTransform[4];
			pieceObject [0] = GameObject.Find ("piece1");
			pieceObject [1] = GameObject.Find ("piece2");
			pieceObject [2] = GameObject.Find ("piece3");
			pieceObject [3] = GameObject.Find ("piece4");
			for (int i = 0; i < 4; i++) {
				piecePos[i] = pieceObject[i].GetComponent<RectTransform> ();
			}

			pieceTargetPos = new RectTransform[4];
			pieceTargetPos [0] = GameObject.Find ("tsPiecePos1").GetComponent<RectTransform> ();
			pieceTargetPos [1] = GameObject.Find ("tsPiecePos2").GetComponent<RectTransform> ();
			pieceTargetPos [2] = GameObject.Find ("tsPiecePos3").GetComponent<RectTransform> ();
			pieceTargetPos [3] = GameObject.Find ("tsPiecePos4").GetComponent<RectTransform> ();
			for (int i = 0; i < 4; i++) {
				swapIndex = Random.Range (0, 4);
				tempTargetPos = pieceTargetPos [swapIndex];
				pieceTargetPos [swapIndex] = pieceTargetPos [i];
				pieceTargetPos [i] = tempTargetPos;
			}
				
		}
			

		StartCoroutine (scrambleDelay ());

	}

	void Update(){
		if (waiting == false) {
			for (int i = 0; i < pieceCount; i++) {
				if (Vector2.Distance (piecePos [i].position, pieceTargetPos [i].position) < 0.5f && pieceArrived [i] == false) {
					pieceArrived [i] = true;
					countArrived++;
				} else if(pieceObject[i].GetComponent<Piece_Properties>().getDrag() == false) {
					piecePos [i].position = Vector2.MoveTowards (piecePos [i].position, pieceTargetPos [i].position, pieceSpeed * Time.deltaTime);
				}
			}

			if (countArrived>=pieceCount && reachTarget == false) {
				reachTarget = true;


				foreach (GameObject piece in GameObject.FindGameObjectsWithTag("piece")) {
					piece.GetComponent<Piece_Properties>().toSmall();
				}

			}

			if (pieceCompleted >= pieceCount && isWin==false) { //you win
				SaveLoad.Save(levelID,stageNum);
				nextButton.SetActive (true);
				nameImage.SetActive (true);
				soundTrigger.SetActive (true);
				StartCoroutine (waitWinSound ());
				isWin = true;
			}
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
		SceneManager.LoadScene (1);
	}

	public void playSound(){
		if (audioSource.isPlaying==false) {
			audioSource.PlayOneShot (stageSounds [stageNum]);
		}
	}

	IEnumerator waitWinSound(){
		audioSource.PlayOneShot (winSound);
		yield return new WaitForSeconds(winSound.length);
		audioSource.PlayOneShot (stageSounds [stageNum]);
	}

}
