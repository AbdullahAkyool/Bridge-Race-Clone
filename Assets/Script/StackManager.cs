using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    public Transform StackParent;   //stacklenecek k�plerin bulunaca�� parent nesnesi
    public int StackLimit;
    public List<GameObject> allBlocks;  //stacklenen k�pler list'e ekleniyor

    public float offset;
    public GameObject[] prefabs;    //stacklenebilir k�pler
    public GameObject childBlock, greenCube, blueCube, redCube;   //stacklenen child k�pler

    private bool isDroppingBlock = false;
    public float BlockDropDuration = .2f;   //k�plerin b�rak�lmas� i�leminde bekleme s�resi
    public int IndexBlock = 0;

    public CubeSpawn SpawnAgain;    //al�nan k�p�n tekar spawnlanmas� i�in CubeSpawn scriptinden �ekilen metod

    public int renkKod;
    public string renk;

    public TopDownCharacterController player;
    public GameObject stair;

    private Animator anim;

    //public Color red, blue, green;


    public void Start()
    {
        //RandomColorSelect();

        StackSpawn();
        
        InitializeBlockList();
        renk = "greenCube";

        anim = GetComponent<Animator>();

    }

    /*public void RandomColorSelect()
    {
        renkKod = Random.Range(0, 3);  //random se�ilen say�n�n hangi renk k�pe denk geldi�inin belirlenmesi ve kar��l���nda karakterin renginin de�i�mesi

        if (renkKod == 0) { renk = "greenCube"; player.characterModel.GetComponent<SkinnedMeshRenderer>().material.color = green; }
        else if (renkKod == 1) { renk = "blueCube"; player.characterModel.GetComponent<SkinnedMeshRenderer>().material.color = blue; }
        else if (renkKod == 2) { renk = "redCube"; player.characterModel.GetComponent<SkinnedMeshRenderer>().material.color = red; }
    }*/
    public void StackSpawn()  //belirlenen stack limiti kadar child k�p objesi olu�turma
    {
        for(int y=0; y<StackLimit; y++)
        {
            Vector3 stackpos = new Vector3(gameObject.transform.position.x, (y * offset) - 2f, gameObject.transform.position.z-.5f);
            childBlock = Instantiate(prefabs[renkKod], stackpos, Quaternion.identity);
            childBlock.gameObject.SetActive(false);
            childBlock.transform.parent = gameObject.transform;   //olu�turulan child objelerin player i�erisinde konumu belirlenen parent nesnesine atanmas�
        }
        allBlocks.Clear();
    }

    public void InitializeBlockList()  //parent nesnesinde bulunan child nesneler list'e aktar�l�yor
    {
        foreach(Transform blockTransform in StackParent)
        {
            blockTransform.gameObject.SetActive(false);
            allBlocks.Add(blockTransform.gameObject);
        }
    }

   public void CollectBlock(GameObject block)   //k�p toplama i�lemi
    {
        if (IndexBlock == allBlocks.Count) return;  //stacklenmi� k�p say�s� izin verilen max k�p say�s�na e�itse metoddan ��k�l�yor
        allBlocks[IndexBlock].SetActive(true);  //0. indexten ba�lanan k�p objeleri trigger sonucunda s�ras� ile aktif ediliyor
        block.GetComponent<Cube>().Collect();  //triggera girilen yerdeki k�p nesnesi i�erisinde bulunan Block metodu �a�r�l�yor
        IndexBlock++;   //child k�p indexi art�l�yor
    }

    public bool DropBlock() //k�p b�rakma i�lemi
    {
        if (IndexBlock <= 0)
        { 
            //anim.SetBool("isRunning", false);
            return false; //stacklenen k�p indexi 0 ise metoddan ��k�l�yor
        }
             
        if (isDroppingBlock) return false;  //k�p drop edilmi� ise bool de�eri true d�ner ve true olan bool de�eri sonucunda metoda girilmez 
        isDroppingBlock = true; //drop edilebilecek k�p bulunuyor ise bool de�eri true d�ner
        allBlocks[IndexBlock-1].SetActive(false);   //her k�p drop i�leminde stack listesinde current indexe sahip objenin aktifli�i false �evriliyor.(-1'in sebebi t�m k�pler drop edilse dahi en son 1 adet k�p kal�yor olu�u)
        IndexBlock--;   //her drop i�leminde index azalt�l�yor
        StartCoroutine(BlockDropTimer()); 
        return true;    //metod ba�ar�l� �al��t�ysa true d�nd�r�l�yor
    }

    private IEnumerator BlockDropTimer()    //k�p drop i�leminin yava��a ger�ekle�mesi i�in coroutine i�lemi
    {
        yield return new WaitForSeconds(BlockDropDuration);
        isDroppingBlock = false;
    }





    
}
