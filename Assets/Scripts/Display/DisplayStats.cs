using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class DisplayStats : MonoBehaviour
{
    [SerializeField] private GameObject _name;
    [SerializeField] private GameObject _description;
    [SerializeField] private GameObject _healthStat;
    [SerializeField] private GameObject _attackStat;
    [SerializeField] private GameObject _defenseStat;
    [SerializeField] private GameObject _luckStat;
    [SerializeField] private GameObject _MoveStat;
    [SerializeField] private GameObject _actionPointStat;

    public void OnDisplayAbility(Ability ability)
    {
        _healthStat.SetActive(false);
        _attackStat.SetActive(false);
        _defenseStat.SetActive(false);
        _luckStat.SetActive(false);
        _MoveStat.SetActive(false);
        _actionPointStat.SetActive(false);

        _name.GetComponent<TextMeshPro>().text = ability.Name;
        _description.GetComponent<TextMeshPro>().text = ability.Description;
    }
    public void OnDisplayItem(Item item)
    {
        _healthStat.SetActive(true);
        _attackStat.SetActive(true);
        _defenseStat.SetActive(true);
        _luckStat.SetActive(true);
        _MoveStat.SetActive(true);
        _actionPointStat.SetActive(true);

        _name.GetComponent<TextMeshPro>().text = item.Name;
        _description.GetComponent<TextMeshPro>().text = item.Description;

        if (item.Health != 0)
            _healthStat.GetComponentInChildren<TextMeshPro>().text = item.Health.ToString("+0;-#");
        else
            _healthStat.SetActive(false);
        if (item.Damage != 0)
            _attackStat.GetComponentInChildren<TextMeshPro>().text = item.Damage.ToString("+0;-#");
        else
            _attackStat.SetActive(false);
        if (item.Defense != 0)
            _defenseStat.GetComponentInChildren<TextMeshPro>().text = item.Defense.ToString("+0;-#");
        else
            _defenseStat.SetActive(false);
        if (item.Luck != 0)
            _luckStat.GetComponentInChildren<TextMeshPro>().text = item.Luck.ToString("+0;-#");
        else
            _luckStat.SetActive(false);
        if (item.MoveDistance != 0)
            _MoveStat.GetComponentInChildren<TextMeshPro>().text = item.MoveDistance.ToString("+0;-#");
        else
            _MoveStat.SetActive(false);
        if (item.ActionPoints != 0)
            _actionPointStat.GetComponentInChildren<TextMeshPro>().text = item.ActionPoints.ToString("+0;-#");
        else
            _actionPointStat.SetActive(false);
    }
    public void OnDisplayEntity(Entity entity)
    {
        _healthStat.SetActive(true);
        _attackStat.SetActive(true);
        _defenseStat.SetActive(true);
        _luckStat.SetActive(true);
        _MoveStat.SetActive(true);
        _actionPointStat.SetActive(true);

        _name.GetComponent<TextMeshPro>().text = entity.Name;
        _description.GetComponent<TextMeshPro>().text = entity.Description;

        _healthStat.GetComponentInChildren<TextMeshPro>().text = entity.CurentHealth + "/" + entity.MaxHealth;
        _attackStat.GetComponentInChildren<TextMeshPro>().text = entity.Damage.ToString();
        _defenseStat.GetComponentInChildren<TextMeshPro>().text = entity.Defense.ToString();
        _luckStat.GetComponentInChildren<TextMeshPro>().text = entity.Luck.ToString();
        _MoveStat.GetComponentInChildren<TextMeshPro>().text = entity.MoveDistance.ToString();
        _actionPointStat.GetComponentInChildren<TextMeshPro>().text = entity.BaseActionPoints.ToString();

    }
}
