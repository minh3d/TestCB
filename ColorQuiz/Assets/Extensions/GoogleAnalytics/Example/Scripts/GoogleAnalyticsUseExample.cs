using UnityEngine;
using System.Collections;

public class GoogleAnalyticsUseExample : MonoBehaviour {


	void Start () {

		//This call will be ignored of you already have GoogleAnalytics gameobject on your scene, like in the example scene
		//However if you do not want to create additional object in your initial scene
		//you may use this code to initialize analytics
		//WARNING: if you do not have GoogleAnalytics gamobect and you skip StartTracking call, GoogleAnalytics gameobect will be
		//initialized on first GoogleAnalytics.Client call
		GoogleAnalytics.StartTracking();
	}
	

	void OnGUI () {
		if(GUI.Button(new Rect(10, 10, 150, 50), "Page Hit")) {
			GoogleAnalytics.Client.SendPageHit("mydemo.com ", "/home", "homepage");
		}
		
		
		if(GUI.Button(new Rect(10, 70, 150, 50), "Event Hit")) {
			GoogleAnalytics.Client.SendEventHit("video", "play", "holiday", 300);
		}

		
		if(GUI.Button(new Rect(10, 130, 150, 50), "Transaction Hit")) {
			GoogleAnalytics.Client.SendTransactionHit("12345", "westernWear", "EUR", 50.00f, 32.00f, 12.00f);
		}

		if(GUI.Button(new Rect(10, 190, 150, 50), "Item Hit")) {
			GoogleAnalytics.Client.SendItemHit("12345", "sofa", "u3eqds43", 300f, 2, "furniture", "EUR");
		}

		if(GUI.Button(new Rect(190, 10, 150, 50), "Social Hit")) {
			GoogleAnalytics.Client.SendSocialHit("like", "facebook", "/home ");
		}

		if(GUI.Button(new Rect(190, 70, 150, 50), "Exception Hit")) {
			GoogleAnalytics.Client.SendExceptionHit("IOException", true);
		}

		if(GUI.Button(new Rect(190, 130, 150, 50), "Timing Hit")) {
			GoogleAnalytics.Client.SendUserTimingHit("jsonLoader", "load", 5000, "jQuery");
		}

		if(GUI.Button(new Rect(190, 190, 150, 50), "Screen Hit")) {
			GoogleAnalytics.Client.SendScreenHit("MainMenu");
		}





	
	}

	public void CustomBuildersExamples() {
		/*******/
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.SOCIAL);
		GoogleAnalytics.Client.SetSocialAction("like");
		GoogleAnalytics.Client.SetSocialNetwork("facebook");
		GoogleAnalytics.Client.SetSocialActionTarget("/home");
		
		GoogleAnalytics.Client.Send();

		/*******/

		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.EVENT);
		GoogleAnalytics.Client.AppendData("cm[1-9][0-9]*", "47");
		GoogleAnalytics.Client.Send();

	}
}
