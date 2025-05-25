using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;

[Serializable]
public class RelicData
{
    [XmlAttribute("id")]
    public int relicId;

    public string name;
    public string description;
    public string script;      // 실제 효과 구현 클래스 이름
    public string iconName;    // UI에 쓸 아이콘 (이미지는 나중에 추가해도 됨)
}