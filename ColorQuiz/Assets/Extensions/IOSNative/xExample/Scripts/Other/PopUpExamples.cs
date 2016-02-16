////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopUpExamples : BaseIOSFeaturePreview {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		IOSNativePopUpManager.showMessage ("Wlcome", "Hey there, welcome to pop-ups testing scene!");
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	

	void OnGUI() {

		UpdateToStartPos();
		
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Native Pop Ups", style);

		StartY+= YLableStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Rate PopUp with events")) {
			IOSRateUsPopUp rate = IOSRateUsPopUp.Create("Like this game?", "Please rate to support future updates!");
			rate.addEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
		}
		

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Dialog PopUp")) {
			IOSDialog dialog = IOSDialog.Create("Dialog Titile", "Dialog message");
			dialog.addEventListener(BaseEvent.COMPLETE, onDialogClose);
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Message PopUp")) {
			IOSMessage msg = IOSMessage.Create("Message Titile", "Message message");
			msg.addEventListener(BaseEvent.COMPLETE, onMessageClose);
		}


		StartX = XStartPos;
		StartY+= YButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Dismissed PopUp")) {
			Invoke ("dismissAler", 2f);
			IOSMessage.Create("Hello", "I will die in 2 sec");
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Open App Store")) {
			IOSNativeUtility.RedirectToAppStoreRatingPage();
		}


		StartX = XStartPos;
		StartY+= YButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Preloader ")) {
			IOSNativeUtility.ShowPreloader();
			Invoke("HidePreloader", 3f);
		}
		
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Hide Preloader")) {
			HidePreloader();
		}
		
	}
	

	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void HidePreloader() {
		IOSNativeUtility.HidePreloader();
	}

	private void dismissAler() {
		IOSNativePopUpManager.dismissCurrentAlert ();
	}
	
	private void onRatePopUpClose(CEvent e) {
		(e.dispatcher as IOSRateUsPopUp).removeEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
		string result = e.data.ToString();
		IOSNativePopUpManager.showMessage("Result", result + " button pressed");
	}
	
	private void onDialogClose(CEvent e) {

		//romoving listner
		(e.dispatcher as IOSDialog).removeEventListener(BaseEvent.COMPLETE, onDialogClose);

		//parsing result
		switch((IOSDialogResult)e.data) {
		case IOSDialogResult.YES:
			Debug.Log ("Yes button pressed");
			break;
		case IOSDialogResult.NO:
			Debug.Log ("No button pressed");
			break;

		}

		string result = e.data.ToString();
		IOSNativePopUpManager.showMessage("Result", result + " button pressed");
	}
	
	private void onMessageClose(CEvent e) {
		(e.dispatcher as IOSMessage).removeEventListener(BaseEvent.COMPLETE,  onMessageClose);
		IOSNativePopUpManager.showMessage("Result", "Message Closed");
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
