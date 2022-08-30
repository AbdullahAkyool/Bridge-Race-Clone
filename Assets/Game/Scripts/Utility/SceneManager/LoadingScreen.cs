using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image loadingImage;

    private void Start()
    {
        //StartCoroutine(Load());
        StartCoroutine(FakeLoad());
    }


    private void Load()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync("Loader");
    }

    IEnumerator FakeLoad()
    {
        float first = Random.Range(0f, .5f);
        loadingImage.DOFillAmount(first, 1.5f);
        yield return new WaitForSeconds(1.5f);
        float second = Random.Range(first, 1f);
        loadingImage.DOFillAmount(second, 1.5f);
        yield return new WaitForSeconds(1.5f);
        loadingImage.DOFillAmount(1f, 1.5f);
        yield return new WaitForSeconds(2f);
        Load();
    }

}
