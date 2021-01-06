using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonArcherBehavior : ZombieBehavior {

    public GameObject myArrow;
    public GameObject arrowPrefab;
    public Transform arrowPosition;
    public float arrowTime;
    public float arrowSpeed;

    protected override void Start()
    {
        myArrow.GetComponent<Renderer>().enabled = false;
        anim = GetComponent<Animator>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Hero");
        attributes = GetComponent<SkeletonArcherAttributes>();
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

    public void ThrowArrow()
    {
        anim.SetBool("recoil", true);
        myArrow.GetComponent<Renderer>().enabled = false;
        GameObject special = Instantiate(arrowPrefab, arrowPosition.position, arrowPosition.rotation);
        Vector3 direction = (target.transform.position - arrowPosition.position).normalized;
        special.GetComponent<SkeletonArcherArrow>().SetDamage(this.gameObject, gameObject.GetComponent<SkeletonArcherAttributes>().GetDamage());
        special.GetComponent<Rigidbody>().AddForce(arrowSpeed * direction);
    }

    public void FinishAttack()
    {
        anim.SetBool("recoil", false);
        

    }

    public void PickArrow()
    {
        myArrow.GetComponent<Renderer>().enabled = true;
    }


}
