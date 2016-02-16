using UnityEngine;
using System.Collections;

public class UM_GameServiceManager : SA_Singleton<UM_GameServiceManager> {
	

	public const string PLAYER_CONNECTED       			= "player_connected";
	public const string PLAYER_DISCONNECTED   			= "player_disconnected";


	private bool _IsInitedCalled = false;
	private bool _IsDataLoaded = false;

	private int dataEventsCount = 0;
	private int currentEventsCount = 0;


	private GameServicePlayerTemplate _player = null ;
	private UM_ConnectionState _ConnectionSate = UM_ConnectionState.UNDEFINED;



	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	private void Init() {

		_IsInitedCalled = true;

		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			dataEventsCount = UltimateMobileSettings.Instance.Leaderboards.Count + 1;


			foreach(UM_Achievement achievment in UltimateMobileSettings.Instance.Achievements) {
				GameCenterManager.registerAchievement(achievment.IOSId);
			}
			break;
		case RuntimePlatform.Android:
			dataEventsCount = 2;
			break;
		}





		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED,    OnAndroidPlayerConnected);
		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnAndroidPlayerDisconnected);
		GooglePlayManager.instance.addEventListener (GooglePlayManager.ACHIEVEMENTS_LOADED, OnServiceDataLoaded);
		GooglePlayManager.instance.addEventListener (GooglePlayManager.LEADERBOARDS_LOEADED, OnServiceDataLoaded);



		GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_PLAYER_AUTHENTICATED, 			 OnIOSAuth);
		GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_PLAYER_AUTHENTIFICATION_FAILED, OnIOSAuthFailed);

		GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_ACHIEVEMENTS_LOADED, OnServiceDataLoaded);
		GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_LEADER_BOARD_SCORE_LOADED, OnServiceDataLoaded);

	}


	//--------------------------------------
	// Metods
	//--------------------------------------

	public void Connect() {
		if(!_IsInitedCalled) {
			Init();
		}

		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.init();
			break;
		case RuntimePlatform.Android:
			GooglePlayConnection.instance.connect();
			break;
		}

		_ConnectionSate = UM_ConnectionState.CONNECTING;
	}

	public void Diconnect() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:

			break;
		case RuntimePlatform.Android:
			GooglePlayConnection.instance.disconnect();
			break;
		}

	}


	public void ShowAchivmentsUI() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.showAchievements();
			break;
		case RuntimePlatform.Android:
			GooglePlayManager.instance.showAchievementsUI();
			break;
		}
	}
	
	public void ShowLeaderBoardsUI() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.showLeaderBoards();
			break;
		case RuntimePlatform.Android:
			GooglePlayManager.instance.showLeaderBoardsUI();
			break;
		}
	}

	public void ShowLeaderBoardUI(string id) {
		ShowLeaderBoardUI(UltimateMobileSettings.Instance.GetLeaderboardById(id));
	}

	public void ShowLeaderBoardUI(UM_Leaderboard leaderboard) {
		if(leaderboard == null) {
			return;
		}
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.showLeaderBoard(leaderboard.IOSId);
			break;
		case RuntimePlatform.Android:
			GooglePlayManager.instance.showLeaderBoardById(leaderboard.AndroidId);
			break;
		}
	}


	public void SubmitScore(string LeaderboardId, int score) {
		SubmitScore(UltimateMobileSettings.Instance.GetLeaderboardById(LeaderboardId), score);
	}

	public void SubmitScore(UM_Leaderboard leaderboard, int score) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.reportScore(score, leaderboard.IOSId);
			break;
		case RuntimePlatform.Android:
			GooglePlayManager.instance.submitScoreById(leaderboard.AndroidId, score);
			break;
		}
	}


	public void RevealAchievement(string id) {
		RevealAchievement(UltimateMobileSettings.Instance.GetAchievmentById(id));
	}

	public void RevealAchievement(UM_Achievement achievement) {
		switch(Application.platform) {
		
		case RuntimePlatform.Android:
			GooglePlayManager.instance.revealAchievementById(achievement.AndroidId);
			break;
		}
	}


	public void ReportAchievement(string id) {
		RevealAchievement(UltimateMobileSettings.Instance.GetAchievmentById(id));
	}
	
	public void ReportAchievement(UM_Achievement achievement) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.submitAchievement(100f, achievement.IOSId);
			break;
		case RuntimePlatform.Android:
			GooglePlayManager.instance.reportAchievementById(achievement.AndroidId);
			break;
		}
	}


	public void IncrementAchievement(string id,  float percentages) {
		IncrementAchievement(UltimateMobileSettings.Instance.GetAchievmentById(id), percentages);
	}


	public void IncrementAchievement(UM_Achievement achievement, float percentages) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.submitAchievement(percentages, achievement.IOSId);
			break;
		case RuntimePlatform.Android:

			GPAchievement a = GooglePlayManager.instance.GetAchievement(achievement.AndroidId);
			if(a != null) {
				if(a.type == GPAchievementType.TYPE_INCREMENTAL) {
					int steps = Mathf.CeilToInt((a.totalSteps / 100f) * percentages);
					GooglePlayManager.instance.incrementAchievementById(achievement.AndroidId, steps);
				} else {
					GooglePlayManager.instance.reportAchievementById(achievement.AndroidId);
				}
			}
			break;
		}
	}


	public void ResetAchievements() {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GameCenterManager.resetAchievements();
			break;
		case RuntimePlatform.Android:
		
			break;
		}
	}


	public float GetAchievementProgress(string id) {
		return GetAchievementProgress(UltimateMobileSettings.Instance.GetAchievmentById(id));
	}
	
	public float GetAchievementProgress(UM_Achievement achievement) {
		if(achievement == null) {
			return 0f;
		}
		
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			return GameCenterManager.getAchievementProgress(achievement.IOSId);
		case RuntimePlatform.Android:
			
			GPAchievement a = GooglePlayManager.instance.GetAchievement(achievement.AndroidId);
			if(a != null) {
				if(a.type == GPAchievementType.TYPE_INCREMENTAL) {
					return  (a.currentSteps / a.totalSteps) * 100f;
				} else {
					if(a.state == GPAchievementState.STATE_UNLOCKED) {
						return 100f;
					} else {
						return 0f;
					}
				}
			}
			break;
		}
		
		return 0f;
	}
	


	public int GetCurrentPlayerScore(string leaderBoardId) {
		return GetCurrentPlayerScore(UltimateMobileSettings.Instance.GetLeaderboardById(leaderBoardId));
	} 

	public int GetCurrentPlayerScore(UM_Leaderboard leaderboard) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GCLeaderBoard board =  GameCenterManager.GetLeaderBoard(leaderboard.IOSId);
			if(board != null) {
				return board.GetCurrentPlayerScore(GCBoardTimeSpan.ALL_TIME, GCCollectionType.GLOBAL).GetIntScore();
			} else {
				return 0;
			}
		case RuntimePlatform.Android:
			GPLeaderBoard gBoard =  GooglePlayManager.instance.GetLeaderBoard(leaderboard.AndroidId);
			if(gBoard != null) {
				return gBoard.GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME, GPCollectionType.GLOBAL).score;
			} else {
				return 0;
			}
		}

		return 0;
	} 



	public int GetCurrentPlayerRank(string leaderBoardId) {
		return GetCurrentPlayerRank(UltimateMobileSettings.Instance.GetLeaderboardById(leaderBoardId));
	} 
	
	public int GetCurrentPlayerRank(UM_Leaderboard leaderboard) {
		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			GCLeaderBoard board =  GameCenterManager.GetLeaderBoard(leaderboard.IOSId);
			if(board != null) {
				return board.GetCurrentPlayerScore(GCBoardTimeSpan.ALL_TIME, GCCollectionType.GLOBAL).rank;
			} else {
				return 0;
			}
		case RuntimePlatform.Android:
			GPLeaderBoard gBoard =  GooglePlayManager.instance.GetLeaderBoard(leaderboard.AndroidId);
			if(gBoard != null) {
				return gBoard.GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME, GPCollectionType.GLOBAL).rank;
			} else {
				return 0;
			}
		}

		return 0;
	} 


	
	//--------------------------------------
	// Get / Set
	//--------------------------------------


	public UM_ConnectionState ConnectionSate {
		get {
			return _ConnectionSate;
		}
	}

	public GameServicePlayerTemplate player {
		get {
			return _player;
		}
	}
	

	//--------------------------------------
	// Events
	//--------------------------------------

	private void OnServiceConnected() {

		if(_IsDataLoaded) {
			OnAllLoaded();
			return;
		}


		switch(Application.platform) {
		case RuntimePlatform.IPhonePlayer:
			foreach(UM_Leaderboard leaderboard in UltimateMobileSettings.Instance.Leaderboards) {
				GameCenterManager.loadCurrentPlayerScore(leaderboard.IOSId, GCBoardTimeSpan.ALL_TIME, GCCollectionType.GLOBAL);
			}
			break;
		case RuntimePlatform.Android:
			GooglePlayManager.instance.loadAchievements();
			GooglePlayManager.instance.loadLeaderBoards();
			break;
		}
	}


	private void OnServiceDataLoaded(CEvent e) {

		Debug.Log("OnServiceDataLoaded " + e.name);
		currentEventsCount++;
		if(currentEventsCount == dataEventsCount) {
			OnAllLoaded();
		}
		
	}

	private void OnAllLoaded() {
		_ConnectionSate = UM_ConnectionState.CONNECTED;
		_player =  new GameServicePlayerTemplate(GameCenterManager.player, GooglePlayManager.instance.player);


		dispatch(PLAYER_CONNECTED);
	}


	//--------------------------------------
	// IOS Events
	//--------------------------------------

	private void OnIOSAuth() {
		OnServiceConnected();

	}

	private void OnIOSAuthFailed() {
		_ConnectionSate = UM_ConnectionState.DISCONNECTED;
		dispatch(PLAYER_DISCONNECTED);
	}

	//--------------------------------------
	// Android Events
	//--------------------------------------

	private void OnAndroidPlayerConnected() {
		OnServiceConnected();
	}
	
	private void OnAndroidPlayerDisconnected() {
		_ConnectionSate = UM_ConnectionState.DISCONNECTED;
		dispatch(PLAYER_DISCONNECTED);
	}


}
