using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour
{
    public Image timeGuage;
    public Text scoreText;

    void Start()
    {
        SoundManager.instance.Play("MainGameBGM");
    }
    void Update()
    {
        scoreText.text = "" + GameManager.Instance.score;
        timeGuage.fillAmount = GameManager.Instance.time / 60.0f;
        GameManager.Instance.time -= Time.deltaTime;
        if(GameManager.Instance.time < 0)
        {
            SoundManager.instance.Stop("MainGameBGM");
            GameManager.Instance.ChangeScene("Result");
        }
    }
}
