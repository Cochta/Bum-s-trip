using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainPool", menuName = "ScriptableObjects/TerrainPool", order = 1)]

public class TerrainPool : ScriptableObject
{
    public List<TerrainLayout> Borders = new List<TerrainLayout>();
    public List<TerrainLayout> Centers = new List<TerrainLayout>();
}
