using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour {

    public Text maxHealthValue;
    public Text maxEnergyValue;
    public Text healthRateValue;
    public Text energyRateValue;
    public Text damageValue;
    public Text defenseValue;
    public Text agilityValue;
    public Text critChanceValue;
    public Text energyConsumeValue;
    public Text maxHealthBonus;
    public Text maxEnergyBonus;
    public Text healthRateBonus;
    public Text energyRateBonus;
    public Text damageBonus;
    public Text defenseBonus;
    public Text agilityBonus;
    public Text critChanceBonus;
    public Text energyConsumeBonus;

    public HeroStats stats;

    public void Init()
    {
        maxHealthValue = transform.GetChild(10).gameObject.GetComponent<Text>();
        maxEnergyValue = transform.GetChild(11).gameObject.GetComponent<Text>();
        healthRateValue = transform.GetChild(12).gameObject.GetComponent<Text>();
        energyRateValue = transform.GetChild(13).gameObject.GetComponent<Text>();
        damageValue = transform.GetChild(14).gameObject.GetComponent<Text>();
        defenseValue = transform.GetChild(15).gameObject.GetComponent<Text>();
        agilityValue = transform.GetChild(16).gameObject.GetComponent<Text>();
        critChanceValue = transform.GetChild(17).gameObject.GetComponent<Text>();
        energyConsumeValue = transform.GetChild(18).gameObject.GetComponent<Text>();
        maxHealthBonus = transform.GetChild(19).gameObject.GetComponent<Text>();
        maxEnergyBonus = transform.GetChild(20).gameObject.GetComponent<Text>();
        healthRateBonus = transform.GetChild(21).gameObject.GetComponent<Text>();
        energyRateBonus = transform.GetChild(22).gameObject.GetComponent<Text>();
        damageBonus = transform.GetChild(23).gameObject.GetComponent<Text>();
        defenseBonus = transform.GetChild(24).gameObject.GetComponent<Text>();
        agilityBonus = transform.GetChild(25).gameObject.GetComponent<Text>();
        critChanceBonus = transform.GetChild(26).gameObject.GetComponent<Text>();
        energyConsumeBonus = transform.GetChild(27).gameObject.GetComponent<Text>();
        stats = GameObject.FindGameObjectWithTag("Hero").GetComponent<HeroStats>();

        UpdateValues();
    }

    public void UpdateValues()
    {
        maxHealthValue.text = (50 + (stats.strength * 5)).ToString();
        maxEnergyValue.text = (50 + (stats.inteligence * 5)).ToString();
        //healthRateValue.text = (0.5f + ((float)stats.strength / 10)).ToString();
        healthRateValue.text = ((float)stats.strength / 5).ToString();
        //energyRateValue.text = (2f + ((float)stats.inteligence / 5)).ToString();
        energyRateValue.text = stats.inteligence.ToString();
        damageValue.text = (10 + stats.strength).ToString();
        defenseValue.text = stats.dexterity.ToString();
        agilityValue.text = ((float)stats.dexterity / 5).ToString();
        critChanceValue.text = (10 + ((float)stats.dexterity / 2)).ToString() + "%";
        energyConsumeValue.text = "-" + (2.5f * stats.inteligence).ToString() + "%";

        if (stats.itemHP > 0 || stats.itemStrength > 0)
        {
            int value = stats.itemHP + (stats.itemStrength * 5);
            maxHealthBonus.text = "+" + value.ToString();
        }
        else maxHealthBonus.text = "";

        if (stats.itemEnergy > 0 || stats.itemInteligence > 0)
        {
            int value = stats.itemEnergy + (stats.itemInteligence * 5);
            maxEnergyBonus.text = "+" + value.ToString();
        }
        else maxEnergyBonus.text = "";

        if (stats.itemHPRate > 0 || stats.itemStrength > 0)
        {
            float value = stats.itemHPRate + ((float)stats.itemStrength / 5);
            healthRateBonus.text = "+" + value.ToString();
        }
        else healthRateBonus.text = "";

        if (stats.itemEnergyRate > 0 || stats.itemInteligence > 0)
        {
            float value = stats.itemEnergyRate + stats.itemInteligence;
            energyRateBonus.text = "+" + value.ToString();
        }
        else energyRateBonus.text = "";

        if (stats.itemDamage > 0 || stats.itemStrength > 0)
        {
            float value = stats.itemDamage + ((float)stats.itemStrength);
            damageBonus.text = "+" + value.ToString();
        }
        else damageBonus.text = "";

        if (stats.itemDefense > 0 || stats.itemDexterity > 0)
        {
            float value = stats.itemDefense + ((float)stats.itemDexterity);
            defenseBonus.text = "+" + value.ToString();
        }
        else defenseBonus.text = "";

        if (stats.itemMovement > 0 || stats.itemDexterity > 0)
        {
            float value = stats.itemMovement + ((float)stats.itemDexterity/5);
            agilityBonus.text = "+" + value.ToString();
        }
        else agilityBonus.text = "";

        if (stats.itemCritChance > 0 || stats.itemDexterity > 0)
        {
            float value = stats.itemCritChance + ((float)stats.itemDexterity / 2);
            critChanceBonus.text = "+" + value.ToString() + "%";
        }
        else critChanceBonus.text = "";

        if (stats.itemEnergyConsume < 1 || stats.itemInteligence > 0)
        {
            float value = ((1 - stats.itemEnergyConsume)*100) + (2.5f * stats.itemInteligence);
            energyConsumeBonus.text = "-" + value.ToString() + "%";
        }
        else energyConsumeBonus.text = "";
    }
}
