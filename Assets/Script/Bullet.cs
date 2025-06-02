using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    public Rigidbody2D rb;
    public DamageModel damageModel = new DamageModel();

    public CardEffect cardEffect; // 현재 적용된 카드 효과
    public bool canPenetrate = false;

    private void Awake()
    {
        damageModel.baseDamage = 2f;
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void SetDirection(Vector2 direction, DamageModel damage)
    {
        rb.velocity = direction.normalized * speed;
        damageModel = damage;

        // 카드 효과 발동 (OnFire)
        cardEffect?.OnFire(this);

        // Lifetime 이후 삭제
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damageModel);

            // 카드 효과 발동 (OnHit)
            cardEffect?.OnHit(this, collision.gameObject);

            foreach (var relic in PlayerController.instance.relics)
            {
                if (relic != null && collision.TryGetComponent(out EnemyUnit enemy))
                {
                    relic.OnHit(PlayerController.instance, enemy);
                }
            }

            if (!canPenetrate)
            {
                Destroy(gameObject);
            }
        }
    }
}