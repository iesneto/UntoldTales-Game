using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldItem : MonoBehaviour {

    public GameObject myShadow;
    public GameControl control;
    //public Item myself;
    public DataItemScriptableObject item;
    public int amount;
    //public ItemDatabase itemDatabase;
    public InteractableObject interactScript;
    public Inventory heroInventory;
    public int id;
    public string title;
    public Canvas myCanvas;
    public Text myCanvasText;
    public Color wasteColor;
    public Color oldColor;
    public Color normalColor;
    public Color goodColor;
    public Color greatColor;
    public Color flawlessColor;

    public void InitiateItem(DataItemScriptableObject newItem, int _amount)
    {
        myShadow.SetActive(false);
        control = GameControl.control;
        heroInventory = GameObject.Find("HeroInventory").GetComponent<Inventory>();
        //itemDatabase = GameObject.Find("HeroInventory").GetComponent<ItemDatabase>();
        interactScript = gameObject.GetComponent<InteractableObject>();
        myCanvas = transform.Find("Canvas").GetComponent<Canvas>();
        myCanvasText = transform.Find("Canvas/Panel/Text").GetComponent<Text>();



        //myself = newItem;
        item = newItem;
        amount = _amount;
        //id = myself.ID;
        id = item.id;
        //title = myself.Title;
        title = item.title;
        interactScript.Initiate(InteractableObject.tipo.ITEM, id);

        //switch ((int)myself.Quality)
        switch ((int)item.rarity)
        {
            case 0:
                myCanvasText.color = wasteColor;
                break;
            case 1:
                myCanvasText.color = oldColor;
                break;
            case 2:
                myCanvasText.color = normalColor;
                break;
            case 3:
                myCanvasText.color = goodColor;
                break;
            case 4:
                myCanvasText.color = greatColor;
                break;
            case 5:
                myCanvasText.color = flawlessColor;
                break;
            default: break;

        }
        myCanvasText.text = title;
        myCanvas.enabled = false;
        Invoke("DisableRigidbody", 1.5f);
    }

    void DisableRigidbody()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        myShadow.SetActive(true);
    }

    public void TryToAddItem()
    {
        //Item myself = gameObject.GetComponent<WorldItem>().myself;

        if (heroInventory.AddItem(item.id, amount))
        {
            control.UpdateControl();
            HideItemTip();
            Destroy(this.gameObject);
        }
        else Debug.Log("Inventory FUll");
    }

    public void ShowItemTip()
    {
        myCanvas.enabled = true;
        
    }
    
    public void HideItemTip()
    {
        myCanvas.enabled = false;
    }

}
