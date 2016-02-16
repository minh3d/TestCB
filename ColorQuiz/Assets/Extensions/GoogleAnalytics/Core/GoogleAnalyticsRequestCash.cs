using UnityEngine;
using System.Collections;

public class GoogleAnalyticsRequestCash  {

	private const string DATA_SPLITTER = "|";
	private const string GA_DATA_CASH_KEY = "GoogleAnalyticsRequestCash";

	public static void SaveRequest(string cash) {
		string data = SavedData;
		if(data != string.Empty) {
			data = data + DATA_SPLITTER + cash;
		} else {
			data = cash;
		}

		SavedData = data;
	}

	public static void SendChashedRequests() {

		string data = SavedData;
		if(data != string.Empty) {
			string[] requests = data.Split(DATA_SPLITTER [0]);
			foreach(string request in requests) {
				GoogleAnalytics.SendSkipCash(request);
			}
			
		} 

		Clear();
	}


	public static void Clear() {
		PlayerPrefs.DeleteKey(GA_DATA_CASH_KEY);
	}

	public static string SavedData {
		get {
			if(PlayerPrefs.HasKey(GA_DATA_CASH_KEY)) {
				return PlayerPrefs.GetString(GA_DATA_CASH_KEY);
			} else {
				return string.Empty;
			}
		}

		set {
			PlayerPrefs.SetString(GA_DATA_CASH_KEY, value);
		}
	}
}
