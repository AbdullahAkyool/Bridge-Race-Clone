using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using TMPro;

public class VerticalAscendMultiplier : MonoBehaviour, IMultiplier
{
    [SerializeField] private List<Color> multiplierColors = new List<Color>();
    [SerializeField] private List<Transform> multipliers = new List<Transform>();
    [SerializeField] private float ascendTime = 2f;
    private int currentMultiplierIndex = 0;
    public float ascendHeight = 5f;
    Transform playerTransform;

    private void Start() {
        Finish.Instance.multiplier = this;
    }

    public void ActivateMultiplier(){
        playerTransform = Player.Instance.transform;
        playerTransform.DOMoveY(ascendHeight + playerTransform.position.y,ascendTime).SetEase(Ease.OutSine).OnUpdate(()=>{
            CheckMultipliers();
        }).OnComplete(()=>{
            GameManager.Instance.SuccessLevel(.3f);
        });
    }

    public void CheckMultipliers(){
        if(playerTransform.position.y>multipliers[currentMultiplierIndex].position.y){
            while(playerTransform.position.y>multipliers[currentMultiplierIndex].position.y){
                TriggerMultiplier(currentMultiplierIndex);
                currentMultiplierIndex++;
            }
        }
    }

    public void TriggerMultiplier(int index){
        multipliers[index].transform.DOPunchScale(Vector3.one * .4f,.5f,0,0);
        multipliers[index].GetComponent<MeshRenderer>().material.color = multiplierColors[index%multiplierColors.Count];
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        try{
            CurrencyManager.Instance.SetMultiplierAmount(1f + (float)index * .1f);
            UISuccessPanelCurrencyCount.Instance.AssignCurrencyAmountToDisplay();
        }
        catch{
            Debug.Log("You need to add a currency in order to make multipliers work");
        }
    }
}
