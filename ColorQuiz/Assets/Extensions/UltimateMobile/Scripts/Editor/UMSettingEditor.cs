
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(UltimateMobileSettings))]
public class UMSettingEditor : Editor {


	GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");
	GUIContent SupportEmail = new GUIContent("Support [?]", "If you have any technical quastion, feel free to drop an e-mail");



	private UltimateMobileSettings settings;


	public override void OnInspectorGUI() {
		settings = UltimateMobileSettings.Instance;

		GUI.changed = false;



		EditorGUILayout.HelpBox("Ultimate Mobile Pluging", MessageType.None);

		GeneralOptions();
		EditorGUILayout.Space();
		InAppSettings();
		EditorGUILayout.Space();
		GameServiceSettings();
		EditorGUILayout.Space();
		AdSettings();
		EditorGUILayout.Space();
		AboutGUI();

	

		if(GUI.changed) {
			DirtyEditor();
		}
	}


	GUIContent LID = new GUIContent("Leaderboard Id[?]:", "Uniquie Leaderboard Id");
	GUIContent IOSLID = new GUIContent("IOS Leaderboard Id[?]:", "IOS Leaderboard Id");
	GUIContent ANDROIDLID = new GUIContent("Android Leaderboard Id[?]:", "Android Leaderboard Id");



	GUIContent AID = new GUIContent("Achievement Id[?]:", "Uniquie Leaderboard Id");
	GUIContent ALID = new GUIContent("IOS Achievement Id[?]:", "IOS Leaderboard Id");
	GUIContent ANDROIDAID = new GUIContent("Android Achievement Id[?]:", "Android Leaderboard Id");
	

	GUIContent Base64KeyLabel = new GUIContent("Base64 Key[?]:", "Base64 Key app key.");
	GUIContent InAppID = new GUIContent("ProductId[?]:", "UniquieProductId");
	GUIContent IsCons = new GUIContent("Is Consumable[?]:", "Is prodcut allowed to be purchased more than once?");

	GUIContent IOSSKU = new GUIContent("IOS SKU[?]:", "IOS SKU");
	GUIContent AndroidSKU = new GUIContent("Android SKU[?]:", "Android SKU");
	GUIContent WP8SKU = new GUIContent("WP8 SKU[?]:", "WP8 SKU");


	private const string version_info_file = "Plugins/StansAssets/Versions/UM_VersionInfo.txt"; 


	public static bool IsInstalled {
		get {

			if(FileStaticAPI.IsFileExists(PluginsInstalationUtil.ANDROID_DESTANATION_PATH + "androidnative.jar") && FileStaticAPI.IsFileExists(PluginsInstalationUtil.IOS_DESTANATION_PATH + "GoogleMobileAdBanner.h")) {
				return true;
			} else {
				return false;
			}

		}
	}
	
	public static bool IsUpToDate {
		get {
			if(UltimateMobileSettings.VERSION_NUMBER.Equals(DataVersion)) {
				return true;
			} else {
				return false;
			}
		}
	}
	
	
	public static string DataVersion {
		get {
			if(FileStaticAPI.IsFileExists(version_info_file)) {
				return FileStaticAPI.Read(version_info_file);
			} else {
				return "Unknown";
			}
		}
	}
	
	public static void UpdateVersionInfo() {
		FileStaticAPI.Write(version_info_file, UltimateMobileSettings.VERSION_NUMBER);
		AndroidNativeSettingsEditor.UpdateVersionInfo();

	}



	private void GeneralOptions() {

		if(!IsInstalled) {
			EditorGUILayout.HelpBox("Install Required ", MessageType.Error);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			Color c = GUI.color;
			GUI.color = Color.cyan;
			if(GUILayout.Button("Install Plugin",  GUILayout.Width(120))) {
				PluginsInstalationUtil.Android_InstallPlugin();
				PluginsInstalationUtil.IOS_InstallPlugin();
				UpdateVersionInfo();
			}
			GUI.color = c;
			EditorGUILayout.EndHorizontal();
		}
		
		if(IsInstalled) {
			if(!IsUpToDate) {
				EditorGUILayout.HelpBox("Update Required \nResources version: " + DataVersion + " Plugin version: " + UltimateMobileSettings.VERSION_NUMBER, MessageType.Warning);
				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				Color c = GUI.color;
				GUI.color = Color.cyan;
				if(GUILayout.Button("Update to " + UltimateMobileSettings.VERSION_NUMBER,  GUILayout.Width(250))) {
					PluginsInstalationUtil.Android_UpdatePlugin();
					PluginsInstalationUtil.IOS_UpdatePlugin();
					UpdateVersionInfo();
				}
				
				GUI.color = c;
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				
			} else {
				EditorGUILayout.HelpBox("Ultimate Mobile Plugin v" + UltimateMobileSettings.VERSION_NUMBER + " is installed", MessageType.Info);
			}
		}
		
		
		EditorGUILayout.Space();
		
	}


	private void GameServiceSettings() {
	
		EditorGUI.indentLevel = 0;
		settings.IsGameServiceOpen = EditorGUILayout.Foldout(settings.IsGameServiceOpen, "Game Service");
		if(settings.IsGameServiceOpen) {
			EditorGUI.indentLevel++; {
				settings.IsLeaderBoardsOpen = EditorGUILayout.Foldout(settings.IsLeaderBoardsOpen, "Leaderboards");
				if(settings.IsLeaderBoardsOpen) {
				
					if(settings.Leaderboards.Count == 0) {
						EditorGUILayout.HelpBox("No Leaderboards Added", MessageType.Warning);
					}
					
					foreach(UM_Leaderboard leaderbaord in settings.Leaderboards) {
						EditorGUILayout.BeginVertical (GUI.skin.box);
						leaderbaord.IsOpen = EditorGUILayout.Foldout(leaderbaord.IsOpen, leaderbaord.id);
						if(leaderbaord.IsOpen) {
							EditorGUI.indentLevel++; {
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField(LID);
								leaderbaord.id	 	= EditorGUILayout.TextField(leaderbaord.id);
								EditorGUILayout.EndHorizontal();
								
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField(IOSLID);
								leaderbaord.IOSId	 	= EditorGUILayout.TextField(leaderbaord.IOSId);
								EditorGUILayout.EndHorizontal();
								
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField(ANDROIDLID);
								leaderbaord.AndroidId	 	= EditorGUILayout.TextField(leaderbaord.AndroidId);
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.Space();
								if(GUILayout.Button("Remove",  GUILayout.Width(80))) {
									settings.RemoveLeaderboard(leaderbaord);
									break;
								}
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.Space();
								
							} EditorGUI.indentLevel--;
						}
						EditorGUILayout.EndVertical();
					}
					
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					if(GUILayout.Button("Add",  GUILayout.Width(80))) {
						settings.AddLeaderboard(new UM_Leaderboard());
					}
					EditorGUILayout.EndHorizontal();
					
					
					EditorGUILayout.Space();

				}

				settings.IsAchievmentsOpen = EditorGUILayout.Foldout(settings.IsAchievmentsOpen, "Achievements");
				if(settings.IsAchievmentsOpen) {
					if(settings.Achievements.Count == 0) {
						EditorGUILayout.HelpBox("No Achievements Added", MessageType.Warning);
					}
					
					foreach(UM_Achievement achievement in settings.Achievements) {
						EditorGUILayout.BeginVertical (GUI.skin.box);
						achievement.IsOpen = EditorGUILayout.Foldout(achievement.IsOpen, achievement.id);
						if(achievement.IsOpen) {
							EditorGUI.indentLevel++; {
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField(AID);
								achievement.id	 	= EditorGUILayout.TextField(achievement.id);
								EditorGUILayout.EndHorizontal();
								
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField(ALID);
								achievement.IOSId	 	= EditorGUILayout.TextField(achievement.IOSId);
								EditorGUILayout.EndHorizontal();
								
								
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField(ANDROIDAID);
								achievement.AndroidId	 	= EditorGUILayout.TextField(achievement.AndroidId);
								EditorGUILayout.EndHorizontal();

								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.Space();
								if(GUILayout.Button("Remove",  GUILayout.Width(80))) {
									settings.RemoveAchievement(achievement);
									break;
								}
								EditorGUILayout.EndHorizontal();
								
								EditorGUILayout.Space();
								
							} EditorGUI.indentLevel--;
						}
						EditorGUILayout.EndVertical();
					}


					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					if(GUILayout.Button("Add",  GUILayout.Width(80))) {
						settings.AddAchievment(new UM_Achievement());
					}
					EditorGUILayout.EndHorizontal();
					
					
					EditorGUILayout.Space();

				}



			} EditorGUI.indentLevel--;
		}
	}


	GUIContent IOS_UnitAdId  	 = new GUIContent("Banners Ad Unit Id [?]:",  "IOS Banners Ad Unit Id ");
	GUIContent IOS_InterstAdId   = new GUIContent("Interstitials Ad Unit Id [?]:", "IOS Interstitials Ad Unit Id");
	
	GUIContent Android_UnitAdId  	 = new GUIContent("Banners Ad Unit Id [?]:",  "Android Banners Ad Unit Id ");
	GUIContent Android_InterstAdId   = new GUIContent("Interstitials Ad Unit Id [?]:", "Android Interstitials Ad Unit Id");
	
	GUIContent WP8_UnitAdId  	 = new GUIContent("Banners Ad Unit Id [?]:",  "WP8 Banners Ad Unit Id ");
	GUIContent WP8_InterstAdId   = new GUIContent("Interstitials Ad Unit Id [?]:", "WP8 Interstitials Ad Unit Id");

	GUIContent Ad_Endgine   = new GUIContent("Ad Engine [?]:", "Ad Engine for seleceted platform");
	
	
	
	private void AdSettings() {
		EditorGUI.indentLevel = 0;
		settings.IsaAdvertisementSettingsOpen = EditorGUILayout.Foldout(settings.IsaAdvertisementSettingsOpen, "Advertisement");
		if(settings.IsaAdvertisementSettingsOpen) {

			EditorGUI.indentLevel++;
			settings.AdIOSSettings = EditorGUILayout.Foldout(settings.AdIOSSettings, "IOS");
			if(settings.AdIOSSettings) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(Ad_Endgine);
				settings.IOSAdEdngine = (UM_IOSAdEngineOprions) EditorGUILayout.EnumPopup(settings.IOSAdEdngine);


				EditorGUILayout.EndHorizontal();


				if(settings.IOSAdEdngine == UM_IOSAdEngineOprions.GoogleMobileAd) {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(IOS_UnitAdId);
					GoogleMobileAdSettings.Instance.IOS_BannersUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.IOS_BannersUnitId);
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(IOS_InterstAdId);
					GoogleMobileAdSettings.Instance.IOS_InterstisialsUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.IOS_InterstisialsUnitId);
					EditorGUILayout.EndHorizontal();
				}
			}
			
			settings.AdAndroidSettings = EditorGUILayout.Foldout(settings.AdAndroidSettings, "Android");
			if(settings.AdAndroidSettings) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(Android_UnitAdId);
				GoogleMobileAdSettings.Instance.Android_BannersUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.Android_BannersUnitId);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(Android_InterstAdId);
				GoogleMobileAdSettings.Instance.Android_InterstisialsUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.Android_InterstisialsUnitId);
				EditorGUILayout.EndHorizontal();
			}
			

			settings.AdWp8Settings = EditorGUILayout.Foldout(settings.AdWp8Settings, "WP8");
			if(settings.AdWp8Settings) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(Ad_Endgine);
				settings.WP8AdEdngine = (UM_WP8AdEngineOprions) EditorGUILayout.EnumPopup(settings.WP8AdEdngine);
				
				
				EditorGUILayout.EndHorizontal();
				
				
				if(settings.WP8AdEdngine == UM_WP8AdEngineOprions.GoogleMobileAd) {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(WP8_UnitAdId);
					GoogleMobileAdSettings.Instance.WP8_BannersUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.WP8_BannersUnitId);
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(WP8_InterstAdId);
					GoogleMobileAdSettings.Instance.WP8_InterstisialsUnitId	 	= EditorGUILayout.TextField(GoogleMobileAdSettings.Instance.WP8_InterstisialsUnitId);
					EditorGUILayout.EndHorizontal();
				}
			}


			EditorGUI.indentLevel--;
		}
	}



	private void InAppSettings() {

		EditorGUI.indentLevel = 0;
		settings.IsInAppSettingsOpen = EditorGUILayout.Foldout(settings.IsInAppSettingsOpen, "In-App Purchases");
		if(settings.IsInAppSettingsOpen) {


			EditorGUI.indentLevel = 1;
			settings.IsInAppSettingsProductsOpen = EditorGUILayout.Foldout(settings.IsInAppSettingsProductsOpen, "Products");
			if(settings.IsInAppSettingsProductsOpen) {

				if(settings.InAppProducts.Count == 0) {
					EditorGUILayout.HelpBox("No In-App Products Added", MessageType.Warning);
				}
				

				foreach(UM_InAppProduct p in settings.InAppProducts) {
					EditorGUILayout.BeginVertical (GUI.skin.box);
			
				
					p.IsOpen = EditorGUILayout.Foldout(p.IsOpen, p.id);
					if(p.IsOpen) {
						EditorGUI.indentLevel++;


						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(InAppID);
						p.id	 	= EditorGUILayout.TextField(p.id);
						EditorGUILayout.EndHorizontal();



						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(IsCons);
						p.IsConsumable	 	= EditorGUILayout.Toggle(p.IsConsumable);
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.Space();


						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(IOSSKU);
						p.IOSId	 	= EditorGUILayout.TextField(p.IOSId);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(AndroidSKU);
						p.AndroidId	 	= EditorGUILayout.TextField(p.AndroidId);
						EditorGUILayout.EndHorizontal();
					
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(WP8SKU);
						p.WP8Id	 	= EditorGUILayout.TextField(p.WP8Id);
						EditorGUILayout.EndHorizontal();



						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.Space();
						if(GUILayout.Button("Remove",  GUILayout.Width(80))) {
							settings.RemoveProduct(p);
							break;
						}
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.Space();
						EditorGUI.indentLevel--;
					}





					EditorGUILayout.EndVertical();
				}
				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if(GUILayout.Button("Add",  GUILayout.Width(80))) {
					settings.AddProduct(new UM_InAppProduct());
				}
				EditorGUILayout.EndHorizontal();
				
				
				EditorGUILayout.Space();
				
			
			}


			settings.IsInAppSettingsPlatfromsOpen = EditorGUILayout.Foldout(settings.IsInAppSettingsPlatfromsOpen, "Platfroms Settings");
			if(settings.IsInAppSettingsPlatfromsOpen) {

				EditorGUILayout.LabelField("Android:");
				EditorGUI.indentLevel = 2;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(Base64KeyLabel);
				AndroidNativeSettings.Instance.base64EncodedPublicKey	 	= EditorGUILayout.TextField(AndroidNativeSettings.Instance.base64EncodedPublicKey);
				EditorGUILayout.EndHorizontal();

				EditorGUI.indentLevel = 1;
			}
		}




		EditorGUI.indentLevel = 0;
	}





	private void AboutGUI() {

		EditorGUILayout.HelpBox("About Ultimate Mobile", MessageType.None);
		EditorGUILayout.Space();
		
		SelectableLabelField(SdkVersion, UltimateMobileSettings.VERSION_NUMBER);
		SelectableLabelField(SupportEmail, "stans.assets@gmail.com");

	}
	
	private void SelectableLabelField(GUIContent label, string value) {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
		EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
		EditorGUILayout.EndHorizontal();
	}



	private static void DirtyEditor() {
		#if UNITY_EDITOR
		EditorUtility.SetDirty(UltimateMobileSettings.Instance);
		#endif
	}
	
	
}
