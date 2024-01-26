using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    public StackManager stackmanager;
    public StairManager stairmanager;
    Collider[] searchZone;
    Vector3 cubePos;
    public LayerMask katman;

    public CubeColor aiColor;
    public GameObject characterModel;
    public Color CharacterColor;

    private bool hasTarget = false;
    private bool isGoingNextArea = false;
    private Cube targetCube;

    private int kopruKod;
    public Transform[] Bridges;
    public Transform targetBridge;
    public Transform baseArea;
    public Transform baseArea2;

    public GameObject trash;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        FindCube();
        characterModel.GetComponent<SkinnedMeshRenderer>().material.color = CharacterColor;
        RandomBridgeSelect();
    }

    public void RandomBridgeSelect()
    {
        kopruKod = Random.Range(0, 3);
        if (kopruKod == 0) { targetBridge = Bridges[0]; }
        else if (kopruKod == 1) { targetBridge = Bridges[1]; }
        else if (kopruKod == 2) { targetBridge = Bridges[2]; }
    }

    public void FindTarget()
    {
        if (stackmanager.IndexBlock <= 0)
        {
            hasTarget = false;
            agent.SetDestination(baseArea.position);
            
        }
        else if (stairmanager.stairIndex == stairmanager.stairMeshRenderers.Count)
        { 
            isGoingNextArea = true;
            hasTarget = false;
            agent.SetDestination(trash.transform.position);
        }
        // else if(stackmanager.IndexBlock >= stackmanager.StackLimit && isGoingNextArea)
        // {
        //     hasTarget = true;
        //     anim.SetBool("isRunning",false);
        // }
    }

    public void FindCube()
    {
        if (stackmanager.IndexBlock >= stackmanager.StackLimit)
        {
            isGoingNextArea = true;
            agent.SetDestination(targetBridge.position);
        }
        else
        {
            if(hasTarget){return;}
            searchZone = Physics.OverlapSphere(transform.position,20f,katman);
            Collider cubeCollider = searchZone[Random.Range(0, searchZone.Length)];
            targetCube = cubeCollider.gameObject.GetComponent<Cube>();
            cubePos = targetCube.transform.position;
            agent.SetDestination(cubePos);
            hasTarget = true;
        }
        
        anim.SetBool("isRunning", true);
    }

    void CollectCube(Cube cube)
    {
        stackmanager.CollectBlock(cube.gameObject);
     
    }

    private void Update()
    {
        CheckPosition();
        FindTarget();
    }

    void CheckPosition()
    {
        FindCube();

        if (Vector3.Distance(agent.destination, agent.transform.position) <= 1f)
        {
            hasTarget = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cube>(out Cube cube))
        {
            if(cube.cubeColor == aiColor)
            {
                CollectCube(cube);
            }               
        }
        else if (other.CompareTag("stair"))
        {
            stairmanager.ChangeStairColor(other.gameObject,stackmanager);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("barrier"))
        {
            stairmanager.OpenBarrier(collision.gameObject, stackmanager);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            stackmanager.DropBlock(); //trigger nesnesi Finish objesi ise DropBlock metodu ile stacklenen k�pler bo�alt�l�yor
        }

    }
}
