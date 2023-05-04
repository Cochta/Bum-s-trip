using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private Image[] _deadImgs;

    public IEnumerator Die(float time)
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(true);
        foreach (var img in _deadImgs)
        {
            img.fillAmount = 0;
        }
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            foreach (var img in _deadImgs)
            {
                img.fillAmount = t;
            }
            yield return null;
        }
        gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }
}
