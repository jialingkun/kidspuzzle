using UnityEngine;
using System.Collections;

public class Piece_Properties : MonoBehaviour {
	public enum resizePatternX {RIGHT, LEFT, CENTER};
	public enum resizePatternY {UP, DOWN, CENTER};
	public resizePatternX xPositionPattern;
	public resizePatternY yPositionPattern;

	private RectTransform image;
	public Vector2 smallImagePosition;
	public Vector2 smallImageDimension;
	public Vector2 smallDimension;
	public Vector2 bigImagePosition;
	public Vector2 bigImageDimension;
	public Vector2 bigDimension;
	private Vector2 scale;

	public bool isDrag;

	void Start () {
		image = this.transform.Find ("Image").GetComponent<RectTransform> ();
		string level = GameObject.Find ("Canvas").GetComponent<Global_Variable> ().getLevel ();
		if (Equals (level, "ts")) {
			scale = new Vector2(0.5f ,0.5f);
		}
		Debug.Log (scale);
		bigDimension = this.GetComponent<RectTransform> ().sizeDelta;
		smallDimension = Vector2.Scale(bigDimension,scale);
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
		this.GetComponent<RectTransform> ().position = Input.mousePosition;
		this.GetComponent<RectTransform> ().sizeDelta = bigDimension;
		image.sizeDelta = bigImageDimension;
		image.localPosition = bigImagePosition;
		isDrag = true;
	}

	public void drop(){
		toSmall ();
		isDrag = false;
	}

	public bool getDrag(){
		return isDrag;
	}
}
