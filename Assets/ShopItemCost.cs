using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemCost : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    public int Value = 15;

    private void Update()
    {
        _text.text = Value.ToString();
    }
}
