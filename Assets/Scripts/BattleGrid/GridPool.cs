using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridPool", menuName = "ScriptableObjects/GridPool", order = 1)]
public class GridPool : ScriptableObject
{
    public List<GameObject> Grids = new List<GameObject>();
}
