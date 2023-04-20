using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ListWrapper
{
    public List<EnemyLayout> layouts;
}


[CreateAssetMenu(fileName = "EnemyLayoutPool", menuName = "ScriptableObjects/EnemyLayoutPool", order = 1)]
public class EnemyPool : ScriptableObject
{
    public List<ListWrapper> Layouts = new List<ListWrapper>();

    public List<GameObject> PoolCave = new List<GameObject>();

    public List<GameObject> PoolCaveBoss = new List<GameObject>();
}
