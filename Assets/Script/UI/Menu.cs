using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public VectorValue startPos;
    public ArrayValue openedChests;
    public ArrayValue playerStats;
    public AudioMixer audioMixer;

    public void PlayGame()
    {
        SaveSystem.ClearData();

        SceneManager.LoadScene("Map1");
        startPos.defaultValue.x = 17.0f;
        startPos.defaultValue.y = -12.0f;
        startPos.initialValue = startPos.defaultValue;

        openedChests.defaultValue[0] = 0;
        openedChests.defaultValue[1] = 0;
        openedChests.initialValue = openedChests.defaultValue;

        playerStats.defaultValue[0] = 3;
        playerStats.defaultValue[1] = 2;
        playerStats.initialValue = playerStats.defaultValue;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SetVolumn(float vol)
    {
        audioMixer.SetFloat("volumn", vol);
    }

    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null) return;

        startPos.defaultValue.x = data.position[0];
        startPos.defaultValue.y = data.position[1];
        startPos.initialValue = startPos.defaultValue;

        openedChests.defaultValue = data.openedChests;

        playerStats.defaultValue = data.playerStats;

        SceneManager.LoadScene(data.map);
    }
}
