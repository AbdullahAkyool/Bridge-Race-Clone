using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CubeColor
{
    Red,
    Green,
    Blue
}

public class Cube : MonoBehaviour
{

    CubeSpawn CubeSpawn;
    public CubeColor cubeColor;

    public bool isCollected = false;

    public void Collect()   //k�p�n yerden al�nmas� sonucunda �al��acak metod
    {
        isCollected = true; 
        gameObject.GetComponent<BoxCollider>().enabled = false; //al�nan k�p�n collider ve meshrenderer componentleri kapat�l�yor
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(SpawnAgain());
    }



    public IEnumerator SpawnAgain()     //al�nan k�p�n tekrardan spawn olabilmesi i�in bir coroutine metodu �a�r�l�yor
    {
        yield return new WaitForSeconds(4f);
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        isCollected = false;
    }
}

