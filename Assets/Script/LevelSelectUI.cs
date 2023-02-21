using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    public void EasyButton()
    {
        SoundManager.instance.Play("Button");
        GameManager.Instance.difficulty = 0;
        GameManager.Instance.ChangeScene("MainGame");
    }
    public void NormalButton()
    {
        SoundManager.instance.Play("Button");
        GameManager.Instance.difficulty = 1;
        GameManager.Instance.ChangeScene("MainGame");
    }

    public void HardButton()
    {
        SoundManager.instance.Play("Button");
        GameManager.Instance.difficulty = 2;
        GameManager.Instance.ChangeScene("MainGame");
    }
}
