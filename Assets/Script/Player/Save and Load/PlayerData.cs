using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string map;
    public float[] position;
    public int[] openedChests;
    public int[] playerStats;

    public PlayerData (Player player, SceneTransition scene)
    {
        map = scene.sceneToLoad;

        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;

        playerStats = new int[2];
        playerStats[0] = player.maxHealth;
        playerStats[1] = player.maxArrowNum;

        openedChests = new int[2];
        openedChests[0] = player.openedChests[0];
        openedChests[1] = player.openedChests[1];
    }
}
