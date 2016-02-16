using UnityEngine;
using System.Collections;

public class TAPPNotDestroy : MonoBehaviour {

    void Awake()
    {
        if (GameObject.FindObjectsOfType<TAPPNotDestroy>().Length > 2)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }


	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
