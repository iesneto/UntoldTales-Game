using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsInfoPanel : MonoBehaviour {

    public GameObject aboutTextObj;
    public Text aboutText;
    public GameObject selectedInfo;

    public void Start()
    {
        aboutTextObj = transform.GetChild(1).gameObject;
        aboutText = transform.GetChild(1).gameObject.GetComponent<Text>();
        aboutText.text = "";
    }

    public void SetText(GameObject obj, string msg)
    {
        if (selectedInfo == null)
        {
            aboutText.text = msg;
            selectedInfo = obj;
            selectedInfo.GetComponent<AttributeInfo>().Select();
        }
        else if(selectedInfo == obj)
        {
            selectedInfo.GetComponent<AttributeInfo>().Deselect();
            aboutText.text = "";
            selectedInfo = null;
        }
        else
        {
            selectedInfo.GetComponent<AttributeInfo>().Deselect();
            selectedInfo = obj;
            aboutText.text = msg;
            selectedInfo.GetComponent<AttributeInfo>().Select();
        }
    }
}
