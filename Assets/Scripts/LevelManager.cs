using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    
    public void StartLevel()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
