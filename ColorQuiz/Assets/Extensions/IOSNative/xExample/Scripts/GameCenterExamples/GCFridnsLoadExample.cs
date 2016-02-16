using UnityEngine;
using System.Collections;

public class GCFridnsLoadExample : MonoBehaviour {

	private string ChalangeLeadeboard =  "your.leaderbord2.id.here";
	private string ChalangeAchivment =   "your.achievement.id.here ";
	
	
	public GUIStyle headerStyle;
	public GUIStyle boardStyle;
	


	private bool renderFriendsList = false;
	
	void Awake() {

		GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_PLAYER_AUTHENTICATED, OnAuth);
		GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_PLAYER_AUTHENTIFICATION_FAILED, OnAuthFailed);
		
		GameCenterManager.dispatcher.addEventListener(GameCenterManager.GAME_CENTER_FRIEND_LIST_LOADED, OnFriendListLoaded);
		
		//Initializing Game Cneter class. This action will triger authentication flow
		GameCenterManager.init();
	}




	void OnGUI() {
		
		GUI.Label(new Rect(10, 20, 400, 40), "Friend List Load Example", headerStyle);
		
		if(GUI.Button(new Rect(300, 10, 150, 50), "Load Friends")) {
			GameCenterManager.RetrieveFriends();
		}


		if(!renderFriendsList) {
			return;
		}

		if(GUI.Button(new Rect(500, 10, 180, 50), "Leaberboard Chalange All")) {
			GameCenterManager.issueLeaderboardChallenge(ChalangeLeadeboard, "Your message here", GameCenterManager.friendsList.ToArray());
		}
		
		
		if(GUI.Button(new Rect(730, 10, 180, 50), "Achievement Chalange All")) {
			GameCenterManager.issueAchievementChallenge(ChalangeAchivment, "Your message here", GameCenterManager.friendsList.ToArray());
		}
		

		GUI.Label(new Rect(10,  90, 100, 40), "id", boardStyle);
		GUI.Label(new Rect(150, 90, 100, 40), "name", boardStyle);;
		GUI.Label(new Rect(300, 90, 100, 40), "avatar ", boardStyle);

		int i = 1;
		foreach(string FriendId in GameCenterManager.friendsList) {

			GameCenterPlayerTemplate player = GameCenterManager.GetPlayerById(FriendId);
			if(player != null) {
				GUI.Label(new Rect(10,  90 + 70 * i, 100, 40), player.playerId, boardStyle);
				GUI.Label(new Rect(150, 90 + 70 * i , 100, 40), player.alias, boardStyle);
				if(player.avatar != null) {
					GUI.DrawTexture(new Rect(300, 75 + 70 * i, 50, 50), player.avatar);
				} else  {
					GUI.Label(new Rect(300, 90 + 70 * i, 100, 40), "no photo ", boardStyle);
				}

				if(GUI.Button(new Rect(450, 90 + 70 * i, 150, 30), "Chalange Leaberboard")) {
					GameCenterManager.issueLeaderboardChallenge(ChalangeLeadeboard, "Your message here", FriendId);
				}

				if(GUI.Button(new Rect(650, 90 + 70 * i, 150, 30), "Chalange Achievement")) {
					GameCenterManager.issueAchievementChallenge(ChalangeAchivment, "Your message here", FriendId);
				}


				i++;
			}

		}


	}

	private void OnFriendListLoaded() {
		renderFriendsList = true;
	}
	
	private void OnAuth() {
		Debug.Log("Player Authed");


	}
	
	private void OnAuthFailed() {
		IOSNativePopUpManager.showMessage("Game Cneter ", "Player auntification failed");
		
		//if you got this event it means that player canseled auntification flow. With probably mean that playr do not whant to use gamcenter in your game
	}

}
