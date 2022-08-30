using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class IncrementalManager : MonoSingleton<IncrementalManager>
{
    [Header("1st Incremental")]
    [SerializeField] private Button _firstIncrementalButton;
    [SerializeField] private TextMeshProUGUI _firstIncrementalPriceText;
    [SerializeField] private TextMeshProUGUI  _firstIncrementalLevelText;
    [SerializeField] private Sprite _firstIncremnetalButtonActiveSprite;
    [SerializeField] private int[] _firstIncrementalPrices;
    private int _firstIncrementalLevel;


    [Header("2nd Incremental")]
    [SerializeField] private Button _secondIncrementalButton;
    [SerializeField] private TextMeshProUGUI _secondIncrementalPriceText;
    [SerializeField] private TextMeshProUGUI  _secondIncrementalLevelText;
    [SerializeField] private Sprite _secondIncremnetalButtonActiveSprite;
    [SerializeField] private int[] _secondIncrementalPrices;
    private int _secondIncrementalLevel; 

    [Header("Other Settings")]
    [SerializeField] private Sprite _deactivatedButtonActiveSprite;

    private void Start() {
        Init();
    }

    private void Init(){
        if(SceneController.Instance.levelNoText == 1) transform.localScale = Vector3.zero;

        _firstIncrementalLevel = PlayerPrefs.GetInt("FirstIncrementalLevel",0);
        _secondIncrementalLevel = PlayerPrefs.GetInt("SecondIncrementalLevel",0);

        _firstIncrementalPriceText.text = "$" + _firstIncrementalPrices[_firstIncrementalLevel].ToString();
        _secondIncrementalPriceText.text = "$" + _secondIncrementalPrices[_secondIncrementalLevel].ToString();

        _firstIncrementalLevelText.text = (_firstIncrementalLevel+1).ToString() + "\nLvl";
        _secondIncrementalLevelText.text = (_secondIncrementalLevel+1).ToString() + "\nLvl";

        HandleAffordability();
    }

    public void UpgradeFirstIncremental(){

        if(CurrencyManager.Instance.GetCurrencyCount()<_firstIncrementalPrices[_firstIncrementalLevel]){
            HandleAffordability();
            return;
        } 
        
        CurrencyManager.Instance.AddCurrencyCount(-_firstIncrementalPrices[_firstIncrementalLevel]);
        UICollectable.Instance.UpdateCurrencyText();

        _firstIncrementalButton.transform.DOComplete();
        _firstIncrementalButton.transform.DOPunchScale(Vector3.one * .1f,.2f,0,0f);

        _firstIncrementalLevel++;
        _firstIncrementalPriceText.text = "$" + _firstIncrementalPrices[_firstIncrementalLevel].ToString();
        _firstIncrementalLevelText.text = (_firstIncrementalLevel+1).ToString() + "\nLvl";
        PlayerPrefs.SetInt("FirstIncrementalLevel",_firstIncrementalLevel);

        Haptics.Medium();
        HandleAffordability();
    }

    public void UpgradeSecondIncremental(){

        if(CurrencyManager.Instance.GetCurrencyCount()<_secondIncrementalPrices[_secondIncrementalLevel]){
            HandleAffordability();
            return;
        } 

        CurrencyManager.Instance.AddCurrencyCount(- _secondIncrementalPrices[_secondIncrementalLevel]);
        UICollectable.Instance.UpdateCurrencyText();

        _secondIncrementalButton.transform.DOComplete();
        _secondIncrementalButton.transform.DOPunchScale(Vector3.one * .1f,.2f,0,0f);


        _secondIncrementalLevel++;
        _secondIncrementalPriceText.text = "$" + _secondIncrementalPrices[_secondIncrementalLevel].ToString();
        _secondIncrementalLevelText.text = (_secondIncrementalLevel+1).ToString() + "\nLvl";
        PlayerPrefs.SetInt("SecondIncrementalLevel",_secondIncrementalLevel);

        Haptics.Medium();
        HandleAffordability();
    }

    public void HandleAffordability(){
        //Check for both incremental prices
        if(CurrencyManager.Instance.GetCurrencyCount() <_firstIncrementalPrices[_firstIncrementalLevel]){
            _firstIncrementalButton.interactable = false;
            _firstIncrementalButton.image.sprite = _deactivatedButtonActiveSprite;
        }
        if(CurrencyManager.Instance.GetCurrencyCount() <_secondIncrementalPrices[_secondIncrementalLevel]){
            _secondIncrementalButton.interactable = false;
            _secondIncrementalButton.image.sprite = _deactivatedButtonActiveSprite;
        }

        if(CurrencyManager.Instance.GetCurrencyCount()<0){
            CurrencyManager.Instance.SetCurrencyCount(0);
            UICollectable.Instance.UpdateCurrencyText();
        }

        if(_firstIncrementalLevel>=_firstIncrementalPrices.Length-1){
            _firstIncrementalButton.interactable = false;
            _firstIncrementalButton.image.sprite = _deactivatedButtonActiveSprite;
            _firstIncrementalPriceText.text = "MAX";
        }

        if(_secondIncrementalLevel>=_secondIncrementalPrices.Length-1){
            _secondIncrementalButton.interactable = false;
            _secondIncrementalButton.image.sprite = _deactivatedButtonActiveSprite;
            _secondIncrementalPriceText.text = "MAX";
        }
    }

    
    
}
