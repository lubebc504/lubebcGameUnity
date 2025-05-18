using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : MonoBehaviour
{
    public int expAmount = 1;
    public float moveSpeed = 3f;
    public float attractRange = 3f; // 흡수 거리
    private Transform player;

    private void Start()
    {
        player = PlayerController.instance.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= attractRange)
        {
            // 플레이어 방향으로 이동
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.GetExp(expAmount);
            Destroy(gameObject);
        }
    }
}