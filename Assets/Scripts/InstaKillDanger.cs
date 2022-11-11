using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaKillDanger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            return;
        }

        collision.collider.TryGetComponent(out PlayerController playerController);
        playerController.DamagePlayer();
    }
}
