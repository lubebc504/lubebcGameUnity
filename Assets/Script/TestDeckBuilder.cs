using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDeckBuilder : MonoBehaviour
{
    private CardLoader cardLoader;

    public List<CardData> myTestDeck = new List<CardData>();

    private void Awake()
    {
        cardLoader = FindObjectOfType<CardLoader>();

        if (cardLoader == null || cardLoader.loadedCards == null)
        {
            Debug.LogError("CardLoader�� �����ϴ�!");
            return;
        }

        BuildTestDeck();
    }

    private void BuildTestDeck()
    {
        //  ���ϴ� ī�� ��� myTestDeck�� �߰�
        myTestDeck.Clear();
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 3)); // Penetrate źȯ
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 1)); // DoubleShoot
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 2)); // BleedShoot
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 3)); // Penetrate
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 3)); // Penetrate
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 3)); // Penetrate
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 1)); // DoubleShoot
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 2)); // BleedShoot
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 3)); // Penetrate
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 1)); // DoubleShoot
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 2)); // BleedShoot
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 3)); // Penetrate
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 1)); // DoubleShoot
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 2)); // BleedShoot
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 3)); // Penetrate

        Debug.Log($"�׽�Ʈ �� �ϼ�! ī�� ��: {myTestDeck.Count}");
    }
}