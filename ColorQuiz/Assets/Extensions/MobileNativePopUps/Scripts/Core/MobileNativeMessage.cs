using UnityEngine;
using System.Collections;

public class MobileNativeMessage : EventDispatcherBase {

	public MobileNativeMessage(string title, string message) {
		init(title, message, "Ok");
	}
	
	public MobileNativeMessage(string title, string message, string ok) {
		init(title, message, ok);
	}
	
	
	private void init(string title, string message, string ok) {
		
		#if UNITY_WP8 || UNITY_METRO
		MNWP8Message msg  = MNWP8Message.Create(title, message);
		msg.addEventListener(BaseEvent.COMPLETE, OnComplete);
		#endif
		
		
		#if UNITY_IPHONE
		MNIOSMessage msg  = MNIOSMessage.Create(title, message, ok);
		msg.addEventListener(BaseEvent.COMPLETE, OnComplete);
		#endif
		
		#if UNITY_ANDROID
		MNAndroidMessage msg  = MNAndroidMessage.Create(title, message, ok);
		msg.addEventListener(BaseEvent.COMPLETE, OnComplete);
		#endif
		

	}
	
	
	
	private void OnComplete(CEvent e) {
		dispatch(BaseEvent.COMPLETE, e.data);
	}
}

