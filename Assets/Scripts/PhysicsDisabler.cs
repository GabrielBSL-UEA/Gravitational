using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDisabler : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private void Awake()
    {
        TryGetComponent(out rb2d);
    }

    private void OnEnable()
    {
        GameController.Instance.TurnManager.onRotation.AddListener(SetRigidbodyKinematic);
    }

    private void OnDisable()
    {
        GameController.Instance.TurnManager.onRotation.RemoveListener(SetRigidbodyKinematic);
    }

    private void SetRigidbodyKinematic(bool toKinematic)
    {
        rb2d.velocity = Vector2.zero;
        rb2d.isKinematic = toKinematic;
    }
}
