using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class GaminicsMenu : Editor
{   
    #region Collectable Currency Methods
    private static GameObject UICollectableObject;
    [MenuItem("Gaminics/Create/UI Component/UI Collectible/Diamond")]
    private static void CreateUICollectibleDiamond(){
        UICollectableObject = Instantiate(Resources.Load<GameObject>("UICollectableObject"),Vector3.zero,Quaternion.identity,FindObjectOfType<UIManager>().transform.GetChild(3));
        UICollectableObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-100f,-115f,0f);
        UICollectableObject.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("CollectableIcons/Diamond");
        UICollectableObject.GetComponent<UICollectable>().gold.GetComponent<RawImage>().texture = Resources.Load<Sprite>("CollectableIcons/Diamond").texture;
        Instantiate(Resources.Load<GameObject>("CollectablePrefabs/Diamond"),new Vector3(0f,1f,5f),Quaternion.Euler(90f,0f,0f));
        GameObject UICountArea = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/CurrencyCountArea.prefab"),Vector3.zero,Quaternion.identity,FindObjectOfType<UIManager>().transform.GetChild(1));
        UICountArea.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        UICountArea.GetComponent<UISuccessPanelCurrencyCount>().AssignCurrencyCountTexture(Resources.Load<Sprite>("CollectableIcons/Diamond").texture);
        UICountArea.transform.localScale = Vector3.zero;
    }
    [MenuItem("Gaminics/Create/UI Component/UI Collectible/Gold")]
    private static void CreateUICollectibleGold(){
        UICollectableObject = Instantiate(Resources.Load<GameObject>("UICollectableObject"),Vector3.zero,Quaternion.identity,FindObjectOfType<UIManager>().transform.GetChild(3));
        UICollectableObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-100f,-115f,0f);
        UICollectableObject.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("CollectableIcons/Gold");
        UICollectableObject.GetComponent<UICollectable>().gold.GetComponent<RawImage>().texture = Resources.Load<Sprite>("CollectableIcons/Gold").texture;
        Instantiate(Resources.Load<GameObject>("CollectablePrefabs/Coin"),new Vector3(0f,1f,5f),Quaternion.identity);
        GameObject UICountArea = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/CurrencyCountArea.prefab"),Vector3.zero,Quaternion.identity,FindObjectOfType<UIManager>().transform.GetChild(1));
        UICountArea.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        UICountArea.GetComponent<UISuccessPanelCurrencyCount>().AssignCurrencyCountTexture(Resources.Load<Sprite>("CollectableIcons/Gold").texture);
        UICountArea.transform.localScale = Vector3.zero;
    }
    [MenuItem("Gaminics/Create/UI Component/UI Collectible/Ruby")]
    private static void CreateUICollectibleRuby(){
        UICollectableObject = Instantiate(Resources.Load<GameObject>("UICollectableObject"),Vector3.zero,Quaternion.identity,FindObjectOfType<UIManager>().transform.GetChild(3));
        UICollectableObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-100f,-115f,0f);
        UICollectableObject.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("CollectableIcons/Ruby");
        UICollectableObject.GetComponent<UICollectable>().gold.GetComponent<RawImage>().texture = Resources.Load<Sprite>("CollectableIcons/Ruby").texture;
        Instantiate(Resources.Load<GameObject>("CollectablePrefabs/Ruby"),new Vector3(0f,1f,5f),Quaternion.Euler(90f,0f,0f));
        GameObject UICountArea = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/CurrencyCountArea.prefab"),Vector3.zero,Quaternion.identity,FindObjectOfType<UIManager>().transform.GetChild(1));
        UICountArea.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        UICountArea.GetComponent<UISuccessPanelCurrencyCount>().AssignCurrencyCountTexture(Resources.Load<Sprite>("CollectableIcons/Ruby").texture);
        UICountArea.transform.localScale = Vector3.zero;
    }
    [MenuItem("Gaminics/Create/UI Component/UI Collectible/Money")]
    private static void CreateUICollectibleMoney(){
        UICollectableObject = Instantiate(Resources.Load<GameObject>("UICollectableObject"),Vector3.zero,Quaternion.identity,FindObjectOfType<UIManager>().transform.GetChild(3));
        UICollectableObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-100f,-115f,0f);
        UICollectableObject.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("CollectableIcons/Money");
        UICollectableObject.GetComponent<UICollectable>().gold.GetComponent<RawImage>().texture = Resources.Load<Sprite>("CollectableIcons/Money").texture;
        Instantiate(Resources.Load<GameObject>("CollectablePrefabs/Money"),new Vector3(0f,1f,5f),Quaternion.Euler(90f,0f,0f));
        GameObject UICountArea = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/CurrencyCountArea.prefab"),Vector3.zero,Quaternion.identity,FindObjectOfType<UIManager>().transform.GetChild(1));
        UICountArea.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        UICountArea.GetComponent<UISuccessPanelCurrencyCount>().AssignCurrencyCountTexture(Resources.Load<Sprite>("CollectableIcons/Money").texture);
        UICountArea.transform.localScale = Vector3.zero;
        UICollectableObject.GetComponent<UICollectable>().ChangeTextPrefix("$");
    }
    #endregion
    #region UI
    [MenuItem("Gaminics/Create/UI Component/Temporary UI Text/Tap To Start")]
    private static void CreateTapToStartText(){
        UICollectableObject = Instantiate(Resources.Load<GameObject>("TapToStartText"),Vector3.zero,Quaternion.identity,FindObjectOfType<FloatingText>().transform);
        UICollectableObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f,300f,0f);
    }
    [MenuItem("Gaminics/Create/UI Component/Temporary UI Text/Drag To Play")]
    private static void CreateDragToPlayText(){
        GameObject UIFillableObject = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/DragToPlay.prefab"),Vector3.zero,Quaternion.identity,FindObjectOfType<FloatingText>().transform);
        UICollectableObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f,300f,0f);
    }
    [MenuItem("Gaminics/Create/UI Component/UI Fillable")]
    private static void CreateUIFillable(){
        GameObject UIFillableObject = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/UI Fillable.prefab"),Vector3.zero,Quaternion.identity,FindObjectOfType<UIManager>().transform.GetChild(1));
        UIFillableObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
    [MenuItem("Gaminics/Create/UI Component/IncrementalTemplate/Double")]
    private static void CreateUIIncrementaltemplate()
    {
        GameObject incrementaltemplate = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/IncrementalManager.prefab"),Vector3.zero,Quaternion.Euler(0f,0f,0f),FindObjectOfType<UIManager>().transform.GetChild(0));
        incrementaltemplate.GetComponent<RectTransform>().anchoredPosition3D = Vector3.up * 250f;
    }
    #endregion
    #region Runner Templates
    [MenuItem("Gaminics/Create/RunnerTemplate/Linear")]
    private static void CreateLinearRunnerTemplate()
    {
        GameObject cc = Instantiate(Resources.Load<GameObject>("CharacterControllers/Character Controller (Linear)"),Vector3.zero,Quaternion.Euler(0f,0f,0f));
        GameObject road = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/Roads/StraightRoad.prefab"),Vector3.zero,Quaternion.identity);
        road.transform.position = Vector3.down * 1.5f;
        CameraManager.Instance.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().m_Follow = cc.transform.GetChild(0);
        CameraManager.Instance.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().m_LookAt = cc.transform.GetChild(0);
    }
    [MenuItem("Gaminics/Create/RunnerTemplate/Spline/Straight")]
    private static void CreateSplineRunnerTemplateStraight()
    {
        GameObject cc = Instantiate(Resources.Load<GameObject>("CharacterControllers/Character Controller (Path)"),Vector3.zero,Quaternion.Euler(0f,0f,0f));
        GameObject road = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/Roads/SplineRoadStraight.prefab"),Vector3.zero,Quaternion.identity);
        cc.GetComponent<Dreamteck.Splines.SplineFollower>().spline = road.GetComponent<Dreamteck.Splines.SplineComputer>();
        cc.GetComponent<Dreamteck.Splines.SplineFollower>().startPosition = .05f;
        CameraManager.Instance.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().m_Follow = cc.transform.GetChild(0);
        CameraManager.Instance.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().m_LookAt = cc.transform.GetChild(0);
    }
    [MenuItem("Gaminics/Create/RunnerTemplate/Spline/LeftTurnRightTurn")]
    private static void CreateSplineRunnerTemplateLTRT()
    {
        GameObject cc = Instantiate(Resources.Load<GameObject>("CharacterControllers/Character Controller (Path)"),Vector3.zero,Quaternion.Euler(0f,0f,0f));
        GameObject road = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/Roads/SplineRoadLR.prefab"),Vector3.zero,Quaternion.identity);
        cc.GetComponent<Dreamteck.Splines.SplineFollower>().spline = road.GetComponent<Dreamteck.Splines.SplineComputer>();
        cc.GetComponent<Dreamteck.Splines.SplineFollower>().startPosition = .05f;
        CameraManager.Instance.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().m_Follow = cc.transform.GetChild(0);
        CameraManager.Instance.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().m_LookAt = cc.transform.GetChild(0);
    }
    #endregion
    #region TopDown Templates
    [MenuItem("Gaminics/Create/TopDown Template")]
    private static void CreateTopDownTemplate()
    {
        GameObject cc = Instantiate(Resources.Load<GameObject>("CharacterControllers/Character Controller (TopDown)"),Vector3.zero,Quaternion.Euler(0f,0f,0f));
        Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/Floating Joystick.prefab"),Vector3.zero,Quaternion.identity,FindObjectOfType<FloatingText>().transform);
        Cinemachine.CinemachineVirtualCamera vcam = CameraManager.Instance.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
        vcam.m_Follow = cc.transform;
        vcam.m_LookAt = cc.transform;
        vcam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_BindingMode = Cinemachine.CinemachineTransposer.BindingMode.WorldSpace;
        FindObjectOfType<Cinemachine.CinemachineBrain>().m_UpdateMethod = Cinemachine.CinemachineBrain.UpdateMethod.FixedUpdate;
    }
    #endregion
    #region FinishMultipliers
    [MenuItem("Gaminics/Create/Multiplier/VerticalAscend")]
    private static void CreateVerticalAscendMultiplier(){
        if(Finish.Instance == null){
            Debug.LogError("There is no FINISH object to put multiplier on, please make sure you have one before you create a multiplier");
            return;
        }
        GameObject multiplier = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gaminics/Reusable Assets/Multipliers/VerticalAscendMultiplier.prefab"),Vector3.zero,Quaternion.identity,FindObjectOfType<Finish>().transform);
        multiplier.transform.localPosition = new Vector3(0f,0f,8.87f);

    }
    #endregion
}

