using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01PrimaryMagic : EnemySpell
{
    public GameObject spell;
    public ParticleSystem feedback;
    public ParticleSystem ball;
    public ParticleSystem collision;
    public Vector3 targetPosition;
    private Vector3 shootDirection;
    public float velocity;
    public bool go;
    public float delay;
    public float destroyTime;

    public override void Init(GameObject p, Vector3 pos)
    {
        base.Init(p, pos);
        damage = parent.GetComponent<BossAttributes>().GetDamage();
        targetPosition = pos;
        StartCoroutine("DelayStart");
        shootDirection = (targetPosition - transform.position).normalized;
        transform.LookAt(targetPosition);
        Destroy(gameObject, 4);

    }

    IEnumerator DelayStart()
    {
        
        yield return new WaitForSeconds(delay);
        go = true;
    }

    protected override void Behavior()
    {
        base.Behavior();
        if (go)
        {
            float step = Time.deltaTime * velocity;
            transform.position += shootDirection * step;
            //if (Vector3.Distance(transform.position, targetPosition) <= 0.2f)
            //{
            //    go = false;
            //    Destroy(gameObject);
            //}
        }
    }

    

    protected override void TriggerCollision(collisionType col, GameObject other)
    {
        switch(col)
        {
            case collisionType.HERO:
                other.gameObject.GetComponent<HeroStats>().TakeDamage(null, damage);
                Stop();
                break;
            case collisionType.FLOOR:
                Stop();
                break;
            default:
                break;
        }
    }

    public override void Stop()
    {
        if (go)
        {
            base.Stop();
            go = false;
            collision.Play();
            ball.Stop();
            Destroy(gameObject, destroyTime);
        }

    }


}
