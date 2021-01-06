using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {

    public Text strengthValue;
    public Text dexterityValue;
    public Text inteligenceValue;
    public Text strengthBonus;
    public Text dexterityBonus;
    public Text inteligenceBonus;
    public HeroStats stats;
    public Text levelValue;
    public Image xpBarFill;
    public Text xpBarText;

    public void Init()
    {
        strengthValue = transform.GetChild(5).gameObject.GetComponent<Text>();
        dexterityValue = transform.GetChild(6).gameObject.GetComponent<Text>();
        inteligenceValue = transform.GetChild(7).gameObject.GetComponent<Text>();
        strengthBonus = transform.GetChild(8).gameObject.GetComponent<Text>();
        dexterityBonus = transform.GetChild(9).gameObject.GetComponent<Text>();
        inteligenceBonus = transform.GetChild(10).gameObject.GetComponent<Text>();
        levelValue = transform.GetChild(12).gameObject.GetComponent<Text>();
        xpBarFill = transform.GetChild(13).GetChild(0).gameObject.GetComponent<Image>();
        xpBarText = transform.GetChild(13).GetChild(1).gameObject.GetComponent<Text>();
        stats = GameObject.FindGameObjectWithTag("Hero").GetComponent<HeroStats>();

        UpdateValues();
    }

    public void UpdateValues()
    {
        strengthValue.text = stats.strength.ToString();
        dexterityValue.text = stats.dexterity.ToString();
        inteligenceValue.text = stats.inteligence.ToString();
        levelValue.text = stats.level.ToString();
        if(stats.itemStrength > 0)
            strengthBonus.text = "+" + stats.itemStrength.ToString();
        else strengthBonus.text = "";

        if (stats.itemDexterity> 0)
            dexterityBonus.text = "+" + stats.itemDexterity.ToString();
        else dexterityBonus.text = "";

        if (stats.itemInteligence > 0)
            inteligenceBonus.text = "+" + stats.itemInteligence.ToString();
        else inteligenceBonus.text = "";
    }

    public void UpdateXPBar()
    {
        xpBarFill.fillAmount = stats.experience / stats.nextExperienceLevel;
        xpBarText.text = stats.experience.ToString() + "/" + stats.nextExperienceLevel.ToString(); 
    }
}
