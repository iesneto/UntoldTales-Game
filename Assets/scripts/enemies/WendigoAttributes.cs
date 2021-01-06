using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WendigoAttributes : EnemyAttributes {


    protected override void Awake()
    {
        base.Awake();
        healthBar = transform.Find("EnemyCanvas/HealthBG/Health").GetComponent<Image>();
        healthBarBG = transform.Find("EnemyCanvas/HealthBG").GetComponent<Image>();
        //combatIcon = transform.Find("EnemyCanvas/CombatIcon").GetComponent<Image>();
        zombieScr = GetComponent<WendigoBehavior>();
        //maxHealth = 170;
        currHealth = maxHealth;
        //velocity = 1.3f;
        //rewardPoints = 100;
        //dropChance = 20;
        //attackRate = 1;
        //minDamage = 40;
       // maxDamage = 50;
        //attackRange = 2.0f;
        //visibilityRange = 8;
        patrolRange = 6;
        //defense = 2;
        currentDefense = defense;
       // numberItems = Random.Range(0, 2);
       // itemsRarity = 1;

        //specialCoolDown = 10;
        //specialRangeMin = 3;
        //specialRangeMax = 7;
        //canBeNerfed = false;
    }
}
