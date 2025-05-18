using System;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
[XmlRoot("RelicDataList")]
public class RelicDataList
{
    [XmlElement("Relic")]
    public List<RelicData> relics = new List<RelicData>();
}