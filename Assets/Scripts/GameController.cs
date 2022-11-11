using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
    public ScenarioTurnManager TurnManager { get; private set; }

    public float CurrentScenarioAngle { get; private set; } = 0;

    [SerializeField] private float turnTime = .75f;

    private GameInterface _interface;
    private PlayerController _player;
    private Door _stageDoor;
    private bool _keyMoving;

    private void Awake()
    {
        /*
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        */

        Instance = this;
        Time.timeScale = 1;
        TurnManager = FindObjectOfType<ScenarioTurnManager>();
        _player = FindObjectOfType<PlayerController>();
        _interface = FindObjectOfType<GameInterface>();
        _stageDoor = FindObjectOfType<Door>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    public void TurnScenarioLeft()
    {
        if(_player.OnGround && !_keyMoving && TurnManager.TurnScenario(90, turnTime))
        {
            CurrentScenarioAngle = CurrentScenarioAngle == 270 ? 0 : CurrentScenarioAngle + 90;
        }
    }

    public void TurnScenarioRight()
    {
        if (_player.OnGround && !_keyMoving && TurnManager.TurnScenario(-90, turnTime))
        {
            CurrentScenarioAngle = CurrentScenarioAngle == -270 ? 0 : CurrentScenarioAngle - 90;
        }
    }

    public void UnlockDoor(Key doorKey)
    {
        _keyMoving = true;
        var keyTransfom = doorKey.transform;

        var distance = _stageDoor.transform.position - doorKey.transform.position;

        LeanTweenType xTween;
        LeanTweenType yTween;

        if(distance.x > distance.y)
        {
            xTween = LeanTweenType.easeOutCubic;
            yTween = LeanTweenType.easeInCubic;
        }
        else
        {
            xTween = LeanTweenType.easeInCubic;
            yTween = LeanTweenType.easeOutCubic;
        }

        keyTransfom.LeanMoveX(_stageDoor.transform.position.x, 1f)
            .setEase(xTween);

        keyTransfom.LeanMoveY(_stageDoor.transform.position.y, 1f)
            .setEase(yTween)
            .setOnComplete(() =>
            {
                _keyMoving = false;
                _stageDoor.Unlock();
                Destroy(doorKey.gameObject);
            });
    }

    public void StartSceneTransition(bool sameLevel = false)
    {
        _interface.AnimateTransition(false, sameLevel);
    }

    public void EndStage(int sceneJump)
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + sceneJump) % SceneManager.sceneCountInBuildSettings);
    }

    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        TurnManager = FindObjectOfType<ScenarioTurnManager>();
        _player = FindObjectOfType<PlayerController>();
        _interface = FindObjectOfType<GameInterface>();
        _stageDoor = FindObjectOfType<Door>();
    }
}
