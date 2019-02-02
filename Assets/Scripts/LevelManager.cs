using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public void StartLevel ()
    {
        SceneManager.LoadScene("GamePlayScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
