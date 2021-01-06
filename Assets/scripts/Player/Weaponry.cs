using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponry : MonoBehaviour {

    private GameControl control;
    public InteractableObject interactScript;
    public int myID;
    public bool shield;
    public bool sword;
    public GameObject messageObj;
    private bool init;

    // Use this for initialization
    void Start () {
        control = GameControl.control;
        interactScript = gameObject.GetComponent<InteractableObject>();
        interactScript.Initiate(InteractableObject.tipo.WEAPONRY, myID);
        if(shield)
        {
            if (control.canBlock) gameObject.SetActive(false);
        }
        messageObj.GetComponent<MessageObject>().Activate(false);
    }


    private void Update()
    {
        if(!init)
        {
            init = true;
            messageObj.GetComponent<MessageObject>().Activate(false);
        }
    }
    // Update is called once per frame
    public void PickUp()
    {
        if(shield) control.PickUpShield();
        messageObj.GetComponent<MessageObject>().Activate(true);
        gameObject.SetActive(false);
    }
}
