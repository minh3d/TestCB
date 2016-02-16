////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;

public class NotificationExample : BaseIOSFeaturePreview {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	void OnGUI() {

		UpdateToStartPos();
		
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Local and Push Notifications", style);
		
		
		StartY+= YLableStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Schedule Notification Silet")) {
			IOSNotificationController.instance.ScheduleNotification (15, "Your Notification Text No Sound", false, 0);
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Schedule Notification")) {
			IOSNotificationController.instance.ScheduleNotification (15, "Your Notification Text", true, 0);
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Cansel All Notifications")) {
			IOSNotificationController.instance.CancelNotifications();
		}


		StartX = XStartPos;
		StartY+= YButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Reg Device For Push Notif. ")) {
			#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			//push notifocations and RemoteNotificationType class can be used only on IOS Platfrom.
			
			IOSNotificationController.instance.RegisterForRemoteNotifications (RemoteNotificationType.Alert |  RemoteNotificationType.Badge |  RemoteNotificationType.Sound);
			IOSNotificationController.instance.addEventListener (IOSNotificationController.DEVICE_TOKEN_RECEIVED, OnTokenReived);
			
			#endif

		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Notification Banner")) {
			IOSNotificationController.instance.ShowNotificationBanner("Title", "Message");
		}


	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnTokenReived(CEvent e) {
		IOSNotificationDeviceToken token = e.data as IOSNotificationDeviceToken;
		Debug.Log ("OnTokenReived");
		Debug.Log (token.tokenString);

		IOSDialog.Create("OnTokenReived", token.tokenString);

		IOSNotificationController.instance.removeEventListener (IOSNotificationController.DEVICE_TOKEN_RECEIVED, OnTokenReived);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
