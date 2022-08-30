using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{

    [Header("Panels")]
    [SerializeField] private RectTransform _successPanel;
    [SerializeField] private RectTransform _failPanel;
    [SerializeField] private RectTransform _gamePanel;
    [SerializeField] private RectTransform _levelTransitionPanel;

    [Header("Settings")]
    [Range(0f, 2f)]
    [SerializeField] private float _panelOpeningDuration = .5f;

    [Header("TMP Fields")]
    [SerializeField] private TextMeshProUGUI levelText;
    

    private void Awake()
    {
        Init();
    }

    private void Init(){
        levelText.text = "LEVEL " + PlayerPrefs.GetInt("LevelNoText", 1).ToString();
        _levelTransitionPanel.GetComponent<Image>().color = new Color(0f,.5f,1f,1f);
        FadeOutLevelTransitionPanel();
    }

    

    
    #region Panel Operations
    public void OpenSuccessPanel(float delay = 0f)
    {
        StartCoroutine(OpenSuccessPanelCO(delay));

        IEnumerator OpenSuccessPanelCO(float delay){
            yield return new WaitForSeconds(delay);
            GameManager.Instance.OnSuccessPanelOpened.Invoke();
            Image panelImage = _successPanel.GetComponent<Image>();
            float currentTint = 0f;
            DOTween.To(()=> currentTint,x=> currentTint =x,.8f,.4f).OnUpdate(()=>{
                panelImage.color = new Color(1,1,1,currentTint);
            });
            _successPanel.GetChild(0).GetComponent<RectTransform>().DOAnchorPos3DY(-150f,.4f);
            _successPanel.GetChild(1).DOScale(Vector3.one,.4f).SetDelay(.4f);
            for(int i = 2;i<_successPanel.childCount;i++){
                _successPanel.GetChild(i).DOScale(Vector3.one,.4f);
            }
        }
    }

    public void OpenFailPanel(float delay = 0f)
    {
        StartCoroutine(OpenFailPanelCO(delay));

        IEnumerator OpenFailPanelCO(float delay){
            yield return new WaitForSeconds(delay);
            GameManager.Instance.OnFailPanelOpened.Invoke();
            Image panelImage = _failPanel.GetComponent<Image>();
            float currentTint = 0f;
            DOTween.To(()=> currentTint,x=> currentTint =x,.8f,.4f).OnUpdate(()=>{
                panelImage.color = new Color(1,1,1,currentTint);
            });
            _failPanel.GetChild(0).GetComponent<RectTransform>().DOAnchorPos3DY(-150f,.4f);
            _failPanel.GetChild(1).DOScale(Vector3.one,.4f).SetDelay(.4f);
            for(int i = 2;i<_failPanel.childCount;i++){
                _failPanel.GetChild(i).DOScale(Vector3.one,.4f);
            }
        }
    }
    


    public void OpenGamePanel(float delay = 0f)
    {
        OpenGamePanelCo(delay);

        IEnumerator OpenGamePanelCo(float delay)
        {
            yield return new WaitForSeconds(delay);
            Image panelImage = _gamePanel.GetComponent<Image>();
            float currentTint = 0f;
            DOTween.To(()=> currentTint,x=> currentTint =x,.8f,.4f).OnUpdate(()=>{
                panelImage.color = new Color(1,1,1,currentTint);
            });
            _gamePanel.GetChild(0).GetComponent<RectTransform>().DOAnchorPos3DY(-150f,.4f);
            _gamePanel.GetChild(1).DOScale(Vector3.one,.4f).SetDelay(.4f);
            for(int i = 2;i<_gamePanel.childCount;i++){
                _gamePanel.GetChild(i).DOScale(Vector3.one,.4f);
            }
        }
    }

    //
    public void FadeOutLevelTransitionPanel(){
        _levelTransitionPanel.GetComponent<Image>().DOColor(new Color(0f,.5f,1f,0f),.25f);
    }
    public void FadeInLevelTransitionPanel(){
        _levelTransitionPanel.GetComponent<Image>().DOColor(new Color(0f,.5f,1f,1f),.25f);
    }
    #endregion

}