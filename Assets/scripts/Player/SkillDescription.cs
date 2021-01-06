using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDescription : MonoBehaviour {

    public GameObject title;
    public GameObject command;
    public GameObject description;
    public GameObject damage;
    public GameObject energyConsume;

    private bool _init;

    private void Start()
    {
        Init();
        SetSkillDescription();
    }

    private void Init()
    {
        if (_init) return;

        title = transform.GetChild(0).gameObject;
        command = transform.GetChild(1).gameObject;
        description = transform.GetChild(2).gameObject;
        damage = transform.GetChild(3).gameObject;
        energyConsume = transform.GetChild(4).gameObject;
        

        _init = true;

    }

    public void SetSkillDescription()
    {
        Init();
        title.GetComponent<Text>().text = "";
        command.GetComponent<Text>().text = "";
        description.GetComponent<Text>().text = "";
        damage.GetComponent<Text>().text = "";
        energyConsume.GetComponent<Text>().text = "";
    }

    public void SetSkillDescription(string titleText, 
                                    string commandText, 
                                    string descriptionText, 
                                    string damageText,
                                    string energyConsumeText)
    {
        Init();
        title.GetComponent<Text>().text = titleText;
        command.GetComponent<Text>().text = commandText;
        description.GetComponent<Text>().text = descriptionText;
        damage.GetComponent<Text>().text = damageText;
        energyConsume.GetComponent<Text>().text = energyConsumeText;
    }
}
