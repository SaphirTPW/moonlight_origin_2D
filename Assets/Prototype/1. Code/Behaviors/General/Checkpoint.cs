using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Checkpoint Updated");
                GameManager.Instance.UpdateCheckpoint(transform);
            }
        }
    }
}
