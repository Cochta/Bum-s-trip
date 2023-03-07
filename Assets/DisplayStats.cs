using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class DisplayStats : MonoBehaviour
{
    

    [SerializeField] private TextMeshPro _healthStat;
    [SerializeField] private TextMeshPro _attackStat;
    [SerializeField] private TextMeshPro _defenseStat;
    [SerializeField] private TextMeshPro _speedStat;
    [SerializeField] private TextMeshPro _dodgeStat;
    [SerializeField] private TextMeshPro _critStat;

    // Start is called before the first frame update
    public void OnDisplay(Item item)
    {
        _healthStat.text = item._health.ToString("+0;-#");
        _attackStat.text = item._attack.ToString("+0;-#");
        _defenseStat.text = item._defense.ToString("+0;-#");
        _speedStat.text = item._speed.ToString("+0;-#");
        _dodgeStat.text = item._dodge.ToString("+0;-#");
        _critStat.text = item._crit.ToString("+0;-#");
    }
}
