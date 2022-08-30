using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BoundChanger : MonoBehaviour, IInterractable
{
    [SerializeField] private Vector2 newBounds;
    [SerializeField] private float changeTime;
    private Collider col;
    private void Awake() {
        col = GetComponent<Collider>();
    }
    public void Interract(Transform interractor){
        Player.Instance.cc.ChangeBounds(newBounds,changeTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.right * newBounds.x,.3f);
        Gizmos.DrawSphere(transform.position + transform.right * newBounds.y,.3f);
        try{
            Gizmos.DrawWireCube(col.bounds.center,col.bounds.size);
        }
        catch{

        }
    }
}
