using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicManager : MonoBehaviour
{
    [Header("Relic System")]
    public Transform relicArea;

    public GameObject relicdataPrefab;

    // Start is called before the first frame update
    private void Start()
    {
        UpdateRelicUI();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void UpdateRelicUI()
    {
        foreach (Transform child in relicArea)
        {
            Destroy(child.gameObject);
        }
        //Test
        List<RelicEffect> relics = PlayerController.instance.relics;

        foreach (RelicEffect relic in relics)
        {
            GameObject slot = Instantiate(relicdataPrefab, relicArea);

            Text stackText = slot.GetComponentInChildren<Text>();
            stackText.text = relic.stack.ToString();

            Image relicImage = slot.GetComponent<Image>();
            Sprite icon = Resources.Load<Sprite>($"Image/{relic.data.iconName}");
            if (icon != null)
                relicImage.sprite = icon;
        }
    }
}