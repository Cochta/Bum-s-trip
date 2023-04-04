using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _name;
    [SerializeField] private GameObject _description;
    [SerializeField] private GameObject _healthStat;
    [SerializeField] private GameObject _attackStat;
    [SerializeField] private GameObject _defenseStat;
    [SerializeField] private GameObject _luckStat;
    [SerializeField] private GameObject _MoveStat;
    [SerializeField] private GameObject _actionPointStat;
    [SerializeField] private GameObject _coinsStat;

    public MovePoolManager Pool;
    public void OnDisplayPlayer()
    {
        _healthStat.SetActive(true);
        _attackStat.SetActive(true);
        _defenseStat.SetActive(true);
        _luckStat.SetActive(true);
        _MoveStat.SetActive(true);
        _actionPointStat.SetActive(true);

        _healthStat.GetComponentInChildren<TextMeshPro>().text = PlayerData.Instance.CurrentHealth + "/" + PlayerData.Instance.MaxHealth;
        _attackStat.GetComponentInChildren<TextMeshPro>().text = PlayerData.Instance.Damage.ToString();
        _defenseStat.GetComponentInChildren<TextMeshPro>().text = PlayerData.Instance.Defense.ToString();
        _luckStat.GetComponentInChildren<TextMeshPro>().text = PlayerData.Instance.Luck.ToString();
        _MoveStat.GetComponentInChildren<TextMeshPro>().text = PlayerData.Instance.MoveDistance.ToString();
        _actionPointStat.GetComponentInChildren<TextMeshPro>().text = PlayerData.Instance.ActionPoints.ToString();
        _coinsStat.GetComponentInChildren<TextMeshPro>().text = PlayerData.Instance.Coins.ToString();

    }
}
