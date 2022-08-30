using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;


public class ForwardStack : MonoSingleton<ForwardStack>
{
    [Header("StackSeetings")]
    [SerializeField] internal List<Transform> CollectedStackParts;
    [SerializeField] private int _stackEaseOffset = 5;
    [SerializeField] private float _spaceBetweenParts = 1f;
    [Range(.25f,1f)] [SerializeField] private float _smoothTime = .5f;

    private float _playerMaxHorizontalSpeed;
    private void Start() {
        _playerMaxHorizontalSpeed = LinearCharacterController.Instance.maxHorizontalSpeed  * _smoothTime;
    }
    private void LateUpdate() {
        HandleStackObjectPosses();
    }
    public void HandleStackObjectPosses(){
        int dnaCount = CollectedStackParts.Count;
        float playerPosX = LinearCharacterController.Instance.player.transform.position.x;
        float playerPosZ = LinearCharacterController.Instance.player.transform.position.z;
        for(int i = 0;i< dnaCount;i++){

            CollectedStackParts[i].position = Vector3.Lerp(CollectedStackParts[i].position,
            new Vector3(Mathf.Lerp(CollectedStackParts[i].position.x,playerPosX,Mathf.Pow(((float)((dnaCount-i)+_stackEaseOffset)/(float)(dnaCount+_stackEaseOffset)),3) * 
            _playerMaxHorizontalSpeed * Time.deltaTime)
            ,1f,playerPosZ + (i * _spaceBetweenParts)),((float)((dnaCount-i)+_stackEaseOffset)/(float)(dnaCount+_stackEaseOffset)) + .5f);

            CollectedStackParts[i].rotation = Quaternion.Euler(0f,0f,Time.time * -200f);
        }
    }

    

    public void AddPart(Transform part){
        part.gameObject.tag = "Part";
        CollectedStackParts.Add(part);
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        StartCoroutine(BlendPartIn_Co());
    }

    public void RemoveRest(Transform part){
        
        int index = CollectedStackParts.IndexOf(part);
        List<Transform> removedTransforms = new List<Transform>();
        MMVibrationManager.Haptic(HapticTypes.Failure);
        for(int i = index;i< CollectedStackParts.Count;i++){
            removedTransforms.Add(CollectedStackParts[i]);
        }
        CollectedStackParts.RemoveRange(index,CollectedStackParts.Count-index);
        foreach(Transform trns in removedTransforms){
            trns.DORotate(new Vector3(-90f,0f,0f),.8f);
            trns.DOJump(trns.position + new Vector3(Random.Range(-3f,3f),0f,Random.Range(20f,28f)),6f,1,.8f).SetEase(Ease.Linear).OnComplete(()=>{
                trns.tag = "Collectable Part";
            });
        }
        CheckFail();
    }

    IEnumerator BlendPartIn_Co(){
        for(int i = CollectedStackParts.Count - 1;i>=0;i--){
            CollectedStackParts[i].DOComplete();
            CollectedStackParts[i].DOPunchScale(Vector3.one,.2f);
            yield return new WaitForSeconds(.02f);
        }
    }

    public void RemovePart(Transform part)
    {
        CollectedStackParts.Remove(part);
        Destroy(part.gameObject);
        CheckFail();
    }

    private void CheckFail(){
        if(CollectedStackParts.Count == 0){
            LinearCharacterController.Instance.enabled = false;
            GameManager.Instance.FailLevel();
        }
    }
}
