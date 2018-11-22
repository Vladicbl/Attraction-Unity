using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

    public void StartLevel ()
    {
        Application.LoadLevel("GamePlayScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
