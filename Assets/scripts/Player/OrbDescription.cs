using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class OrbDescription : MonoBehaviour, IPointerClickHandler  {

    public Color selectedColor;
    public Color startColor;
    public Image orbImage;
    public Image unknownImage;
    public Image background;
    public GameObject title;
    public GameObject description;
    public GameObject active;
    public bool revealed;
    public OrbsPanel orbPanelObject;
    public int myID;

    public void RevealOrb(int id)
    {
        revealed = true;
        unknownImage.enabled = false;
        title.SetActive(true);
        description.SetActive(true);
        myID = id;
    }

    public void Activate(bool _active)
    {
        if (_active)
        {
            background.color = selectedColor;
            active.SetActive(true);
        }
        else
        {
            background.color = startColor;
            active.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(revealed)
            orbPanelObject.SelectOrb(gameObject, myID);
    }
}
