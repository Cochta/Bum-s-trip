using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public abstract class Entity : MonoBehaviour
{
    public int CurentHealth = 0;
    public int MaxHealth = 0;
    public int Damage = 0;
    public int Defense = 0;
    public int Luck = 0;
    public int MoveDistance = 0; // nb of tiles it can move
    public int ActionPoints = 0;

    public Tile tile;
    public Player Player;
    public Dictionary<Vector2, Tile> grid;

    public string Name = null;

    public Sprite _sprite;

    public bool Isdead = false;

    [SerializeField] private SpriteRenderer _deadSR;
    [SerializeField] private Sprite _deadSprite;

    protected virtual void Awake()
    {

    }

    public virtual void PerformAction()
    {

    }

    protected virtual void Move()
    {

    }
    protected virtual void Attack(Player player, int Damage)
    {
        player.TakeDamage(Damage);
    }
    public virtual void TakeDamage(int Damage)
    {
        CurentHealth -= Damage - Defense;
        if (CurentHealth <= 0)
        {
            Isdead = true;
            _deadSR.sprite = _deadSprite;
        }
    }
    protected void GetGrid()
    {
        grid = transform.parent.parent.GetComponentInParent<Grid>()._tiles;
    }

    protected Tile GetTile()
    {
        return transform.parent.GetComponentInParent<Tile>();
    }

    //protected virtual void OnMouseEnter()
    //{
    //    var display = transform.parent.parent.GetComponentInParent<EntityDisplay>();
    //    display._stats.SetActive(true);
    //    display.Display.OnDisplayEntity(this);
    //}

    //protected virtual void OnMouseExit()
    //{
    //    var display = transform.parent.parent.GetComponentInParent<EntityDisplay>();
    //    display._stats.SetActive(false);
    //}
}
