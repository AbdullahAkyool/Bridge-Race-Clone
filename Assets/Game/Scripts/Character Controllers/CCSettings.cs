using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CCSettings", menuName = "ScriptableObjects/CreateCharacterControllerSettings", order = 1)]

public class CCSettings : ScriptableObject
{
    [Header("Global Character Controller Settings")]
    public float MovementSpeed;
    public float MaxHorizontalSpeed;
    public float DragMultiplier;
    public bool StopMovementOnRelease;
    public bool RotateOnMovement;
    public float RotationAmount;
    public float MaxRotationDelta;
}
