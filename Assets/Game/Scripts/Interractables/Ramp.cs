using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour, IInterractable
{
    public float rampTargetHeight;
    public float rampClimbTime;
    public Vector2 rampTargetBounds;
    
    private Collider col;
    private void Start() {
        col = GetComponent<Collider>();
    }

    public void Interract(Transform interractor){
        Debug.Log("WOOOOOOOOOW");
        LinearCharacterController.Instance.ChangeBounds(rampTargetBounds,.4f);
        LinearCharacterController.Instance.ChangePlayerHeight(rampTargetHeight,rampClimbTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x,0,transform.position.z)
         + (Vector3.up * rampTargetHeight) + (LinearCharacterController.Instance.movementSpeed
          * rampClimbTime)*Vector3.forward/2f,new Vector3(transform.localScale.x,.2f,
          LinearCharacterController.Instance.movementSpeed * rampClimbTime));
        
        Gizmos.color = Color.green;

        Gizmos.DrawCube(new Vector3(rampTargetBounds.x,1f,transform.position.z), new Vector3(.2f,1f,1f));
        Gizmos.DrawCube(new Vector3(rampTargetBounds.y,1f,transform.position.z), new Vector3(.2f,1f,1f));
        Gizmos.DrawWireCube(new Vector3((rampTargetBounds.y+rampTargetBounds.x)/2f,rampTargetHeight,transform.position.z),new Vector3(rampTargetBounds.y-rampTargetBounds.x,6f,0f));
    }
}
