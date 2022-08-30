using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class barrier : MonoBehaviour
{
    NavMeshObstacle navmeshobs;

    private void Start()
    {
        navmeshobs = GetComponent<NavMeshObstacle>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log(collision.gameObject.name);
    }
}
