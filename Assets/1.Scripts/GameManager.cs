using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Scriptables scriptables;
    public Transform playerT;
    Player player;

    void Awake()
    {
        Instance = this;
    }

    async void Start()
    {
        player = playerT.GetComponent<Player>();
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        player.Spawn(scriptables.playerShips[0]);
    }
}
