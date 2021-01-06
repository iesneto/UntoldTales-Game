using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceGolemBehavior : ZombieBehavior
{
    public GameObject blizzardParticles;
    public float blizzardTime;
    public float blizzardSpeedReduction;
    public IceGolemBlizzard blizzardScript;
    public Transform specialPos;
    public float animSpeedControl;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Hero");
        attributes = GetComponent<IceGolemAttributes>();
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
        if (state != stateMachine.special)
        {
            state = stateMachine.special;

            navigator.isStopped = true;
            transform.LookAt(target.transform.position);
            anim.SetBool("walk", false);
            anim.SetBool("attack", false);
            anim.SetBool("special", true);
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

    public void StartParticles()
    {
        blizzardParticles.GetComponent<ParticleSystem>().Play();
    }

    public void Throw()
    {
        
        anim.speed = animSpeedControl;
        blizzardScript.StartBlizzardCollider();

        //GameObject special = Instantiate(rock, specialPos.position, specialPos.rotation);
        //Vector3 direction = (target.transform.position - specialPos.position).normalized;
        //special.GetComponent<StoneGolemRock>().SetDamage(attributes.GetDamage() * rockDamageModifier, this.gameObject);
        //special.GetComponent<Rigidbody>().AddForce(rockSpeed * direction);
    }

    public void BlizzardHit()
    {
        target.GetComponent<HeroBehavior>().EnemySpecial(HeroBehavior.enemySpecial.blizzard, blizzardTime, blizzardSpeedReduction, gameObject);
    }

    public void RestoreAnimationSpeed()
    {
        anim.speed = attributes.GetVelocity();
        blizzardParticles.GetComponent<ParticleSystem>().Stop();
        blizzardScript.StopBlizzardCollider();
    }

    

    protected override void FinishSpecialAttack()
    {
        //base.FinishSpecialAttack();
        anim.SetBool("special", false);
        state = stateMachine.idle;
        specialReady = false;
        specialTime = 0;
        
    }
}
