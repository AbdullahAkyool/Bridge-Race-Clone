using UnityEngine;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LinearCharacterController : MonoSingleton<LinearCharacterController> , ICharacterController
{
    [Header("Global Character Controller Settings")]
    public CCSettings CCSettings;
    public bool _useGlobalCCSettings;
    
    [Tooltip("Child transform to move on the X axis")] public Transform player;
    [Tooltip("Transform of the Player model")] [SerializeField] private Transform playerModel;

    [Header("Movement")]
    [Tooltip("Forward movement speed")] public float movementSpeed = 1f;
    [Tooltip("Max distance player can take per second (Vertical)")] public float maxHorizontalSpeed;
    [Tooltip("Drag sensivity")] [SerializeField] private float dragMultiplier = 2f;
    [Tooltip("Option for stop on input release")] [SerializeField] private bool stopMovementOnRelease = false;
    [Tooltip("Allowed area for X axis movement (X: Min, Y: Max)")] [SerializeField] private Vector2 bounds;
    
    [Header("Rotation")]
    [Tooltip("Rotates characte on movement if activated")] public bool rotateOnMovement = false;
    [HideInInspector] public float rotationAmount = 3000f;
    [HideInInspector] public float maxRotationDelta = 200f;

    [Tooltip("Max rotation bounds (Degrees)")] [HideInInspector] public Vector2 rotationBounds = new Vector2(-45f,45f);
    
    private float mouseStartPos;
    private float xOffset = 0f;
    private float mouseCurrentPos = 0.5f;
    private float playerCurrentPos;
    private float previousXPos = 0f;
    private float playerHeight = 0f;
    private float initialMovementSpeed;

    //Check bools
    private bool isBouncingBack = false;

    private void Start() {
        Init();
    }
    void LateUpdate()
    {
        HandLeMovement();
        
    }
    void HandLeMovement(){
        if(Input.GetMouseButtonDown(0)){
            playerCurrentPos = player.localPosition.x;
            mouseStartPos = Input.mousePosition.x / Screen.width;
        }
        if(Input.GetMouseButton(0)){
            mouseCurrentPos = Input.mousePosition.x / Screen.width;
            if(stopMovementOnRelease)
                transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
        }
        player.localPosition = Vector3.MoveTowards(player.localPosition,
        Vector3.up * playerHeight + Vector3.right * Mathf.Clamp((playerCurrentPos + (mouseCurrentPos - mouseStartPos) * dragMultiplier + xOffset),
        bounds.x,bounds.y),maxHorizontalSpeed * Time.deltaTime);

        if(!stopMovementOnRelease){
            transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
        }
        HandleRotation();
    }

    void HandleRotation(){
        if(!rotateOnMovement) return;
        playerModel.localRotation = Quaternion.RotateTowards(playerModel.localRotation,Quaternion.Euler(playerModel.localRotation.eulerAngles.x,
        Mathf.Clamp((player.localPosition.x - previousXPos) * rotationAmount * Time.deltaTime,rotationBounds.x,rotationBounds.y),
        playerModel.localRotation.eulerAngles.z),maxRotationDelta * Time.deltaTime);
        previousXPos = player.localPosition.x;
    }

    public float GetXPos(){
        Debug.Log(player.localPosition.x);
        return player.localPosition.x;
    }

    public void ChangeBounds(float center, float lenght,float changeTime = .2f){
        Vector2 currentBounds = bounds;
        Vector2 newBounds = new Vector3(-lenght/2f,lenght/2f);
        transform.DOMoveX(center,changeTime).OnUpdate(()=>{
            xOffset = -transform.position.x;
        });
        DOTween.To(()=>currentBounds,x => currentBounds = x,newBounds,changeTime).OnUpdate(()=>{
            bounds = currentBounds;
        });
    }
    public void ChangeBounds(Vector2 boundsNew,float changeTime = .2f){
        Vector2 currentBounds = bounds;
        float lenght = (boundsNew.y - boundsNew.x);
        Vector2 newBounds = new Vector3(-lenght/2f,lenght/2f);
        float center = (boundsNew.x + boundsNew.y) / 2f;
        transform.DOMoveX(center,changeTime).OnUpdate(()=>{
            xOffset = -transform.position.x;
        });
        DOTween.To(()=>currentBounds,x => currentBounds = x,newBounds,changeTime).OnUpdate(()=>{
            bounds = currentBounds;
        });
    }
    public void ChangePlayerHeight(float newHeight,float changeTime){
        float currentHeight = playerHeight;
        DOTween.To(()=>currentHeight,x => currentHeight = x,newHeight,changeTime).SetEase(Ease.Linear).OnUpdate(()=>{
            playerHeight = currentHeight;
        });
    }

    public void BounceBack(float bounceLenght,float bounceHeight,float bounceTime){
        if(isBouncingBack) return;
        isBouncingBack = true;
        float tempMovementSpeed = movementSpeed;
        movementSpeed = 0f;
        transform.DOJump(transform.position + Vector3.back * bounceLenght,bounceHeight,1,bounceTime).OnComplete(()=>{
            movementSpeed = tempMovementSpeed;
            isBouncingBack = false;
        });
    }

    void Init() {

        playerCurrentPos = player.localPosition.x;
        mouseStartPos = 0.5f;
        playerHeight = player.localPosition.y;
        initialMovementSpeed = movementSpeed;
        
        if(_useGlobalCCSettings){
            movementSpeed = CCSettings.MovementSpeed;
            maxHorizontalSpeed = CCSettings.MaxHorizontalSpeed;
            dragMultiplier = CCSettings.DragMultiplier;
            stopMovementOnRelease = CCSettings.StopMovementOnRelease;
            rotateOnMovement = CCSettings.RotateOnMovement;
            rotationAmount = CCSettings.RotationAmount;
            maxRotationDelta = CCSettings.MaxRotationDelta;
        }
    }

    public void StopMovement(){
        movementSpeed = 0f;
    }
    public void StartMovement(){
        movementSpeed = initialMovementSpeed;
    }
    public void Disable(){
        movementSpeed = 0f;
        this.enabled = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(bounds.x + transform.position.x,player.position.y+1,transform.position.z),new Vector3(.2f,1f,2f));
        Gizmos.DrawWireCube(new Vector3(bounds.y + transform.position.x,player.position.y+1,transform.position.z),new Vector3(.2f,1f,2f));
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(LinearCharacterController))]
public class LinearCharacterController_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LinearCharacterController script = (LinearCharacterController)target;

        //Makes rotation Fields active on the Inspector if the "Rotate On Movement" is checked.
        if(script.rotateOnMovement){ 
            script.rotationBounds = EditorGUILayout.Vector2Field("Rotation Bounds",script.rotationBounds);
            script.rotationAmount = EditorGUILayout.FloatField("Rotation Amount",script.rotationAmount);
            script.maxRotationDelta = EditorGUILayout.FloatField("Max Rotation Delta",script.maxRotationDelta);
        }
    }
}
#endif
