using UnityEngine;

public static class DataManager
{

    #region Game Setting Values
    public static void SetAudio(bool key)
    {
        if(key) PlayerPrefs.SetInt("Audio",1);
        else PlayerPrefs.SetInt("Audio",0);
    }
    public static bool GetAudio()
    {
        return PlayerPrefs.GetInt("Audio",1) == 1;
    }

    public static void SetVibration(bool key)
    {
        if(key) PlayerPrefs.SetInt("Vibration",1);
        else PlayerPrefs.SetInt("Vibration",0);
    }
    
    public static bool GetVibration()
    {
        return PlayerPrefs.GetInt("Vibration",1) == 1;
    }

    #endregion
    
}
