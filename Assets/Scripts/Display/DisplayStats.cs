using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayStats : MonoBehaviour
{


    [SerializeField] private GameObject _healthStat;
    [SerializeField] private GameObject _attackStat;
    [SerializeField] private GameObject _defenseStat;
    [SerializeField] private GameObject _speedStat;
    [SerializeField] private GameObject _dodgeStat;
    [SerializeField] private GameObject _critStat;

    // Start is called before the first frame update
    public void OnDisplay(Item item)
    {
        _healthStat.SetActive(true);
        _attackStat.SetActive(true);
        _defenseStat.SetActive(true);
        _speedStat.SetActive(true);
        _dodgeStat.SetActive(true);
        _critStat.SetActive(true);

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
        if (item.Speed != 0)
            _speedStat.GetComponentInChildren<TextMeshPro>().text = item.Speed.ToString("+0;-#");
        else
            _speedStat.SetActive(false);
        if (item.MoveDistance != 0)
            _dodgeStat.GetComponentInChildren<TextMeshPro>().text = item.MoveDistance.ToString("+0;-#");
        else
            _dodgeStat.SetActive(false);
        if (item.ActionPoints != 0)
            _critStat.GetComponentInChildren<TextMeshPro>().text = item.ActionPoints.ToString("+0;-#");
        else
            _critStat.SetActive(false);





    }
}
