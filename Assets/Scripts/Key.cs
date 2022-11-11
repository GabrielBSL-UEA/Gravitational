using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private bool _obtained;

    private void Update()
    {
        transform.localEulerAngles = -transform.parent.eulerAngles;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || _obtained)
        {
            return;
        }

        _obtained = true;
        GameController.Instance.UnlockDoor(this);
    }
}
