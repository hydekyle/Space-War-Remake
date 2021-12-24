using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Scriptables scriptables;
    public Transform playerT;
    [HideInInspector]
    public float xMaxPos, yMaxPos;
    [HideInInspector]
    public Player player;

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

    void Initialize()
    {
        player = playerT.GetComponent<Player>();
        SetMapBoundaries();
    }

    void SetMapBoundaries()
    {
        var boundaries = transform.Find("Boundaries").GetComponent<BoxCollider2D>();
        xMaxPos = boundaries.size.x / 2;
        yMaxPos = boundaries.size.y / 2;
    }
}
