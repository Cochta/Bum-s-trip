using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVictory : MonoBehaviour
{
    public IEnumerator Win(float time)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainMenu");
    }
}
