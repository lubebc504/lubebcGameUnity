using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class CardLoader : MonoBehaviour
{
    public List<CardData> loadedCards;

    private void Awake()
    {
        LoadCardData();
    }

    public void LoadCardData()
    {
        TextAsset xmlAsset = Resources.Load<TextAsset>("Cards");
        if (xmlAsset == null)
        {
            Debug.LogError("Cards.xml ������ ã�� �� �����ϴ�!");
            return;
        }

        XmlSerializer serializer = new XmlSerializer(typeof(CardDataList));
        StringReader reader = new StringReader(xmlAsset.text);
        CardDataList dataList = (CardDataList)serializer.Deserialize(reader);
        loadedCards = dataList.cards;

        Debug.Log($"ī�� {loadedCards.Count}�� �ε� �Ϸ�");
    }
}