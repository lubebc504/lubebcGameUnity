using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float playerhp;
    public HealthBar healthBar;

    public Vector2 inputVec;
    public float speed;
    private Rigidbody2D playerRigid;
    public SpriteRenderer sprite;
    public Animator playerAnim;

    public static PlayerController instance;

    public List<RelicEffect> relics = new List<RelicEffect>();
    public bool isLive;

    private void Awake()
    {
        if (instance == null) instance = this;
        playerRigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        playerhp = 100f;
        maxHealth = playerhp;
        isLive = true;
        healthBar.SetHealth(playerhp, maxHealth);
    }

    private void Update()
    {
        GetAxis();
        if (isLive)
        {
            PlayerMove();
        }
        healthBar.SetHealth(playerhp, maxHealth);

        if (Input.GetKeyDown(KeyCode.T))
        {
            // �ε��� ���� �� id=1�� �� ã��
            var relicLoader = FindObjectOfType<RelicLoader>();
            RelicData bleedRelic = relicLoader.loadedRelicData.Find(r => r.relicId == 2);

            if (bleedRelic != null)
            {
                AcquireRelic(bleedRelic);
                Debug.Log($"�׽�Ʈ ���� '{bleedRelic.name}' ���� �Ϸ�");
            }
            else
            {
                Debug.LogError("���� ���� ���� �����͸� ã�� �� �����ϴ�!");
            }
        }
    }

    private void FixedUpdate()
    {
    }

    private void GetAxis()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    private void PlayerMove()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        playerRigid.MovePosition(playerRigid.position + nextVec);
        playerAnim.SetFloat("IsRunning", inputVec.magnitude);
    }

    public void TakeDamage(DamageModel damageModel)
    {
        if (isLive)
        {
            playerhp -= damageModel.baseDamage;
            StartCoroutine(ChanColoronDamage());
            Debug.Log($"{gameObject.name} took {damageModel.baseDamage} damage of type {damageModel.damageType}. Remaining health: {playerhp}");
        }

        if (playerhp <= 0f)
        {
            PlayerDead();
        }
    }

    public void PlayerDead()
    {
        isLive = false;
        playerAnim.SetTrigger("IsDead");
        DestroyChildren();  // �ڽ� ������Ʈ ����
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            Destroy(playerCollider);  // Collider2D ����
        }
    }

    private void DestroyChildren()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Gun"))
            {
                Destroy(child.gameObject);
                Debug.Log("�� ����");
            }

            Destroy(healthBar.gameObject);
            Debug.Log("ü�¹� ����");
        }
    }

    public IEnumerator ChanColoronDamage()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in sprites)
        {
            // SpriteRenderer�� �ı��Ǿ����� Ȯ��
            if (sprite != null)
            {
                sprite.color = new Color(1f, 0f, 0f, 1f);
            }
        }

        yield return new WaitForSeconds(0.5f);

        foreach (SpriteRenderer sprite in sprites)
        {
            // SpriteRenderer�� �ı��Ǿ����� Ȯ��
            if (sprite != null)
            {
                sprite.color = Color.white;
            }
        }
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
    }

    public void AcquireRelic(RelicData relicData)
    {
        var existing = relics.Find(r => r.data.relicId == relicData.relicId);

        if (existing != null)
        {
            Debug.Log("���� ��ø �׽�Ʈ. ����:" + (existing.stack + 1));
            existing.OnStack(this); // ��ø ó��
        }
        else
        {
            System.Type type = System.Type.GetType("RelicEffect_" + relicData.script);
            if (type != null && typeof(RelicEffect).IsAssignableFrom(type))
            {
                RelicEffect effect = gameObject.AddComponent(type) as RelicEffect;
                effect.data = relicData;
                effect.stack = 1;
                effect.OnEquip(this);
                relics.Add(effect);
                Debug.Log("RelicEffect ������Ʈ �߰���: " + effect);
                Debug.Log("���� relics.Count = " + relics.Count);
                Debug.Log($"���� '{relicData.name}' ȹ��! ���� 1");
            }
            else
            {
                Debug.LogWarning($"RelicEffect Ŭ���� '{relicData.script}'�� ã�� �� ����");
            }
        }
    }
}