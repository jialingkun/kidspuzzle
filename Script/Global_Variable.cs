using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Global_Variable : MonoBehaviour {
	public float pieceSpeed;
	public float waitingTime;
	public Sprite[] stageImages;
	private string stageID;
	private string level;
	private GameObject selectedTemplate;
	private GameObject[] pieceObject;
	private RectTransform[] piecePos;
	private RectTransform[] pieceTargetPos;

	private int pieceCount;
	private int countArrived;
	private bool[] pieceArrived;
	private bool waiting;
	private bool reachTarget;
	// Use this for initialization
	void Start () {
		waiting = true;
		reachTarget = false;
		countArrived = 0;
		stageID = PlayerPrefs.GetString ("selectedStage");
		level = stageID.Substring(0,2);
		int stageNum = int.Parse(stageID.Substring(3,2));

		int templateNum = Random.Range (1, 2); //number of template +1
		string templateName = level + "Template" + templateNum;
		Debug.Log (templateName);

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
			pieceTargetPos [0] = GameObject.Find ("piecePos1").GetComponent<RectTransform> ();
			pieceTargetPos [1] = GameObject.Find ("piecePos2").GetComponent<RectTransform> ();
			pieceTargetPos [2] = GameObject.Find ("piecePos3").GetComponent<RectTransform> ();
			pieceTargetPos [3] = GameObject.Find ("piecePos4").GetComponent<RectTransform> ();
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
			if (Equals (level, "ts")) {
				pieceCount = 4;

			}
			for (int i = 0; i < pieceCount; i++) {
				
				if (Vector2.Distance (piecePos [i].position, pieceTargetPos [i].position) == 0 && pieceArrived [i] == false) {
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

		}
	}

	IEnumerator scrambleDelay(){
		yield return new WaitForSeconds(waitingTime);
		waiting = false;
	}

	public string getLevel(){
		return level;
	}

}
