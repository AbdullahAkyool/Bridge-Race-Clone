using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackable : MonoBehaviour, IInterractable
{
    public void Interract(Transform interractor){
        
        GetComponent<Collider>().enabled = false;
        interractor.GetComponentInChildren<VerticalStack>().AddToStack(transform);
    }
}
