using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCardUI : MonoBehaviour
{
    public Transform activeCardContainer;
    public GameObject cardSlotPrefab;

    public void RefreshActiveCards(List<CardData> activeCards)
    {
        foreach (Transform child in activeCardContainer)
            Destroy(child.gameObject);

        foreach (CardData card in activeCards)
        {
            GameObject slot = Instantiate(cardSlotPrefab, activeCardContainer);
            CardUI ui = slot.GetComponent<CardUI>();
            ui.SetCard(card);
        }
    }
}