using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuakeField : MonoBehaviour
{
    private float duration;
    private float damagePerSecond;
    private float tickInterval;
    private float radius;

    public void Init(float duration, float damagePerSecond, float tickInterval, float radius)
    {
        this.duration = duration;
        this.damagePerSecond = damagePerSecond;
        this.tickInterval = tickInterval;
        this.radius = radius;
        this.transform.localScale = new Vector3(radius * 2f, radius * 2f, 1f);
        StartCoroutine(DoDamage());
    }

    private IEnumerator DoDamage()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (var hit in hits)
            {
                EnemyUnit enemy = hit.GetComponent<EnemyUnit>();
                if (enemy != null)
                {
                    enemy.TakeDamage(new DamageModel { baseDamage = damagePerSecond });
                    StartCoroutine(SlowSpeed(enemy, 0.9f, 1f));
                }
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        Destroy(gameObject);
    }

    private IEnumerator SlowSpeed(EnemyUnit enemy, float slowMultipiler, float slowDuration)
    {
        if (enemy == null) yield break;

        float origin = enemy.speed;
        enemy.speed *= 0.9f;

        yield return new WaitForSeconds(slowDuration);

        if (enemy != null)
        {
            enemy.speed = origin;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}