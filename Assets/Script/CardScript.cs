using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public CardData cardData; // XML ��� ī��
    public Text nameText;
    public Text descriptionText;
    public Image cardImage;

    private void Start()
    {
        nameText.text = cardData.cardName;
        descriptionText.text = cardData.description;
        cardImage.sprite = LoadCardSprite(cardData.spriteName); // �̹��� �ε�
    }

    private Sprite LoadCardSprite(string spriteName)
    {
        return Resources.Load<Sprite>("CardImages/" + spriteName);
    }
}