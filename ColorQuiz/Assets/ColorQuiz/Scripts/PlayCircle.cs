using UnityEngine;
using System.Collections;

public class PlayCircle : MonoBehaviour {

	public void Playgame() {
		Application.LoadLevel (StarGame.selected);
	}
}
