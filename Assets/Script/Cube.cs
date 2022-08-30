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

    public void Collect()   //küpün yerden alýnmasý sonucunda çalýþacak metod
    {
        isCollected = true; 
        gameObject.GetComponent<BoxCollider>().enabled = false; //alýnan küpün collider ve meshrenderer componentleri kapatýlýyor
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(SpawnAgain());
    }



    public IEnumerator SpawnAgain()     //alýnan küpün tekrardan spawn olabilmesi için bir coroutine metodu çaðrýlýyor
    {
        yield return new WaitForSeconds(4f);
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        isCollected = false;
    }
}

