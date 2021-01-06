using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMessageSystem : MonoBehaviour {

    public Text message;
    private string m_msg;
    private string m_msg2;
    private bool stackMessages;
    public bool ignoreUnblock;
    private bool inInventory;
    public bool delayed;
    public bool delayedStack;
    public float delayTime;
    public float stackDelayTime;
    public float timer;
    public Canvas myCanvas;
    public MouseInterpreter heroTouchInput;

    private void Start()
    {
        myCanvas.enabled = false;
        heroTouchInput = GameObject.FindGameObjectWithTag("Hero").GetComponent<MouseInterpreter>();
    }

    private void Update()
    {
        if (delayed)
        {
            timer += Time.unscaledDeltaTime;
            if(timer >= delayTime)
            {
                timer = 0;
                delayed = false;
                ShowMessage(m_msg);
            }
        }
    }

    public void ShowMessage(string msg, float delay)
    {
        if (delay > 0)
        {
            delayed = true;
            delayTime = delay;
            timer = 0;
            m_msg = msg;
        }
        else ShowMessage(msg);
    }

    public void ShowMessage(string msg)
    {
        heroTouchInput.CanBlockRaycast();
        
        myCanvas.enabled = true;
        //Time.timeScale = 0;
        message.text = msg;
    }

    public void Close()
    {
        
        myCanvas.enabled = false;
        if (!ignoreUnblock)
        {
            heroTouchInput.UnblockRaycast();
            //Time.timeScale = 1;

        }
        else ignoreUnblock = false;
        if (stackMessages)
        {
            stackMessages = false;
            if (delayedStack)
            {
                ShowMessage(m_msg2, stackDelayTime);
                delayedStack = false;

            }
            else ShowMessage(m_msg2);
            if (inInventory)
            {
                ignoreUnblock = true;
                inInventory = false;
            }
        }
    }

    public void StackMessage(string msg, bool ignore)
    {
        m_msg2 = msg;
        stackMessages = true;
        inInventory = ignore;
    }

    public void StackMessage(string msg, bool ignore, float delay)
    {
        m_msg2 = msg;
        stackMessages = true;
        inInventory = ignore;
        stackDelayTime = delay;
        delayedStack = true;
    }

    public void IgnoreUnblockRaycast()
    {
        ignoreUnblock = true;
    }
}
