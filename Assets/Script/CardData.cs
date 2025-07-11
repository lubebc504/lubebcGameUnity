using System;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CardData
{
    [XmlAttribute("id")]
    public int cardId;

    public string cardName;
    public string description;
    public string script;

    public float duration = 5f; //  ±‚∫ª 5√ 
}

[Serializable]
[XmlRoot("CardDataList")]
public class CardDataList
{
    [XmlElement("Card")]
    public List<CardData> cards = new List<CardData>();
}