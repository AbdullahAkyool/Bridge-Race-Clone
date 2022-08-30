using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Threading.Tasks;

public class UserTracking : MonoSingleton<UserTracking>
{
    
    [HideInInspector] public UnityEvent OnCollidedWithObstacle;

    private void Awake() {
        GameManager.Instance.OnLevelStarted.AddListener(StartGettingXPos);
    }

    public float constantDataWriteInterval;
    private List<float> xPosses = new List<float>();
    public void StartGettingXPos(int level){        
        InvokeRepeating("GetPlayerXPos",0f,constantDataWriteInterval);
    }

    public void StopGettingXPos(){
        CancelInvoke("GetPlayerXPos");
    }
    
    void GetPlayerXPos(){
        xPosses.Add(Player.Instance.cc.GetXPos());
    }

    private void SendPlayerXPosses(){
        string filePath = "/Users/omerefeakbas/Desktop";

        
        using (StreamWriter writer = new StreamWriter(filePath + "/xposses.txt"))  
        {  
            writer.Write("");
            for(int i = 0;i<xPosses.Count;i++){
                writer.WriteLine(xPosses[i].ToString());
            }
        }  
    }

    private void OnApplicationQuit() {
        SendPlayerXPosses();
    }
}

