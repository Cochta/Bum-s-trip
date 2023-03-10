using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StuffHolder : MonoBehaviour
{
    private Stuff _stuff;

    [SerializeField] private ItemDisplay _weaponHolder;
    [SerializeField] private ItemDisplay _shieldHolder;
    [SerializeField] private ItemDisplay _headHolder;
    [SerializeField] private ItemDisplay _torsoHolder;
    [SerializeField] private ItemDisplay _handsHolder;
    [SerializeField] private ItemDisplay _legsHolder;
    [SerializeField] private ItemDisplay _feetsHolder;
    [SerializeField] private ItemDisplay _trinketHolder;

    private void Start()
    {
        _stuff = GetComponent<Stuff>();
    }
    private void Update()
    {
        if (_stuff.Weapon != null)
            _weaponHolder._item = _stuff.Weapon;
        if (_stuff.Shield != null)
            _shieldHolder._item = _stuff.Shield;
        if (_stuff.Head != null)
            _headHolder._item = _stuff.Head;
        if (_stuff.Torso != null)
            _torsoHolder._item = _stuff.Torso;
        if (_stuff.Hands != null)
            _handsHolder._item = _stuff.Hands;
        if (_stuff.Legs != null)
            _legsHolder._item = _stuff.Legs;
        if (_stuff.Feets != null)
            _feetsHolder._item = _stuff.Feets;
        if (_stuff.Trinket != null)
            _trinketHolder._item = _stuff.Trinket;
    }

}
