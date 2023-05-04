using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MovePoolManager : MonoBehaviour
{
    public List<Ability> Abilities;
    public List<GameObject> AbilitiesPrefab;
    private List<GameObject> _instantiatedObjects;

    [SerializeField] public EntityDisplay Display;
    [SerializeField] private GameObject _move;
    [SerializeField] private GameObject _attack;

    public PlayerInBattle Player { get; set; }

    public void SetAbilities()
    {
        GetComponent<TextMeshPro>().text = "Abilities:";

        AbilitiesPrefab = new List<GameObject>();

        Abilities = new List<Ability>();
        AbilitiesPrefab.Add(_move);
        AbilitiesPrefab.Add(_attack);

        foreach (var item in PlayerData.Instance.Items)
        {
            if (item.ability != null)
            {
                item.ability.GetComponent<Ability>().SetSprite(item.Sprite);
                AbilitiesPrefab.Add(item.ability);
            }
        }

        _instantiatedObjects = new List<GameObject>();

        foreach (var prefab in AbilitiesPrefab)
        {
            var obj = Instantiate(prefab, transform);
            var ability = obj.GetComponent<Ability>();
            _instantiatedObjects.Add(obj);
            Abilities.Add(ability);
        }
        float nbObj = _instantiatedObjects.Count;
        float startX = -0.42f;
        float startY = -0.12f;

        for (int i = 0; i < nbObj; i++)
        {
            if (i == 5)
            {
                startY *= 2;
                startX = (startX - (0.2f * i));
            }
            _instantiatedObjects[i].transform.localPosition = new Vector3(startX + (0.2f * i), startY, 0f);
        }
    }

    public void ClearMovepool()
    {
        foreach (var prefab in _instantiatedObjects)
        {
            Destroy(prefab);
        }
        GetComponent<TextMeshPro>().text = string.Empty;
    }
}
