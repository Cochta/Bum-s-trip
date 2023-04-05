using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArrow : MonoBehaviour
{
    public Node Node;

    private Color _defaultColor = Color.grey;

    public void SetDefaultColor()
    {
        GetComponent<SpriteRenderer>().color = _defaultColor;
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}
