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

public class WP8Message : WP8PopupBase {
		
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
			
	public static WP8Message Create(string title, string message) {
		WP8Message dialog;
		dialog  = new GameObject("WP8Message").AddComponent<WP8Message>();
		dialog.title = title;
		dialog.message = message;
		dialog.init();
		
		return dialog;
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public void init() {
		#if UNITY_WP8 || UNITY_METRO
		NativePopUps.PopUp.ShowMessageWindow_OK(message, title, onPopUpCallBack);
		#endif
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	public void onPopUpCallBack() {
		dispatch(BaseEvent.COMPLETE);
		Destroy(gameObject);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}