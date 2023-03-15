using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Tile tile;
    public GameObject Player;
    public Dictionary<Vector2, Tile> grid;

    public string Name = null;

    public Sprite _sprite;

    public bool Isdead = false;

    protected virtual void Awake()
    {

    }

    public virtual void PerformAction()
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
    protected void GetGrid()
    {
        grid = transform.parent.parent.GetComponentInParent<Grid>()._tiles;
    }

    protected Tile GetTile()
    {
        return transform.parent.GetComponentInParent<Tile>();

    }
}
