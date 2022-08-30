using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIComponentBounceAnimation : MonoBehaviour
{
    private enum BounceSpeed{
        Slow,
        Normal,
        Fast
    }
    private enum BounceSize{
        Narrow,
        Normal,
        Wide
    }
    private float bounceInterval = .2f;
    private float bounceSizeValue = 1.1f;
    [SerializeField] BounceSpeed bounceSpeed;
    [SerializeField] BounceSize bounceSize;

    

    private void Start() {
        if(bounceSpeed == BounceSpeed.Fast){
            bounceInterval = .2f;
        }
        else if(bounceSpeed == BounceSpeed.Normal){
            bounceInterval = .4f;
        }
        else{
            bounceInterval = .6f;
        }

        if(bounceSize == BounceSize.Narrow){
            bounceSizeValue = 1.1f;
        }
        else if(bounceSize == BounceSize.Normal){
            bounceSizeValue = 1.25f;
        }
        else{
            bounceSizeValue = 1.4f;
        }

        StartCoroutine(BounceAnimation_Co());
    }



    IEnumerator BounceAnimation_Co(){
        transform.DOScale(Vector3.one * bounceSizeValue,bounceInterval);
        yield return new WaitForSeconds(bounceInterval);
        transform.DOScale(Vector3.one,bounceInterval);
        yield return new WaitForSeconds(bounceInterval);
        StartCoroutine(BounceAnimation_Co());
    }
}
