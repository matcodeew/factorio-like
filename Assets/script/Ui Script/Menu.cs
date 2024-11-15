using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    ShipController _shipController;
    public static bool GameIsPaused { get; private set; }
    public static bool Finish { get; set; }
    public void LaunchGame()
    {
        SceneManager.LoadScene("MVP");
    }

    public void LoadIntro()
    {
        SceneManager.LoadScene("Shipintro");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void LoadMainMenu()
    {
        GameIsPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnPause()
    {
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void OnResume()
    {
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    public void OnSettings()
    {
        Time.timeScale = 1.0f;
        GameIsPaused = true;
    }

    public void FinishTrue()
    {
        Finish = true;
    }
}
