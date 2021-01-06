using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherArrow : MonoBehaviour {

    protected float damage;
    protected GameObject father;

    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    public void SetDamage(GameObject _father, float _damage)
    {
        father = _father;
        damage = _damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.isTrigger)
            return;

        GameObject target = collision.gameObject;



        if (target.tag == "Hero")
        {
            //target.GetComponent<HeroBehavior>().EnemySpecial(HeroBehavior.enemySpecial.wrapped, webTime, 0, father);
            target.GetComponent<HeroStats>().TakeDamage(father, damage);
            Destroy(gameObject);
        }
    }
}
