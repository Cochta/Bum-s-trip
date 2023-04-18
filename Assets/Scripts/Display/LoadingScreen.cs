using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image _loadingBar;

    public IEnumerator Load(float time)
    {
        gameObject.SetActive(true);
        _loadingBar.fillAmount = 0;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            _loadingBar.fillAmount = t;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
