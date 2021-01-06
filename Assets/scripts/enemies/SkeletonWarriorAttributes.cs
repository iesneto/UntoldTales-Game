using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkeletonWarriorAttributes : EnemyAttributes
{

    protected override void Awake()
    {
        base.Awake();
        healthBar = transform.Find("EnemyCanvas/HealthBG/Health").GetComponent<Image>();
        healthBarBG = transform.Find("EnemyCanvas/HealthBG").GetComponent<Image>();
        //combatIcon = transform.Find("EnemyCanvas/CombatIcon").GetComponent<Image>();
        zombieScr = GetComponent<SkeletonWarriorBehavior>();
        //maxHealth = 60;
        currHealth = maxHealth;
        //velocity = 1.4f;
        //rewardPoints = 85;
        //dropChance = 20;
        //attackRate = 1;
       // minDamage = 15;
       // maxDamage = 20;
        //attackRange = 2.0f;
        //visibilityRange = 8;
        patrolRange = 6;
        //defense = 2;
        currentDefense = defense;
        //numberItems = Random.Range(0, 2);
        //itemsRarity = 1;

        //specialCoolDown = 15;
        //specialRangeMin = 1;
        //specialRangeMax = 2;
        //canBeNerfed = true;
    }
}
