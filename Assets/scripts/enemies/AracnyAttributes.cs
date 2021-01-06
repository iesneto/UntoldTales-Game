using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AracnyAttributes : EnemyAttributes {


    protected override void Awake()
    {
        base.Awake();
        healthBar = transform.Find("EnemyCanvas/HealthBG/Health").GetComponent<Image>();
        healthBarBG = transform.Find("EnemyCanvas/HealthBG").GetComponent<Image>();
        //combatIcon = transform.Find("EnemyCanvas/CombatIcon").GetComponent<Image>();
        zombieScr = GetComponent<AracnyBehavior>();
       // maxHealth = 40;
        currHealth = maxHealth;
      //  velocity = 1.6f;
       // rewardPoints = 20;
       // dropChance = 10;
        //attackRate = 1;
       // minDamage = 12;
      //  maxDamage = 15;
        //attackRange = 2.0f;
        //visibilityRange = 8;
        patrolRange = 6;
      //  defense = 0;
        currentDefense = defense;
        //numberItems = Random.Range(0, 2);
       // itemsRarity = 1;

        //specialCoolDown = 10;
        //specialRangeMin = 3;
        //specialRangeMax = 7;
        //canBeNerfed = true;
    }
}
