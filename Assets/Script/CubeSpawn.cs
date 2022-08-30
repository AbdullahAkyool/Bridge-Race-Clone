using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawn : MonoBehaviour
{
    public float offset;
    public int horizontalCount;
    public int verticalCount;
    public GameObject[] prefab;
    public Transform SpawnParent;
    public List<GameObject> allSpawns;

    public List<GameObject> BlueBlocks;
    public List<GameObject> GreenBlocks;
    public List<GameObject> RedBlocks;

    void Start()
    {
        SpawnCube();
        InitializeSpawnList();
    }

    public void SpawnCube() //yerde k�p spawn i�lemi
    {

        for (int z = 0; z < verticalCount; z++) //belirlenen limitler i�erisinde �nce x ekseninde sonra z ekseninde k�pler spawn ediliyor
        {
            for (int x = 0; x < horizontalCount; x++)
            {
                Vector3 spawnPos = new Vector3(transform.position.x + x * offset, -2.7F, transform.position.z + z * offset); //spawn objesinin konumuna g�re konumlanma
                GameObject childStair = Instantiate(prefab[Random.Range(0,3)], spawnPos, Quaternion.identity);
                childStair.transform.parent = gameObject.transform; //olu�turulan objenin parent'�n�n belirlenmesi
            }
        }
    }

    public void InitializeSpawnList()  //parent nesnesinde bulunan child nesneler list'e aktar�l�yor
    {
        foreach (Transform spawnTransform in SpawnParent)
        {
            allSpawns.Add(spawnTransform.gameObject);    
        }

        foreach (GameObject block in allSpawns)
        {
            if (block.CompareTag("greenCube"))
            {
                GreenBlocks.Add(GameObject.FindGameObjectWithTag("greenCube"));
            }
            else if (block.CompareTag("blueCube"))
            {
                BlueBlocks.Add(GameObject.FindGameObjectWithTag("blueCube"));
            }
            else if (block.CompareTag("redCube"))
            {
                RedBlocks.Add(GameObject.FindGameObjectWithTag("redCube"));
            }

        }
    }

    private void OnDrawGizmos() //gizmos'ta k�plerin spawn konumlar�n�n g�r�lmesi
    {
        for (int z = 0; z < verticalCount; z++) 
        {
            for (int x = 0; x < horizontalCount; x++)
            {
                Vector3 spawnPos = new Vector3(transform.position.x + x * offset, -2.7F, transform.position.z + z * offset);
                Gizmos.DrawCube(spawnPos,new Vector3(1f,.5F,.5f));
            }
        }
    }

}

