using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandManager : MonoBehaviour
{
    [Header("Card System")]
    public Transform handArea;

    public GameObject cardUIPrefab;

    public ActivateCardUI activeCardUI;

    private List<CardData> activeCards = new List<CardData>();

    public List<CardData> deck = new List<CardData>();
    public List<CardData> discardPile = new List<CardData>();
    public List<GameObject> handCards = new List<GameObject>();

    private Revolver revolver;
    private Coroutine effectRoutine;

    private bool isEffectActive = false;

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
        if (isEffectActive) return;

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
        StartCoroutine(DrawMultipleCards(4));
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

            yield return null;
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
        SoundManager.instance.PlaySFX(SoundManager.ESfx.SFX_BUTTON);
        // 적용
        ApplyCard(selectedCardUI.cardData);
        Debug.Log(selectedCardUI.cardData.cardName + "지속시간 :" + selectedCardUI.cardData.duration);

        for (int i = 0; i < handCards.Count; i++)
        {
            CardUI ui = handCards[i].GetComponent<CardUI>();
            if (ui != null)
            {
                if (i == index)
                {
                    ui.SetHighlight(true);
                }
                else
                {
                    ui.SetDark(true);
                }
            }
        }
        isEffectActive = true;
    }

    private void ApplyCard(CardData selectedCard)
    {
        if (revolver == null) return;

        if (revolver.activeCardEffect != null)
            revolver.activeCardEffect = null;

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

        if (effectRoutine != null)
            StopCoroutine(effectRoutine);

        effectRoutine = StartCoroutine(EffectDurationTimer(selectedCard.duration));

        activeCards.Clear();
        activeCards.Add(selectedCard);
        activeCardUI.RefreshActiveCards(activeCards);
    }

    private IEnumerator EffectDurationTimer(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (revolver.activeCardEffect != null)
        {
            revolver.activeCardEffect = null;
        }

        foreach (var obj in handCards)
        {
            CardUI ui = obj.GetComponent<CardUI>();
            if (ui != null)
                discardPile.Add(ui.cardData);

            Destroy(obj);
        }
        handCards.Clear();
        activeCards.Clear();
        activeCardUI.RefreshActiveCards(activeCards);
        DrawHand();
        isEffectActive = false;
    }
}