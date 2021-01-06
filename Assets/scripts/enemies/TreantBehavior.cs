using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TreantBehavior : ZombieBehavior {

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Hero");
        attributes = GetComponent<TreantAttributes>();
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

    
}
