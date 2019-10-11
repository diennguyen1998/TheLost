using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioManager audio;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);// Load First Scene
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ButtonHover()
    {
        audio.Play("ButtonHover");
    }

    public void ButtonClick()
    {
        audio.Play("ButtonHit");
    }

}
