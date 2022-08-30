using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController
{
    void StopMovement();
    void StartMovement();
    void BounceBack(float bounceLenght,float bounceHeight,float bounceTime);
    void ChangeBounds(Vector2 newBounds,float changeTime);
    float GetXPos();
    void Disable();
}
