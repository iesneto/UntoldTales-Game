using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabPanelInterface : MonoBehaviour, IPointerClickHandler
{
    private GameControl control;
    public Image myImage;
    public GameObject tabObj1;
    public GameObject tabObj2;
    public GameObject tabObj3;
    public bool firstTouch;
    public string tutorialMessage;
    public int myID;
    public bool canShowMessage;
    public TutorialMessageSystem messageSystem;

    private void Start()
    {
        
        myImage = transform.Find("TabIcon").gameObject.GetComponent<Image>();
        UpdateTabPanelInterface();
    }

    public void UpdateTabPanelInterface()
    {
        control = GameControl.control;
        if (canShowMessage)
        {
            if (control.VerifyTutorialMessage(myID))
            {
                firstTouch = false;
            }
            else firstTouch = true;
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<RectTransform>().SetAsLastSibling();
        SetColor(Color.white);
        if(firstTouch)
        {
            firstTouch = false;
            control.TutorialMessageSent(myID);
            messageSystem.ShowMessage(tutorialMessage);
            messageSystem.IgnoreUnblockRaycast();
        }
    }

    public void OnPointerDown()
    {
        GetComponent<RectTransform>().SetAsLastSibling();
        SetColor(Color.white);
        if (firstTouch)
        {
            firstTouch = false;
            control.TutorialMessageSent(myID);
            messageSystem.ShowMessage(tutorialMessage);
            messageSystem.IgnoreUnblockRaycast();
        }
    }

    public void SetColor(Color c)
    {
        //Color color = myImage.color;
        //color.a = a;
        if(myImage != null)
            myImage.color = c;
        else
        {
            myImage = transform.Find("TabIcon").gameObject.GetComponent<Image>();
            myImage.color = c;
        }
        if (c == Color.white)
        {
            tabObj1.GetComponent<TabPanelInterface>().SetColor(Color.gray);
            tabObj2.GetComponent<TabPanelInterface>().SetColor(Color.gray);
            tabObj3.GetComponent<TabPanelInterface>().SetColor(Color.gray);
        }
    }
}
