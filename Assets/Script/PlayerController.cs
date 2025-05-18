using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    private float maxHealth;
    public float playerhp;
    public HealthBar healthBar;

    public Vector2 inputVec;
    public float speed;
    private Rigidbody2D playerRigid;
    public SpriteRenderer sprite;
    public Animator playerAnim;

    public static PlayerController instance;

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
}