using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public class RelicData : MonoBehaviour
{
    [XmlAttribute("id")]
    public int relicId;

    public string name;
    public string description;
    public string script;      // ���� ȿ�� ���� Ŭ���� �̸�
    public string iconName;    // UI�� �� ������ (�̹����� ���߿� �߰��ص� ��)
}