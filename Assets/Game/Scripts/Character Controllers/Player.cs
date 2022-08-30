using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoSingleton<Player>
{
    [HideInInspector] public ICharacterController cc;

    private void Start() {
        cc = transform.parent.GetComponent<ICharacterController>();
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("trigger");
        if(other.TryGetComponent<IInterractable>(out IInterractable ii)){
            ii.Interract(transform);
        }
    }
}
