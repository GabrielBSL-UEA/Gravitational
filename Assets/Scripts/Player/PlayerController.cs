using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerAnimation _playerAnimation;

    [HideInInspector] public Rigidbody2D Rb2d;

    private CircleCollider2D _circleCollider2D;
    [SerializeField] private float turnTime = .4f;
    [SerializeField] private LayerMask groundLayer;

    public bool OnGround { get; private set; }

    private void Awake()
    {
        TryGetComponent(out Rb2d);
        TryGetComponent(out _playerMovement);
        TryGetComponent(out _playerAnimation);
        TryGetComponent(out _circleCollider2D);

        _playerMovement.controller = this;
        _playerAnimation.controller = this;
    }

    private void OnEnable()
    {
        GameController.Instance.TurnManager.onRotation.AddListener(DisablePhysics);
    }

    private void OnDisable()
    {
        GameController.Instance.TurnManager.onRotation.RemoveListener(DisablePhysics);
    }

    private void FixedUpdate()
    {
        CheckForGround();
    }

    private void CheckForGround()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _circleCollider2D.radius - .1f);
        Vector2 size = new Vector2(_circleCollider2D.radius, .1f);

        OnGround = DrawingRaycast2D.BoxCast(origin, size, groundLayer, Color.green, Color.red);
    }

    private void DisablePhysics(bool toDisable)
    {
        if (toDisable)
        {
            Rb2d.velocity = Vector2.zero;
        }
        else
        {
            TurnPlayerToGround();
        }

        Rb2d.isKinematic = toDisable;
    }

    private void TurnPlayerToGround()
    {
        float desiredAngle = transform.parent.localEulerAngles.z - GameController.Instance.CurrentScenarioAngle;

        transform.LeanRotateZ(desiredAngle, turnTime)
            .setEaseOutQuint();
    }
}
