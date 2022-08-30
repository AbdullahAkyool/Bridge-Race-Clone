using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class SettingsPanel : MonoBehaviour
{
    
    [SerializeField] private Button _hapticsButton;
    [SerializeField] private Image _hapticsIcon;
    [SerializeField] private Sprite _hapticsOnSprite;
    [SerializeField] private Sprite _hapticsOffSprite;

    [SerializeField] private Button _soundButton;
    [SerializeField] private Image _soundIcon;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;

    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inactiveSprite;

    [SerializeField] private Slider _qualitySlider;

    private bool _isHapticsOn = true;
    private bool _isSoundOn = true;
    private int _qualityLevel = 3;
    private bool _isInitialized = false;
    [SerializeField] RectTransform[] _qualityLevelTexts;

    // Start is called before the first frame update
    void Start()
    {
        _isHapticsOn = DataManager.GetVibration();
        _isSoundOn = DataManager.GetAudio();
        _qualityLevel = PlayerPrefs.GetInt("QualityLevel",1);
        InitPanel();
    }

    private void InitPanel(){
        if(!_isHapticsOn){
            _hapticsButton.image.sprite = _inactiveSprite;
            _hapticsIcon.sprite = _hapticsOffSprite;
        }
        if(!_isSoundOn){
            _soundButton.image.sprite = _inactiveSprite;
            _soundIcon.sprite = _soundOffSprite;
        }
        _qualitySlider.value = PlayerPrefs.GetInt("QualityLevel",3) - 1;
        _isInitialized = true;
    }

    public void ToggleHaptics(){
        if(_isHapticsOn){
            _isHapticsOn = false;
            _hapticsButton.image.sprite = _inactiveSprite;
            _hapticsIcon.sprite = _hapticsOffSprite;
            DataManager.SetVibration(false);
        }
        else{
            _isHapticsOn = true;
            _hapticsButton.image.sprite = _activeSprite;
            _hapticsIcon.sprite = _hapticsOnSprite;
            DataManager.SetVibration(true);
        }
        _hapticsButton.DOComplete();
        _hapticsButton.transform.DOPunchScale(Vector3.one * .15f,.15f);
        AudioManager.Instance.Play("Pop_2",volume: .4f,pitch:.5f);
        Haptics.Medium();
    }

    public void ToggleSound(){
        if(_isSoundOn){
            _isSoundOn = false;
            _soundButton.image.sprite = _inactiveSprite;
            _soundIcon.sprite = _soundOffSprite;
            PlayerPrefs.SetInt("AudioState",0);
            DataManager.SetAudio(false);
        }
        else{
            _isSoundOn = true;
            _soundButton.image.sprite = _activeSprite;
            _soundIcon.sprite = _soundOnSprite;
            PlayerPrefs.SetInt("AudioState",1);
            DataManager.SetAudio(true);
        }
        _soundButton.DOComplete();
        _soundButton.transform.DOPunchScale(Vector3.one * .15f,.15f);
        AudioManager.Instance.Play("Pop_2",volume: .4f,pitch:.5f);
        Haptics.Medium();
    }

    public void ClosePanel(){
        transform.DOScale(Vector3.zero,.2f);
    }
    public void OpenPanel(){
        transform.DOScale(Vector3.one,.2f);
    }

    public void HandleQualityChange(){
        foreach(RectTransform rt in _qualityLevelTexts){
            rt.DOScale(Vector3.one,.2f);
        }
        if(_qualitySlider.value == 0){
            QualitySettings.SetQualityLevel(1);
            _qualityLevelTexts[0].DOComplete();
            _qualityLevelTexts[0].DOScale(Vector3.one * 1.85f,.2f);
            PlayerPrefs.SetInt("QualityLevel",1);
        }
        else if(_qualitySlider.value == 1){
            QualitySettings.SetQualityLevel(2);
            _qualityLevelTexts[1].DOComplete();
            _qualityLevelTexts[1].DOScale(Vector3.one * 1.85f,.2f);
            PlayerPrefs.SetInt("QualityLevel",2);
        }
        else{
            QualitySettings.SetQualityLevel(3);
            _qualityLevelTexts[2].DOComplete();
            _qualityLevelTexts[2].DOScale(Vector3.one * 1.85f,.2f);
            PlayerPrefs.SetInt("QualityLevel",3);
        }
        if(_isInitialized){
            AudioManager.Instance.Play("Pop_2",volume: .4f,pitch:1f);
            Haptics.Medium();
        }
        
    }
}
