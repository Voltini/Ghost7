using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public PlayerControl player;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("checkpointed");
            player.Checkpoint();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("checkpointed by collision");
    }
}
