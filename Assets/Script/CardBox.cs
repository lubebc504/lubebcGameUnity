using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBox : MonoBehaviour
{
    private bool opened = false;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (opened) return;

        if (collision.CompareTag("Player"))
        {
            opened = true;

            LevelUpManager manager = FindObjectOfType<LevelUpManager>();
            if (manager != null)
            {
                manager.ShowChoices();
            }
            Destroy(gameObject);
        }
    }
}