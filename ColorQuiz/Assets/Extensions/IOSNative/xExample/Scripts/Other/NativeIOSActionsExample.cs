using UnityEngine;
using System.Collections;

public class NativeIOSActionsExample : BaseIOSFeaturePreview {

	public Texture2D hello_texture;
	public Texture2D darawTexgture = null;


	void Awake() {
		IOSSharedApplication.instance.addEventListener(IOSSharedApplication.URL_SCHEME_EXISTS, UrlExcists);
		IOSSharedApplication.instance.addEventListener(IOSSharedApplication.URL_SCHEME_NOT_FOUND, UrlNotFound);


		//Example how to use action instead of events
		IOSCamera.instance.OnImagePicked += OnImage;
	}



	void OnGUI() {
		UpdateToStartPos();


		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Using Url Scheme", style);
		
		
		StartY+= YLableStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Check if FB App exists")) {
			IOSSharedApplication.instance.CheckUrl("fb://");
		}
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Open Fb Profile")) {
			IOSSharedApplication.instance.OpenUrl("fb://profile");
		}
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Open App Store")) {
			IOSSharedApplication.instance.OpenUrl("itms-apps://");
		}

		StartX = XStartPos;
		StartY+= YButtonStep;
		StartY+= YLableStep;
		
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Camera Roll", style);
		
		StartY+= YLableStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Save Screenshot To Camera Roll")) {
			IOSCamera.instance.SaveScreenshotToCameraRoll();
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Save Texture To Camera Roll")) {
			IOSCamera.instance.SaveTextureToCameraRoll(hello_texture);
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Get Image From Album")) {
			IOSCamera.instance.GetImageFromAlbum();
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Get Image From Camera")) {
			IOSCamera.instance.GetImageFromCamera();
		}

		StartX = XStartPos;
		StartY+= YButtonStep;
		StartY+= YLableStep;
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "PickedImage", style);
		StartY+= YLableStep;

		if(darawTexgture != null) {
			GUI.DrawTexture(new Rect(StartX, StartY, buttonWidth, buttonWidth), darawTexgture);
		}
	

	}

	private void UrlExcists(CEvent e) {
		string url = e.data as string;
		IOSMessage.Create("Url Exists", "The " + url + " is registred" );
	}


	private void UrlNotFound(CEvent e) {
		string url = e.data as string;
		IOSMessage.Create("Url Exists", "The " + url + " wasn't registred" );
	}


	

	private void OnImage (IOSImagePickResult result) {
		if(result.IsSucceeded) {
			darawTexgture = result.image;
		}
	}
}
