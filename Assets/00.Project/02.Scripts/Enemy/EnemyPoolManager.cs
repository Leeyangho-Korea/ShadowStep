using UnityEngine;
using System.Collections.Generic;

public class EnemyPoolManager : MonoBehaviour
{
    [System.Serializable]
    public struct EnemyUnit
    {
        public GameObject enemy;
        public GameObject healthBar;
    }

    public GameObject enemyPrefab;
    public GameObject healthBarPrefab;
    public int initialCount = 10;
    public Transform enemyParent;
    public Transform healthBarParent;

    private Queue<EnemyUnit> enemyPool = new();

    public static EnemyPoolManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < initialCount; i++)
        {
            CreateNewUnit();
        }
    }

    private void CreateNewUnit()
    {
        GameObject enemyGO = Instantiate(enemyPrefab, enemyParent);
        GameObject hpBarGO = Instantiate(healthBarPrefab, healthBarParent);

        enemyGO.SetActive(false);
        hpBarGO.SetActive(false);

        var unit = new EnemyUnit
        {
            enemy = enemyGO,
            healthBar = hpBarGO
        };

        enemyPool.Enqueue(unit);
    }

    public EnemyUnit GetEnemy(Vector3 position)
    {
        Debug.Log($"GetEnemy실행");
        if (enemyPool.Count == 0) CreateNewUnit();

        var unit = enemyPool.Dequeue();
        Debug.Log($"Dequeue 실행");
        unit.enemy.gameObject.SetActive(true);
        Debug.Log($"활성화");
        unit.enemy.transform.position = position;

        var healthBar = unit.healthBar.GetComponent<EnemyHealthBar>();
        var enemyState = unit.enemy.GetComponent<EnemyState>();

        unit.healthBar.gameObject.SetActive(true);
        healthBar.target = unit.enemy.transform;
       healthBar.SetHealth(enemyState.maxHP);
        healthBar.target = unit.enemy.transform;
        enemyState.healthBar = healthBar;

        return unit;
    }

    public void ReturnEnemy(EnemyUnit unit)
    {
        unit.enemy.gameObject.SetActive(false);
        unit.healthBar.gameObject.SetActive(false);
        enemyPool.Enqueue(unit);
    }
}