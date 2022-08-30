using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;
    private Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //_transform.LookAt(cam);
        _transform.rotation = cam.rotation;
    }
}
