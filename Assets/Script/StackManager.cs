using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    public Transform StackParent;   //stacklenecek küplerin bulunacaðý parent nesnesi
    public int StackLimit;
    public List<GameObject> allBlocks;  //stacklenen küpler list'e ekleniyor

    public float offset;
    public GameObject[] prefabs;    //stacklenebilir küpler
    public GameObject childBlock, greenCube, blueCube, redCube;   //stacklenen child küpler

    private bool isDroppingBlock = false;
    public float BlockDropDuration = .2f;   //küplerin býrakýlmasý iþleminde bekleme süresi
    public int IndexBlock = 0;

    public CubeSpawn SpawnAgain;    //alýnan küpün tekar spawnlanmasý için CubeSpawn scriptinden çekilen metod

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
        renkKod = Random.Range(0, 3);  //random seçilen sayýnýn hangi renk küpe denk geldiðinin belirlenmesi ve karþýlýðýnda karakterin renginin deðiþmesi

        if (renkKod == 0) { renk = "greenCube"; player.characterModel.GetComponent<SkinnedMeshRenderer>().material.color = green; }
        else if (renkKod == 1) { renk = "blueCube"; player.characterModel.GetComponent<SkinnedMeshRenderer>().material.color = blue; }
        else if (renkKod == 2) { renk = "redCube"; player.characterModel.GetComponent<SkinnedMeshRenderer>().material.color = red; }
    }*/
    public void StackSpawn()  //belirlenen stack limiti kadar child küp objesi oluþturma
    {
        for(int y=0; y<StackLimit; y++)
        {
            Vector3 stackpos = new Vector3(gameObject.transform.position.x, (y * offset) - 2f, gameObject.transform.position.z-.5f);
            childBlock = Instantiate(prefabs[renkKod], stackpos, Quaternion.identity);
            childBlock.gameObject.SetActive(false);
            childBlock.transform.parent = gameObject.transform;   //oluþturulan child objelerin player içerisinde konumu belirlenen parent nesnesine atanmasý
        }
        allBlocks.Clear();
    }

    public void InitializeBlockList()  //parent nesnesinde bulunan child nesneler list'e aktarýlýyor
    {
        foreach(Transform blockTransform in StackParent)
        {
            blockTransform.gameObject.SetActive(false);
            allBlocks.Add(blockTransform.gameObject);
        }
    }

   public void CollectBlock(GameObject block)   //küp toplama iþlemi
    {
        if (IndexBlock == allBlocks.Count) return;  //stacklenmiþ küp sayýsý izin verilen max küp sayýsýna eþitse metoddan çýkýlýyor
        allBlocks[IndexBlock].SetActive(true);  //0. indexten baþlanan küp objeleri trigger sonucunda sýrasý ile aktif ediliyor
        block.GetComponent<Cube>().Collect();  //triggera girilen yerdeki küp nesnesi içerisinde bulunan Block metodu çaðrýlýyor
        IndexBlock++;   //child küp indexi artýlýyor
    }

    public bool DropBlock() //küp býrakma iþlemi
    {
        if (IndexBlock <= 0)
        { 
            //anim.SetBool("isRunning", false);
            return false; //stacklenen küp indexi 0 ise metoddan çýkýlýyor
        }
             
        if (isDroppingBlock) return false;  //küp drop edilmiþ ise bool deðeri true döner ve true olan bool deðeri sonucunda metoda girilmez 
        isDroppingBlock = true; //drop edilebilecek küp bulunuyor ise bool deðeri true döner
        allBlocks[IndexBlock-1].SetActive(false);   //her küp drop iþleminde stack listesinde current indexe sahip objenin aktifliði false çevriliyor.(-1'in sebebi tüm küpler drop edilse dahi en son 1 adet küp kalýyor oluþu)
        IndexBlock--;   //her drop iþleminde index azaltýlýyor
        StartCoroutine(BlockDropTimer()); 
        return true;    //metod baþarýlý çalýþtýysa true döndürülüyor
    }

    private IEnumerator BlockDropTimer()    //küp drop iþleminin yavaþça gerçekleþmesi için coroutine iþlemi
    {
        yield return new WaitForSeconds(BlockDropDuration);
        isDroppingBlock = false;
    }





    
}
