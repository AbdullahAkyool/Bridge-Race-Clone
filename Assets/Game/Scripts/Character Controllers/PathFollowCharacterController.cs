using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

//You have to use bezier path package in order to use this character controller
//[RequireComponent(typeof(PathFollower))] 
[RequireComponent(typeof(SplineFollower))]
public class PathFollowCharacterController : MonoSingleton<PathFollowCharacterController>, ICharacterController
{
    [Header("Global Character Controller Settings")]
    public CCSettings CCSettings;
    public bool _useGlobalCCSettings;

    [Tooltip("Child transform to move on the X axis")] [SerializeField] private Transform player;
    [Tooltip("Transform of the Player model")] [SerializeField] private Transform playerModel;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 1f;
    [Tooltip("Max distance player can take per second (Vertical)")] [SerializeField] private float maxHorizontalSpeed;
    [Tooltip("Drag sensivity")] [SerializeField] private float dragMultiplier = 2f;
    [Tooltip("Allowed area for X axis movement (X: Min, Y: Max)")] [SerializeField] private Vector2 bounds;
    [Tooltip("Option for stop on input release")] [SerializeField] private bool stopMovementOnRelease = false;
    
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
    private OffsetModifier.Key offsetModifierKey;
    

    private bool isBouncingBack = false;

    public SplineFollower splineFollower;

    
    private void Start() {
        Init();
    }
    void Init() {
        
        splineFollower = GetComponent<SplineFollower>();
        playerCurrentPos = player.localPosition.x;
        mouseStartPos = 0.5f;
        playerHeight = player.localPosition.y;
        splineFollower.followSpeed = movementSpeed;
        initialMovementSpeed = movementSpeed;
        offsetModifierKey = splineFollower.offsetModifier.keys[0];

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

    void Update()
    {
        HandLeMovement();
    }
    
    void HandLeMovement(){

        if(Input.GetMouseButtonDown(0)){
            if(stopMovementOnRelease) StartMovement();
            playerCurrentPos = player.localPosition.x;
            mouseStartPos = Input.mousePosition.x / Screen.width;
        }

        if(Input.GetMouseButton(0)){
            mouseCurrentPos = Input.mousePosition.x / Screen.width;
            player.localPosition = Vector3.MoveTowards(player.localPosition,
            Vector3.up * playerHeight + Vector3.right * Mathf.Clamp((playerCurrentPos + (mouseCurrentPos - mouseStartPos) * dragMultiplier + xOffset),
            bounds.x,bounds.y),maxHorizontalSpeed * Time.deltaTime);
        }
        else{
            player.localPosition = Vector3.MoveTowards(player.localPosition,Vector3.right * Mathf.Clamp(player.localPosition.x,bounds.x,bounds.y) + Vector3.up * playerHeight,maxHorizontalSpeed * Time.deltaTime);
        }

    
        if(Input.GetMouseButtonUp(0)){
            if(stopMovementOnRelease)
                StopMovement();
        }

        HandleRotation();
    }

    void HandleRotation(){
        if(!rotateOnMovement) return;
        playerModel.localRotation = Quaternion.RotateTowards(playerModel.localRotation,Quaternion.Euler(playerModel.localRotation.eulerAngles.x,Mathf.Clamp((player.localPosition.x - previousXPos) * rotationAmount * Time.deltaTime,rotationBounds.x,rotationBounds.y),playerModel.localRotation.eulerAngles.z),maxRotationDelta * Time.deltaTime);
        previousXPos = player.localPosition.x;
    }

    public float GetXPos(){
        
        return player.localPosition.x;
    }

    public void BounceBack(float bounceLenght,float bounceHeight,float bounceTime){
        
        if(isBouncingBack) return;
        float initialPlayerHeight = playerHeight;
        isBouncingBack = true;
        float tempMovementSpeed = movementSpeed;
        splineFollower.direction = Spline.Direction.Backward;
        movementSpeed = bounceLenght/bounceTime;
        SetMovementSpeed(movementSpeed);
    
        float currentPlayerHeight = playerHeight;
        DOTween.To(()=>currentPlayerHeight,x=> currentPlayerHeight =x,bounceHeight,bounceTime/2f).SetEase(Ease.OutSine).OnUpdate(()=>{
            playerHeight = currentPlayerHeight;
        }).OnComplete(()=>{
            DOTween.To(()=>currentPlayerHeight,x=> currentPlayerHeight =x,initialPlayerHeight,bounceTime/2f).SetEase(Ease.InSine).OnUpdate(()=>{
                playerHeight = currentPlayerHeight;
            }).OnComplete(()=>{
                splineFollower.direction = Spline.Direction.Forward;
                movementSpeed = tempMovementSpeed;
                SetMovementSpeed(movementSpeed);
                isBouncingBack = false;
            });
        });
        
    }

    #region Movement Manipulation
    public void StartMovement() {
        splineFollower.followSpeed = movementSpeed;
    }
    public void StopMovement(){
        splineFollower.followSpeed = 0f;
    }
    public void SetMovementSpeed(float speed){
        splineFollower.followSpeed = speed;
    }
    #endregion


    public void ChangeBounds(Vector2 boundsNew,float changeTime = .2f){
        Vector2 currentBounds = bounds;
        float lenght = (boundsNew.y - boundsNew.x);
        Vector2 newBounds = new Vector3(-lenght/2f,lenght/2f);
        float center = (boundsNew.x + boundsNew.y) / 2f;

        float currentXOffset = splineFollower.offsetModifier.keys[0].offset.x;

        /* DOTween.To(()=> currentXOffset,x => currentXOffset = x,center,changeTime).OnUpdate(()=>{
            splineFollower.offsetModifier.keys[0].offset = new Vector2(currentXOffset,0f);
            xOffset = -currentXOffset;
        });
        transform.DOMoveX(center,changeTime).OnUpdate(()=>{
            xOffset = -transform.position.x;
        }); */
        DOTween.To(()=>currentBounds,x => currentBounds = x,boundsNew,changeTime).OnUpdate(()=>{
            bounds = currentBounds;
        });
    }

    public void ChangeFollowPath(SplineComputer newPath){
        Debug.Log(newPath.transform.name);
        splineFollower.spline = newPath;
        splineFollower.SetDistance(0f);
    }
    public void Disable(){
        movementSpeed = 0f;
        splineFollower.enabled = false;
        this.enabled = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + transform.right * bounds.x + transform.up * .2f,.3f);
        Gizmos.DrawSphere(transform.position + transform.right * bounds.y + transform.up * .2f,.3f);
    }

    
}

#if UNITY_EDITOR
[CustomEditor(typeof(PathFollowCharacterController))]
public class PathFollowCharacterController_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PathFollowCharacterController script = (PathFollowCharacterController)target;

        //Makes rotation Fields active on the Inspector if the "Rotate On Movement" is checked;
        if(script.rotateOnMovement){ 
            script.rotationBounds = EditorGUILayout.Vector2Field("Rotation Bounds",script.rotationBounds);
            script.rotationAmount = EditorGUILayout.FloatField("Rotation Amount",script.rotationAmount);
            script.maxRotationDelta = EditorGUILayout.FloatField("Max Rotation Delta",script.maxRotationDelta);
        }
    }
}
#endif