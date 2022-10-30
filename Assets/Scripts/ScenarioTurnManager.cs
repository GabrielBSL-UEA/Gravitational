using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioTurnManager : MonoBehaviour
{
    private bool _rotating;
    private float _rotateDelay = .1f;
    private float _rotateDelayTime = 0;

    [HideInInspector] public UnityEvent<bool> onRotation;

    public bool TurnScenario(float direction, float time)
    {
        if (_rotating || _rotateDelayTime > Time.time)
        {
            return false;
        }

        _rotating = true;
        onRotation?.Invoke(_rotating);

        transform.LeanRotateZ(transform.eulerAngles.z + direction, time)
            .setEaseOutQuint()
            .setOnComplete(() =>
            {
                _rotateDelayTime = _rotateDelay + Time.time;
                _rotating = false;
                onRotation?.Invoke(_rotating);
            });

        return true;
    }
}
