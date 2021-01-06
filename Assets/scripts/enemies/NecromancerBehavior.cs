using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NecromancerBehavior : ZombieBehavior
{
    public GameObject projectile;
    public float projectileSpeed;
    public float respawnDistance;

    public Transform specialPos;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Hero");
        attributes = GetComponent<NecromancerAttributes>();
        enemyCanvas = GetComponentInChildren<Canvas>();
        targetStats = target.GetComponent<HeroStats>();
        myCollider = GetComponent<Collider>();
        feedbackCanvas = transform.Find("FeedbackCanvas").gameObject.GetComponent<Canvas>();
        feedbackText = feedbackCanvas.gameObject.transform.Find("FeedbackText").gameObject.GetComponent<Text>();
        feedbackCanvas.enabled = false;
        state = stateMachine.idle;

        navigator.speed = attributes.GetVelocity();
        anim.speed = attributes.GetVelocity();
        startPosition = transform.position;

        move = false;
        submerge = false;
        invokeDefined = false;
        throwBack = false;
        stun = false;
        distanceThrowBack = 4;
        nerfed = false;
        specialTime = attributes.GetSpecialCoolDown();

    }

    

    protected override void SpecialAttack()
    {
        //base.SpecialAttack();
        if (state != stateMachine.special && attributes.GetAttackedProperty())
        {
            state = stateMachine.special;

            //navigator.isStopped = true;
            
            transform.LookAt(target.transform.position);
            anim.SetBool("walk", false);
            anim.SetBool("attack", false);
            anim.SetBool("special", true);
            verifyTargetDistanceTimerMax = 5;
        }

    }

    protected override void stateSpecial()
    {
        //base.stateSpecial();
        if (targetStats.GetCurrentHealth() >= 0)
        {
            //if (Vector3.Distance(target.transform.position, transform.position) <= attributes.GetSpecialRangeMin()
            //|| Vector3.Distance(target.transform.position, transform.position) >= attributes.GetSpecialRangeMax())
            //{
            //    FinishSpecialAttack();
            //}
            transform.LookAt(target.transform.position);
        }
        else
        {
            FinishSpecialAttack();
        }
    }

    protected Vector3 GetRespawnPosition()
    {
        Vector3 newPosition;
        do
        {
            newPosition = new Vector3(transform.position.x + Random.Range(-respawnDistance, respawnDistance),
                                        transform.position.y,
                                        transform.position.z + Random.Range(-respawnDistance, respawnDistance));
            

            UnityEngine.AI.NavMeshHit hit;
            UnityEngine.AI.NavMesh.SamplePosition(newPosition, out hit, 40.0f, 1);
            //newPosition = new Vector3(hit.position.x, transform.position.y, hit.position.z);
            newPosition = hit.position;


        } while (Vector3.Distance(newPosition, startPosition) <= respawnDistance/2);
        //bug.Log("Fora da Area");
        //else Debug.Log("Dentro da Area");

        return newPosition;
    }

    public void Teleport()
    {
        //Desaparecer
        attributes.Disapear(true);

        // calcula nova posicao
        Vector3 newPos = GetRespawnPosition();

        navigator.speed = 1000000;
        navigator.SetDestination(newPos);
        navigator.isStopped = false;
        transform.position = newPos;
        //myCollider.enabled = false;
    }

    public void Reapear()
    {
        attributes.Disapear(false);
    }

    public void Throw()
    {
        GameObject special = Instantiate(projectile, specialPos.position, specialPos.rotation);
        Vector3 direction = (target.transform.position - specialPos.position).normalized;
        special.GetComponent<StoneGolemRock>().SetDamage(attributes.GetDamage(), this.gameObject);
        special.GetComponent<Rigidbody>().AddForce(projectileSpeed * direction);
    }

    protected override void FinishSpecialAttack()
    {
        //base.FinishSpecialAttack();
        anim.SetBool("special", false);
        attributes.SetAttackedProperty(false);

        state = stateMachine.idle;
        specialReady = false;
        specialTime = 0;
        navigator.speed = attributes.GetVelocity();
        verifyTargetDistanceTimerMax = 0.5f;
        //myCollider.enabled = true;
    }
}
