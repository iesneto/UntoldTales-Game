using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemDataScriptable", order = 1)]
public class DataItemScriptableObject : ScriptableObject
{
    public enum modifier { XP, STRENGTH, DEXTERITY, INTELIGENCE, HP,
                           ENERGY, HPRATE, ENERGYRATE, DAMAGE, CRITCHANCE,
                            DEFENSE, AGILITY, ENERGYCONSUME, GOLDGAIN, HPRESTORE, ENERGYRESTORE}
    public enum QualityType { DAMAGED, OLD, NORMAL, GOOD, GREAT, FLAWLESS}


    public int id;
    public string title;
    public modifier type;
    public int value;
    public string description;
    public bool stackable;
    public QualityType rarity;
    public Sprite imageSprite;
    public Sprite imageQuality;
    public DataItemScriptableObject upgradeItem;
}
