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

    private SpriteRenderer _playerSprite;
    private BoxCollider2D _boxCollider2D;
    [SerializeField] private float turnTime = .4f;
    [SerializeField] private LayerMask groundLayer;

    public bool OnGround { get; private set; }
    private bool _crushed;

    private void Awake()
    {
        TryGetComponent(out Rb2d);
        TryGetComponent(out _playerMovement);
        TryGetComponent(out _playerAnimation);
        TryGetComponent(out _boxCollider2D);
        transform.GetChild(0).TryGetComponent(out _playerSprite);

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
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _boxCollider2D.bounds.extents.y + _boxCollider2D.edgeRadius - .1f);
        Vector2 size = new Vector2(_boxCollider2D.bounds.extents.y + _boxCollider2D.edgeRadius, .1f);

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckForSmash(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckForSmash(collision);
    }

    private void CheckForSmash(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Box") || _crushed || !OnGround)
        {
            return;
        }

        collision.collider.TryGetComponent(out Rigidbody2D boxRig);

        if (collision.transform.position.y > transform.position.y + _boxCollider2D.bounds.extents.y + _boxCollider2D.edgeRadius && !boxRig.isKinematic)
        {
            _crushed = true;
            _playerMovement.enabled = false;
            Rb2d.velocity = Vector2.zero;
            StartCoroutine(StartCrush(collision.transform, collision.contacts[0].point));
        }
    }

    private IEnumerator StartCrush(Transform objectCrushingPlayer, Vector2 collisionPoint)
    {
        var radius = _boxCollider2D.bounds.extents.y + _boxCollider2D.edgeRadius;
        var boxDistanceToGround = collisionPoint.y - transform.position.y + radius;
        var initialBoxYPosition = objectCrushingPlayer.position.y;
        var initialPlayerYPosition = transform.position.y;

        _boxCollider2D.enabled = false;
        float minimalDifference = .001f;
        float lastCrushingPercentage = float.MaxValue;

        while(true)
        {
            yield return new WaitForFixedUpdate();

            var currentBoxYPosititon = objectCrushingPlayer.position.y;
            var yDifference = initialBoxYPosition - currentBoxYPosititon;

            var crushingPercentage = (boxDistanceToGround - yDifference) / boxDistanceToGround;

            if(lastCrushingPercentage != float.MaxValue && (lastCrushingPercentage - crushingPercentage) < minimalDifference)
            {
                break;
            }

            lastCrushingPercentage = crushingPercentage;
            transform.localScale = new Vector3(1, crushingPercentage, 1);
            transform.position = new Vector3(transform.position.x, initialPlayerYPosition - (radius - (radius * crushingPercentage)));
        }

        _playerAnimation.StartDeathEffect();
        _playerSprite.enabled = false;

        yield return new WaitForSeconds(.5f);
        GameController.Instance.StartSceneTransition(true);
    }

    public void DamagePlayer()
    {
        StartCoroutine(StartDeathSequence());
    }

    private IEnumerator StartDeathSequence()
    {
        _playerAnimation.PlayAnimation("player_damage");
        _playerMovement.enabled = false;
        Rb2d.velocity = Vector2.zero;
        Rb2d.bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(.7f);

        _playerAnimation.StartDeathEffect();
        _playerSprite.enabled = false;

        yield return new WaitForSeconds(.5f);
        GameController.Instance.StartSceneTransition(true);
    }
}
