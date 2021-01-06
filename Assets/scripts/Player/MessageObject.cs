using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageObject : MonoBehaviour {

    private GameControl control;
    public string message;
    public string message02;
    public bool isStackable;
    public GameObject heroMessageSystem;
    public float delayTime;
    public float stackDelayTime;
    public bool isHistory;
   // private bool isTutorial;
    private bool canShow;
    public int myID;
    private float timer;
    public float timerLimit;
    private bool active;
    public bool canRoll;

    private void Start()
    {
        Activate(true);
        canShow = true;
        control = GameControl.control;
        heroMessageSystem = GameObject.FindGameObjectWithTag("Hero").transform.Find("Messages").gameObject;
        
        if (isHistory)
        {
            //isTutorial = false;
            if(control.VerifyHistoryMessage(myID))
            {
                canShow = false;
            }
        }
        else
        {
            //isTutorial = true;
            if (control.VerifyTutorialMessage(myID))
            {
                canShow = false;
            }
        }
        
    }

    public void Activate(bool a)
    {
        active = a;
    }

    private void Update()
    {
        if(canShow && active)
        {
            timer += Time.deltaTime;
            if (timer >= timerLimit)
            {
                timer = 0;
                Collider[] cols = Physics.OverlapSphere(transform.position, 4.0f,LayerMask.GetMask("player"));
                if (cols.Length > 0)
                {
                    if (isStackable)
                    {
                        if (delayTime > 0)
                        {
                            heroMessageSystem.GetComponent<TutorialMessageSystem>().ShowMessage(message, delayTime);
                        }
                        else heroMessageSystem.GetComponent<TutorialMessageSystem>().ShowMessage(message);
                        if(stackDelayTime > 0) heroMessageSystem.GetComponent<TutorialMessageSystem>().StackMessage(message02, false, stackDelayTime);
                        else heroMessageSystem.GetComponent<TutorialMessageSystem>().StackMessage(message02, false);
                        heroMessageSystem.GetComponent<TutorialMessageSystem>().IgnoreUnblockRaycast();
                    }
                    else
                    {
                        if (delayTime > 0)
                        {
                            heroMessageSystem.GetComponent<TutorialMessageSystem>().ShowMessage(message, delayTime);
                        }
                        else heroMessageSystem.GetComponent<TutorialMessageSystem>().ShowMessage(message);
                    }
                    if (isHistory) control.HistoryMessageSent(myID);
                    else control.TutorialMessageSent(myID);
                    canShow = false;
                    if (canRoll) control.LearnToRoll();
                    
                }
            }
        }
    }

}
