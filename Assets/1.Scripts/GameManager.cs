using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using EZObjectPools;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Scriptables scriptables;
    public Transform playerT;
    public BoxCollider2D boundariesCollider;
    [HideInInspector]
    public Vector2 boundaries;
    [HideInInspector]
    public Player player;
    Dictionary<string, EZObjectPool> enemyPools = new Dictionary<string, EZObjectPool>();
    List<string> enemies = new List<string>();
    int enemyIterator = 0;

    void Awake()
    {
        Initialize();
        Instance = this;
    }

    async void Start()
    {
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        player.Spawn(scriptables.playerShips[0]);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) SpawnNextEnemy();
    }

    void SpawnNextEnemy()
    {
        var nextEnemyName = enemies[enemyIterator++];
        if (enemyIterator >= enemies.Count - 1) enemyIterator = 0;
        enemyPools[nextEnemyName].TryGetNextObject(Vector3.zero, Quaternion.identity);
    }

    void PrewarmEnemies()
    {
        foreach (var enemyGO in scriptables.enemiesBasic)
        {
            var enemy = enemyGO.GetComponent<Enemy>();
            enemyPools[enemy.enemyName] = EZObjectPool.CreateObjectPool(enemyGO, enemy.enemyName, enemy.frequency, false, true, true);
            for (var x = 0; x < enemy.frequency; x++) enemies.Add(enemy.enemyName);
        }
        enemies = Helpers.ShuffleList(enemies);
    }

    void Initialize()
    {
        player = playerT.GetComponent<Player>();
        SetMapBoundaries();
        PrewarmEnemies();
    }

    void SetMapBoundaries()
    {
        boundaries = new Vector2(boundariesCollider.bounds.extents.x, boundariesCollider.bounds.extents.y);
    }
}
