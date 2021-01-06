using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGolemRock : MonoBehaviour {

    private float rockDamage;
    private GameObject father;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    public void SetDamage(float t, GameObject f)
    {
        rockDamage = t;
        father = f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;

        if (target.tag == "Hero")
        {
            //target.GetComponent<HeroBehavior>().EnemySpecial(HeroBehavior.enemySpecial.entangle, rockDamage, 0);
            target.GetComponent<HeroStats>().TakeDamage(father, rockDamage);
            Destroy(gameObject);
        }
    }

}
