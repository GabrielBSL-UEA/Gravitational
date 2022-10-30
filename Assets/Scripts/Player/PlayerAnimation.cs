using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [HideInInspector] public PlayerController controller;
    [SerializeField] private GameObject playerSprite;

    private SpriteRenderer _spriteRenderer;

    private bool _facingRight = true;

    private void Awake()
    {
        playerSprite.TryGetComponent(out _spriteRenderer);
    }

    private void FixedUpdate()
    {
        if(controller.Rb2d.velocity.x > .1f && !_facingRight)
        {
            _spriteRenderer.flipX = false;
            _facingRight = true;
        }
        else if(controller.Rb2d.velocity.x < -.1f && _facingRight)
        {
            _spriteRenderer.flipX = true;
            _facingRight = false;
        }
    }
}
