using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    public float damage;
    protected enum collisionType { HERO, FLOOR}
    protected GameObject parent;

    private void Update()
    {
        Behavior();
    }

    public virtual void Init(GameObject p)
    {
        parent = p;
    }

    public virtual void Init(GameObject p, Vector3 pos)
    {
        parent = p;
    }

    public virtual void Stop()
    {

    }

    protected virtual void Behavior()
    {

    }

    protected virtual void TriggerCollision(collisionType col, GameObject other)
    {

    }

    public virtual void Event()
    {

    }

    public virtual void Event(int e)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.gameObject.tag == "Hero")
        {
            TriggerCollision(collisionType.HERO, other.gameObject);
            //other.gameObject.GetComponent<HeroStats>().TakeDamage(null, damage);
            //Destroy(gameObject);

        }
        if(other.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            //Debug.Log("Colidiu chao");
            TriggerCollision(collisionType.FLOOR, null);
        }
        
    }
}
