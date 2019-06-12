using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevelAsync : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    
    void Start()
    {
        StartCoroutine(StartLevelAsync());
    }

    public IEnumerator StartLevelAsync()
    {
        AsyncOperation asyncLoading = SceneManager.LoadSceneAsync("GamePlayScene");

        while (asyncLoading.progress < 1)
        {
            progressBar.fillAmount = asyncLoading.progress;
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }
}
