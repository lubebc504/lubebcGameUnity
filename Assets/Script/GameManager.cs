using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public delegate void LevelUpHandler();

    public static event LevelUpHandler OnLevelUp;

    public SpawnData[] spawnData;

    [Header("# Game Object")]
    public PlayerController player;

    public static GameManager Instance;

    public GameObject[] enemyPrefab; // �� ������
    public List<GameObject>[] pools;

    public float spawnInterval = 1.5f; // �� ���� ���� (��)
    public float spawnRadius = 10f; // �÷��̾� ���� ���� �ݰ�
    public int spawnCount = 2; // �� ���� ������ �� ����

    private float spawnTimer = 0f; // Ÿ�̸�

    public Transform playertr;

    [Header("# Game Control")]
    public float gameTime;

    public float maxGameTime = 2000f;
    public int spawnlevel;

    [Header("# Player Info")]
    public int level;

    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

    //private Transform player; // �÷��̾� ����
    public void Awake()
    {
        if (Instance == null) Instance = this;

        pools = new List<GameObject>[enemyPrefab.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    private void Start()
    {
        playertr = PlayerController.instance.transform; // �÷��̾��� Transform ��������
    }

    private void Update()
    {
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
    }

    // ���� �����ϴ� �޼���
    private void SpawnEnemies()
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
            unit.Init(data.healthMultiplier); // speedMultiplier�� ���� ����
            // ���� Init ���̵� �� �������� ���� ���
        }
    }

    // ������ ���� ��ġ ��� (�÷��̾� �ֺ����� ���� �Ÿ� �ٱ�)
    private Vector3 GetSpawnPosition()
    {
        float angle = Random.Range(0f, 360f); // 0�� ~ 360�� ���� ����
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

            OnLevelUp?.Invoke();
        }
    }

    public RectTransform crosshairUI; // ũ�ν���� �̹���
}