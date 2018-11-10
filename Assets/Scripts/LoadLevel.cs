using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartLevel ()
    {
        Application.LoadLevel("GamePlayScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
