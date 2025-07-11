using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperBoss : EnemyUnit
{
    public GameObject bulletPrefab;

    public int currentPatternIdx;

    public bool attackFinished = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(AttackPtnLoop());
    }

    public IEnumerator AttackPtnLoop()
    {
        while (isLive)
        {
            currentPatternIdx = Random.Range(0, 3);
            attackFinished = false;
            Debug.Log("다음 공격 시작");
            enemyAnimator.SetTrigger("Attack");
            yield return new WaitUntil(() => attackFinished);
            yield return new WaitForSeconds(5f);
        }
    }

    public void BulletPatternAni()
    {
        if (!isLive) return;
        switch (currentPatternIdx)
        {
            case 0:
                BulletCircle(12);

                break;

            case 1:
                BulletSpread(4, 30f);

                break;

            case 2:
                BulletSpread(5, 60f);

                break;
        }
    }

    public void OnAttackEnd()
    {
        attackFinished = true;
    }

    protected override void Die()
    {
        isLive = false;
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        Collider2D enemyCollider = GetComponent<Collider2D>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        DropManager.instance.DropExp(transform.position);
        DropManager.instance.DropExp(transform.position);
        DropManager.instance.DropExp(transform.position);
        DropManager.instance.DropCard(transform.position, 1f);

        GameManager.Instance.kill++;

        enemyAnimator.SetBool("Dead", true);
        Destroy(this.gameObject, 1f);
    }

    public void BulletCircle(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = (360f / count) * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
            SpawnBullet(dir);
        }
    }

    public void BulletSpread(int count, float spreadangle)
    {
        float startangle = -spreadangle / 2;
        float step = spreadangle / (count - 1);

        for (int i = 0; i < count; i++)
        {
            float angle = startangle + (i * step);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * (target.position - (Vector2)transform.position).normalized;
            SpawnBullet(dir);
        }
    }

    public void SpawnBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        BossBullet bulletScript = bullet.GetComponent<BossBullet>();
        bulletScript.SetDirection(direction, bulletScript.damageModel);
    }

    public override void TakeDamage(DamageModel damageModel)
    {
        if (isLive)
        {
            health -= damageModel.baseDamage;
            StartCoroutine(ChanColoronDamage());
            Debug.Log("[피격] 받은 데미지: " + damageModel.baseDamage);
            enemyAnimator.SetTrigger("Hit");
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    public IEnumerator ChanColoronDamage()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in sprites)
        {
            // SpriteRenderer가 파괴되었는지 확인
            if (sprite != null)
            {
                sprite.color = new Color(1f, 0f, 0f, 1f);
            }
        }

        yield return new WaitForSeconds(0.5f);

        foreach (SpriteRenderer sprite in sprites)
        {
            // SpriteRenderer가 파괴되었는지 확인
            if (sprite != null)
            {
                sprite.color = Color.white;
            }
        }
    }
}