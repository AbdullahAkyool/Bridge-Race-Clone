using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

[RequireComponent(typeof(CurrencyManager))]
public class UICollectable : MonoSingleton<UICollectable>
{
    public GameObject gold;
    public RectTransform goldImage;
    private RectTransform rectTransform;

    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private string _textPrefix = "";


    private void Start() {
        _goldText.text = _textPrefix + CurrencyManager.Instance.GetCurrencyCount().ToStringShortenedNumber("0.0");
        rectTransform = GetComponent<RectTransform>();
        GameManager.Instance.OnSuccessPanelOpened.AddListener(RepositionCurrencyCountArea);
    }

    /// <summary> Creates a 2D currency sprite on UI based on the given world position and moves it to the upper-right corner, where the currency count is displayed </summary>
    public void Launch(Vector3 worldPos,int collectedAmount = 1){

        RectTransform uiCollectableRectTransform = UICollectable.Instance.GetComponent<RectTransform>();
        GameObject moneyObject = Instantiate(gold,UISuccessPanelCurrencyCount.Instance.transform.position,Quaternion.identity,UIManager.Instance.transform.GetChild(3));
        RectTransform rt = moneyObject.GetComponent<RectTransform>();
        rt.position = Camera.main.WorldToScreenPoint(worldPos);
        rt.DOAnchorPos(rt.anchoredPosition + new Vector2(Random.Range(-100f,100f),Random.Range(-100f,100f)),Random.Range(.3f,.45f)).SetEase(Ease.OutQuad).OnComplete(()=>{

            float travelTime = Random.Range(.45f,.7f);
            rt.DOPunchScale(Vector3.one * .7f,travelTime,0,0f);
            rt.DOMove(uiCollectableRectTransform.position,travelTime).SetEase(Ease.InQuad).OnComplete(()=>{
                Destroy(rt.gameObject);
                goldImage.DOComplete();
                goldImage.localScale = Vector3.one * 1.35f;
                goldImage.DOScale(Vector3.one,.2f);
                CurrencyManager.Instance.AddTemporaryCurrency(collectedAmount);
                _goldText.text = _textPrefix + (CurrencyManager.Instance.GetCurrencyCount() + CurrencyManager.Instance.GetTemporaryCurrencyCount()).ToStringShortenedNumber("0.0");
            });
        });
    }


    /// <summary> Creates a few currecy sprites and drags them to upper-right corner </summary>
    public void LaunchClaimMoneyAnimation(){

        int moneyCount = 16;
        Haptics.Success();

        List<RectTransform> transforms = new List<RectTransform>();
        RectTransform uiSuccessRectTransform = UISuccessPanelCurrencyCount.Instance.GetComponent<RectTransform>();
        RectTransform uiCollectableRectTransform = UICollectable.Instance.GetComponent<RectTransform>();

        for(int i = 0;i<moneyCount;i++){
            GameObject moneyObject = Instantiate(gold,UISuccessPanelCurrencyCount.Instance.transform.position,Quaternion.identity,UISuccessPanelCurrencyCount.Instance.transform);
            RectTransform rt = moneyObject.GetComponent<RectTransform>();
            rt.anchoredPosition = uiSuccessRectTransform.anchoredPosition;
            transforms.Add(rt);
            rt.DOAnchorPos(rt.anchoredPosition + new Vector2(Random.Range(-195f,195f),Random.Range(-195f,195f)),Random.Range(.4f,.9f)).SetEase(Ease.OutQuad).SetDelay(Random.Range(0f,.35f)).OnComplete(()=>{
                float travelTime = Random.Range(.5f,1f);
                rt.DOPunchScale(Vector3.one * .7f,travelTime,0,0f);
                rt.DOMove(uiCollectableRectTransform.position,travelTime).SetEase(Ease.InQuad).OnComplete(()=>{
                    Haptics.Soft();
                    Destroy(rt.gameObject);
                    goldImage.DOComplete();
                    goldImage.localScale = Vector3.one * 1.35f;
                    goldImage.DOScale(Vector3.one,.2f);
                });
            });
        }

    }
    

    public void UpdateCurrencyText(){
        transform.DOComplete();
        transform.DOPunchScale(Vector3.one * .15f,.3f,0,0f);
        _goldText.text = _textPrefix + CurrencyManager.Instance.GetCurrencyCount().ToStringShortenedNumber("0.0");
    }


    public void RepositionCurrencyCountArea(){
        rectTransform.DOAnchorPos(new Vector2(-100f,-340f),.33f);
    }
    public void ChangeTextPrefix(string newTextPrefix){
        _textPrefix = newTextPrefix;
    }
}
