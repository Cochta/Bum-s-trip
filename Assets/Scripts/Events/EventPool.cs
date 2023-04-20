using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventPool", menuName = "ScriptableObjects/EventPool", order = 1)]

public class EventPool : ScriptableObject
{
    public List<RandomEvent> PoolCave = new List<RandomEvent>();
}
