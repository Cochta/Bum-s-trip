using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLayoutPool", menuName = "ScriptableObjects/EnemyLayoutPool", order = 1)]

public class EnemyPool : ScriptableObject
{
    public List<EnemyLayout> PoolCave = new List<EnemyLayout>();
}
