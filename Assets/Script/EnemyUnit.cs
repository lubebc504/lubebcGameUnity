using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float health;
    public HealthBar healthBar;

    public float speed;
    protected Rigidbody2D enemyRigid;
    public Rigidbody2D target;
    public SpriteRenderer sprite;
    public bool isLive;

    public DamageModel model;
    protected Animator enemyAnimator;
    private Coroutine damageCoroutine;

    public bool bleedImmune = false;
    public Coroutine bleedCoroutine;
    public float bleedRemainingTime = 0f;
    public bool isBleeding = false;

    public GameObject expItemPrefab;
    public GameObject cardBoxPrefab;

    public bool isSlowed = false;

    protected virtual void OnEnable()
    {
        enemyRigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        model = new DamageModel();

        enemyAnimator = GetComponent<Animator>();
        isLive = true;
        target = PlayerController.instance.GetComponent<Rigidbody2D>();
    }

    public virtual void Init(float healthMultiplier)
    {
        maxHealth *= healthMultiplier;
        health = maxHealth;

        healthBar.SetHealth(health, maxHealth);
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        healthBar.SetHealth(health, maxHealth);
    }

    protected virtual void FixedUpdate()
    {
        if (!isLive) return;

        Vector2 dir = target.position - enemyRigid.position;
        Vector2 nextVec = dir.normalized * speed * Time.fixedDeltaTime;
        enemyRigid.MovePosition(enemyRigid.position + nextVec);
        enemyRigid.velocity = Vector2.zero;
    }

    protected virtual void LateUpdate()
    {
        sprite.flipX = target.position.x < enemyRigid.position.x;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();

            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(player));
            }
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    protected virtual IEnumerator DealDamageOverTime(PlayerController player)
    {
        while (player.isLive && isLive)
        {
            player.TakeDamage(model);
            yield return new WaitForSeconds(1f);
        }
    }

    public virtual void TakeDamage(DamageModel damageModel)
    {
        health -= damageModel.baseDamage;
        Debug.Log("[피격] 받은 데미지: " + damageModel.baseDamage);
        enemyAnimator.SetTrigger("Hit");

        if (health <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
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
        DropManager.instance.DropCard(transform.position, 0.15f);

        GameManager.Instance.kill++;

        enemyAnimator.SetBool("Dead", true);
        Destroy(this.gameObject, 1f);
    }

    public virtual void ApplyBleed(float damage, float duration, float interval)
    {
        if (bleedImmune || !isLive)
            return;

        bleedRemainingTime += duration;

        if (!isBleeding)
            bleedCoroutine = StartCoroutine(BleedCoroutine(damage, interval));
    }

    protected virtual IEnumerator BleedCoroutine(float damage, float interval)
    {
        isBleeding = true;
        yield return new WaitForSeconds(interval);
        while (bleedRemainingTime > 0f && isLive)
        {
            health -= damage;
            enemyAnimator.SetTrigger("Hit");
            Debug.Log($"출혈 피해: {damage}, 남은 체력: {health}");
            if (health <= 0f)
            {
                TakeDamage(new DamageModel { baseDamage = 0 });
                isBleeding = false;
                yield break;
            }

            yield return new WaitForSeconds(interval);
            bleedRemainingTime -= interval;
        }

        isBleeding = false;
    }
}