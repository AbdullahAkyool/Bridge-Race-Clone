using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairManager : MonoBehaviour
{
    public Transform StairParent;  //basamaklar�n bulunaca�� parent stair nesnesi tan�mlan�yor
    public List<MeshRenderer> stairMeshRenderers;
    public int stairIndex = 0;
    void Start()
    {
        InitializeStairList();
    }
    void InitializeStairList()  //basamak objeleri allStairs listine ekleniyor
    {
        foreach (Transform stairTransform in StairParent)
        {
            if(stairTransform.TryGetComponent(out MeshRenderer stairMeshR))
            {
                stairMeshRenderers.Add(stairMeshR);
                stairTransform.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        
    }


    public void OpenBarrier(GameObject stairObject, StackManager stackManager) //basamaklar� a�ma metodu
    {
        if (stackManager.IndexBlock == 0) return;   //Stacklenen k�p kalmad�ysa d�ng�den ��k�l�yor
       // if (!stackmanager.DropBlock()) return; //stackmanager'deki DropBlock metodu false d�nyor ise metoddan ��k�l�yor
       
        stairObject.GetComponent<BoxCollider>().enabled = false;
       // if (stairObject.GetComponent<MeshRenderer>().material.color != stackmanager.childBlock.GetComponent<MeshRenderer>().material.color) stairObject.GetComponent<BoxCollider>().enabled = true;
    }
    public void ChangeStairColor(GameObject stairObject,StackManager stackManager)
    {
        if (stairObject.GetComponent<MeshRenderer>().material.color == stackManager.childBlock.GetComponent<MeshRenderer>().material.color) return;
        if (!stackManager.DropBlock()) return;
        stackManager.DropBlock();
        stairObject.GetComponent<MeshRenderer>().material.color = stackManager.childBlock.GetComponent<MeshRenderer>().material.color;
        stairObject.GetComponent<MeshRenderer>().enabled = true;
        stairIndex++;
    }
}
