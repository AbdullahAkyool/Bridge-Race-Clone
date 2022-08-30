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

    public void SpawnCube() //yerde küp spawn iþlemi
    {

        for (int z = 0; z < verticalCount; z++) //belirlenen limitler içerisinde önce x ekseninde sonra z ekseninde küpler spawn ediliyor
        {
            for (int x = 0; x < horizontalCount; x++)
            {
                Vector3 spawnPos = new Vector3(transform.position.x + x * offset, -2.7F, transform.position.z + z * offset); //spawn objesinin konumuna göre konumlanma
                GameObject childStair = Instantiate(prefab[Random.Range(0,3)], spawnPos, Quaternion.identity);
                childStair.transform.parent = gameObject.transform; //oluþturulan objenin parent'ýnýn belirlenmesi
            }
        }
    }

    public void InitializeSpawnList()  //parent nesnesinde bulunan child nesneler list'e aktarýlýyor
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

    private void OnDrawGizmos() //gizmos'ta küplerin spawn konumlarýnýn görülmesi
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

