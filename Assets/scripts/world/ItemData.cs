using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerClickHandler {

    //public Item item;
    public DataItemScriptableObject item;
    public int slot;
    public int amount;
    public bool drawGUI;
    public Vector2 eventPosition;
    private Inventory inventory;
    public float doubleClickdelay;
    public float clickTime;

    private ToolTip tooltipScript;

    void Start()
    {
        inventory = GameObject.Find("HeroInventory").GetComponent<Inventory>();
        tooltipScript = GameObject.Find("AboutPanelWindow").GetComponent<ToolTip>();
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        //if (item.Stackable)
        if (item.stackable)
        {
            tooltipScript.Activate(item, this, slot);
            if (clickTime == 0)
            {
                clickTime = eventData.clickTime;
            }
            else if ((eventData.clickTime - clickTime) <= doubleClickdelay)
            {
                inventory.UseItem(item, slot);
                tooltipScript.Deactivate();
            }
            else clickTime = eventData.clickTime;
        }
        else tooltipScript.Activate(item, this, slot);
    }

    public void RemoveItem(int slot)
    {
        inventory.RemoveItem(slot);
    }

 
}
