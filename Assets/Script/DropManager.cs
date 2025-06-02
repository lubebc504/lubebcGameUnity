using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    //적이나 레벨업 시 띄우는 화면 출력용
    public GameObject expItemPrefab;

    public GameObject cardBoxPrefab;

    public static DropManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void DropExp(Vector3 position)
    {
        Instantiate(expItemPrefab, position, Quaternion.identity);
    }

    public void DropCard(Vector3 position, float chance)
    {
        if (Random.value < chance)
        {
            Instantiate(cardBoxPrefab, position, Quaternion.identity);
        }
    }
}