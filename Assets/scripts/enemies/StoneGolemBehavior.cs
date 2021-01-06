using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoneGolemBehavior : ZombieBehavior {

    public GameObject rock;
    public float rockSpeed;
    public float rockDamageModifier;
    public Transform specialPos;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Hero");
        attributes = GetComponent<StoneGolemAttributes>();
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

    //protected override void stateIdle()
    //{

    //    if (!invokeDefined)
    //    {
    //        invokeDefined = true;
    //        timerIdle = Random.Range(3, 6);
    //        Invoke("startWalking", timerIdle);
    //    }
    //}

    //public void ReturnToIdle()
    //{
    //    anim.SetBool("idle02", false);
    //}

    //protected override void stateWalk()
    //{
    //    if (Vector3.Distance(target.transform.position, transform.position) >= attributes.GetSpecialRangeMin()
    //         && Vector3.Distance(target.transform.position, transform.position) <= attributes.GetSpecialRangeMax()
    //         && specialReady)
    //    {
    //        Debug.Log(this.name + ": Special");
    //        specialReady = false;
    //        specialTime = 0;
    //    }
    //    if (nerfed)
    //    {

    //        navigator.isStopped = true;


    //    }
    //    else
    //    {

    //        navigator.isStopped = false;
    //    }
    //}

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

    public void Throw()
    {
        GameObject special = Instantiate(rock, specialPos.position, specialPos.rotation);
        Vector3 direction = (target.transform.position - specialPos.position).normalized;
        special.GetComponent<StoneGolemRock>().SetDamage(attributes.GetDamage() * rockDamageModifier, this.gameObject);
        special.GetComponent<Rigidbody>().AddForce(rockSpeed * direction);
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
