using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    GameControl controller;
    HeroStats stats;
    GameObject inventoryPanel;
    GameObject slotPanel;
    ItemDatabase database;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    public Canvas heroInfoCanvas;
    public GameObject worldItem;
    public int[] inventoryItemsID;
    public int[] inventoryItemsAmount;

    int slotsNumber;
    //public List<Item> items = new List<Item>();
    public List<DataItemScriptableObject> items = new List<DataItemScriptableObject>();
    public List<GameObject> slots = new List<GameObject>();
    public DataItemScriptableObject itemOnQueue;

    void Start()
    {
        controller = GameControl.control;
       
        stats = gameObject.GetComponentInParent<HeroStats>();
        database = GetComponent<ItemDatabase>();
        //heroInfoCanvas = GameObject.Find("HeroInfoCanvas").GetComponent<Canvas>();
        HideHeroInfoWindow();
        slotsNumber = 6;
        inventoryItemsID = new int[slotsNumber];
        inventoryItemsAmount = new int[slotsNumber];
        inventoryPanel = GameObject.Find("InventoryPanelWindow");
        slotPanel = inventoryPanel.transform.Find("SlotPanel").gameObject;
        ResetInventory();

    }

    public void ResetInventory()
    {
        
        for (int i = 0; i < slotsNumber; i++)
        {
            //items.Add(new Item());
            items.Add(null);
            
            //slots.Add(Instantiate(inventorySlot));
            //slots[i].transform.SetParent(slotPanel.transform);
            slots.Add(slotPanel.transform.GetChild(i).gameObject);
            inventoryItemsID[i] = -1;
            inventoryItemsAmount[i] = 0;
        }
        
    }

    

    public void PopulateInventory()
    {
        items.Clear();
        slots.Clear();
        ResetInventory();
        for (int i = 0; i < slotsNumber; i++)
        {
            if (slots[i].transform.childCount > 0)
            {
                ItemData itemToDestroy = slots[i].transform.GetChild(0).gameObject.GetComponent<ItemData>();
                //if (!itemToDestroy.item.Stackable) stats.DeactivateItemBonus(itemToDestroy.item);
                if (!itemToDestroy.item.stackable) stats.DeactivateItemBonus(itemToDestroy.item);
                Destroy(slots[i].transform.GetChild(0).gameObject);
            }

            if (controller.inventory[i] == -1)
            {
                if (inventoryItemsID[i] != -1)
                {
                    Destroy(slots[i].transform.GetChild(0).gameObject);
                    //items[i] = new Item();
                }
            }
            else
            {


                //Item itemToAdd = database.FetchItemByID(controller.inventory[i]);
                DataItemScriptableObject itemToAdd = database.FetchItemById(controller.inventory[i]);
                items[i] = itemToAdd;
                
                GameObject itemObj = Instantiate(inventoryItem);
                if (!itemToAdd.stackable)
                {
                    stats.ActivateItemBonus(itemToAdd);
                    //itemObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/" + itemToAdd.Rarity);
                    itemObj.transform.GetChild(0).GetComponent<Image>().sprite = itemToAdd.imageQuality;

                }
                else
                {
                    Color c = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    itemObj.transform.GetChild(0).GetComponent<Image>().color = c;
                }
                

                itemObj.transform.SetParent(slots[i].transform);
                itemObj.transform.localPosition = Vector3.zero;
                itemObj.transform.localRotation = Quaternion.identity;
                itemObj.transform.localScale = Vector3.one;
                //itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;

                //itemObj.transform.GetChild(1).GetComponent<Image>().sprite = itemToAdd.Sprite;
                itemObj.transform.GetChild(1).GetComponent<Image>().sprite = itemToAdd.imageSprite;

                //itemObj.name = itemToAdd.Title;
                itemObj.name = itemToAdd.title;

                ItemData data = itemObj.GetComponent<ItemData>();
                data.amount = controller.inventoryAmount[i];
                if (data.amount == 1)
                {
                    //slots[i].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "";
                    data.transform.GetChild(2).GetComponent<Text>().text = "";

                }
                else data.transform.GetChild(2).GetComponent<Text>().text = data.amount.ToString();

                data.item = itemToAdd;
                data.slot = i;
                //inventoryItemsID[i] = itemToAdd.ID;
                inventoryItemsID[i] = itemToAdd.id;
                inventoryItemsAmount[i] = controller.inventoryAmount[i];
                
            }
            
        }
    }

    public void HideHeroInfoWindow()
    {
        heroInfoCanvas.enabled = false;
        //Time.timeScale = 1;
        heroInfoCanvas.gameObject.transform.Find("HeroStatsWindow").gameObject.GetComponent<TabPanelInterface>().OnPointerDown();
    }

    public void ShowHeroInfoWindow()
    {
        heroInfoCanvas.enabled = true;
        
    }

    public int ReadInventory(int id)
    {
        return inventoryItemsID[id];
    }

    public int ReadInventoryAmount(int id)
    {
        return inventoryItemsAmount[id];
    }
    

    //public bool AddItem(int id)
    //{
    //    Item itemToAdd = database.FetchItemByID(id);
    //    if(itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd))
    //    {
            
    //        return true;
    //    }

    //    for(int i = 0; i < items.Count; i++)
    //    {
    //        if(items[i].ID == -1)
    //        {
                
    //            items[i] = itemToAdd;

                
    //            GameObject itemObj = Instantiate(inventoryItem);
    //            if (!itemToAdd.Stackable)
    //            {
    //                stats.ActivateItemBonus(itemToAdd);
    //                itemObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/" + itemToAdd.Rarity);
    //            }
    //            else
    //            {
    //                Color c = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    //                itemObj.transform.GetChild(0).GetComponent<Image>().color = c;
    //            }

    //            itemObj.transform.SetParent(slots[i].transform);
    //            itemObj.transform.localPosition = Vector3.zero;
    //            itemObj.transform.localRotation = Quaternion.identity;
    //            itemObj.transform.localScale = Vector3.one;
                
    //            //itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
    //            itemObj.transform.GetChild(1).GetComponent<Image>().sprite = itemToAdd.Sprite;

    //            itemObj.name = itemToAdd.Title;
    //            ItemData data = itemObj.GetComponent<ItemData>();
    //            data.amount++;
    //            data.item = itemToAdd;
    //            data.slot = i;
    //            inventoryItemsID[i] = itemToAdd.ID;
    //            inventoryItemsAmount[i] = data.amount;
                
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    public bool AddItem(int id, int amount)
    {
        //Item itemToAdd = database.FetchItemByID(id);
        DataItemScriptableObject itemToAdd = database.FetchItemById(id);
        
        if (itemToAdd.stackable && CheckIfItemIsInInventory(itemToAdd, amount))
        {

            return true;
        }

        for (int i = 0; i < items.Count; i++)
        {
            //if (items[i].id == -1)
            if(items[i] == null)
            {

                items[i] = itemToAdd;


                GameObject itemObj = Instantiate(inventoryItem);
                if (!itemToAdd.stackable)
                {
                    stats.ActivateItemBonus(itemToAdd);
                    //itemObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/items/" + itemToAdd.Rarity);
                    itemObj.transform.GetChild(0).GetComponent<Image>().sprite = itemToAdd.imageQuality;
                }
                else
                {
                    Color c = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    itemObj.transform.GetChild(0).GetComponent<Image>().color = c;
                }

                itemObj.transform.SetParent(slots[i].transform);
                itemObj.transform.localPosition = Vector3.zero;
                itemObj.transform.localRotation = Quaternion.identity;
                itemObj.transform.localScale = Vector3.one;

                //itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                itemObj.transform.GetChild(1).GetComponent<Image>().sprite = itemToAdd.imageSprite;

                itemObj.name = itemToAdd.title;
                ItemData data = itemObj.GetComponent<ItemData>();
                data.amount = amount;
                if(amount > 1) data.transform.GetChild(2).GetComponent<Text>().text = data.amount.ToString();
                data.item = itemToAdd;
                data.slot = i;
                inventoryItemsID[i] = itemToAdd.id;
                inventoryItemsAmount[i] = data.amount;

                return true;
            }
        }

        return false;
    }

    //public void UseItem(Item item, int slot)
    public void UseItem(DataItemScriptableObject item, int slot)
    {
        stats.ActivateItemBonus(item);
        RemoveUsableItem(slot);
    }

    public void RemoveUsableItem(int slot)
    {
        ItemData data = slots[slot].transform.GetChild(0).GetComponent<ItemData>();
        //Item item = data.item;
        if (data.amount > 1)
        {
            data.amount--;
            if (data.amount == 1)
            {
                data.transform.GetChild(2).GetComponent<Text>().text = "";

            }
            else data.transform.GetChild(2).GetComponent<Text>().text = data.amount.ToString();

            inventoryItemsAmount[slot] = data.amount;
        }
        else
        {
            Destroy(slots[slot].transform.GetChild(0).gameObject);
            //items[slot] = new Item();
            items[slot] = null;
            inventoryItemsID[slot] = -1;
            inventoryItemsAmount[slot] = 0;
        }
        controller.UpdateControl();
    }

    public void RemoveItem(int slot)
    {
        ItemData data = slots[slot].transform.GetChild(0).GetComponent<ItemData>();
        //Item item = data.item;
        DataItemScriptableObject item = data.item;
        DropItem(item, data.amount);
        stats.DeactivateItemBonus(item);
        //if (data.amount > 1)
        //{
        //    data.amount--;
        //    if (data.amount == 1)
        //    {
        //        slots[slot].transform.GetChild(0).transform.GetChild(2).GetComponent<Text>().text = "";

        //    }
        //    else data.transform.GetChild(2).GetComponent<Text>().text = data.amount.ToString();

        //    inventoryItemsAmount[slot] = data.amount;

        //}
        //else
        //{
           
       
            Destroy(slots[slot].transform.GetChild(0).gameObject);
        //items[slot] = new Item();
        items[slot] = null;
            inventoryItemsID[slot] = -1;
            inventoryItemsAmount[slot] = 0;
        //}
        controller.UpdateControl();
    }

    //void DropItem(Item item, int _amount)
    void DropItem(DataItemScriptableObject item, int _amount)
    {
        float raio = 2.0f;
        float x = transform.position.x + Random.Range(-raio,raio);
        float z = transform.position.z + Random.Range(-raio, raio);

        Vector3 position = new Vector3(x, transform.position.y, z);
        //Vector3 force = new Vector3(Random.Range(-1, 1), 1, Random.Range(2, 4));
        GameObject itemObj = Instantiate(worldItem, position, transform.rotation);
        //itemObj.GetComponent<Rigidbody>().AddRelativeForce(force);
        WorldItem worldItemScr = itemObj.GetComponent<WorldItem>();
        worldItemScr.InitiateItem(item, _amount);
    }

    //bool CheckIfItemIsInInventory(Item item, int _amount)
    bool CheckIfItemIsInInventory(DataItemScriptableObject item, int _amount)

    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == item.id)
            {

                ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                data.amount += _amount;
                data.transform.GetChild(2).GetComponent<Text>().text = data.amount.ToString();
                inventoryItemsAmount[i] = data.amount;
                return true;
            }
        }
        return false;
    }

    
}
