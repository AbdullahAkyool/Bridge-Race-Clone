using System;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoSingleton<GameManager>
{
    #region Events

    [HideInInspector] public UnityEvent<int> OnLevelStarted;
    [HideInInspector] public UnityEvent OnEnteredFinish;

    [HideInInspector] public UnityEvent OnGameFinished;
    [HideInInspector] public UnityEvent<int> OnLevelSuccess;
    [HideInInspector] public UnityEvent<int> OnLevelFailed;

    [HideInInspector] public UnityEvent OnSuccessPanelOpened;
    [HideInInspector] public UnityEvent OnFailPanelOpened;

    [HideInInspector] public UnityEvent OnTemporaryCurrencyCollected;
    
    [HideInInspector] public UnityEvent<int> OnUIFillableCompleted;
    
    #endregion
    
    internal float levelStartTime = 0f;
    internal GameState gameState;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    public void StartLevel()
    {
        OnLevelStarted.Invoke(SceneController.Instance.levelNoText);
        UIManager.Instance.OpenGamePanel();
        levelStartTime = Time.time;
        Debug.Log("<b>Level <color=yellow>STARTED</color></b>");
    }
    public void SuccessLevel(float panelOpenDelay = 0f)
    {
        OnGameFinished.Invoke();
        OnLevelSuccess.Invoke(SceneController.Instance.levelNoText);
        UIManager.Instance.OpenSuccessPanel(panelOpenDelay);
        Debug.Log("<b>Level <color=green>COMPLETED</color> in <color=orange>" + (Time.time-levelStartTime).ToString("0.0") +" </color>seconds</b>");
    }

    public void FailLevel(float panelOpenDelay = 0f)
    {
        OnGameFinished.Invoke();
        OnLevelFailed.Invoke(SceneController.Instance.levelNoText);
        UIManager.Instance.OpenFailPanel(panelOpenDelay);
        Debug.Log("<b>Level <color=red>FAILED</color> at <color=orange>" + (Time.time-levelStartTime).ToString("0.0") +" </color>seconds</b>");
    }


}
