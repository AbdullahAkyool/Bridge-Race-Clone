using UnityEngine;

public class RotationalCharacterController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Forward movement speed")] [SerializeField] private float movementSpeed = 3f;
    [Tooltip("Drag sensivity")] [SerializeField] private float dragMultiplier = 90f;
    [Tooltip("Opion for stop on input release")] [SerializeField] private bool stopMovementOnRelease = false;
    
    private float mouseStartPos;
    private float mouseCurrentPos;
    private float playerCurrentRot;

    private void Start() {
        Init();
    }

    void Update()
    {
        HandLeMovement();
    }
    
    void HandLeMovement(){

        if(Input.GetMouseButtonDown(0)){
            playerCurrentRot = transform.localRotation.eulerAngles.y;
            mouseStartPos = Input.mousePosition.x / Screen.width;
        }

        if(Input.GetMouseButton(0)){
            mouseCurrentPos = Input.mousePosition.x / Screen.width;
            transform.localRotation = Quaternion.Euler(0f,playerCurrentRot + (mouseCurrentPos - mouseStartPos) * dragMultiplier,0f);

            
            if(stopMovementOnRelease)
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }

        if(!stopMovementOnRelease){
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
    }

    void Init() {
        playerCurrentRot = transform.localRotation.eulerAngles.y;
        mouseStartPos = Input.mousePosition.x / Screen.width;
    }
}
