using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceGolemAttributes : EnemyAttributes
{
    protected override void Awake()
    {
        base.Awake();
        healthBar = transform.Find("EnemyCanvas/HealthBG/Health").GetComponent<Image>();
        healthBarBG = transform.Find("EnemyCanvas/HealthBG").GetComponent<Image>();
        //combatIcon = transform.Find("EnemyCanvas/CombatIcon").GetComponent<Image>();
        zombieScr = GetComponent<IceGolemBehavior>();
        //maxHealth = 90;
        currHealth = maxHealth;
        //velocity = 1.5f;
        //rewardPoints = 50;
        //dropChance = 15;
        //attackRate = 1;
        //minDamage = 25;
        //maxDamage = 35;
        //attackRange = 2.0f;
        //visibilityRange = 10;
        patrolRange = 6;
        //defense = 6;
        currentDefense = defense;
        // numberItems = Random.Range(0, 2);
        // itemsRarity = 1;

        //specialCoolDown = 5;
        //specialRangeMin = 3;
        //specialRangeMax = 9;
        //canBeNerfed = true;
    }
}
