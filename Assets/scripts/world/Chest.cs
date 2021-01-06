using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public MessageObject messageObj;
    public bool tutorialChest;
    //public List<Item> itens;
    public List<DataItemScriptableObject> scriptableItems = new List<DataItemScriptableObject>();
    private float timeToPopulate;
    private float timeElapsed;
    private bool init;
    public ItemDatabase itemDatabase;
    public int fixedItemID1;
    public int fixedItemID2;
    public int capacity;
    private Animator animator;
    public InteractableObject interactScript;
    public GameObject worldItem;
    private bool opened;
    private IEnumerator coroutine;
    private GameControl control;
    public int myID;
    [Header("Soma das Chances nao passar de 1")]
    public float flawlessChance;
    public float greatChance;
    public float goodChance;
    public float normalChance;
    public float oldChance;
    public float wasteChance;
    public float usableItemChance;
    public float healthPotionChance;
    public bool altChest;

    private IEnumerator coinCoroutine;
    [Header("Chance de dropar moeda 0-1")]
    public float coinDropChance;
    protected List<ulong> coinItems = new List<ulong>();
    [Header("Cota máxima de uma moeda")]
    public ulong maxCoinValue;
    protected ulong chestCoinValue;
    [Header("Máximo valor que o baú pode ter em moedas")]
    public int maxValue;
    public GameObject coinPrefab;
   


    private void Start()
    {
        init = false;
        //itens = new List<Item>();
        control = GameControl.control;
        animator = GetComponent<Animator>();
        interactScript = gameObject.GetComponent<InteractableObject>();
        timeToPopulate = 1;
        opened = control.VerifyChest(myID);
        //commonChance = 1.0f - rareChance - epicChance;
        

    }

    private void Update()
    {
        if (!init)
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= timeToPopulate)
            {
                PopulateChest();
                PopulateCoins();
            }
        }
    }

    void PopulateCoins()
    {

        float dropRoll = Random.value;
        if (dropRoll <= coinDropChance)
        {
            chestCoinValue = (ulong)Random.Range(maxValue / 2, maxValue);
            
            if (chestCoinValue >= maxCoinValue)
            {
                chestCoinValue -= maxCoinValue;
                coinItems.Add(maxCoinValue);
                while (chestCoinValue >= maxCoinValue)
                {
                    chestCoinValue -= maxCoinValue;
                    coinItems.Add(maxCoinValue);
                }
                if (chestCoinValue > 0f)
                {
                    coinItems.Add(chestCoinValue);
                }
            }
            else
            {
                coinItems.Add(chestCoinValue);
            }


        }
    }

    void PopulateChest()
    {
        init = true;
        itemDatabase = GameObject.Find("HeroInventory").GetComponent<ItemDatabase>();
        //bool usableItemDroped = false;

        if (fixedItemID1 != -1) scriptableItems.Add(itemDatabase.FetchItemById(fixedItemID1));
        if (fixedItemID2 != -1) scriptableItems.Add(itemDatabase.FetchItemById(fixedItemID2));
        //int row = itemDatabase.numItems / 5;
        
        for (int i = 0; i < capacity; i++)
        {
            
            float rarityChance = Random.value;

            //Primeiro verifica se o item é do tipo sucata
            if (rarityChance <= wasteChance)
            { 
                int itemChance = Random.Range(0, itemDatabase.wasteItemsDatabase.Count);
                scriptableItems.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.wasteItemsDatabase, itemChance));

            }
            // verifica se o item é do tipo velho
            else if (rarityChance <= wasteChance + oldChance)
            { 
                int itemChance = Random.Range(0, itemDatabase.oldItemsDatabase.Count);
                scriptableItems.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.oldItemsDatabase, itemChance));
            }
            // verifica se o item é do tipo bom
            else if (rarityChance <= wasteChance + oldChance + normalChance)
            {
                int itemChance = Random.Range(0, itemDatabase.normalItemsDatabase.Count);
                scriptableItems.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.normalItemsDatabase, itemChance));
            }
            // verifica se o item é do tipo otimo
            else if (rarityChance <= wasteChance + oldChance + normalChance + goodChance)
            {
                int itemChance = Random.Range(0, itemDatabase.goodItemsDatabase.Count);
                scriptableItems.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.goodItemsDatabase, itemChance));
            }
            // verifica se o item é do tipo perfeito
            else if (rarityChance <= wasteChance + oldChance + normalChance + goodChance + greatChance)
            {
                int itemChance = Random.Range(0, itemDatabase.greatItemsDatabase.Count);
                scriptableItems.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.greatItemsDatabase, itemChance));
            }
            else
            {
                int itemChance = Random.Range(0, itemDatabase.flawlessItemsDatabase.Count);
                scriptableItems.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.flawlessItemsDatabase, itemChance));
            }

            //int id = Random.Range(0, itemDatabase.numItems);
            //itens.Add(itemDatabase.FetchItemByID(id));
        }


        //for (int i = 0; i<itens.Count;i++)
        //{
        //    Item it = itens[i];
        //    //Debug.Log(it.Title);
        //}
        interactScript.Initiate(InteractableObject.tipo.CHEST, myID);
        if (tutorialChest) messageObj.Activate(false);
    }

    //void PopulateChest()
    //{
    //    init = true;
    //    itemDatabase = GameObject.Find("HeroInventory").GetComponent<ItemDatabase>();
    //    bool usableItemDroped = false;
        
    //    if (fixedItemID1 != -1) itens.Add(itemDatabase.FetchItemByID(fixedItemID1));
    //    if (fixedItemID2 != -1) itens.Add(itemDatabase.FetchItemByID(fixedItemID2));
    //    int row = itemDatabase.numItems/5;
    //    for (int i = 0; i < capacity; i++)
    //    {
    //        if(usableItemDroped)
    //        {
    //            usableItemDroped = false;
    //            usableItemChance -= 0.2f;
    //        }
    //        float rarityChance = Random.value;
            
    //        //Primeiro verifica se o item é do tipo sucata
    //        if(rarityChance <= wasteChance)
    //        {
                
    //            float stackableChance = Random.value;
    //            // Depois verifica se o item é consumível
    //            if(stackableChance >= usableItemChance)
    //            {
    //                float itemChance = Random.value;
    //                if(itemChance <= healthPotionChance)
    //                {
    //                    itens.Add(itemDatabase.FetchItemByID(row - 2));
    //                }
    //                else itens.Add(itemDatabase.FetchItemByID(row - 1));
    //            }
    //            // Se não for consumível então é modificador
    //            else
    //            {
    //                usableItemDroped = true;
    //                int itemChance = Random.Range(0,row - 2);
    //                itens.Add(itemDatabase.FetchItemByID(itemChance));
    //            }
    //        }
    //        // verifica se o item é do tipo velho
    //        else if (rarityChance <= wasteChance + oldChance)
    //        {
                
    //            float stackableChance = Random.value;
    //            if (stackableChance >= usableItemChance)
    //            {
    //                float itemChance = Random.value;
    //                if (itemChance <= healthPotionChance)
    //                {
    //                    itens.Add(itemDatabase.FetchItemByID((2 * row) - 2));
    //                }
    //                else itens.Add(itemDatabase.FetchItemByID((2 * row) - 1));
    //            }
    //            else
    //            {
    //                usableItemDroped = true;
    //                int itemChance = Random.Range(row, (2*row) - 2);
    //                itens.Add(itemDatabase.FetchItemByID(itemChance));
    //            }
    //        }
    //        // verifica se o item é do tipo bom
    //        else if (rarityChance <= wasteChance + oldChance + goodChance)
    //        {
                
    //            float stackableChance = Random.value;
    //            if (stackableChance >= usableItemChance)
    //            {
    //                float itemChance = Random.value;
    //                if (itemChance <= healthPotionChance)
    //                {
    //                    itens.Add(itemDatabase.FetchItemByID((3 * row) - 2));
    //                }
    //                else itens.Add(itemDatabase.FetchItemByID((3 * row) - 1));
    //            }
    //            else
    //            {
    //                usableItemDroped = true;
    //                int itemChance = Random.Range(2*row, (3*row)-2);
    //                itens.Add(itemDatabase.FetchItemByID(itemChance));
    //            }
    //        }
    //        // verifica se o item é do tipo otimo
    //        else if (rarityChance <= wasteChance + oldChance + goodChance + greatChance)
    //        {
                
    //            float stackableChance = Random.value;
    //            if (stackableChance >= usableItemChance)
    //            {
    //                float itemChance = Random.value;
    //                if (itemChance <= healthPotionChance)
    //                {
    //                    itens.Add(itemDatabase.FetchItemByID((4 * row) - 2));
    //                }
    //                else itens.Add(itemDatabase.FetchItemByID((4 * row) - 1));
    //            }
    //            else
    //            {
    //                usableItemDroped = true;
    //                int itemChance = Random.Range(3 * row, (4 * row) - 2);
    //                itens.Add(itemDatabase.FetchItemByID(itemChance));
    //            }
    //        }
    //        // verifica se o item é do tipo perfeito
    //        else
    //        {
                
    //            float stackableChance = Random.value;
    //            if (stackableChance >= usableItemChance)
    //            {
    //                float itemChance = Random.value;
    //                if (itemChance <= healthPotionChance)
    //                {
    //                    itens.Add(itemDatabase.FetchItemByID((5 * row) - 2));
    //                }
    //                else itens.Add(itemDatabase.FetchItemByID((5 * row) - 1));
    //            }
    //            else
    //            {
    //                usableItemDroped = true;
    //                int itemChance = Random.Range(4 * row, (5 * row) - 2);
    //                itens.Add(itemDatabase.FetchItemByID(itemChance));
    //            }
    //        }

    //        //int id = Random.Range(0, itemDatabase.numItems);
    //        //itens.Add(itemDatabase.FetchItemByID(id));
    //    }

        
    //    //for (int i = 0; i<itens.Count;i++)
    //    //{
    //    //    Item it = itens[i];
    //    //    //Debug.Log(it.Title);
    //    //}
    //    interactScript.Initiate(InteractableObject.tipo.CHEST, myID);
    //    if (tutorialChest) messageObj.Activate(false);
    //}

    public void Open()
    {
        
        if (!opened)
        {
            animator.SetBool("open", true);
            opened = true;
            //for (int i = 0; i <= itens.Count; i++)
            for (int i = 0; i <= scriptableItems.Count; i++)
            {
                coroutine = DropItem(i+0.5f, i);
                StartCoroutine(coroutine);
                if (tutorialChest) messageObj.Activate(true);

            }

            if (coinItems.Count > 0)
            {
                for (int i = 0; i <= coinItems.Count; i++)
                {
                    coinCoroutine = DropCoin(i + 0.5f, i);
                    StartCoroutine(coinCoroutine);

                }
            }
            control.ChestOpened(myID);

            //itens.Clear();
        }
        else
        {
            if (animator.GetBool("open"))
            {
                animator.SetBool("open", false);
            }
            else animator.SetBool("open", true);

        }
    }

    //private IEnumerator DropItem(float waitTime, int id)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    if (id < itens.Count)
    //    {
    //        //Vector3 position = new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z + Random.Range(-3, 3));
    //        Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    //        Vector3 force = new Vector3(Random.Range(-4, 4), 7, Random.Range(2, 7));
    //        GameObject item = Instantiate(worldItem, position, transform.rotation);
    //        if(altChest)
    //        {
    //            item.transform.Rotate(Vector3.right,90,Space.Self);
    //        }
    //        item.GetComponent<Rigidbody>().AddRelativeForce(force);
    //        WorldItem worldItemScr = item.GetComponent<WorldItem>();

    //        worldItemScr.InitiateItem(itens[id], 1);
    //    }
    //    else itens.Clear();

    //}

    

    private IEnumerator DropItem(float waitTime, int id)
    {
        yield return new WaitForSeconds(waitTime);
        if (id < scriptableItems.Count)
        {
            //Vector3 position = new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z + Random.Range(-3, 3));
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Vector3 force = new Vector3(Random.Range(-4, 4), 7, Random.Range(2, 7));
            GameObject item = Instantiate(worldItem, position, transform.rotation);
            if (altChest)
            {
                item.transform.Rotate(Vector3.right, 90, Space.Self);
            }
            item.GetComponent<Rigidbody>().AddRelativeForce(force);
            WorldItem worldItemScr = item.GetComponent<WorldItem>();

            worldItemScr.InitiateItem(scriptableItems[id], 1);
        }
        //else itens.Clear();
        else scriptableItems.Clear();

    }


    private IEnumerator DropCoin(float waitTime, int id)
    {
        yield return new WaitForSeconds(waitTime);
        if (id < coinItems.Count)
        {
            //Vector3 position = new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z + Random.Range(-3, 3));
            Vector3 position = new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y + 1, transform.position.z + Random.Range(-1, 1));
            Vector3 force = new Vector3(Random.Range(-2, 2), 7, Random.Range(2, 3));
            GameObject item = Instantiate(coinPrefab, position, transform.rotation);
            item.GetComponent<Rigidbody>().AddRelativeForce(force);
            float scale = (float)coinItems[id] / (float)maxCoinValue;
            item.GetComponent<Coin>().Initialize(coinItems[id], scale);
            //WorldItem worldItemScr = item.GetComponent<WorldItem>();
            // worldItemScr.InitiateItem(itens[id], 1);
        }
        else coinItems.Clear();

    }

    public void ActivateChest(bool a)
    {
        opened = !a;
    }

    public void Hide()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        transform.GetChild(2).GetComponent<Renderer>().enabled = false;
    }

    public void Show()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        transform.GetChild(2).GetComponent<Renderer>().enabled = true;
    }

}
