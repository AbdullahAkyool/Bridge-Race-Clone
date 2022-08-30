using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Vector3 moveAxis;
    void Update()
    {
        transform.position += moveAxis * Time.deltaTime;
    }
}
