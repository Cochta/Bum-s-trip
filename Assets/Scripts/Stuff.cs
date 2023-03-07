using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuff : MonoBehaviour
{
    [SerializeField]
    private Item _weapon;
    [SerializeField]
    private Item _shield;
    [SerializeField]
    private Item _head;
    [SerializeField]
    private Item _torso;
    [SerializeField]
    private Item _hands;
    [SerializeField]
    private Item _legs;
    [SerializeField]
    private Item _feets;
    [SerializeField]
    private Item _trinket;

    [SerializeField] private ItemDisplay _weaponHolder;
    [SerializeField] private ItemDisplay _shieldHolder;
    [SerializeField] private ItemDisplay _headHolder;
    [SerializeField] private ItemDisplay _torsoHolder;
    [SerializeField] private ItemDisplay _handsHolder;
    [SerializeField] private ItemDisplay _legsHolder;
    [SerializeField] private ItemDisplay _feetsHolder;
    [SerializeField] private ItemDisplay _trinketHolder;

    private void Update()
    {
        if (_weapon != null)
            _weaponHolder._item = _weapon;
        if (_shield != null)
            _shieldHolder._item = _shield;
        if (_head != null)
            _headHolder._item = _head;
        if (_torso != null)
            _torsoHolder._item = _torso;
        if (_hands != null)
            _handsHolder._item = _hands;
        if (_legs != null)
            _legsHolder._item = _legs;
        if (_feets != null)
            _feetsHolder._item = _feets;
        if (_trinket != null)
            _trinketHolder._item = _trinket;
    }

}
