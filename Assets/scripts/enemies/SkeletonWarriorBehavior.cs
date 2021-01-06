using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkeletonWarriorBehavior : ZombieBehavior
{

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Hero");
        attributes = GetComponent<SkeletonWarriorAttributes>();
        enemyCanvas = GetComponentInChildren<Canvas>();
        targetStats = target.GetComponent<HeroStats>();
        myCollider = GetComponent<Collider>();

        feedbackCanvas = transform.Find("FeedbackCanvas").gameObject.GetComponent<Canvas>();
        feedbackText = feedbackCanvas.gameObject.transform.Find("FeedbackText").gameObject.GetComponent<Text>();
        feedbackCanvas.enabled = false;
        state = stateMachine.idle;

        //attributes.StartSeed(seed);
        navigator.speed = attributes.GetVelocity();
        anim.speed = attributes.GetVelocity();
        //attributes.currHealth = attributes.maxHealth;
        // Random.seed = (int)Time.time;
        //Random.seed = seed;
        //transform.Rotate(new Vector3(0.0f, -90.0f, 0.0f));
        startPosition = transform.position;
        //rb = GetComponent<Rigidbody>();
        //  gameControlScript = GameObject.Find("gameController").GetComponent<gameStats>();



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
    //    //move = false;
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
    //        && Vector3.Distance(target.transform.position, transform.position) <= attributes.GetSpecialRangeMax())
    //    {
    //        Debug.Log(this.name + ": Special");
    //        specialReady = false;
    //        specialTime = 0;
    //    }

    //    if (nerfed)
    //    {
    //        //navigator.Stop();
    //        navigator.isStopped = true;

    //        //direction = target.transform.position;
    //        //direction.y = transform.position.y;
    //        //transform.LookAt(direction);

    //        //step = Time.deltaTime * attributes.velocity;

    //        ////transform.position += transform.forward * step;
    //        //rb.MovePosition(transform.position+(transform.forward * step));
    //    }
    //    else
    //    {
    //        //navigator.Resume();
    //        navigator.isStopped = false;
    //    }
    //}
}
