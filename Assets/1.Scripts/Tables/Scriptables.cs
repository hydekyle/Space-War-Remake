using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SCRIPTABLES", fileName = "Scriptables")]
public class Scriptables : ScriptableObject
{
    [Header("Player Ships")]
    public List<Ship> playerShips = new List<Ship>();
    [Header("Enemies Basic")]
    public List<GameObject> enemiesBasic = new List<GameObject>();
    [Header("Enemies Advances")]
    public List<GameObject> enemiesAdvanced = new List<GameObject>();
    [Header("Enemies Bosses")]
    public List<GameObject> enemiesBosses = new List<GameObject>();
}
