using UnityEngine;
using System.Collections;

public class StarGame : MonoBehaviour {

	public Animator circle;
	public static MobileNativeRateUs ratePopUp;
	public GameObject selectgamegroup;
	public static int selected;
	private static int adid;

	private bool isAnimated = false;

	public void run() {
		if (!isAnimated && PlayerPrefs.GetString("sid") != null && PlayerPrefs.GetString("sid") != "") {
			selectgamegroup.SetActive (true);
		}
	}

	public void Rateus() {
		ratePopUp.Start ();
	}

	void Start() {

		//rate us
		if (ratePopUp == null) {
			ratePopUp = new MobileNativeRateUs ("Like this game?", "Please rate to support future updates!");
			ratePopUp.SetAppleId ("916270150");
			ratePopUp.SetAndroidAppUrl ("https://play.google.com/store/apps/details?id=com.tap.colorfun");
			ratePopUp.addEventListener(BaseEvent.COMPLETE, OnRatePopUpClose);
		}
	}

	void OnRatePopUpClose(CEvent e) {
		//parsing result
		switch((MNDialogResult)e.data) {
		case MNDialogResult.RATED:
			PlayerPrefs.SetInt("rateus", 1);
			break;
		case MNDialogResult.REMIND:
			break;
		case MNDialogResult.DECLINED:
			PlayerPrefs.SetInt("rateus", 1);
			break;
		}
	}

	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			MobileNativeDialog dialog = new MobileNativeDialog("Info", "Do you want to quit this game ?");
			dialog.addEventListener(BaseEvent.COMPLETE, OnDialogClose);
			
		}
	}

	void OnDialogClose(CEvent e) {
		//removing listner
		e.dispatcher.removeEventListener(BaseEvent.COMPLETE, OnDialogClose);
		//parsing result
		switch((MNDialogResult)e.data) {
		case MNDialogResult.YES:
			Application.Quit ();
			break;
		case MNDialogResult.NO:
			return;
			break;
		}
	}


	public void game1() {
		isAnimated = true;
		selected = 1;
		selectgamegroup.SetActive (false);
		circle.SetTrigger("Run");
	}

	public void game2() {
		isAnimated = true;
		selected = 2;
		selectgamegroup.SetActive (false);
		circle.SetTrigger("Run");
	}

	public void game3() {
		isAnimated = true;
		selected = 3;
		selectgamegroup.SetActive (false);
		circle.SetTrigger("Run");
	}

	public void game4() {
		MobileNativeMessage msg = new MobileNativeMessage("Info", "Coming soon!");
	}



}
