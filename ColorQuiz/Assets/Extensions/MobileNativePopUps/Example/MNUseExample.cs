////////////////////////////////////////////////////////////////////////////////
//  
// @module <module_name>
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class MNUseExample : MNFeaturePreview {

	public string appleId = "";
	public string apdroidAppUrl = "market://details?id=com.google.earth";

	void Awake() {

	}
	
	void OnGUI() {
		
		UpdateToStartPos();
		
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Native Pop Ups", style);
		StartY+= YLableStep;


		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Rate PopUp with events")) {
			MobileNativeRateUs ratePopUp =  new MobileNativeRateUs("Like this game?", "Please rate to support future updates!");
			ratePopUp.SetAppleId(appleId);
			ratePopUp.SetAndroidAppUrl(apdroidAppUrl);
			ratePopUp.addEventListener(BaseEvent.COMPLETE, OnRatePopUpClose);

			ratePopUp.Start();


		}
		
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Dialog PopUp")) {
			MobileNativeDialog dialog = new MobileNativeDialog("Dialog Titile", "Dialog message");
			dialog.addEventListener(BaseEvent.COMPLETE, OnDialogClose);
		}
		
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Message PopUp")) {
			MobileNativeMessage msg = new MobileNativeMessage("Message Titile", "Message message");
			msg.addEventListener(BaseEvent.COMPLETE, OnMessageClose);
		}

		StartY += YButtonStep;
		StartX = XStartPos;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Prealoder")) {
			MNP.ShowPreloader("Title", "Message");
			Invoke("OnPreloaderTimeOut", 3f);
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Hide Prealoder")) {
			MNP.HidePreloader();
		}
		
	}
	
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------


	private void OnPreloaderTimeOut() {
		MNP.HidePreloader();
	}
	

	
	private void OnRatePopUpClose(CEvent e) {
		//removing listner
		e.dispatcher.removeEventListener(BaseEvent.COMPLETE, OnRatePopUpClose);

		//parsing result
		switch((MNDialogResult)e.data) {
		case MNDialogResult.RATED:
			Debug.Log ("Rate Option pickied");
			break;
		case MNDialogResult.REMIND:
			Debug.Log ("Remind Option pickied");
			break;
		case MNDialogResult.DECLINED:
			Debug.Log ("Declined Option pickied");
			break;
		}

		string result = e.data.ToString();
		new MobileNativeMessage("Result", result + " button pressed");

	}
	
	private void OnDialogClose(CEvent e) {
		
		//removing listner
		e.dispatcher.removeEventListener(BaseEvent.COMPLETE, OnDialogClose);
		
		//parsing result
		switch((MNDialogResult)e.data) {
		case MNDialogResult.YES:
			Debug.Log ("Yes button pressed");
			break;
		case MNDialogResult.NO:
			Debug.Log ("No button pressed");
			break;
			
		}
		
		string result = e.data.ToString();
		new MobileNativeMessage("Result", result + " button pressed");
	}
	
	private void OnMessageClose(CEvent e) {
		//removing listner
		e.dispatcher.removeEventListener(BaseEvent.COMPLETE,  OnMessageClose);
		new MobileNativeMessage("Result", "Message Closed");
	}




}
