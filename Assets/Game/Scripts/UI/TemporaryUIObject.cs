using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TemporaryUIObject : MonoBehaviour
{
    [Tooltip("Activate character controller etc...")] public UnityEvent OnClickedOnScreen;

    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            OnClickedOnScreen.Invoke();
            GameManager.Instance.OnLevelStarted.Invoke(SceneController.Instance.levelNoText);
            gameObject.SetActive(false);
        }
    }
}
