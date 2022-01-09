using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SCRIPTABLES", fileName = "Scriptables")]
public class Scriptables : ScriptableObject
{
    public List<Ship> playerShips = new List<Ship>();
    [Space(8)]
    public List<GameObject> enemiesBasic = new List<GameObject>();
    [Space(8)]
    public List<GameObject> enemiesAdvanced = new List<GameObject>();
    [Space(8)]
    public List<GameObject> enemiesBosses = new List<GameObject>();
    [Space(8)]
    public AudioClip startGame, coinGrab, energyGrab, shot1;
}
