using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public enum InfoType
    { Exp, Level, Kill, Time }

    public InfoType type;

    private Text myText;
    private Slider mySlider;

    private void Awake()
    {
        if (GetComponent<Text>() != null)
        {
            myText = GetComponent<Text>();
        }
        if (GetComponent<Slider>() != null)
        {
            mySlider = GetComponent<Slider>();
        }
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                if (mySlider == null) return;
                float curExp = GameManager.Instance.exp;
                float maxExp = GameManager.Instance.nextExp[GameManager.Instance.level];
                mySlider.value = curExp / maxExp;
                break;

            case InfoType.Level:
                if (myText == null) return;
                myText.text = $"Lv: {GameManager.Instance.level + 1}";
                break;

            case InfoType.Kill:
                if (myText == null) return;
                myText.text = $"{GameManager.Instance.kill}";
                break;

            case InfoType.Time:
                if (myText == null) return;
                float remainTime = GameManager.Instance.maxGameTime - GameManager.Instance.gameTime;
                int minute = Mathf.FloorToInt(remainTime / 60);
                int second = Mathf.FloorToInt(remainTime % 60);
                myText.text = $"{minute:00}:{second:00}";
                break;
        }
    }
}