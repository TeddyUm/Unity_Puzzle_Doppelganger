using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public Text scoreText;

    void Start()
    {
        SoundManager.instance.Play("ResultBGM");
    }

    private void Update()
    {
        scoreText.text = "" + GameManager.Instance.score;
    }

    public void RetryButton()
    {
        SoundManager.instance.Play("Button");
        SoundManager.instance.Stop("ResultBGM");
        GameManager.Instance.time = 60.0f;
        GameManager.Instance.score = 0;
        GameManager.Instance.ChangeScene("LevelSelect");
    }
    public void QuitButton()
    {
        SoundManager.instance.Play("Button");
        Application.Quit();
    }
}
