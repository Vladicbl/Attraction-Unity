using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    [SerializeField] private Text ver;

    void Start()
    {
        ver.text = $"Version: {Application.version}";
    }
}
