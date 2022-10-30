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

    private PlayerController _player;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        TurnManager = FindObjectOfType<ScenarioTurnManager>();
        _player = FindObjectOfType<PlayerController>();
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
        if(_player.OnGround && TurnManager.TurnScenario(90, turnTime))
        {
            CurrentScenarioAngle = CurrentScenarioAngle == 270 ? 0 : CurrentScenarioAngle + 90;
        }
    }

    public void TurnScenarioRight()
    {
        if (_player.OnGround && TurnManager.TurnScenario(-90, turnTime))
        {
            CurrentScenarioAngle = CurrentScenarioAngle == -270 ? 0 : CurrentScenarioAngle - 90;
        }
    }

    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        TurnManager = FindObjectOfType<ScenarioTurnManager>();
        _player = FindObjectOfType<PlayerController>();
    }
}
