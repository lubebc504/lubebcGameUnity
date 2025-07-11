using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    public GameObject levelUpPanel;
    public GameObject cardSelectPanel;
    public GameObject cardSlotPrefab;
    public GameObject relicSlotPrefab;
    public GameObject cardAddButtonPrefab;

    public Transform cardSlotContainer;

    public Transform selectContainer; // 레벨업 때 유물 등등 보여주기

    public int choiceNumber = 3;

    private void OnEnable()
    {
        GameManager.Instance.OnLevelUp.AddListener(ShowRoot);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelUp.RemoveListener(ShowRoot);
    }

    public void ShowRoot()
    {
        GameManager.Instance.isBlockingInput = true;
        levelUpPanel.SetActive(true);
        Time.timeScale = 0f;

        foreach (Transform child in selectContainer)
        {
            Destroy(child.gameObject);
        }

        RelicData relic = GetRandomRelic();
        GameObject relicSlot = Instantiate(relicSlotPrefab, selectContainer);
        relicSlot.GetComponentInChildren<Text>().text = relic.name;

        Image relicImage = relicSlot.transform.Find("RelicImage").GetComponent<Image>();
        Sprite icon = Resources.Load<Sprite>($"Image/{relic.iconName}");
        if (icon != null)
            relicImage.sprite = icon;

        Button relicBtn = relicSlot.GetComponent<Button>();
        relicBtn.onClick.AddListener(() => SelectRelic(relic));

        GameObject cardbtn = Instantiate(cardAddButtonPrefab, selectContainer);
        Button cardBtn = cardbtn.GetComponent<Button>();
        cardBtn.onClick.AddListener(() => ShowChoices());
    }

    public RelicData GetRandomRelic()
    {
        List<RelicData> allRelics = new List<RelicData>(FindObjectOfType<RelicLoader>().loadedRelicData);
        if (allRelics.Count == 0)
        {
            return null;
        }

        int index = Random.Range(0, allRelics.Count);
        Debug.Log("현재 인덱스는 :" + index);
        return allRelics[index];
    }

    public void SelectRelic(RelicData relic)
    {
        PlayerController.instance.AcquireRelic(relic);
        Debug.Log($"{relic.name} 유물 획득!");

        foreach (Transform child in selectContainer)
        {
            if (child.GetComponentInChildren<Text>().text == relic.name)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    public void ShowChoices()
    {
        ClosePanel();
        if (!cardSelectPanel.activeSelf)
        {
            GameManager.Instance.isBlockingInput = true;
            cardSelectPanel.SetActive(true);
            Time.timeScale = 0f;
        }

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
            btn.transition = Selectable.Transition.None;
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
            allCards.RemoveAt(index);
        }

        return result;
    }

    public void SelectCard(CardData selected)
    {
        CardHandManager deck = FindObjectOfType<CardHandManager>();
        deck.deck.Add(selected);

        Debug.Log($"{selected.cardName} 카드 획득!");

        CloseSelectPanel();
    }

    public void Skip()
    {
        Debug.Log("보상을 선택하지 않고 스킵함.");
        CloseSelectPanel();
    }

    public void ClosePanel()
    {
        if (levelUpPanel.activeSelf)
        {
            levelUpPanel.SetActive(false);
        }
    }

    public void CloseSelectPanel()
    {
        cardSelectPanel.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.isBlockingInput = false;
    }
}