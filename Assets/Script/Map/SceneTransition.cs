using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue playerStorage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            playerStorage.initialValue = playerPosition;
            player.transform.position = playerStorage.initialValue;
            SaveGame(player);
            
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void SaveGame(Player player)
    {
        SaveSystem.SavePlayer(player, this);
    }
}
