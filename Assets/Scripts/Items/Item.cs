using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum Parts
    {
        Weapon,
        Shield,
        Head,
        Torso,
        Hands,
        Legs,
        Feets,
        Trinket,
        None
    }
    public enum Rarity
    {
        Common,
        UnCommon,
        Rare,
        Epic,
        Legendary
    }

    public int Health = 0;
    public int Damage = 0;
    public int Defense = 0;
    public int Luck = 0;
    public int MoveDistance = 0; // nb of tiles it can move
    public int ActionPoints = 0;


    public string Name = null;
    public string Description = null;

    public Rarity _rarity = Rarity.Common;
    public Parts _part = Parts.None;

    public Sprite _sprite;

    public Ability ability = null;



    protected virtual void OnAttack()
    {

    }
    protected virtual void OnTakeHit()
    {

    }
    protected virtual void Ability() //prend la case en argument
    {

    }
    protected virtual void Passiv()
    {

    }
}
