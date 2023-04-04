using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private static PlayerData _instance;
    public static PlayerData Instance { get { return _instance; } }

    public enum GameStates
    {
        EnterNewLevel,
        ToMap,
        ToBattle,
        ToShop,
        ToEvent,
        ToTreasure,
        ToBoss
    }

    private GameStates _state;
    public GameStates State { get => _state; set => _state = value; }


    private Node _node;
    public Node Node { get => _node; set => _node = value; }

    private int _coins = 3;
    public int Coins { get => _coins; set => _coins = value; }


    [NonSerialized] public int CurrentHealth;
    [NonSerialized] public int MaxHealth;
    [NonSerialized] public int Damage;
    [NonSerialized] public int Defense;
    [NonSerialized] public int Luck;
    [NonSerialized] public int MoveDistance; // nb of tiles it can move
    [NonSerialized] public int ActionPoints;

    public Stuff Stuff;

    [SerializeField] private PlayerDisplay _display;

    [SerializeField] private BattleManager _battleManager;
    [SerializeField] private Map _map;
    [SerializeField] private LootGenerator _loot;

    public int Level = 0;

    private void Awake()
    {
        _instance = this;
        UpdateData();
        ChangeGameState(GameStates.EnterNewLevel);
        _battleManager.ChangeState(BattleManager.GameStates.Initialisation);
    }

    public void UpdateData()
    {
        int oldHealth = MaxHealth;

        MaxHealth = 5;
        Damage = 2;
        Defense = 1;
        Luck = 0;
        MoveDistance = 2;
        ActionPoints = 2;

        List<Item> items = new List<Item>();
        if (Stuff.Weapon != null)
            items.Add(Stuff.Weapon);
        if (Stuff.Shield != null)
            items.Add(Stuff.Shield);
        if (Stuff.Head != null)
            items.Add(Stuff.Head);
        if (Stuff.Torso != null)
            items.Add(Stuff.Torso);
        if (Stuff.Hands != null)
            items.Add(Stuff.Hands);
        if (Stuff.Legs != null)
            items.Add(Stuff.Legs);
        if (Stuff.Feets != null)
            items.Add(Stuff.Feets);
        if (Stuff.Trinket != null)
            items.Add(Stuff.Trinket);

        foreach (var item in items)
        {
            MaxHealth += item.Health;
            Damage += item.Damage;
            Defense += item.Defense;
            Luck += item.Luck;
            MoveDistance += item.MoveDistance;
            ActionPoints += item.ActionPoints;
        }

        int difference = Math.Abs(MaxHealth - oldHealth);
        if (difference > 0)
        {
            CurrentHealth += difference;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        };

        _display.OnDisplayPlayer();
    }
    public void Attack(Entity entity, int Damage)
    {
        entity.TakeDamage(Damage);
        UpdateData();
    }
    public void TakeDamage(int Damage)
    {
        CurrentHealth -= Damage - Instance.Defense;
        UpdateData();
    }
    public void EnableAbilities()
    {
        foreach (var ability in _display.Pool.Abilities)
        {
            ability._col.enabled = true;
        }
    }
    public void DisableAbilities()
    {
        foreach (var ability in _display.Pool.Abilities)
        {
            ability._col.enabled = false;
            ability.IsSelected = false;
        }
    }

    public void ChangeGameState(GameStates newState)
    {
        _state = newState;
        _map.gameObject.SetActive(false);
        _battleManager.gameObject.SetActive(false);
        _loot.gameObject.SetActive(false);
        switch (_state)
        {
            case GameStates.EnterNewLevel:
                _map.GenerateMap();
                ChangeGameState(GameStates.ToMap);
                Level++;
                break;
            case GameStates.ToMap:
                _map.gameObject.SetActive(true);
                _map.SetPath();
                break;
            case GameStates.ToBattle:
                _battleManager.gameObject.SetActive(true);
                _battleManager.ChangeState(BattleManager.GameStates.StartBattle);
                break;
            case GameStates.ToShop:
                _loot.gameObject.SetActive(true);
                _loot.GenerateLoot();
                break;
            case GameStates.ToEvent:
                break;
            case GameStates.ToTreasure:
                _loot.gameObject.SetActive(true);
                _loot.GenerateLoot();
                break;
            case GameStates.ToBoss:
                break;
        }
    }
}
