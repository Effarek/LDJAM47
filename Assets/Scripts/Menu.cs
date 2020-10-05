using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Start()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("lvl", 1);
    }
    //Menu Manager
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //Menu Pause
    public void ResumeGame(GameObject PauseMenu)
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
