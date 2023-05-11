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
        ToBoss,
        ToBossLoot,
        None
    }

    private GameStates _state;
    public GameStates State { get => _state; set => _state = value; }

    private Node _node;
    public Node Node { get => _node; set => _node = value; }

    private int _coins = 0;
    public int Coins { get => _coins; set => _coins = value; }


    [NonSerialized] public int CurrentHealth;
    [NonSerialized] public int MaxHealth;
    [NonSerialized] public int Damage;
    [NonSerialized] public int Defense;
    [NonSerialized] public int Luck;
    [NonSerialized] public int MoveDistance; // nb of tiles it can move
    [NonSerialized] public int ActionPoints;
    [NonSerialized] public int ActionsRemaining; // nb of tiles it can move

    public Stuff Stuff;
    public List<Item> Items;

    [SerializeField] private PlayerDeath _playerDeath;
    public PlayerVictory PlayerVictory;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private PlayerDisplay _display;
    [SerializeField] private MovePoolManager _movePoolManager;

    [SerializeField] private BattleManager _battleManager;
    [SerializeField] private Map _map;
    public LootGenerator Loot;
    [SerializeField] private EventGenerator _event;

    public int Level = 0;

    private bool _isDead = false;

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

        MaxHealth = 10;
        Damage = 3;
        Defense = 0;
        Luck = 0;
        MoveDistance = 2;
        ActionPoints = 2;

        Items = new List<Item>();
        if (Stuff.Weapon != null)
            Items.Add(Stuff.Weapon);
        if (Stuff.Shield != null)
            Items.Add(Stuff.Shield);
        if (Stuff.Head != null)
            Items.Add(Stuff.Head);
        if (Stuff.Torso != null)
            Items.Add(Stuff.Torso);
        if (Stuff.Hands != null)
            Items.Add(Stuff.Hands);
        if (Stuff.Legs != null)
            Items.Add(Stuff.Legs);
        if (Stuff.Feets != null)
            Items.Add(Stuff.Feets);
        if (Stuff.Trinket != null)
            Items.Add(Stuff.Trinket);

        foreach (var item in Items)
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

        if (CurrentHealth <= 0 && !_isDead)
        {
            _isDead = true;
            StartCoroutine(_playerDeath.Die(5));
            SoundHandeler.Instance.StopMusic();
        }
        _display.OnDisplayPlayer();
    }
    public void ResetActions()
    {
        ActionsRemaining = ActionPoints;
    }
    public void Attack(Entity entity, int Damage)
    {
        entity.TakeDamage(Damage);
        SoundHandeler.Instance.PlayBumAttack();
        UpdateData();
    }
    public void TakeDamage(int Damage)
    {
        if (Damage > Defense)
        {
            CurrentHealth -= Damage - Instance.Defense;
            SoundHandeler.Instance.PlayBumBadEvent();
            UpdateData();
            StartCoroutine(_display.BounceHealth(Color.red));
        }
    }
    public void TakeDirectDamage(int Damage)
    {
        CurrentHealth -= Damage;
        UpdateData();
        StartCoroutine(_display.BounceHealth(Color.red));
        SoundHandeler.Instance.PlayBumBadEvent();
    }
    public void Heal(int health)
    {
        CurrentHealth += health;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
        UpdateData();
        StartCoroutine(_display.BounceHealth(Color.green));
        SoundHandeler.Instance.PlayBumBuff();
    }
    public void GainMoney(int coins)
    {
        Coins += coins;
        UpdateData();
        StartCoroutine(_display.BounceMoney(Color.yellow));
    }
    public void LoseMoney(int coins)
    {
        Coins -= coins;
        UpdateData();
        StartCoroutine(_display.BounceMoney(Color.red));

    }

    public void EnableAbilities()
    {
        foreach (var ability in _display.Pool.Abilities)
        {
            ability.Enable();
        }
    }
    public void DisableAbilities()
    {
        foreach (var ability in _display.Pool.Abilities)
        {
            ability.Disable();
            ability.IsSelected = false;
        }
    }
    public void DecrementCooldown()
    {
        foreach (var ability in _display.Pool.Abilities)
        {
            ability.RemainingCooldown -= 1;
        }
    }

    public void ChangeGameState(GameStates newState)
    {
        _state = newState;
        _map.gameObject.SetActive(false);
        _battleManager.gameObject.SetActive(false);
        Loot.gameObject.SetActive(false);
        _event.gameObject.SetActive(false);
        switch (_state)
        {
            case GameStates.EnterNewLevel:
                SoundHandeler.Instance.PlayNormalMusic();
                SoundHandeler.Instance.PlayBumCrazy();
                StartCoroutine(_loadingScreen.Load(0f));
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
                _movePoolManager.SetAbilities();
                _battleManager.ChangeState(BattleManager.GameStates.StartBattle);
                break;
            case GameStates.ToShop:
                Loot.gameObject.SetActive(true);
                Loot.GenerateLoot();
                break;
            case GameStates.ToEvent:
                _event.gameObject.SetActive(true);
                _event.GenerateEvent();
                break;
            case GameStates.ToTreasure:
                Loot.gameObject.SetActive(true);
                Loot.GenerateLoot();
                break;
            case GameStates.ToBoss:
                SoundHandeler.Instance.PlayBossMusic();
                _battleManager.gameObject.SetActive(true);
                _movePoolManager.SetAbilities();
                _battleManager.ChangeState(BattleManager.GameStates.StartBattle);
                break;
            case GameStates.ToBossLoot:
                Loot.gameObject.SetActive(true);
                Loot.GenerateLoot();
                break;
        }
    }
}
