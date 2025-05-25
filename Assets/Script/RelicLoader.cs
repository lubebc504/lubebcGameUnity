using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class RelicLoader : MonoBehaviour
{
    public List<RelicData> loadedRelicData;

    private void Awake()
    {
        LoadRelics();
    }

    public void LoadRelics()
    {
        TextAsset xmlAsset = Resources.Load<TextAsset>("Relics");
        if (xmlAsset == null)
        {
            Debug.LogError("Relics.xml을 찾을 수 없습니다!");
            return;
        }

        XmlSerializer serializer = new XmlSerializer(typeof(RelicDataList));
        StringReader reader = new StringReader(xmlAsset.text);
        RelicDataList dataList = (RelicDataList)serializer.Deserialize(reader);
        loadedRelicData = dataList.relics;

        Debug.Log($"유물 {loadedRelicData.Count}개 로딩 완료!");
    }
}