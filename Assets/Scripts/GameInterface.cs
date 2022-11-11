using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInterface : MonoBehaviour
{
    public void TurnLeft()
    {
        GameController.Instance.TurnScenarioLeft();
    }
    public void TurnRight()
    {
        GameController.Instance.TurnScenarioRight();
    }
}
