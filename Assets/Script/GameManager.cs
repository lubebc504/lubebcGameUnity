using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public float spawnTime;              // 스폰 간격
    public int[] possibleEnemyTypes;     // 등장 가능한 적 인덱스 목록
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

    public GameObject[] enemyPrefab; // 적 프리팹
    public List<GameObject>[] pools;

    public float spawnInterval = 1.5f; // 적 스폰 간격 (초)
    public float spawnRadius = 10f; // 플레이어 기준 스폰 반경
    public int spawnCount = 2; // 한 번에 생성할 적 개수

    private float spawnTimer = 0f; // 타이머

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

    //private Transform player; // 플레이어 참조
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
        playertr = PlayerController.instance.transform; // 플레이어의 Transform 가져오기
    }

    private void Update()
    {
        if (playertr == null) return; // 플레이어가 없으면 실행하지 않음

        spawnTimer += Time.deltaTime;
        spawnlevel = Mathf.Min(Mathf.FloorToInt(gameTime / 20f), spawnData.Length - 1);

        if (spawnTimer > (spawnData[spawnlevel].spawnTime))
        {
            spawnTimer = 0;
            SpawnEnemies();
        }

        Vector3 mousePosition = Input.mousePosition; // 마우스 화면 좌표 가져오기
        crosshairUI.position = mousePosition; // 크로스헤어 위치 변경

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    // 적을 스폰하는 메서드
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
            unit.Init(data.healthMultiplier); // speedMultiplier는 현재 무시
            // 별도 Init 없이도 각 프리팹의 스탯 사용
        }
    }

    // 랜덤한 스폰 위치 계산 (플레이어 주변에서 일정 거리 바깥)
    private Vector3 GetSpawnPosition()
    {
        float angle = Random.Range(0f, 360f); // 0도 ~ 360도 랜덤 각도
        float distance = Random.Range(spawnRadius, spawnRadius + 3f); // 일정 거리 이상 떨어지도록 설정

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

    public RectTransform crosshairUI; // 크로스헤어 이미지
}