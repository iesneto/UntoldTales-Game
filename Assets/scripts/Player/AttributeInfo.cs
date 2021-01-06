using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class AttributeInfo : MonoBehaviour, IPointerClickHandler {

    public GameObject infoPanel;
    public string msg;
    public Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        infoPanel.GetComponent<StatsInfoPanel>().SetText(gameObject, msg);
        
    }

    public void Select()
    {
        outline.enabled = true;
    }

    public void Deselect()
    {
        outline.enabled = false;
    }
}
