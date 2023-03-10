using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int CurentHealth = 0;
    public int MaxHealth = 0;
    public int Damage = 0;
    public int Defense = 0;
    public int Speed = 0; // initiative
    public int MoveDistance = 0; // nb of tiles it can move
    public int ActionPoints = 0;

    public Tile tile = null;

    public string Name = null;

    public Sprite _sprite;

    public abstract void Start();

    protected virtual void PerformAction()
    {

    }

    protected virtual void Move()
    {

    }
    protected virtual void Attack(Entity entity, int Damage)
    {
        entity.TakeDamage(Damage);
    }
    protected virtual void TakeDamage(int Damage)
    {
        CurentHealth -= Damage - Defense;
    }
    protected Dictionary<Vector2, Tile> GetGrid()
    {
        return GetComponentInParent<Grid>()._tiles;
    }
}
