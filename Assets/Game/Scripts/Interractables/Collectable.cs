using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, IInterractable
{
    public void Interract(Transform interractor){
        GetComponent<Collider>().enabled = false;
        Haptics.Soft();
        Destroy(gameObject);
        FloatingText.Instance.Launch(1,Color.green,transform.position,true,"$");
        UICollectable.Instance.Launch(transform.position,1);
    }
}
