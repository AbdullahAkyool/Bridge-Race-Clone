using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEditor;

public class SceneController : MonoSingleton<SceneController>
{
    internal int levelNo;
    internal int levelNoText;
    private int uniqueLevelCount;
    [SerializeField] private float sceneTransitionTime = 1f;
    [SerializeField] private RectTransform crossFadePanel;

    public int GetUniqueLevelCount(){
        int levelCount = 0;
        List<string> sceneNames = new List<string>();
        foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if(scene.enabled)
            {
                sceneNames.Add(scene.path.Replace("Assets/Game/Scenes/","").Replace(".unity",""));
            }
        }
        foreach(string sceneName in sceneNames){
            int number;
            if(int.TryParse(sceneName,out number)){
                levelCount++;
            }
        }
        return levelCount;
    }
    

    private void Awake()
    {
        levelNo = PlayerPrefs.GetInt("LevelNo", 1);
        levelNoText = PlayerPrefs.GetInt("LevelNoText", 1);
        uniqueLevelCount = GetUniqueLevelCount();
    }


    public void NextScene(float delay)
    {
        StartCoroutine(NextSceneCo(delay));
    }

    IEnumerator NextSceneCo(float delay){
        yield return new WaitForSeconds(delay);
        UIManager.Instance.FadeInLevelTransitionPanel();
        yield return new WaitForSeconds(.25f);
        PlayerPrefs.SetInt("LevelNoText", levelNoText + 1);
        if (levelNo == uniqueLevelCount)
            levelNo = 0;
        levelNo++;
        PlayerPrefs.SetInt("LevelNo", levelNo);
        SceneManager.LoadScene("Loader");
    }

    public void RetryScene()
    {
        SceneManager.LoadScene("Loader");
    }








    private void SmoothTransitionIn()
    {
        //burada level kapanırken smooth bir sahne kapanış olacak
        //Init
        //crossFadePanel.GetComponent<Image>().color.a = 0F;
        crossFadePanel.localScale = new Vector3(13F,13F,13F);
        //Process
        crossFadePanel.DOScale(Vector3.one,sceneTransitionTime);
    }

    private void SmoothTransitionOut()
    {
        // burada level yüklenirken smooth bir sahne açılış olacak
        //transparency --
        //panel ++
        crossFadePanel.localScale = new Vector3(1F,1F,1F);
        crossFadePanel.DOScale(new Vector3(13F,13F,13F),sceneTransitionTime);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SmoothTransitionIn();
            
        }
    }
}
