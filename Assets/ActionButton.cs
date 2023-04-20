using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public RandomEvent Event;

    private SpriteRenderer _sr;

    public EventGenerator Generator;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        var rnd = Random.Range(0, 101);
        if (rnd <= Event.SuceedChance + PlayerData.Instance.Luck)
        {
            Event.GoodEnding();
        }
        else
        {
            Event.BadEnding();
        }

        OnMouseExit();
        Generator.WindowState(true);
    }

    private void OnMouseEnter()
    {
        _sr.color = Color.cyan;
    }

    private void OnMouseExit()
    {
        _sr.color = Color.white;
    }
}
