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
            // 로딩된 유물 중 id=1인 것 찾기
            var relicLoader = FindObjectOfType<RelicLoader>();
            RelicData bleedRelic = relicLoader.loadedRelicData.Find(r => r.relicId == 2);

            if (bleedRelic != null)
            {
                AcquireRelic(bleedRelic);
                Debug.Log($"테스트 유물 '{bleedRelic.name}' 지급 완료");
            }
            else
            {
                Debug.LogError("피의 인장 유물 데이터를 찾을 수 없습니다!");
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
        DestroyChildren();  // 자식 오브젝트 삭제
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            Destroy(playerCollider);  // Collider2D 삭제
        }
    }

    private void DestroyChildren()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Gun"))
            {
                Destroy(child.gameObject);
                Debug.Log("총 제거");
            }

            Destroy(healthBar.gameObject);
            Debug.Log("체력바 제거");
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

    public void StopTime()
    {
        Time.timeScale = 0f;
    }

    public void AcquireRelic(RelicData relicData)
    {
        var existing = relics.Find(r => r.data.relicId == relicData.relicId);

        if (existing != null)
        {
            Debug.Log("유물 중첩 테스트. 개수:" + (existing.stack + 1));
            existing.OnStack(this); // 중첩 처리
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
                Debug.Log("RelicEffect 컴포넌트 추가됨: " + effect);
                Debug.Log("현재 relics.Count = " + relics.Count);
                Debug.Log($"유물 '{relicData.name}' 획득! 스택 1");
            }
            else
            {
                Debug.LogWarning($"RelicEffect 클래스 '{relicData.script}'을 찾을 수 없음");
            }
        }
    }
}