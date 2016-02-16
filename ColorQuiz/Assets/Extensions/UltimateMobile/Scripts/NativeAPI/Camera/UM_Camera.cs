using System;
using UnityEngine;
using System.Collections;

public class UM_Camera : SA_Singleton<UM_Camera> {

	//Actions
	public Action<UM_ImagePickResult> OnImagePicked;
	
	//Events
	public const string  IMAGE_PICKED = "image_picked";



	void Awake() {
		DontDestroyOnLoad(gameObject);

		AndroidCamera.instance.OnImagePicked += OnAndroidImagePicked;
		IOSCamera.instance.OnImagePicked += OnIOSImagePicked;
	}

	public void SaveImageToGalalry(Texture2D image) {
		switch(Application.platform) {
			case RuntimePlatform.Android:
				AndroidCamera.instance.SaveImageToGalalry(image);
				break;
			case RuntimePlatform.IPhonePlayer:
				IOSCamera.instance.SaveTextureToCameraRoll(image);
				break;
		}
		
	}
	
	
	public void SaveScreenshotToGallery() {
		switch(Application.platform) {
			case RuntimePlatform.Android:
				AndroidCamera.instance.SaveScreenshotToGallery();
				break;
			case RuntimePlatform.IPhonePlayer:
				IOSCamera.instance.SaveScreenshotToCameraRoll();
				break;
		}
	}
	
	
	public void GetImageFromGallery() {
		switch(Application.platform) {
			case RuntimePlatform.Android:
				AndroidCamera.instance.GetImageFromGallery();
				break;
			case RuntimePlatform.IPhonePlayer:
				IOSCamera.instance.GetImageFromAlbum();
				break;
		}
	}
	
	
	
	public void GetImageFromCamera() {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			AndroidCamera.instance.GetImageFromCamera();
			break;
		case RuntimePlatform.IPhonePlayer:
			IOSCamera.instance.GetImageFromCamera();
			break;
		}
	}
	
	

	void OnAndroidImagePicked (AndroidImagePickResult obj) {
		UM_ImagePickResult result = new UM_ImagePickResult(obj.image);
		if(OnImagePicked != null) {
			OnImagePicked(result);
		}

		dispatch(IMAGE_PICKED, result);
	}

	void OnIOSImagePicked (IOSImagePickResult obj) {
		UM_ImagePickResult result = new UM_ImagePickResult(obj.image);
		if(OnImagePicked != null) {
			OnImagePicked(result);
		}
		
		dispatch(IMAGE_PICKED, result);
	}
}
