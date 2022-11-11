using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInterface : MonoBehaviour
{

    [Header("Scene Transition")]
    [SerializeField] private RectTransform transitionImage;
    [SerializeField] private RectTransform rectCanvas;

    private float m_TransitionImageIdlePosition;

    private void Start()
    {
        SetTransitionImageSize();
        AnimateTransition(true);
    }

    public void TurnLeft()
    {
        GameController.Instance.TurnScenarioLeft();
    }
    public void TurnRight()
    {
        GameController.Instance.TurnScenarioRight();
    }

    private void SetTransitionImageSize()
    {
        float newSize = Mathf.Pow(Mathf.Pow(Screen.width / rectCanvas.localScale.x, 2) + Mathf.Pow(Screen.height / rectCanvas.localScale.y, 2), .5f);
        m_TransitionImageIdlePosition = ((Screen.width / rectCanvas.localScale.x) + newSize) / 2;

        transitionImage.sizeDelta = new Vector2(newSize, newSize);
    }

    public void AnimateTransition(bool goingOut = false, bool sameLevel = false)
    {
        transitionImage.gameObject.SetActive(true);

        if (goingOut)
        {
            transitionImage.anchoredPosition = Vector2.zero;
            transitionImage.LeanMoveLocalX(-m_TransitionImageIdlePosition, 1f)
                .setEaseOutSine()
                .setIgnoreTimeScale(true)
                .setOnComplete(() => {
                    transitionImage.gameObject.SetActive(false);
                });
        }
        else
        {
            transitionImage.anchoredPosition = new Vector2(m_TransitionImageIdlePosition, 0);
            transitionImage.LeanMoveLocalX(0, 1f)
                .setEaseOutSine()
                .setIgnoreTimeScale(true)
                .setOnComplete(() => {
                    GameController.Instance.EndStage(sameLevel ? 0 : 1);
                });
        }
    }
}
