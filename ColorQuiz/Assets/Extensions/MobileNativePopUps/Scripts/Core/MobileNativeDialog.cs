using UnityEngine;
using System.Collections;

public class MobileNativeDialog : EventDispatcherBase {

	public MobileNativeDialog(string title, string message) {
		init(title, message, "Yes", "No");
	}

	public MobileNativeDialog(string title, string message, string yes, string no) {
		init(title, message, yes, no);
	}


	private void init(string title, string message, string yes, string no) {

		#if UNITY_WP8 || UNITY_METRO
		MNWP8Dialog dialog  = MNWP8Dialog.Create(title, message);
		dialog.addEventListener(BaseEvent.COMPLETE, OnComplete);
		#endif


		#if UNITY_IPHONE
		MNIOSDialog dialog  = MNIOSDialog.Create(title, message, yes, no);
		dialog.addEventListener(BaseEvent.COMPLETE, OnComplete);
		#endif

		#if UNITY_ANDROID
		MNAndroidDialog dialog  = MNAndroidDialog.Create(title, message, yes, no);
		dialog.addEventListener(BaseEvent.COMPLETE, OnComplete);
		#endif


	}



	private void OnComplete(CEvent e) {
		dispatch(BaseEvent.COMPLETE, e.data);
	}
}

