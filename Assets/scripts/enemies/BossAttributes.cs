using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAttributes : MonoBehaviour {


    public GameObject damageCanvas;
    protected bool showDamage;
    protected float timer;
    protected float timeElapsed;
    public GameObject selectedCanvas;

    protected Text damageText;
    protected float currHealth;
    public float maxHealth;
    public float velocity;
    protected float currentDefense;
    public float defense;
    public float rewardPoints;
    //protected float dropChance;
    public float attackRate;
    public float minDamage;
    public float maxDamage;
    public float attackRange;
    public float visibilityRange;
    //protected GameObject dropPrefab;
    protected Image healthBarBG;
    protected Image healthBar;
    //protected Image combatIcon;
    protected bool combat;
    protected float patrolRange;
    protected BossBehavior bossScr;
    protected int numberItems;
   // protected int itemsRarity;
    public float combatArea;


    public Renderer myRender;
    private Color flashColour = new Color(1f, 0f, 0f, 1f);
    private bool damaged;
    private float flashSpeed = 20;

    private IEnumerator coinCoroutine;
    public float coinDropChance;
    public List<ulong> coinItems;
    public ulong maxCoinValue;
    public ulong coinValue;
    public GameObject coinPrefab;

    public int maxLoot;
    protected GameObject dropPrefab;
    public float dropChance;
    public float flawlessLoot;
    public float greatLoot;
    public float goodLoot;
    public float normalLoot;
    public float oldLoot;
    public float wasteLoot;
    //public List<Item> itens = new List<Item>();
    public List<DataItemScriptableObject> items = new List<DataItemScriptableObject>();
    public ItemDatabase itemDatabase;
    public GameObject worldItem;
    private IEnumerator coroutine;




    protected virtual void Awake()
    {
        healthBar = transform.Find("BossCanvas/HealthBG/Health").GetComponent<Image>();
        healthBarBG = transform.Find("BossCanvas/HealthBG").GetComponent<Image>();
        
        //combatIcon = transform.Find("BossCanvas/CombatIcon").GetComponent<Image>();
        combat = false;
        bossScr = GetComponent<BossBehavior>();
        //maxHealth = 750;
        currHealth = maxHealth;
        //velocity = 1.2f;
        //rewardPoints = 450;
        //dropChance = 15;
        //attackRate = 1;
        //minDamage = 90;
        //maxDamage = 110;
        //attackRange = 2.5f;
        //visibilityRange = 13;
        //combatArea = 16;
        //patrolRange = 6;
        //defense = 10;
        currentDefense = defense;
        //numberItems = Random.Range(0, 2);
       // itemsRarity = 1;
        timer = 1f;
        damageCanvas.SetActive(false);
        selectedCanvas.SetActive(false);

        itemDatabase = GameObject.Find("HeroInventory").GetComponent<ItemDatabase>();
        float dropRoll = Random.value;
        if (dropRoll <= (dropChance / 100f) )
        {

            PopulateDrop();
        }
        PopulateCoins();
    }


    void PopulateCoins()
    {

        float dropRoll = Random.value;
        if (dropRoll <= coinDropChance)
        {
            coinValue = (ulong)Random.Range(maxHealth / 2, maxHealth);
            if (coinValue >= maxCoinValue)
            {
                coinValue -= maxCoinValue;
                coinItems.Add(maxCoinValue);
                while (coinValue >= maxCoinValue)
                {
                    coinValue -= maxCoinValue;
                    coinItems.Add(maxCoinValue);
                }
                if (coinValue > 0f)
                {
                    coinItems.Add(coinValue);
                }
            }
            else
            {
                coinItems.Add(coinValue);
            }


        }
    }

    void PopulateDrop()
    {
        int capacity = Random.Range(1, maxLoot + 1);

        for (int i = 0; i<capacity; i++)
        {
            
            float rarityChance = Random.value;

            //Primeiro verifica se o item é do tipo sucata
            if (rarityChance <= wasteLoot)
            { 
                int itemChance = Random.Range(0, itemDatabase.wasteItemsDatabase.Count);
                items.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.wasteItemsDatabase, itemChance));

            }
            // verifica se o item é do tipo velho
            else if (rarityChance <= wasteLoot + oldLoot)
            { 
                int itemChance = Random.Range(0, itemDatabase.oldItemsDatabase.Count);
                items.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.oldItemsDatabase, itemChance));
            }
            // verifica se o item é do tipo bom
            else if (rarityChance <= wasteLoot + oldLoot + normalLoot)
            {
                int itemChance = Random.Range(0, itemDatabase.normalItemsDatabase.Count);
                items.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.normalItemsDatabase, itemChance));
            }
            // verifica se o item é do tipo otimo
            else if (rarityChance <= wasteLoot + oldLoot + normalLoot + goodLoot)
            {
                int itemChance = Random.Range(0, itemDatabase.goodItemsDatabase.Count);
                items.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.goodItemsDatabase, itemChance));
            }
            // verifica se o item é do tipo perfeito
            else if (rarityChance <= wasteLoot + oldLoot + normalLoot + goodLoot + greatLoot)
            {
                int itemChance = Random.Range(0, itemDatabase.greatItemsDatabase.Count);
                items.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.greatItemsDatabase, itemChance));
            }
            else
            {
                int itemChance = Random.Range(0, itemDatabase.flawlessItemsDatabase.Count);
                items.Add(itemDatabase.FetchItemByDatabaseAndIndex(itemDatabase.flawlessItemsDatabase, itemChance));
            }

            //int id = Random.Range(0, itemDatabase.numItems);
            //itens.Add(itemDatabase.FetchItemByID(id));
        }


        //for (int i = 0; i<itens.Count;i++)
        //{
        //    Item it = itens[i];
        //    //Debug.Log(it.Title);
        //}
        
    }

    //void PopulateDrop()
    //{
    //    //int row = itemDatabase.numItems / 5;
    //    //int maxNumLoot = (int)dropChance / 5; 

    //    int capacity = Random.Range(1, maxLoot + 1);

    //    for (int i = 0; i < capacity; i++)
    //    {
    //        float rarityChance = Random.value;

    //        if (rarityChance <= wasteLoot)
    //        {

    //            float stackableChance = Random.value;

    //            if (stackableChance <= 0.8f)
    //            {

    //                float itemChance = Random.value;
    //                if (itemChance <= 0.7f)
    //                {

    //                    itens.Add(itemDatabase.FetchItemByID(row - 2));
    //                }
    //                else itens.Add(itemDatabase.FetchItemByID(row - 1));
    //            }
    //            else
    //            {

    //                int itemChance = Random.Range(0, row - 2);
    //                itens.Add(itemDatabase.FetchItemByID(itemChance));

    //            }

    //        }
    //        else if (rarityChance <= wasteLoot + oldLoot)
    //        {

    //            float stackableChance = Random.value;
    //            if (stackableChance <= 0.8f)
    //            {
    //                float itemChance = Random.value;
    //                if (itemChance <= 0.7f)
    //                {
    //                    itens.Add(itemDatabase.FetchItemByID((2 * row) - 2));
    //                }
    //                else itens.Add(itemDatabase.FetchItemByID((2 * row) - 1));
    //            }
    //            else
    //            {
    //                int itemChance = Random.Range(row, (2 * row) - 2);
    //                itens.Add(itemDatabase.FetchItemByID(itemChance));
    //            }
    //        }
    //        else if (rarityChance <= wasteLoot + oldLoot + goodLoot)
    //        {

    //            float stackableChance = Random.value;
    //            if (stackableChance <= 0.8f)
    //            {
    //                float itemChance = Random.value;
    //                if (itemChance <= 0.7f)
    //                {
    //                    itens.Add(itemDatabase.FetchItemByID((3 * row) - 2));
    //                }
    //                else itens.Add(itemDatabase.FetchItemByID((3 * row) - 1));
    //            }
    //            else
    //            {
    //                int itemChance = Random.Range(2 * row, (3 * row) - 2);
    //                itens.Add(itemDatabase.FetchItemByID(itemChance));
    //            }
    //        }
    //        else if (rarityChance <= wasteLoot + oldLoot + goodLoot + greatLoot)
    //        {

    //            float stackableChance = Random.value;
    //            if (stackableChance <= 0.8f)
    //            {
    //                float itemChance = Random.value;
    //                if (itemChance <= 0.7f)
    //                {
    //                    itens.Add(itemDatabase.FetchItemByID((4 * row) - 2));
    //                }
    //                else itens.Add(itemDatabase.FetchItemByID((4 * row) - 1));
    //            }
    //            else
    //            {
    //                int itemChance = Random.Range(3 * row, (4 * row) - 2);
    //                itens.Add(itemDatabase.FetchItemByID(itemChance));
    //            }
    //        }
    //        else
    //        {

    //            float stackableChance = Random.value;
    //            if (stackableChance <= 0.8f)
    //            {
    //                float itemChance = Random.value;
    //                if (itemChance <= 0.7f)
    //                {
    //                    itens.Add(itemDatabase.FetchItemByID((5 * row) - 2));
    //                }
    //                else itens.Add(itemDatabase.FetchItemByID((5 * row) - 1));
    //            }
    //            else
    //            {
    //                int itemChance = Random.Range(4 * row, (5 * row) - 2);
    //                itens.Add(itemDatabase.FetchItemByID(itemChance));
    //            }
    //        }

    //        //int id = Random.Range(0, itemDatabase.numItems);
    //        //itens.Add(itemDatabase.FetchItemByID(id));
    //    }
    //}


    public void Loot()
    {
        if (items.Count > 0)
        {
            for (int i = 0; i <= items.Count; i++)
            {
                coroutine = DropItem(i + 0.5f, i);
                StartCoroutine(coroutine);

            }
        }

        if (coinItems.Count > 0)
        {
            for (int i = 0; i <= coinItems.Count; i++)
            {
                coinCoroutine = DropCoin(i + 0.5f, i);
                StartCoroutine(coinCoroutine);

            }
        }
    }

    private IEnumerator DropItem(float waitTime, int id)
    {
        yield return new WaitForSeconds(waitTime);
        if (id < items.Count)
        {
            //Vector3 position = new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z + Random.Range(-3, 3));
            Vector3 position = new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y + 1, transform.position.z + Random.Range(-1, 1));
            Vector3 force = new Vector3(Random.Range(-2, 2), 7, Random.Range(2, 3));
            GameObject item = Instantiate(worldItem, position, transform.rotation);
            item.GetComponent<Rigidbody>().AddRelativeForce(force);
            WorldItem worldItemScr = item.GetComponent<WorldItem>();
            worldItemScr.InitiateItem(items[id], 1);
        }
        else items.Clear();

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

    void Update()
    {
        // Se recebeu dano...
        if (damaged)
        {
            // ...torna a tela vermelha com a imagem de flash
            myRender.material.color = flashColour;
        }
        else
        {
            // ... se nao, entao volta a limpar a tela
            myRender.material.color = Color.Lerp(myRender.material.color, Color.white, flashSpeed * Time.deltaTime);
        }
        damaged = false;

        if (showDamage)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timer)
            {
                timeElapsed = 0;
                showDamage = false;
                damageCanvas.SetActive(false);
            }
        }
    }

    public float GetRewardPoints()
    {
        return rewardPoints;
    }

    public float GetCombatArea()
    {
        return combatArea;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetVisibilityRange()
    {
        return visibilityRange;
    }

    public float GetVelocity()
    {
        return velocity;
    }

    public float GetCurrentHealth()
    {
        return currHealth;
    }

    public float GetPatrolRange()
    {
        return patrolRange;
    }

    public float GetDamage()
    {
        float damage = Random.Range(minDamage, maxDamage);
        return damage;

    }

    public void Damage(float d)
    {
        if (d - currentDefense >= 0)
            currHealth -= (d - currentDefense);
        healthBar.fillAmount = currHealth / maxHealth;
        Color c = myRender.material.color;
        c = Color.red;
        myRender.material.color = c;
        damaged = true;
        showDamage = true;
        damageCanvas.SetActive(true);
        timeElapsed = 0;
        int d_int = (int)d;
        damageCanvas.transform.GetChild(0).gameObject.GetComponent<Text>().text = "-"+d_int.ToString();
    }

    public void Select(bool s)
    {
        selectedCanvas.SetActive(s);
    }

    public void CombatIcon()
    {
        //combatIcon.enabled = (combatIcon.enabled) ? false : true;
        //combat = combat ? false : true;
        //Invoke("OnMouseExit", 0.5f);
    }

    public void ShowHealth()
    {
        healthBar.enabled = true;
        healthBarBG.enabled = true;
        //Invoke("HideHealth", 2);
    }

    public void HideHealth()
    {
        
        healthBar.enabled = false;
        healthBarBG.enabled = false;
        
    }

    protected void OnMouseEnter()
    {
       // healthBar.enabled = true;
       // healthBarBG.enabled = true;
    }

    protected void OnMouseExit()
    {
       // healthBar.enabled = combat ? true : false;
       // healthBarBG.enabled = combat ? true : false;
    }

    public void ThrowBack(float d, float defReduction)
    {
        //zombieScr.ThrowBack(d);
        //if (currentDefense == defense)
        //    currentDefense -= currentDefense * defReduction / 100;
    }

    public void RestoreDefense()
    {
        currentDefense = defense;
    }

    public void Stun(float d)
    {
        //zombieScr.Stun(d);
    }
}
