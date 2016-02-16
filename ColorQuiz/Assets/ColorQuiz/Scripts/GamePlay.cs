﻿using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour {


	//obj
	public UISprite circleTime;
	public UILabel colorText;
	public UILabel scoreText;
	public UILabel scoreText2;
	public UILabel highScoreText;

	//Group
	public GameObject startGameGroup;
	public GameObject gameGroup;
	public GameObject headerGroup;
	public GameObject buttonGroup;
	public GameObject gameOverGroup;


	public Animator gameAnimator;

	//Rotate circle time
	private float time = 3.0f; // x2
	private float timeDelta = 0.04f; // x2 - 0.04
	private float lastRotate = 0;
	private float delta = 0;
	private float rotateRate = 0.01f;
	private int rangeColor = 8;

	//gameplay
	private bool anwser = false;
	private bool playing = false;
	private int score = -1;

	//Private color
	private Color[] colors = new Color[10];
	private string[] colorStrings = {"Yellow", "Red", "Blue", "Green", "Orange", "Purple", "Pink", "Brown"};

	// Use this for initialization
	void Start () {
		GoogleAnalytics.Client.SendScreenHit ("Game1");
		//Show highscore
		highScoreText.text = PlayerPrefs.GetInt ("highgame").ToString();

		//Iniut
		Color yellow = new Color(0.933f, 0.9f, 0.0549f);
		Color red = new Color(0.91f, 0.294f, 0.22f);
		Color blue = new Color(0.196f, 0.6f, 0.8627f);
		Color green = new Color(0.1686f, 0.8f, 0.431f);
		Color orange = new Color(0.9372f, 0.4588f, 0.0196f);
		Color purple = new Color(0.6039f, 0.34117f, 0.7137f);
		Color pink = new Color(1f, 0.53f, 0.9019f);
		Color brown = new Color(0.647f, 0.447f, 0.192f);

		colors [0] = yellow;
		colors [1] = red;
		colors [2] = blue;
		colors [3] = green;
		colors [4] = orange;
		colors [5] = purple;
		colors [6] = pink;
		colors [7] = brown;

		gameAnimator.SetTrigger("StartGame");
	}

	public void startGame() {
		startGameGroup.SetActive (false);
		gameGroup.SetActive (true);
		headerGroup.SetActive (true);
		buttonGroup.SetActive (true);

		newLevel ();
		playing = true;
	}

	void newLevel() {
		score ++;
		circleTime.fillAmount = 0;

		//Set score
		scoreText.text = score.ToString ();
		scoreText2.text = score.ToString ();
		gameAnimator.SetTrigger("ShakeScore");

		//Random answer
		int ranColor;
		int ranText;
		int anwserInt = Random.Range (0, 2);
		if (anwserInt == 1) {
			//Random color
			ranColor = ranText = Random.Range (0, rangeColor);
			anwser = true;
		} else {
			ranColor = Random.Range (0, rangeColor);
			ranText = Random.Range (0, rangeColor);

			while (ranText == ranColor) {
				ranText = Random.Range (0, rangeColor);
			}
			anwser = false;
		}

		colorText.text = colorStrings [ranText];
		colorText.color = colors [ranColor];
		circleTime.color = colors [ranColor];

		//Calc new rate
		if (time > 0.9f) {
			time -= timeDelta;
			delta = rotateRate / time;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Time.time >= lastRotate && circleTime.fillAmount < 1 && playing) {
			circleTime.fillAmount += delta;
			lastRotate = Time.time + rotateRate;
			if (circleTime.fillAmount >= 1) {
				//newLevel();
				GameOver();
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			MobileNativeDialog dialog = new MobileNativeDialog("Info", "Do you want back to main menu ?");
			dialog.addEventListener(BaseEvent.COMPLETE, OnDialogClose);
			Time.timeScale = 0;
		}
	}

	void OnDialogClose(CEvent e) {
		//removing listner
		e.dispatcher.removeEventListener(BaseEvent.COMPLETE, OnDialogClose);
		//parsing result
		Time.timeScale = 1;
		switch((MNDialogResult)e.data) {
		case MNDialogResult.YES:
			Application.LoadLevel (0);
			break;
		case MNDialogResult.NO:
			return;
			break;
		}
	}

	void GameOver() {
		playing = false;
		headerGroup.SetActive (false);
		gameGroup.SetActive (false);
		buttonGroup.SetActive (false);
		gameOverGroup.SetActive (true);

		if (score > PlayerPrefs.GetInt ("highgame")) {
			PlayerPrefs.SetInt ("highgame", score);
		}

		if (PlayerPrefs.GetInt("rateus") == 0) {
			StarGame.ratePopUp.Start ();
		}
	}


	public void TrueBtn() {
		if (anwser == true) {
			newLevel();
		} else {
			GameOver();
		}
	}

	public void FalseBtn() {
		if (anwser == false) {
			newLevel();
		} else {
			GameOver();
		}
	}

	public void BackBtn() {
		Application.LoadLevel (0);
	}

	public void RestartBtn() {
		Application.LoadLevel (Application.loadedLevel);
	}

	public void ShareBtn() {
		StartCoroutine (TakeScreenshot());
	}

	private IEnumerator TakeScreenshot() 
	{
		yield return new WaitForEndOfFrame();
		var width = Screen.width;
		var height = Screen.height;
		var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		byte[] screenshot = tex.EncodeToPNG();
		
		var wwwForm = new WWWForm();
		wwwForm.AddBinaryData("image", screenshot, "batchu.png");
		
		UM_ShareUtility.ShareMedia("Hihi","This is my text to share", tex);
	}
}