using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicMover : MonoBehaviour
{
    [SerializeField] private Vector3 axis;
    [SerializeField] private float periodSpeed;
    private Vector3 ofset;
    private RectTransform rectTransform;

    private void Start() {
        ofset = transform.position;
    }

    private void Update() {
        transform.position = ofset + Mathf.Sin(Time.time * periodSpeed) * axis;
    }
}

