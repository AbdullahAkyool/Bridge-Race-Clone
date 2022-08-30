using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VerticalStack : MonoBehaviour
{
    [SerializeField] private float _heightPerStack;
    [SerializeField] private Vector3 _rotationOffset;
    [SerializeField] private int _maxStackCount = 0;
    private int _currentStackCount = 0;
    private Transform _owner;
    private Stack<Transform> _stackTransforms = new Stack<Transform>();
    private bool _isLocked = false;

    public void ChangeMaxStackCount(int newMaxStackCount){
        _maxStackCount = newMaxStackCount;
        _owner = transform.parent;
    }
    
    public void AddToStack(Transform transformToAdd){
        if(_currentStackCount == _maxStackCount){
            Debug.Log("Stack is <b><color=red>FULL</color></b>");
            return;
        }
        transformToAdd.GetComponent<Collider>().enabled = false;

        if(_owner.TryGetComponent<Player>(out Player player)){
            Haptics.Light();
        }
        
        _currentStackCount++;
        _stackTransforms.Push(transformToAdd);
        transformToAdd.parent = transform;
        transformToAdd.DOLocalJump(Vector3.up * _heightPerStack * _currentStackCount,_currentStackCount * _heightPerStack/2f + .5f,1,.4f).SetEase(Ease.Linear).OnComplete(()=>{
            transformToAdd.DOPunchScale(new Vector3(.7f,-.3f,.7f),.3f,0,0f);
            Destroy(transformToAdd.GetComponent<Collider>());
        });
        transformToAdd.DOLocalRotate(_rotationOffset,.3f,RotateMode.Fast).SetEase(Ease.Linear);
    }

    public void RemoveFromStack(Vector3 landTarget){
        if(_currentStackCount == 0 || _isLocked){
            return;
        }
        _currentStackCount--;
        Transform transformToRemove = _stackTransforms.Pop();
        transformToRemove.parent = null;
        transformToRemove.DOJump(landTarget,.5f,1,.4f).SetEase(Ease.Linear);
        transformToRemove.DOLocalRotate(_rotationOffset,.3f,RotateMode.Fast).SetEase(Ease.Linear);
    }

    public Transform TakeFromStack(){
        if(_currentStackCount == 0 || _isLocked){
            Debug.Log("Stack is <b><color=yellow>EMPTY</color></b>");
            return null;
        }
        _currentStackCount--;
        Transform transformToRemove = _stackTransforms.Pop();
        return transformToRemove;
    }

    public int GetStackCount(){
        return _currentStackCount;
    }

    public void LockStack(){
        _isLocked = true;
    }

    public void UnlockStack(){
        _isLocked = false;
    }

    

}
