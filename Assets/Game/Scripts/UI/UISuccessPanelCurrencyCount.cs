using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class UISuccessPanelCurrencyCount : MonoSingleton<UISuccessPanelCurrencyCount>
{
    [SerializeField] private AnimationCurve currecyScaleCurve;
    [SerializeField] private RawImage currencyCountImage;
    [SerializeField] private TextMeshProUGUI currencyCountText;
    [Tooltip("You can use this field to display a symbol before the currency count (like $)")] [SerializeField] private string currencyTextPrefix = "";
    private int currencyAmountToDisplay = 0;

    private void Start() {
        GameManager.Instance.OnGameFinished.AddListener(AssignCurrencyAmountToDisplay);
        GameManager.Instance.OnSuccessPanelOpened.AddListener(AssignCurrencyCount);
    }

    public void AssignCurrencyCountTexture(Texture currencyCountTexture){
        currencyCountImage.texture = currencyCountTexture;
    }   

    public void AssignCurrencyCount(){
        StartCoroutine(AssignCurrencyCount_Co());
    }

    IEnumerator AssignCurrencyCount_Co(){
        yield return new WaitForSeconds(.2f);
        int displayedCurrency = 0;
        transform.DOShakePosition(.8f,10,25);
        transform.DOScale(Vector3.one * .7f,.8f);
        DOTween.To(()=>displayedCurrency,x=> displayedCurrency = x,currencyAmountToDisplay,.8f).OnUpdate(()=>{
            currencyCountText.text = currencyTextPrefix + displayedCurrency;
        }).OnComplete(()=>{
            transform.DOScale(Vector3.one * 1.2f,.4f).SetEase(currecyScaleCurve).OnComplete(()=>{
                MMVibrationManager.Haptic(HapticTypes.Success);
            });
        });
        
    }
    public void AssignCurrencyAmountToDisplay(){
        currencyAmountToDisplay = (int)(CurrencyManager.Instance.GetTemporaryCurrencyCount() * CurrencyManager.Instance.GetMultiplierAmount());
    }
}
 