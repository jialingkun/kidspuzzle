using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balloon : MonoBehaviour {
	private Global_Variable globalScript;
	private Vector2 initialPosition;
	private bool isMoving;
	private bool clicked;
	private float moveSpeed;
	private float minSpeed;
	private float maxSpeed;
	private float yAxisLimit;
	private Image imageComponent;
	private Sprite selectedBalloonSprite;
	private Sprite selectedBlastSprite;

	private Vector2 targetPosition;

	private Game_Data permanentData;
	private AudioSource audioSource;
	private AudioClip blast;
	// Use this for initialization
	void Start () {
		permanentData = SaveLoad.getPermanentData ();
		//audio
		audioSource = permanentData.getSEaudioSource();
		blast = permanentData.blastSound;

		globalScript = GameObject.Find ("Canvas").GetComponent<Global_Variable> ();

		//sprite
		imageComponent = this.GetComponent<Image> ();
		selectedBalloonSprite = permanentData.balloonImage [Random.Range (0, permanentData.balloonImage.Length)];
		selectedBlastSprite = permanentData.blastImage [Random.Range (0, permanentData.blastImage.Length)];
		imageComponent.sprite = selectedBalloonSprite;

		initialPosition = this.transform.position;
		isMoving = false;
		clicked = false;
		minSpeed = 0.5f;
		maxSpeed = 0.9f;
		yAxisLimit = GameObject.Find("heightLimit").transform.position.y;
		moveSpeed = 1f;

		targetPosition = new Vector2 (this.transform.position.x, yAxisLimit);
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving == true) {
			this.transform.position = Vector2.MoveTowards (this.transform.position, targetPosition, moveSpeed * Time.deltaTime * Screen.currentResolution.height);
		}

		if (Vector2.Distance (this.transform.position, targetPosition) < 0.5f && isMoving == true) {
			borderDestroyBalloon ();
		}
	}

	public void moveUp(){
		moveSpeed = Random.Range (minSpeed, maxSpeed);
		isMoving = true;
	}

	public void clickDestroyBalloon(){
		if (clicked == false) {
			clicked = true;
			isMoving = false;
			audioSource.PlayOneShot (blast, 0.6f);
			StartCoroutine (balloonDestroyed ());
		}

	}

	public void borderDestroyBalloon(){
		isMoving = false;
		StartCoroutine (balloonDestroyed ());
	}


	IEnumerator balloonDestroyed(){
		imageComponent.sprite = selectedBlastSprite;
		yield return new WaitForSeconds(0.3f);
		this.transform.position = initialPosition;
		globalScript.balloonBlast ();
		refresh ();
	}

	public void refresh(){
		selectedBalloonSprite = permanentData.balloonImage [Random.Range (0, permanentData.balloonImage.Length)];
		selectedBlastSprite = permanentData.blastImage [Random.Range (0, permanentData.blastImage.Length)];
		imageComponent.sprite = selectedBalloonSprite;
		clicked = false;
	}

}
