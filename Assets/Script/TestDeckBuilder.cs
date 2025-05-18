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
            Debug.LogError("CardLoader가 없습니다!");
            return;
        }

        BuildTestDeck();
    }

    private void BuildTestDeck()
    {
        //  원하는 카드 골라서 myTestDeck에 추가
        myTestDeck.Clear();
        myTestDeck.Add(cardLoader.loadedCards.Find(card => card.cardId == 3)); // Penetrate 탄환
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

        Debug.Log($"테스트 덱 완성! 카드 수: {myTestDeck.Count}");
    }
}