using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffScreenObjectIndicator : MonoBehaviour
{
    private Image indicator;
    
    private RectTransform indicatorTransform;
    [SerializeField] private Sprite indicatorSprite;
    private Camera _camera;
    private Renderer _meshRenderer;

    private Vector2 screenSize;

    private bool isSeen;
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<Renderer>();
        _camera = Camera.main;
        screenSize = new Vector2(Screen.width, Screen.height);
        
        GameObject imageObject = new GameObject();
        imageObject.AddComponent<Image>();
        imageObject.transform.parent = FindObjectOfType<UIManager>().transform.GetChild(0);
        
        indicator = imageObject.GetComponent<Image>();
        indicator.sprite = indicatorSprite;
        indicatorTransform = indicator.GetComponent<RectTransform>();
        indicatorTransform.anchorMin = Vector2.zero;
        indicatorTransform.anchorMax = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_meshRenderer.isVisible)
        {
            indicator.gameObject.SetActive(true);
            Vector2 viewportPoint = _camera.WorldToViewportPoint(transform.position);
            viewportPoint.x = Mathf.Clamp(viewportPoint.x, .05f, .95f);
            viewportPoint.y = Mathf.Clamp(viewportPoint.y, .03f, .97f);
            indicatorTransform.anchoredPosition = new Vector2(viewportPoint.x * screenSize.x, viewportPoint.y * screenSize.y);
            
        }
        else
        {
            indicator.gameObject.SetActive(false);
            isSeen = true;
        }
    }
}
