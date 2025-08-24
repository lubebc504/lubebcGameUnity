using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SpawnData
{
    public float spawnTime;              // ���� ����
    public int[] possibleEnemyTypes;     // ���� ������ �� �ε��� ���
    public float healthMultiplier = 1f;
}

public class GameManager : MonoBehaviour
{
    public bool isBlockingInput = false;

    public UnityEvent OnLevelUp = new UnityEvent();

    public SpawnData[] spawnData;

    [Header("# Game Object")]
    public PlayerController player;

    public static GameManager Instance;

    public GameObject[] enemyPrefab; // �� ������

    public float spawnInterval = 1.5f; // �� ���� ���� (��)
    public float spawnRadius = 10f; // �÷��̾� ���� ���� �ݰ�
    public int spawnCount = 2; // �� ���� ������ �� ����

    private float spawnTimer = 0f; // Ÿ�̸�

    public Transform playertr;

    [Header("Game Control")]
    public float gameTime;

    public float maxGameTime = 2000f;
    public int spawnlevel;

    [Header("layer Info")]
    public int level;

    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

    [Header("Boss Spawn")]
    public GameObject bossPrefab;

    public float bossSpawnTime = 30f;
    private bool bossSpawned = false;

    //private Transform player; // �÷��̾� ����
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Start()
    {
        playertr = PlayerController.instance.transform; // �÷��̾��� Transform ��������
        SoundManager.instance.PlayBGM(SoundManager.EBgm.BGM_GAME);
    }

    private void Update()
    {
        if (Time.timeScale != 0f)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0f;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1f;
            }
        }

        if (playertr == null) return; // �÷��̾ ������ �������� ����

        spawnTimer += Time.deltaTime;
        spawnlevel = Mathf.Min(Mathf.FloorToInt(gameTime / 20f), spawnData.Length - 1);

        if (spawnTimer > (spawnData[spawnlevel].spawnTime))
        {
            spawnTimer = 0;
            SpawnEnemies();
        }

        Vector3 mousePosition = Input.mousePosition; // ���콺 ȭ�� ��ǥ ��������
        crosshairUI.position = mousePosition; // ũ�ν���� ��ġ ����

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }

        if (!bossSpawned && gameTime >= bossSpawnTime)
        {
            SpawnBoss();
        }
    }

    // ���� �����ϴ� �޼���
    public void SpawnEnemies()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = GetSpawnPosition();
            SpawnData data = spawnData[spawnlevel];

            int[] possibleTypes = data.possibleEnemyTypes;
            if (possibleTypes == null || possibleTypes.Length == 0) continue;

            int enemyIndex = possibleTypes[Random.Range(0, possibleTypes.Length)];
            EnemyUnit unit = Instantiate(enemyPrefab[enemyIndex], spawnPos, Quaternion.identity)
           .GetComponent<EnemyUnit>();
            unit.Init(data.healthMultiplier);
        }
    }

    public void SpawnBoss()
    {
        Vector3 spawnPos = GetSpawnPosition();
        Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        bossSpawned = true;
        Debug.Log("���� ����!");
    }

    // ������ ���� ��ġ ��� (�÷��̾� �ֺ����� ���� �Ÿ� �ٱ�)
    public Vector3 GetSpawnPosition()
    {
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(spawnRadius, spawnRadius + 3f); // ���� �Ÿ� �̻� ���������� ����

        Vector3 spawnPos = playertr.position + new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
            Mathf.Sin(angle * Mathf.Deg2Rad) * distance,
            0f
        );

        return spawnPos;
    }

    public void GetExp(int getExp)
    {
        exp += getExp;
        if (exp >= nextExp[level])
        {
            exp -= nextExp[level];
            level++;
            player.playerhp += 20;
            if (player.playerhp >= player.maxHealth)
            {
                player.playerhp = player.maxHealth;
            }
            OnLevelUp?.Invoke();
        }
    }

    public RectTransform crosshairUI; // ũ�ν���� �̹���
}