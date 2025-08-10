using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;
    public Rigidbody2D rb;
    public DamageModel damageModel = new DamageModel();

    public bool canPenetrate = false;

    private void Awake()
    {
        damageModel.baseDamage = 15f;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void SetDirection(Vector2 direction, DamageModel damage)
    {
        rb.velocity = direction.normalized * speed;
        damageModel = damage;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damageModel);

            if (!canPenetrate)
            {
                Destroy(gameObject);
            }
        }
    }
}