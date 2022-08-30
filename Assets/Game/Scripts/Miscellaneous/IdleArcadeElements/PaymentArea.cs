using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PaymentArea : MonoBehaviour, IInterractable
{
    [SerializeField] private int _price;
    [SerializeField] private Image _fillImage;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private UnlockableField _unlockableField;

    private int _moneyAmountLeftToUnlock;
    private int _actualMoneyAmountLeft;
    private float _lastDepositTime = 0f;
    

    [SerializeField] private GameObject _moneyObject;

    void Start()
    {
        _moneyAmountLeftToUnlock = _price;
        _actualMoneyAmountLeft = _price;
        _moneyText.text = "$" + _price.ToStringShortenedNumber();
        _unlockableField = transform.parent.GetComponentInChildren<UnlockableField>();
        
    }

    public void Interract(Transform interractor){
        
        if(Time.time - _lastDepositTime < .07f || _actualMoneyAmountLeft <= 0 || CurrencyManager.Instance.GetCurrencyCount() <=0) return;
        
        _lastDepositTime = Time.time;
        int moneyToDeposit = _price/20;;

        moneyToDeposit = Mathf.Clamp(moneyToDeposit,1,_actualMoneyAmountLeft);
        Debug.Log("After Actual Money:" + moneyToDeposit);
        moneyToDeposit = Mathf.Clamp(moneyToDeposit,1,CurrencyManager.Instance.GetCurrencyCount());
        Debug.Log("After Currency Money:" + moneyToDeposit);

        _actualMoneyAmountLeft -= moneyToDeposit;


        AudioManager.Instance.Play("Pop",volume: .4f,pitch: .8f + (float)(_price - _actualMoneyAmountLeft) / (float)_price);
        Haptics.Light();

        CurrencyManager.Instance.AddCurrencyCount(-moneyToDeposit);
        GameObject money = Instantiate(_moneyObject,Player.Instance.transform.position,Quaternion.Euler(-90f,Random.Range(0f,360f),0f));

        money.transform.localScale = Vector3.zero;
        money.transform.DOScale(Vector3.one,.3f).SetEase(Ease.Linear);
        money.transform.DORotate(new Vector3(-90f,0f,0f),.65f,RotateMode.Fast).SetEase(Ease.Linear);
        money.transform.DOJump(transform.position,3.5f,1,.65f).SetEase(Ease.Linear).OnComplete(()=>{
            Destroy(money);
            
            AddMoneyToPayment(moneyToDeposit);
        });
    }

    private void AddMoneyToPayment(int addedMoney = 1){
        _moneyAmountLeftToUnlock -= addedMoney;
        Debug.Log(addedMoney);
        _moneyText.text = "$" + _moneyAmountLeftToUnlock.ToStringShortenedNumber();
        _fillImage.fillAmount = (float)(_price - _moneyAmountLeftToUnlock) / (float)_price;
        CheckPaymentCompleted();
    }

    private void CheckPaymentCompleted(){
        if(_moneyAmountLeftToUnlock<=0){
            transform.DOScale(Vector3.zero,.3f).SetEase(Ease.InSine);
            _unlockableField.Unlock();
        }
    }
}
