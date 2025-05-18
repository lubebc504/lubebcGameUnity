using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandManager : MonoBehaviour
{
    [Header("Card System")]
    public Transform handArea;

    public GameObject cardUIPrefab;

    public List<CardData> deck = new List<CardData>();
    public List<CardData> discardPile = new List<CardData>();
    public List<GameObject> handCards = new List<GameObject>();

    private Revolver revolver;
    private Coroutine effectRoutine;

    private void Start()
    {
        TestDeckBuilder builder = FindObjectOfType<TestDeckBuilder>();
        if (builder != null && builder.myTestDeck != null)
        {
            deck = new List<CardData>(builder.myTestDeck);
            ShuffleDeck();
            DrawHand();
        }
        else
        {
            Debug.LogError("TestDeckBuilder나 덱이 없습니다!");
        }

        revolver = FindObjectOfType<Revolver>();
        if (revolver == null)
            Debug.LogError("Revolver를 찾을 수 없습니다!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseCard(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseCard(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseCard(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseCard(3);
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            CardData temp = deck[i];
            int randIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randIndex];
            deck[randIndex] = temp;
        }
    }

    private void DrawHand()
    {
        ClearHand();
        StartCoroutine(DrawMultipleCards(4)); // 4장 목표
    }

    private IEnumerator DrawMultipleCards(int drawCount)
    {
        int remaining = drawCount;

        while (remaining > 0)
        {
            if (deck.Count == 0)
            {
                if (discardPile.Count > 0)
                {
                    deck.AddRange(discardPile);
                    discardPile.Clear();
                    ShuffleDeck();
                }
                else
                {
                    Debug.Log("덱과 버린 더미 모두 비었습니다!");
                    break;
                }
            }

            DrawCard();
            remaining--;

            yield return null; // 1프레임 기다리면서 자연스럽게 생성
        }
    }

    private void DrawCard()
    {
        if (deck.Count == 0)
            return;

        CardData cardData = deck[0];
        deck.RemoveAt(0);

        GameObject cardObj = Instantiate(cardUIPrefab, handArea);
        CardUI cardUI = cardObj.GetComponent<CardUI>();
        if (cardUI != null)
            cardUI.SetCard(cardData);

        handCards.Add(cardObj);
    }

    private void ClearHand()
    {
        foreach (var cardObj in handCards)
        {
            Destroy(cardObj);
        }
        handCards.Clear();
    }

    public void UseCard(int index)
    {
        if (index < 0 || index >= handCards.Count) return;

        CardUI selectedCardUI = handCards[index].GetComponent<CardUI>();
        if (selectedCardUI == null) return;

        // 적용
        ApplyCard(selectedCardUI.cardData);
        Debug.Log(selectedCardUI.cardData.cardName + "지속시간 :" + selectedCardUI.cardData.duration);
        // 사용한 카드 + 나머지 카드 다 discardPile로
        foreach (var obj in handCards)
        {
            CardUI ui = obj.GetComponent<CardUI>();
            if (ui != null)
                discardPile.Add(ui.cardData);

            Destroy(obj);
        }
        handCards.Clear();
    }

    private void ApplyCard(CardData selectedCard)
    {
        if (revolver == null) return;

        // 기존 효과 제거
        if (revolver.activeCardEffect != null)
            Destroy(revolver.activeCardEffect);

        // 새 효과 적용
        string className = $"CardEffect_{selectedCard.script}";
        System.Type type = System.Type.GetType(className);

        if (type != null && typeof(CardEffect).IsAssignableFrom(type))
        {
            CardEffect newEffect = revolver.gameObject.AddComponent(type) as CardEffect;
            revolver.activeCardEffect = newEffect;
        }
        else
        {
            Debug.LogWarning($"CardEffect 클래스 '{className}'을 찾을 수 없습니다.");
        }

        // 지속시간 코루틴 시작
        if (effectRoutine != null)
            StopCoroutine(effectRoutine);

        effectRoutine = StartCoroutine(EffectDurationTimer(selectedCard.duration));
    }

    private IEnumerator EffectDurationTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (revolver.activeCardEffect != null)
        {
            Destroy(revolver.activeCardEffect);
            revolver.activeCardEffect = null;
        }

        DrawHand();
    }
}