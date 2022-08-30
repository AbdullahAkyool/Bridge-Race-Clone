using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TopDownCharacterController : MonoBehaviour
{
    [SerializeField] private Transform directionIndicator;
    [SerializeField] private float movementSpeed = 3f;

    public GameObject cubeMethod;
    private Rigidbody _rb;
    private FloatingJoystick _joystick;
    private Transform _transform;
    private Vector3 _targetPos;
    public bool IsMoving;

    public StackManager stackmanager;
    public StairManager stairmanager;
    public CubeSpawn cubespawn;

    Animator anim;
    public GameObject characterModel;
    public Color CharacterColor;


    void Start()
    {
        Init();
        characterModel.GetComponent<SkinnedMeshRenderer>().material.color = CharacterColor;

    }

    private void Init(){
        _rb = GetComponent<Rigidbody>();
        _joystick = FindObjectOfType<FloatingJoystick>().GetComponent<FloatingJoystick>();
        _transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    public void SetSpeed(float newSpeed){
        movementSpeed = newSpeed;
    }
    
    void Update()
    {
        float magnitude = new Vector2(_joystick.Direction.x,_joystick.Direction.y).magnitude;
        float magnitudeSpeed = magnitude * movementSpeed;
        directionIndicator.position = _transform.position + new Vector3(_joystick.Direction.x,-.49f,_joystick.Direction.y) * 2f;
        
        if(Input.GetMouseButton(0))
        {
            Vector3 indicatorPos = directionIndicator.position;
            
            _targetPos = Vector3.MoveTowards(_targetPos,
                new Vector3(indicatorPos.x, transform.position.y, indicatorPos.z), Time.deltaTime * movementSpeed);
            
            _transform.LookAt(_targetPos);
            _rb.velocity = _transform.forward * magnitudeSpeed;
            anim.SetBool("isRunning", true);
        }
        else{
            _rb.velocity = Vector3.zero;
            anim.SetBool("isRunning", false);
        }
    }

    public void DisableController(){
        _rb.velocity = Vector3.zero;
        enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == stackmanager.renk)  //stackmanager'den gelen random renk stringine göre player'ýn gireceði trigger belirleniyor
        {
            stackmanager.CollectBlock(other.gameObject); //trigger esnasýnda stackmanager'den CollectBlock metodu ile küp toplanýyor
        }
        else if (other.tag == "stair")
        {
            stairmanager.ChangeStairColor(other.gameObject,stackmanager);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
         if(collision.gameObject.tag == "barrier")
        {
            stairmanager.OpenBarrier(collision.gameObject, stackmanager);
        }    
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Finish")
        {
            stackmanager.DropBlock(); //trigger nesnesi Finish objesi ise DropBlock metodu ile stacklenen küpler boþaltýlýyor
        }
        
    }



}
