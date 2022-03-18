using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using EZObjectPools;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Scriptables scriptables;
    public Transform playerT;
    public BoxCollider2D boundariesCollider;
    [HideInInspector]
    public static Vector2 boundaries;
    [HideInInspector]
    public Player player;
    public AudioController musicController;
    public static AudioSource audioSourceSFX;
    Dictionary<string, EZObjectPool> enemyPools = new Dictionary<string, EZObjectPool>();
    List<string> enemiesList = new List<string>();
    int enemyIterator = 0;
    public LayerMask enemyLayerMask;
    public TextMeshProUGUI healthTextUI, energyTextUI;

    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
        Initialize();
    }

    void Initialize()
    {
        player = playerT.GetComponent<Player>();
        audioSourceSFX = GetComponent<AudioSource>();
        SetMapBoundaries();
        PrewarmEnemies();
    }

    async void Start()
    {
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        player.Spawn(scriptables.playerShips[0]);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) SpawnNextEnemy();
        if (Input.GetKeyDown(KeyCode.F1) && Application.isEditor) player.isGodMode = !player.isGodMode;
    }

    void SpawnNextEnemy()
    {
        var nextEnemyName = enemiesList[enemyIterator++];
        if (enemyIterator >= enemiesList.Count - 1) enemyIterator = 0;
        enemyPools[nextEnemyName].TryGetNextObject(Vector3.zero, Quaternion.identity);
    }

    void PrewarmEnemies()
    {
        foreach (var enemyGO in scriptables.enemiesBasic)
        {
            var enemy = enemyGO.GetComponent<Enemy>();
            enemyPools[enemy.enemyName] = EZObjectPool.CreateObjectPool(enemyGO, enemy.enemyName, enemy.frequency, false, true, true);
            for (var x = 0; x < enemy.frequency; x++) enemiesList.Add(enemy.enemyName);
        }
        enemiesList = Helpers.ShuffleList(enemiesList);
    }

    void SetMapBoundaries()
    {
        boundaries = new Vector2(boundariesCollider.bounds.extents.x, boundariesCollider.bounds.extents.y);
    }

    async public void GameOver()
    {
        var hits = Physics2D.CircleCastAll(playerT.position, 1000f, Vector2.one, 1000f, enemyLayerMask);
        for (var x = 0; x < hits.Length; x++)
        {
            hits[x].transform.GetComponent<Enemy>()?.Sleep();
            await UniTask.NextFrame();
        }
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        SceneManager.LoadScene(0);
    }
}
