////////////////////////////////////////////////////////////////////////////////
//  
// @module V2D
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

public class GoogleAnalyticsMenu : EditorWindow {
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	#if UNITY_EDITOR

	[MenuItem("Window/Google Analytics/Edit Settings")]
	public static void Edit() {
		Selection.activeObject = GoogleAnalyticsSettings.Instance;
	}

	[MenuItem("Window/Google Analytics/Create Analytics GameObject")]
	public static void Create() {
		GameObject an = new GameObject("Google Analytics");
		an.AddComponent<GoogleAnalytics>();
		Selection.activeObject = an;
	}



	[MenuItem("Window/Google Analytics/Measurement Protocol Developer Guide")]
	public static void ProtocolDocumentation() {
		string url = "https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide";
		Application.OpenURL(url);
	}


	[MenuItem("Window/Google Analytics/Measurement Protocol Parameter Reference")]
	public static void ParamDocumentation() {
		string url = "https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Google Analytics/Plugin Online Documentation")]
	public static void OpenDocumentation() {
		string url = "http://goo.gl/XmJX8M";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Google Analytics/Discussions/Unity Forum")]
	public static void UnityForum() {
		string url = "http://goo.gl/B7YHzf";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Google Analytics/Discussions/PlayMaker Forum")]
	public static void PlayMakerForum() {
		string url = "http://goo.gl/0bLwcT";
		Application.OpenURL(url);
	}


	
	

	#endif

}
