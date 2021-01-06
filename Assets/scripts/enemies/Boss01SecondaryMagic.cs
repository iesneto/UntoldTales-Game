using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01SecondaryMagic : EnemySpell
{
    public GameObject spell;
    public ParticleSystem initialRing;
    public ParticleSystem fragments;
    public ParticleSystem spears;
    public SphereCollider mcollider;
    public float killTime;
    public bool hit;


    public override void Init(GameObject p)
    {
        base.Init(p);
        damage = parent.GetComponent<BossAttributes>().GetDamage();
        initialRing.Play();
        fragments.Stop();
        spears.Stop();
        mcollider.enabled = false;
        hit = false;
        //targetPosition = pos;
        // StartCoroutine("DelayStart");
        //shootDirection = (targetPosition - transform.position).normalized;
        // transform.LookAt(targetPosition);
        Destroy(gameObject, killTime);

    }

    //IEnumerator DelayStart()
    //{

    //    yield return new WaitForSeconds(delay);
    //    go = true;
    //}

    protected override void Behavior()
    {
        base.Behavior();
        //if (go)
        //{
        //    float step = Time.deltaTime * velocity;
        //    transform.position += shootDirection * step;
        //    //if (Vector3.Distance(transform.position, targetPosition) <= 0.2f)
        //    //{
        //    //    go = false;
        //    //    Destroy(gameObject);
        //    //}
        //}
    }

    public override void Event()
    {
        base.Event();
        fragments.Play();
        spears.Play();
        mcollider.enabled = true;

    }

    protected override void TriggerCollision(collisionType col, GameObject other)
    {
        
        if(col == collisionType.HERO)
        {
                other.gameObject.GetComponent<HeroStats>().TakeDamage(null, damage);
                mcollider.enabled = false;
            Debug.Log("Hit");
        }
        
    }

    public override void Stop()
    {
        //if (go)
        //{
        //    base.Stop();
        //    go = false;
        //    collision.Play();
        //    ball.Stop();
        //    Destroy(gameObject, destroyTime);
        //}

    }
}
