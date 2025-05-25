using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    public GameObject levelUpPanel;
    public Transform cardSlotContainer;
    public GameObject cardSlotPrefab;

    public int choiceNumber = 3;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnEnable()
    {
        GameManager.OnLevelUp += ShowChoices;
    }

    private void OnDisable()
    {
        GameManager.OnLevelUp -= ShowChoices;
    }

    public void ShowChoices()
    {
        GameManager.Instance.isBlockingInput = true;
        levelUpPanel.SetActive(true);
        Time.timeScale = 0f;
        foreach (Transform child in cardSlotContainer)
        {
            Destroy(child.gameObject);
        }

        List<CardData> rewardcards = GetRandomCard(choiceNumber);

        foreach (CardData card in rewardcards)
        {
            GameObject slot = Instantiate(cardSlotPrefab, cardSlotContainer);
            CardUI ui = slot.GetComponent<CardUI>();
            ui.SetCard(card);

            Button btn = slot.AddComponent<Button>();
            btn.transition = Selectable.Transition.None; // 버튼처럼 보이지 않게
            btn.onClick.AddListener(() => SelectCard(card));
        }
    }

    public List<CardData> GetRandomCard(int number)
    {
        List<CardData> allCards = new List<CardData>(FindObjectOfType<CardLoader>().loadedCards);
        List<CardData> result = new List<CardData>();

        while (result.Count < number && allCards.Count > 0)
        {
            int index = Random.Range(0, allCards.Count);
            CardData pick = allCards[index];
            result.Add(pick);
            allCards.RemoveAt(index); // 복사본에서만 제거
        }

        return result;
    }

    public void SelectCard(CardData selected)
    {
        CardHandManager deck = FindObjectOfType<CardHandManager>();
        deck.deck.Add(selected);

        Debug.Log($"{selected.cardName} 카드 획득!");

        ClosePanel();
    }

    public void Skip()
    {
        Debug.Log("보상을 선택하지 않고 스킵함.");
        ClosePanel();
    }

    public void ClosePanel()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.isBlockingInput = false;
    }
}