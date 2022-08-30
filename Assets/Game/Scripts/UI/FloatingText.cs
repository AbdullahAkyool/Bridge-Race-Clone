using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingText : MonoSingleton<FloatingText>
{
    [SerializeField] private GameObject collectingText;
    private GameObject currentCollectingText;
    private int lastValue = 0;
    // Start is called before the first frame update
    
    /// <summary> Creates a 2D floating text on GUI based on the given word position </summary>
    public void Launch(string text, Color color,Vector3 worldPos,bool reuse = false){
        StopAllCoroutines();
        StartCoroutine(Launch_Co(text,color,worldPos,reuse));
    }

    public void Launch(int text, Color color,Vector3 worldPos,bool reuse = false,string prefix = "",string postfix = ""){
        StopAllCoroutines();
        StartCoroutine(Launch_Co(text,color,worldPos,reuse,prefix,postfix));
    }

    IEnumerator Launch_Co(string text, Color color,Vector3 worldPos,bool reuse){
        
        if(reuse && currentCollectingText != null){
            currentCollectingText.transform.DOKill();
            currentCollectingText.transform.DOScale(Vector3.zero,.15f);
        }
        GameObject textObject = Instantiate(collectingText,Vector2.zero,Quaternion.identity,transform);
        TextMeshProUGUI textMesh = textObject.GetComponent<TextMeshProUGUI>();
        RectTransform rect = textObject.GetComponent<RectTransform>();
        currentCollectingText = textObject;


        Destroy(textObject,1f);

        rect.position = Camera.main.WorldToScreenPoint(worldPos);
        textMesh.color = color;
        textMesh.text = text;
        rect.DOScale(Vector3.one * 1.4f,.4f);
        rect.DOMoveY(rect.position.y + Screen.height/24f,.3f).SetEase(Ease.OutQuad).OnComplete(()=>{
            lastValue = 0;
            Color textColor = textMesh.color;
            textColor.a = 0f;
            textMesh.DOColor(textColor,.3f).SetDelay(.2f);
        });
        yield break;

    }

    IEnumerator Launch_Co(int text, Color color,Vector3 worldPos,bool reuse,string prefix = "",string postfix = ""){
        
        if(reuse && currentCollectingText != null){
            currentCollectingText.transform.DOKill();
            currentCollectingText.transform.DOScale(Vector3.zero,.5f);
        }
        GameObject textObject = Instantiate(collectingText,Vector2.zero,Quaternion.identity,transform);
        TextMeshProUGUI textMesh = textObject.GetComponent<TextMeshProUGUI>();
        RectTransform rect = textObject.GetComponent<RectTransform>();
        currentCollectingText = textObject;
        Destroy(textObject,1f);

        rect.position = Camera.main.WorldToScreenPoint(worldPos);
        textMesh.color = color;
        textMesh.text = prefix+(text + lastValue).ToString()+postfix;
        lastValue+=text;
        rect.DOScale(Vector3.one * 1.4f,.4f);
        rect.DOMoveY(rect.position.y + Screen.height/24f,.3f).SetEase(Ease.OutQuad).OnComplete(()=>{
            lastValue = 0;
            Color textColor = textMesh.color;
            textColor.a = 0f;
            textMesh.DOColor(textColor,.3f).SetDelay(.2f);
        });
        yield break;
    }
}
