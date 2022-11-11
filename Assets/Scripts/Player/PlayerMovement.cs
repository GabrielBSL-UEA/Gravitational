using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public PlayerController controller;

    private PlayerInputs inputActions;

    [SerializeField] private float maxVelocity = 7.5f;
    [SerializeField] private float acceleration = 5;
    [SerializeField] private float deacceleration = 8;

    private float horizontalDirection;

    private void Awake()
    {
        inputActions = new PlayerInputs();

        inputActions.Player.Walk.performed += ctx => horizontalDirection = ctx.ReadValue<float>();
        inputActions.Player.Walk.canceled += _ => horizontalDirection = 0;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void FixedUpdate()
    {
        ApplyHorizontalVelocity();
    }

    private void ApplyHorizontalVelocity()
    {
        float airboundMultiplier = controller.OnGround ? 1 : .3f;

        float targetSpeed = horizontalDirection * airboundMultiplier * maxVelocity;
        float speedDifference = targetSpeed - controller.Rb2d.velocity.x;
        float accelerationRate = Mathf.Sign(controller.Rb2d.velocity.x) == MathF.Sign(targetSpeed) && targetSpeed != 0
            ? acceleration
            : deacceleration;

        controller.Rb2d.AddForce(transform.right * (accelerationRate * speedDifference));
    }
}
