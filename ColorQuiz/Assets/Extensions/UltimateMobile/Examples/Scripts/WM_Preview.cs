using UnityEngine;
using System.Collections;

public class WM_Preview : BaseIOSFeaturePreview {

	public static IOSNativePreviewBackButton back = null;
	
	
	
	void Awake() {
		if(back == null) {
			back = IOSNativePreviewBackButton.Create();
		}
		
	}
	
	
	void OnGUI() {
		
		UpdateToStartPos();
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Unified  API Examples", style);
		
		StartY+= YLableStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Billing Preview")) {
			Application.LoadLevel("UM_BillingPreview");
		}
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Game Service Example")) {
			Application.LoadLevel("UM_GameServiceBasics");
		}
		
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Social API Example")) {
			Application.LoadLevel("UM_SocailExample");
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Ad Example")) {
			Application.LoadLevel("UM_AdExample");
		}
		
		

	}
}
