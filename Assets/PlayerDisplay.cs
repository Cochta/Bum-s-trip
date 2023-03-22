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
    public void OnDisplayPlayer(Player player)
    {
        _healthStat.SetActive(true);
        _attackStat.SetActive(true);
        _defenseStat.SetActive(true);
        _luckStat.SetActive(true);
        _MoveStat.SetActive(true);
        _actionPointStat.SetActive(true);

        _name.GetComponent<TextMeshPro>().text = player.Name;

        _healthStat.GetComponentInChildren<TextMeshPro>().text = player.CurentHealth + "/" + player.MaxHealth;
        _attackStat.GetComponentInChildren<TextMeshPro>().text = player.Damage.ToString();
        _defenseStat.GetComponentInChildren<TextMeshPro>().text = player.Defense.ToString();
        _luckStat.GetComponentInChildren<TextMeshPro>().text = player.Luck.ToString();
        _MoveStat.GetComponentInChildren<TextMeshPro>().text = player.MoveDistance.ToString();
        _actionPointStat.GetComponentInChildren<TextMeshPro>().text = player.ActionPoints.ToString();

    }
}
