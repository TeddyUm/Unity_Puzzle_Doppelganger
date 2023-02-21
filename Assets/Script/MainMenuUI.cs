using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private void Start()
    {
        SoundManager.instance.Play("OPBGM");
    }
    public void StartGame()
    {
        SoundManager.instance.Stop("OPBGM");
        SoundManager.instance.Play("Button");
        GameManager.Instance.ChangeScene("LevelSelect");
    }

    public void QuitGame()
    {
        SoundManager.instance.Play("Button");
        Application.Quit();
    }
}
