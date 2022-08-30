using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis;
    void Update()
    {
        transform.Rotate(rotationAxis * Time.deltaTime,Space.Self);
    }
}
