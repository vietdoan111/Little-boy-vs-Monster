using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject openChest;
    public GameObject closeChest;
    public GameObject talk;
    public PlayerMovement player;
    public int itemIndex;
    public int chestIndex;

    bool isOpen = false;
    bool isContact = false;

    // Start is called before the first frame update
    void Start()
    {
        if(player.openedChests[chestIndex] == 1) isOpen = true;
        openChest.SetActive(isOpen);
        closeChest.SetActive(!isOpen);
    }

    private void Update()
    {
        if (isOpen) return;
        if (isContact && Input.GetButtonDown("Fire3"))
        {
            isOpen = true;
            isContact = false;
            openChest.SetActive(isOpen);
            closeChest.SetActive(!isOpen);
            talk.SetActive(false);
            player.openedChests[chestIndex] = 1;
            if (itemIndex == 0) player.IncreaseMaxHealth();
            if (itemIndex == 1) player.IncreaseMaxArrow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isOpen) return;
        if(collision.CompareTag("Player"))
        {
            talk.SetActive(true);

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isOpen) return;
        isContact = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        talk.SetActive(false);
        isContact = false;
    }
}
