using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class UIFillable : MonoSingleton<UIFillable>
{
    [System.Serializable] 
    public struct FillableData {
        public Sprite fillableImage;
        public int neededFillCount;
    }

    private int progressionCount;
    private int currentFillableDataIndex;
    

    [SerializeField] private List<FillableData> fillableDatas = new List<FillableData>();
    [SerializeField] private Image blackPreviewImage;
    [SerializeField] private Image fillableImage;
    [SerializeField] private Transform percentageArea;
    
    [SerializeField] private TextMeshProUGUI percentageText;
    private void Start() {
        progressionCount = PlayerPrefs.GetInt("FillableProgressionCount",0);
        currentFillableDataIndex = PlayerPrefs.GetInt("CurrentFillableDataIndex",0);
        blackPreviewImage.sprite = fillableDatas[currentFillableDataIndex].fillableImage;
        blackPreviewImage.color = Color.black;
        fillableImage.sprite = fillableDatas[currentFillableDataIndex].fillableImage;
        fillableImage.fillAmount = progressionCount/(float)fillableDatas[currentFillableDataIndex].neededFillCount;
        percentageText.text = ((progressionCount/(float)fillableDatas[currentFillableDataIndex].neededFillCount) * 100).ToString("0") +"%";
    }

    public void AddProgression(int progression = 1){
        StartCoroutine(AddProgression_Co(progression));
    }

    IEnumerator AddProgression_Co(int progression){
        float currentFillCountFloat = (float)progressionCount;
        progressionCount += progression;
        
        PlayerPrefs.SetInt("FillableProgressionCount",progressionCount);

        Haptics.Continuous(.5f,0f,.6f);

        DOTween.To(() => currentFillCountFloat, x => currentFillCountFloat = x, progressionCount,.6f).OnUpdate(() => {
            fillableImage.fillAmount = currentFillCountFloat/(float)fillableDatas[currentFillableDataIndex].neededFillCount;
            percentageText.text = ((currentFillCountFloat/(float)fillableDatas[currentFillableDataIndex].neededFillCount) * 100).ToString("0") +"%";
        });
        

        yield return new WaitForSeconds(.6f);
        if(progressionCount == fillableDatas[currentFillableDataIndex].neededFillCount){
            CompleteFillable();
        }
    }

    private void CompleteFillable(){

        GameManager.Instance.OnUIFillableCompleted.Invoke(currentFillableDataIndex);

        if(currentFillableDataIndex == fillableDatas.Count-1){
            PlayerPrefs.SetInt("CurrentFillableDataIndex",0);
        }
        else{
            PlayerPrefs.SetInt("CurrentFillableDataIndex",currentFillableDataIndex + 1);
        }

        PlayerPrefs.SetInt("FillableProgressionCount",0);


        blackPreviewImage.transform.DOScale(Vector3.one * 1.1f,.6f);
        fillableImage.transform.DOScale(Vector3.one * 1.1f,.6f);
        percentageArea.DOScale(Vector3.zero,.6f);

        
    }
}