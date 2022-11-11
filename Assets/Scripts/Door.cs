using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject leftEnd;
    [SerializeField] private GameObject rightEnd;

    private Animator _animator;

    private bool _playerInSight;
    private bool _unlocked;

    private void Awake()
    {
        leftEnd.SetActive(false);
        rightEnd.SetActive(false);
        TryGetComponent(out _animator);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !_unlocked)
        {
            return;
        }

        collision.TryGetComponent(out PlayerController playerController);

        if(!playerController.OnGround)
        {
            leftEnd.SetActive(false);
            rightEnd.SetActive(false);
            return;
        }

        if (_playerInSight)
        {
            return;
        }

        _playerInSight = true;

        var xDifference = collision.transform.position.x - transform.position.x;

        leftEnd.SetActive(xDifference > 0);
        rightEnd.SetActive(xDifference <= 0);
    }

    public void Unlock()
    {
        _unlocked = true;
        _animator.Play("door_opening");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        _playerInSight = false;
    }
}
