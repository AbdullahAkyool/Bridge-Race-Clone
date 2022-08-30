
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        int levelNo = PlayerPrefs.GetInt("LevelNo", 1);
        string loadingScene = levelNo.ToString();
        SceneManager.LoadScene(loadingScene);
    }

}
