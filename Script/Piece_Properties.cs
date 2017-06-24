using UnityEngine;
using System.Collections;

public class Piece_Properties : MonoBehaviour {
	private Global_Variable globalScript;

	public enum resizePatternX {RIGHT, LEFT, CENTER};
	public enum resizePatternY {UP, DOWN, CENTER};
	public resizePatternX xPositionPattern;
	public resizePatternY yPositionPattern;

	private Vector2 smallDimension;
	private Vector2 bigDimension;
	private Vector2 placeHolderPosition;
	private RectTransform image;
	private Vector2 smallImagePosition;
	private Vector2 smallImageDimension;
	private Vector2 bigImagePosition;
	private Vector2 bigImageDimension;
	private Vector2 scale;

	private bool isDrag;
	private bool isCompleted;
	private AudioSource audioSource;

	private Game_Data permanentData;

	void Start () {

		//get permanent data
		permanentData = SaveLoad.getPermanentData ();

		isCompleted = false;
		image = this.transform.Find ("Image").GetComponent<RectTransform> ();
		globalScript = GameObject.Find ("Canvas").GetComponent<Global_Variable> ();
		audioSource = globalScript.getAudioSource ();

		string level = globalScript.getDifficulty ();
		if (Equals (level, "ts")) {
			scale = new Vector2 (0.40f, 0.40f);
		} else if (Equals (level, "ps")) {
			scale = new Vector2 (0.45f, 0.45f);
		} else if (Equals (level, "ks")) {
			scale = new Vector2 (0.55f, 0.55f);
		}
		bigDimension = this.GetComponent<RectTransform> ().sizeDelta;
		smallDimension = Vector2.Scale(bigDimension,scale);
		placeHolderPosition = this.GetComponent<RectTransform> ().localPosition;
		bigImageDimension = image.sizeDelta;
		smallImageDimension = Vector2.Scale(bigImageDimension,scale);
		bigImagePosition = image.localPosition;

		float smallImagePositionX;
		float smallImagePositionY;

		switch (xPositionPattern) {
		case resizePatternX.RIGHT:
			smallImagePositionX = -((smallImageDimension.x - smallDimension.x) / 2); 
			break;
		case resizePatternX.LEFT:
			smallImagePositionX = ((smallImageDimension.x-smallDimension.x) / 2);
			break;
		default:
			smallImagePositionX = 0;
			break;
		}

		switch (yPositionPattern) {
		case resizePatternY.UP:
			smallImagePositionY = -((smallImageDimension.y - smallDimension.y) / 2); 
			break;
		case resizePatternY.DOWN:
			smallImagePositionY = ((smallImageDimension.y-smallDimension.y) / 2);
			break;
		default:
			smallImagePositionY = 0;
			break;
		}

		smallImagePosition = new Vector2 (smallImagePositionX, smallImagePositionY);
	}

	void Update(){
	}

	public void toSmall(){
		this.GetComponent<RectTransform> ().sizeDelta = smallDimension;
		image.sizeDelta = smallImageDimension;
		image.localPosition = smallImagePosition;
	}


	public void drag(){
		if (globalScript.getWaitingStatus() == true && isCompleted == false) {
			this.GetComponent<RectTransform> ().position = Input.mousePosition;
			this.GetComponent<RectTransform> ().sizeDelta = bigDimension;
			this.GetComponent<RectTransform> ().SetAsLastSibling();
			image.sizeDelta = bigImageDimension;
			image.localPosition = bigImagePosition;
			isDrag = true;
		}
	}

	public void drop(){
		if (globalScript.getWaitingStatus () == true && isCompleted == false) {
			float distance = Vector2.Distance (this.GetComponent<RectTransform> ().localPosition, placeHolderPosition);
			if (distance < 50) {
				this.GetComponent<RectTransform> ().localPosition = placeHolderPosition;
				audioSource.PlayOneShot (permanentData.dropSound, 0.6f);
				isCompleted = true;
				globalScript.addPieceCompleted ();
				isDrag = false;
			} else {
				toSmall ();
				isDrag = false;
			}

		}
	}

	public bool getCompleted(){
		return isCompleted;
	}

	public bool getDrag(){
		return isDrag;
	}

	public void refresh(){
		isCompleted = false;
		image = this.transform.Find ("Image").GetComponent<RectTransform> ();
		globalScript = GameObject.Find ("Canvas").GetComponent<Global_Variable> ();
		string level = globalScript.getDifficulty ();
		if (Equals (level, "ts")) {
			scale = new Vector2(0.45f ,0.45f);
		}
		bigDimension = this.GetComponent<RectTransform> ().sizeDelta;
		smallDimension = Vector2.Scale(bigDimension,scale);
		placeHolderPosition = this.GetComponent<RectTransform> ().localPosition;
		bigImageDimension = image.sizeDelta;
		smallImageDimension = Vector2.Scale(bigImageDimension,scale);
		bigImagePosition = image.localPosition;

		float smallImagePositionX;
		float smallImagePositionY;

		switch (xPositionPattern) {
		case resizePatternX.RIGHT:
			smallImagePositionX = -((smallImageDimension.x - smallDimension.x) / 2); 
			break;
		case resizePatternX.LEFT:
			smallImagePositionX = ((smallImageDimension.x-smallDimension.x) / 2);
			break;
		default:
			smallImagePositionX = 0;
			break;
		}

		switch (yPositionPattern) {
		case resizePatternY.UP:
			smallImagePositionY = -((smallImageDimension.y - smallDimension.y) / 2); 
			break;
		case resizePatternY.DOWN:
			smallImagePositionY = ((smallImageDimension.y-smallDimension.y) / 2);
			break;
		default:
			smallImagePositionY = 0;
			break;
		}

		smallImagePosition = new Vector2 (smallImagePositionX, smallImagePositionY);
	}
}
