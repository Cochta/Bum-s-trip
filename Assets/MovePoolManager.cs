using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePoolManager : MonoBehaviour
{
    public Player Player;

    public List<Ability> Abilities;

    public Ability Move;
    public Ability Attack;

    private void Awake()
    {
        Abilities = new List<Ability>();
        Abilities.Add(Move);
        Abilities.Add(Attack);
    }

}
