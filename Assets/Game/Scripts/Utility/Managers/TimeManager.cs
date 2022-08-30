using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    private float _previousPlayTime = 0f;

    private void Start()
    {
        _previousPlayTime = PlayerPrefs.GetFloat("PreviousPlayTime",0);
        Debug.Log("Previous Play Time: " + _previousPlayTime);
    }
    
    public float GetTotalPlayTime(){
        return _previousPlayTime + Time.time;
    }
    
    

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("PreviousPlayTime", _previousPlayTime + Time.time);
    }
}
