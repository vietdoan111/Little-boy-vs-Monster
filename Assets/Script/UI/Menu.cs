using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public VectorValue startPos;

    public void PlayGame()
    {
        SceneManager.LoadScene("Map1");
        startPos.defaultValue.x = 17.0f;
        startPos.defaultValue.y = -12.0f;
        startPos.initialValue = startPos.defaultValue;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
