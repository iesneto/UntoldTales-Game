using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {

    //private Item item;
    private DataItemScriptableObject item;
    //private string data;
    private int slot;
    private GameObject tooltip;
    //public Text descriptionText;
    public Text itemTitle;
    public Text itemType;
    public Text itemDescription;
    public Text itemTip;
    private ItemData itemData;
    private GameObject slotPanel;
    public Color wasteColor;
    public Color oldColor;
    public Color normalColor;
    public Color goodColor;
    public Color greatColor;
    public Color flawlessColor;

    private void Start()
    {
        slot = -1;
        tooltip = GameObject.Find("DescriptionPanel");
        slotPanel = GameObject.Find("SlotPanel");
        //descriptionText = tooltip.transform.FindChild("Text").GetComponent<Text>();
        //descriptionText.text = "alguma coisa";
        itemTitle = tooltip.transform.Find("ItemTitle").GetComponent<Text>();
        itemType = tooltip.transform.Find("ItemType").GetComponent<Text>();
        itemDescription = tooltip.transform.Find("ItemDescription").GetComponent<Text>();
        itemTip = tooltip.transform.Find("ItemTip").GetComponent<Text>();

        tooltip.SetActive(false);
    }

    //public void Activate(Item item, ItemData itemData, int slot)
    public void Activate(DataItemScriptableObject item, ItemData itemData, int slot)
    {
        Outline outline;
        if (this.slot != -1)
        {
            outline = slotPanel.transform.GetChild(this.slot).GetComponent<Outline>();
            outline.enabled = false;
        }
        
        outline = slotPanel.transform.GetChild(slot).GetComponent<Outline>();
        this.item = item;
        this.itemData = itemData;
        this.slot = slot;
        outline.enabled = true;
        ConstructDataString();
        tooltip.SetActive(true);
        return;
        
        
    }

    public void Deactivate()
    {
        Outline outline = slotPanel.transform.GetChild(this.slot).GetComponent<Outline>();
        outline.enabled = false;
        this.slot = -1;
        tooltip.SetActive(false);
    }

    public void RemoveItem()
    {
        itemData.RemoveItem(slot);
        Deactivate();
    }

    public void ConstructDataString()
    {
        //cor pode pegar no color picker do photoshop
        //itemTitle.text = item.Title;
        itemTitle.text = item.title;
        //switch ((int)item.Quality)
        switch ((int)item.rarity)
        {
            case 0:
                itemTitle.color = wasteColor;
                break;
            case 1:
                itemTitle.color = oldColor;
                break;
            case 2:
                itemTitle.color = normalColor;
                break;
            case 3:
                itemTitle.color = goodColor;
                break;
            case 4:
                itemTitle.color = greatColor;
                break;
            case 5:
                itemTitle.color = flawlessColor;
                break;
            default: break;
            
        }
        //if (item.Stackable)
        if (item.stackable)
        {
            itemType.text = "(Consumível)";
            //itemDescription.text = item.Description;
            itemDescription.text = item.description;
            itemTip.text = "DUPLO TOQUE para usar";
        }
        else
        {
            itemType.text = "(Equipamento)";
            //itemDescription.text = item.Description;
            itemDescription.text = item.description;
            itemTip.text = "Efeito Ativado Automaticamente";
        }

        //data = "<color=#0473f0><b>" + item.Title + "</b></color>\n\n" + item.Description;
        //descriptionText.text = data;
    }
}
