using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _actionButton;
    [SerializeField] private GameObject _passButton;
    [SerializeField] private GameObject _okButton;

    private RandomEvent _myEvent;

    [SerializeField] private EventPool _pool;

    public void GenerateEvent()
    {
        _myEvent = _pool.PoolCave[Random.Range(0, _pool.PoolCave.Count)];
        _myEvent.BaseDisplay();
        WindowState(false);

        _actionButton.GetComponentInChildren<TextMeshPro>().text = _myEvent.ActionText;
        _actionButton.GetComponent<ActionButton>().Event = _myEvent;
    }

    public void WindowState(bool isPressed)
    {
        _okButton.SetActive(isPressed);
        _actionButton.SetActive(!isPressed);
        _passButton.SetActive(!isPressed);

        GetComponent<TextMeshPro>().text = _myEvent.DisplayText;
        _okButton.GetComponentInChildren<TextMeshPro>().text = _myEvent.QuitButtonDisplayText;
    }
}
