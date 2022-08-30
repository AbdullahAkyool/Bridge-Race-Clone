using UnityEngine;

public class CurrencyManager : MonoSingleton<CurrencyManager>
{
    private int currencyCount;
    private int temporaryCollectedCount;
    private float multiplierAmount = 1f;

    private void Awake() {
        Init();
    }
    private void Start() {
        GameManager.Instance.OnSuccessPanelOpened.AddListener(AwardPlayerTheCollectedTemporaryCurrency); // Can be changed to -> onlevelcompleted
    }

    private void Init(){
        currencyCount = PlayerPrefs.GetInt("CurrencyCount",1100);
    }

    #region Main Currency
    
    /// <summary> Returns the current currency amount </summary>
    public int GetCurrencyCount(){
        return currencyCount;
    }

    /// <summary> Sets the current currency amount to the given amount</summary>
    public void SetCurrencyCount(int count){
        currencyCount = count;
        PlayerPrefs.SetInt("CurrencyCount",currencyCount);
    }

    /// <summary> Adds the given amount to the current currency amount </summary>
    public void AddCurrencyCount(int count){
        currencyCount += count;
        PlayerPrefs.SetInt("CurrencyCount",currencyCount);
    }

    /// <summary> Decereases the given amount from the current currency amount </summary>
    public void DecreaseCurrencyCount(int count){
        currencyCount -= count;
        PlayerPrefs.SetInt("CurrencyCount",currencyCount);
    }

    
    
    #endregion

    #region Temporary Currency

    /// <summary> Returns the current temporary currency amount </summary>
    public int GetTemporaryCurrencyCount(){
        return temporaryCollectedCount;
    }

    /// <summary> Sets the current temporary currency amount to the given amount</summary>
    public void SetTemporaryCurrencyCount(int count){
        temporaryCollectedCount = count;
    }

    /// <summary> Registers the collected currency on the level which is not yet awarded to the player <code> (Ex: If the player fails the level, registered level currency will be lost) </code> </summary>
    public void AddTemporaryCurrency(int count){
        temporaryCollectedCount += count;
        GameManager.Instance.OnTemporaryCurrencyCollected.Invoke();

        //!!!! If this fuction called after OnGameFinishedEvent added temporary currenct will not be registered!!!!
    }

    /// <summary> Decereases the given amout from the Registered collected currency on the level which is not yet awarded to the player <code> (Ex: If the player fails the level, registered level currency will be lost) </code> </summary>
    public void DecreaseTemporaryCurrency(int count){
        temporaryCollectedCount -= count;
    }

    #endregion

    /// <summary> Transfers collected temporary currency to gained currency</summary>
    private void AwardPlayerTheCollectedTemporaryCurrency(){
        AddCurrencyCount((int)(temporaryCollectedCount * multiplierAmount));
        temporaryCollectedCount = 0;
    }

    /// <summary> Sets the multiplier amount for the awarded currency on finish</summary>
    public void SetMultiplierAmount(float amount){
        multiplierAmount = amount;
    }

    /// <summary> Returns the multiplier amount for the awarded currency on finish</summary>
    public float GetMultiplierAmount(){
        return multiplierAmount;
    }
}
