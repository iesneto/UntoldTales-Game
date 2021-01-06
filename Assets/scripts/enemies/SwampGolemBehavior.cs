using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwampGolemBehavior : ZombieBehavior {

    public float specialRadius;
    public float trumbleTime;
    public ParticleSystem trumbleParticles;
    public float trumbleDefenseWeakened;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Hero");
        attributes = GetComponent<SwampGolemAttributes>();
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

    public void Trumble()
    {
        trumbleParticles.Play();

        
    }


    // metodo utilizado para aplicar no jogador o efeito do especial, para combinar com a animação
    public void SpecialEffect()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance <= specialRadius)
        {
            target.GetComponent<HeroBehavior>().EnemySpecial(HeroBehavior.enemySpecial.reduceDef, trumbleTime, trumbleDefenseWeakened, this.gameObject);
        }
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
