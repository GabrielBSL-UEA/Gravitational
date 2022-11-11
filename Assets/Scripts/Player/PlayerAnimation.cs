using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [HideInInspector] public PlayerController controller;
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private GameObject playerDeathEffect;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _facingRight = true;
    public bool AutomaticAnimation { get; set; } = true;

    private void Awake()
    {
        playerSprite.TryGetComponent(out _spriteRenderer);
        playerSprite.TryGetComponent(out _animator);
    }

    private void FixedUpdate()
    {
        RotateToFacingDirection();

        if (!AutomaticAnimation)
        {
            return;
        }
        CommonAnimationCycle();
    }

    private void RotateToFacingDirection()
    {
        if (controller.Rb2d.velocity.x > .1f && !_facingRight)
        {
            _spriteRenderer.flipX = false;
            _facingRight = true;
        }
        else if (controller.Rb2d.velocity.x < -.1f && _facingRight)
        {
            _spriteRenderer.flipX = true;
            _facingRight = false;
        }
    }

    private void CommonAnimationCycle()
    {
        if (controller.Rb2d.velocity.y < -.1f)
        {
            _animator.Play("player_fall");
            return;
        }

        if (Mathf.Abs(controller.Rb2d.velocity.x) > .1f)
        {
            _animator.Play("player_walk");
            return;
        }

        _animator.Play("player_idle");
    }

    public void StartDeathEffect()
    {
        Instantiate(playerDeathEffect, transform.position, Quaternion.identity);
    }

    public void PlayAnimation(string animationName)
    {
        AutomaticAnimation = false;
        _animator.Play(animationName);
    }
}
