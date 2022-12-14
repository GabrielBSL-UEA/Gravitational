using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCompletionTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        Time.timeScale = 0;
        GameController.Instance.StartSceneTransition();
    }
}
