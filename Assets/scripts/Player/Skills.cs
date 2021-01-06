using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Skills : MonoBehaviour, IPointerClickHandler
{
    public Color selectColor;
    public Color upgradeColor;
    public Color startColor;
    public Outline outline;
    public GameObject skillWindow;
    public bool upgraded;
    public GameObject skillDescriptionPanel;
    public string title;
    public string command;
    public string description;
    public string damage;
    public string energyConsume;

    public void Init(GameObject o, int level)
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        upgraded = false;
        ResetColor();
        if(level > 0)
        {
            upgraded = true;
            ChangeToUpgradeColor();
        }
        skillWindow = o;
    }

    public void Upgrade()
    {
        DisableOutline();
        upgraded = true;
        ChangeToUpgradeColor();
    }


    public void ResetColor()
    {
        if (upgraded)
        {
            ChangeToUpgradeColor();
        }
        else GetComponent<Image>().color = startColor;
        skillDescriptionPanel.GetComponent<SkillDescription>().SetSkillDescription();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeToSelectedColor();
        skillWindow.GetComponent<SkillPanel>().SelectSkill(gameObject);
        
    }

    public void ChangeToUpgradeColor()
    {
        GetComponent<Image>().color = upgradeColor;
    }

    public void ChangeToSelectedColor()
    {
        GetComponent<Image>().color = selectColor;
        skillDescriptionPanel.GetComponent<SkillDescription>().SetSkillDescription(title, command, description, damage, energyConsume);
    }

    public bool canBeUpgraded()
    {
        return outline.enabled;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }
}
