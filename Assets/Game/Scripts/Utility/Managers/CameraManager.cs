using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private Transform virtualCamerasParent;
    private List<Cinemachine.CinemachineVirtualCamera> vcams = new List<CinemachineVirtualCamera>();
    private Cinemachine.CinemachineVirtualCamera activeVirtualCam;

    private void Start() {
        Init();
    }
    
    private void Init(){
        vcams = virtualCamerasParent.GetComponentsInChildren<Cinemachine.CinemachineVirtualCamera>(true).ToList<Cinemachine.CinemachineVirtualCamera>();
        activeVirtualCam = vcams[0];
    }

    #region Cam Selection

    public void ActivateVirtualCamera(int index){
        
        if(index >= vcams.Count){
            Debug.LogError("There are " + vcams.Count + " virtualcam(s) in the scene. The index you gave (" + index.ToString() + ") is not valid! Please give an index between 0-"+(vcams.Count-1).ToString());
            return;
        }
        foreach(Cinemachine.CinemachineVirtualCamera vcam in vcams){
            vcam.gameObject.SetActive(false);
        }
        vcams[index].gameObject.SetActive(true);
        activeVirtualCam = vcams[index];
    }

    public Cinemachine.CinemachineVirtualCamera GetActiveVirtualCamera(){
        if(vcams.Count == 0){
            vcams = virtualCamerasParent.GetComponentsInChildren<Cinemachine.CinemachineVirtualCamera>(true).ToList<Cinemachine.CinemachineVirtualCamera>();
            activeVirtualCam = vcams[0];
        }
        return activeVirtualCam;
    }

    #endregion

    #region Cam Operations
    public void ShakeCamera(float duration = .4f,float amount = 3f)
    {   
        CinemachineBasicMultiChannelPerlin noise = GetActiveVirtualCamera().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        float remainingamount = amount;
        DOTween.To(() => remainingamount, x => remainingamount = x,0f,duration).OnUpdate(() => {
            noise.m_AmplitudeGain = remainingamount;
        });
    }

    public void TranslateCameraBodyFollowOffset(Vector3 targetOffset, float duration = .5f){
        Cinemachine.CinemachineTransposer transposer = GetActiveVirtualCamera().GetCinemachineComponent<CinemachineTransposer>();
        Vector3 currentOffset = transposer.m_FollowOffset;
        DOTween.To(() => currentOffset, x => currentOffset = x,targetOffset, duration).OnUpdate(()=>{
            transposer.m_FollowOffset = currentOffset;
        });
    }

    public void TranslateCameraLookFollowOffset(Vector3 targetOffset, float duration = .5f){
        Cinemachine.CinemachineComposer composer = GetActiveVirtualCamera().GetCinemachineComponent<CinemachineComposer>();
        Vector3 currentOffset = composer.m_TrackedObjectOffset;
        DOTween.To(() => currentOffset, x => currentOffset = x,targetOffset, duration).OnUpdate(()=>{
            composer.m_TrackedObjectOffset = currentOffset;
        });
    }

    public void TranslateCamera(Vector3 followOffset, Vector3 lookOffset, float duration){
        TranslateCameraBodyFollowOffset(followOffset,duration);
        TranslateCameraLookFollowOffset(lookOffset,duration);
    }
    #endregion
    

    
    public void CreateVcam(){
        Instantiate(virtualCamerasParent.GetChild(0).gameObject,virtualCamerasParent.GetChild(0).position,Quaternion.identity,virtualCamerasParent);
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CameraManager script = (CameraManager)target;

        if(GUILayout.Button("Add vcam")){
            script.CreateVcam();
        }
    }
}
#endif
