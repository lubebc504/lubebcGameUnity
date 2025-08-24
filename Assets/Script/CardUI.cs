using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;

    public CardData cardData;

    public void SetCard(CardData data)
    {
        cardData = data;
        nameText.text = data.cardName;
        descriptionText.text = data.description;
    }

    public void SetHighlight(bool on)
    {
        GetComponent<CanvasGroup>().alpha = on ? 1f : 0.5f;
    }

    public void SetDark(bool on)
    {
        GetComponent<CanvasGroup>().alpha = on ? 0.3f : 1f;
    }
}