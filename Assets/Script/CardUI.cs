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
}