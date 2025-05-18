using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public CardData cardData; // XML 기반 카드
    public Text nameText;
    public Text descriptionText;
    public Image cardImage;

    private void Start()
    {
        nameText.text = cardData.cardName;
        descriptionText.text = cardData.description;
        cardImage.sprite = LoadCardSprite(cardData.spriteName); // 이미지 로드
    }

    private Sprite LoadCardSprite(string spriteName)
    {
        return Resources.Load<Sprite>("CardImages/" + spriteName);
    }
}