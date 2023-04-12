using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int heath;
    public int arrowNum = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Arrow"))
        {
            Debug.Log("Pick up arrow");
            Destroy(collision.gameObject);
            arrowNum++;
        }
    }
}
