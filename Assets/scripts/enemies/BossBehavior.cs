using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBehavior : MonoBehaviour {

    public AudioClip music;
    public HeroLoader heroStartPoint;
    public GameObject orbPrefab;
    public GameObject finishStage;
    public int myID;
    protected GameControl control;
    protected Animator Anim;           // referencia para o controlador de animação
    protected UnityEngine.AI.NavMeshAgent navigator;
    protected Collider myCollider;
    protected bool invokeDefined;
    protected float step;              // 
    public enum stateMachine { idle, walk, attack, die }; // maquina de estados
    public stateMachine state;  // estado atual
    public GameObject target;
    public Vector3 targetPosition;
    protected HeroStats targetStats;
    protected float timerIdle;
    public GameObject deathEmmiter;
    //Rigidbody rb;
    //public float magnitude;
    //public crittersControllerBehav controllerScr;
    // public int type;

    //public gameStats gameControlScript;
    public BossAttributes attributes;           // classe com os atributos do inimigo
    protected bool move;
    // public Vector3 direction;
    protected Vector3 movePosition;
    protected bool submerge;
    public int seed;

    //public float patrolRange;
    protected Vector3 startPosition;
    protected float distanceThrowBack;
    protected bool throwBack;
    protected bool stun;
    protected float timeThrowBack;
    public bool nerfed;
    public float timeToNextPrimaryMagic;
    public float timeToSecondaryMagic;
    public float passedTime;
    public float areaRadius;
    public bool doSecondaryMagic;
    public bool doPrimaryMagic;
    public int primaryMagicShooted;
    public int maxPrimaryMagic;
    public int maxSecondaryMagic;
    //public GameObject primaryMagic;
    //public GameObject secondaryMagic;
    private List<GameObject> secondaryMagicList = new List<GameObject>();
    public Transform[] secondaryMagicLocations;
    public bool[] secondaryMagicLocationsControl;
    public GameObject primaryAttackPosition;
    //public GameObject primaryMagicFeedback;
    //public GameObject secondaryMagicFeedback;
    public bool lockRotation;
    private bool init;
    public float targetDistance;

    protected Canvas enemyCanvas;
    protected Canvas feedbackCanvas;
    protected Text feedbackText;

    public GameObject[] spells;
    public Transform cameraPosition;
    public Transform cameraLookRotation;
     

    protected virtual void Start()
    {
        if (init) return;
        init = true;
        control = GameControl.control;
        target = GameObject.FindGameObjectWithTag("Hero");
        Anim = GetComponent<Animator>();
        //navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        attributes = GetComponent<BossAttributes>();
        enemyCanvas = GetComponentInChildren<Canvas>();
        targetStats = target.GetComponent<HeroStats>();
        myCollider = GetComponent<Collider>();
       
        //timeToSecondaryMagic = 7.0f;
        state = stateMachine.idle;
        //primaryMagicFeedback.SetActive(false);
        //secondaryMagicFeedback.SetActive(false);
        deathEmmiter.SetActive(false);

        feedbackCanvas = transform.Find("FeedbackCanvas").gameObject.GetComponent<Canvas>();
        feedbackText = feedbackCanvas.gameObject.transform.Find("FeedbackText").gameObject.GetComponent<Text>();
        feedbackCanvas.enabled = false;

        //attributes.StartSeed(seed);
        // navigator.speed = attributes.GetVelocity();

        //attributes.currHealth = attributes.maxHealth;
        // Random.seed = (int)Time.time;
        //Random.InitState(seed);
        //transform.Rotate(new Vector3(0.0f, -90.0f, 0.0f));
        startPosition = transform.position;
        //rb = GetComponent<Rigidbody>();
        //  gameControlScript = GameObject.Find("gameController").GetComponent<gameStats>();



        move = false;
        submerge = false;
        invokeDefined = false;
        throwBack = false;
        stun = false;
        distanceThrowBack = 5;
        nerfed = false;
        lockRotation = false;

    }

    public void DefineFinishStage(GameObject finishObj)
    {
        Start();
        finishStage = finishObj;
        if (control.VerifyBoss(myID))
        {
            //finishStage.GetComponent<FinishStagePoint>().ShowFinishPoint();
            if (!control.orbs[myID]) DropOrb();
            else finishStage.GetComponent<FinishStagePoint>().ShowFinishPoint();
            gameObject.SetActive(false);
        }
    }

    void DropOrb()
    {
        
        Vector3 position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z) + transform.forward*3;
        Vector3 force = new Vector3(Random.Range(-4, 4), 10, Random.Range(-4, 4));
        GameObject orb = Instantiate(orbPrefab, position, Quaternion.identity);
        orb.GetComponent<Rigidbody>().AddRelativeForce(force);
        orb.GetComponent<InteractableObject>().Initiate(InteractableObject.tipo.ORB, myID);
        orb.GetComponent<InteractableObject>().FinishStage(finishStage);
    }
    //public void DefineSpawnPoint(GameObject spawner)
    //{
    //    spawnPoint = spawner.GetComponent<enemyRespawner>();
    //}



    protected void FixedUpdate()
    {
        verifyHealth();
        verifyTargetDistance();

        //rb.velocity = Vector3.zero;
        // Aqui quando todos speciais
       // DoSpecial();
        switch (state)
        {
            case stateMachine.idle:
                stateIdle();
                break;
            case stateMachine.walk:
                stateWalk();
                break;
            case stateMachine.attack:
                stateAttack();
                break;
            case stateMachine.die:
                stateDie();
                break;
            default: break;

        }

    }

    // Fica em idle por timerIdle segundos, então começa a andar
    protected virtual void stateIdle()
    {
        //move = false;
        if (!invokeDefined)
        {
            invokeDefined = true;
            timerIdle = Random.Range(3, 6);
            Invoke("startWalking", timerIdle);
        }
    }

    //Estado caminhar
    protected virtual void stateWalk()
    {
        //if (move && !nerfed)
        //{
        //    //navigator.Resume();
        //    navigator.isStopped = false;

        //    //direction = target.transform.position;
        //    //direction.y = transform.position.y;
        //    //transform.LookAt(direction);

        //    //step = Time.deltaTime * attributes.velocity;

        //    ////transform.position += transform.forward * step;
        //    //rb.MovePosition(transform.position+(transform.forward * step));
        //}
        //else
        //{
        //    //navigator.Stop();
        //    navigator.isStopped = true;
        //}
    }

    //Estado Atacar
    protected void stateAttack()
    {
        

        if (target == null || targetStats.GetCurrentHealth() <= 0)
        {
            state = stateMachine.idle;
            Anim.SetBool("attack", false);

        }
        else
        {
            Vector3 lookAtPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            if (!lockRotation)
                transform.LookAt(lookAtPosition);

            if (doSecondaryMagic /*&& passedTime >= timeToSecondaryMagic */)
            {
                // Debug.Log("Faz a segunda");
                Anim.SetBool("secondaryAttack", true);
                doSecondaryMagic = false;
                lockRotation = true;
                //timeToNextPrimaryMagic = Random.Range(2.0f, 3.0f);
                //passedTime = 0;
            }
        }
       // passedTime += Time.deltaTime;

        

        //if(/*passedTime >= timeToNextPrimaryMagic && */ doPrimaryMagic)
        //{

        //    //GameObject magic = Instantiate(primaryMagic, primaryAttackPosition.transform.position, Quaternion.identity);
        //    // magic.GetComponent<BossPrimaryMagic>().Initiate(attributes.GetDamage(), target.transform.position);
        //    //timeToNextPrimaryMagic = Random.Range(1.0f, 2.0f);
        //    //passedTime = 0;
            
        //}



    }

    public virtual void StartSpell(int i)
    {
        GameObject magic;
        switch(i)
        {
            case 0:
                targetPosition = target.transform.position;
                lockRotation = true;
                primaryMagicShooted++;
                magic = Instantiate(spells[i], primaryAttackPosition.transform.position, Quaternion.identity);
                magic.GetComponent<Boss01PrimaryMagic>().Init(this.gameObject, targetPosition);
                lockRotation = false;
                break;
            case 1:
                for (int j = 0; j < maxSecondaryMagic; j++)
                {
                    //Vector3 randomLocation = new Vector3(Random.Range(-areaRadius, areaRadius) + transform.position.x,
                    //                                        transform.position.y,
                    //                                        Random.Range(-areaRadius, areaRadius) + transform.position.z);
                    Vector3 randomLocation = RandomSpellLocation();
                    magic = Instantiate(spells[i], randomLocation, Quaternion.identity);
                    magic.GetComponent<Boss01SecondaryMagic>().Init(this.gameObject);
                    
                    secondaryMagicList.Add(magic);
                }
                ClearSpellLocationControl();
                lockRotation = true;
                break;
            default:
                break;
        }
        
        if (primaryMagicShooted == maxPrimaryMagic)
        {

            doSecondaryMagic = true;
            primaryMagicShooted = 0;
        }

    }

    private Vector3 RandomSpellLocation()
    {
        int i;
        do
        {
            i = Random.Range(0, secondaryMagicLocationsControl.Length);
        }
        while (secondaryMagicLocationsControl[i]);
        
        secondaryMagicLocationsControl[i] = true;
        return secondaryMagicLocations[i].position;
        
    }

    private void ClearSpellLocationControl()
    {
        for(int i = 0; i < secondaryMagicLocationsControl.Length; i++)
        {
            secondaryMagicLocationsControl[i] = false;
        }
    }

    public virtual void EndSpell(int i)
    {
        Anim.SetBool("secondaryAttack", false);
        lockRotation = false;
        if (state != stateMachine.die)
            secondaryMagicList.Clear();
    }

    public virtual void SpellEvent(int e)
    {
        for(int i = 0; i < maxSecondaryMagic; i++)
        {
            secondaryMagicList[i].GetComponent<Boss01SecondaryMagic>().Event();
        }
    }

    public void FeedbackPrimaryMagic()
    {
        //if (state != stateMachine.die)
        //{
        //    primaryMagicFeedback.SetActive(true);
        //    //targetPosition = target.transform.position;
        //    //lockRotation = true;
        //}
    }

    public void FeedbackSecondaryMagic()
    {
        //if (state != stateMachine.die)
        //{
        //    secondaryMagicFeedback.SetActive(true);
        //    for (int i = 0; i < maxSecondaryMagic; i++)
        //    {
        //        Vector3 randomLocation = new Vector3(Random.Range(-areaRadius, areaRadius) + transform.position.x,
        //                                                transform.position.y,
        //                                                Random.Range(-areaRadius, areaRadius) + transform.position.z);
        //        GameObject magic = Instantiate(secondaryMagic, randomLocation, Quaternion.identity);
        //        BossSecondaryMagic sMagicScript = magic.GetComponent<BossSecondaryMagic>();
        //        sMagicScript.LoadMagic(attributes.GetDamage());
        //        secondaryMagicList.Add(magic);
        //    }
        //}
    }

    public void EndSecondaryMagicFeedback()
    {
        //secondaryMagicFeedback.SetActive(false);
    }

    public void CastSecondaryMagic()
    {
        //for(int i = 0; i < secondaryMagicList.Count; i++)
        //{
        //    BossSecondaryMagic sMagicScript = secondaryMagicList[i].GetComponent<BossSecondaryMagic>();
        //    sMagicScript.ActivateMagic();
        //}
        
    }

    public void EndSecondaryMagic()
    {
        //Anim.SetBool("secondaryAttack", false);
        //lockRotation = false;
        //if(state != stateMachine.die)
        //    secondaryMagicList.Clear();

    }

    public void CastPrimaryMagic()
    {
        //targetPosition = target.transform.position;
        //lockRotation = true;
        //primaryMagicFeedback.SetActive(false);
        //primaryMagicShooted++;
        //GameObject magic = Instantiate(primaryMagic, primaryAttackPosition.transform.position, Quaternion.identity);
        //magic.GetComponent<BossPrimaryMagic>().Initiate(attributes.GetDamage(), targetPosition);
        //lockRotation = false;
        //if (primaryMagicShooted == maxPrimaryMagic)
        //{
            
        //    doSecondaryMagic = true;
        //    primaryMagicShooted = 0;
        //}
    }

    //Estado Morto
    protected void stateDie()
    {
        if (submerge)
        {
            transform.position -= Vector3.up * 0.05f;
        }
    }

    // Mudança para o estado caminhar
    protected void startWalking()
    {
        //invokeDefined = false;
        //if (state != stateMachine.attack)
        //{
        //    // Randomiza uma posição de movimento
        //    movePosition = GetNewPosition();
        //    navigator.SetDestination(movePosition);
        //    //move = true;

        //    transform.LookAt(movePosition);
        //    Anim.SetBool("walk", true);
            

        //}
    }

    protected Vector3 GetNewPosition()
    {
        //Vector3 newPosition;
        //do
        //{
        //    newPosition = new Vector3(transform.position.x + Random.Range(-4, 4),
        //                                transform.position.y,
        //                                transform.position.z + Random.Range(-4, 4));
        //    state = stateMachine.walk;
        //    UnityEngine.AI.NavMeshHit hit;
        //    UnityEngine.AI.NavMesh.SamplePosition(newPosition, out hit, 40.0f, 1);
        //    //newPosition = new Vector3(hit.position.x, transform.position.y, hit.position.z);
        //    newPosition = hit.position;


        //} while (Vector3.Distance(newPosition, startPosition) >= attributes.GetPatrolRange());
        ////bug.Log("Fora da Area");
        ////else Debug.Log("Dentro da Area");

        //return newPosition;
        return Vector3.zero;
    }

    // metodo usado pela animação de caminhar em pontos onde a animação não se move
    public void stopMove()
    {
       // move = !move;
    }

    // Metodo usado pela animação de ataque para causar dano
    public void Hit()
    {
        //targetStats.TakeDamage(this.gameObject, attributes.GetDamage());
    }

    // Faz alguma coisa quando termina animação de morte
    public void Dead()
    {
        deathEmmiter.SetActive(false);
        feedbackCanvas.enabled = false;
        finishStage.GetComponent<FinishStagePoint>().ShowFinishPoint();
        control.BossDefeated(myID);
        DropOrb();
        heroStartPoint.PlayMusic();
        attributes.Loot();
        // Solta orb
    }

    public void Submerge()
    {
        submerge = true;
        //navigator.enabled = false;
        //if (spawnPoint)
        //{
        //    spawnPoint.RemoveFromList(this.gameObject);
        //}
        Destroy(gameObject, 3);
        enemyCanvas.enabled = false;
        CameraFollowing camScript = Camera.main.gameObject.GetComponent<CameraFollowing>();
        if (camScript.VerifyLocked())
        {
            //camScript.UnlockCamera();
            camScript.UnlockAndRotate();

        }
    }

    // Verifica a distancia do alvo, se necessário inicia o ataque
    protected void verifyTargetDistance()
    {
        // Verifica se morreu ou está nerfed
        // então não pode mover nem atacar
        if (state != stateMachine.die /*&& state != stateMachine.attack*/ && targetStats.GetCurrentHealth() > 0)
        {
            targetDistance = Vector3.Distance(target.transform.position, transform.position);
            if (Vector3.Distance(target.transform.position, transform.position) <= attributes.GetCombatArea())
            {
                CameraFollowing camScript = Camera.main.gameObject.GetComponent<CameraFollowing>();
                if(!camScript.VerifyLocked())
                {
                    //camScript.LockCamera(transform.position, new Vector3(0.0f, 26.0f, -17.5f));
                    camScript.LockAndRotate(cameraPosition.position, cameraLookRotation.position, true);
                    attributes.ShowHealth();
                    targetStats.LockHUD(true);
                    control.ChangeMusic(music);
                }
            }
            else
            {
                CameraFollowing camScript = Camera.main.gameObject.GetComponent<CameraFollowing>();
                if (camScript.VerifyLocked())
                {
                    //camScript.UnlockCamera();
                    camScript.UnlockAndRotate();
                    attributes.HideHealth();
                    targetStats.LockHUD(false);
                    heroStartPoint.PlayMusic();
                }
            }

            if (Vector3.Distance(target.transform.position, transform.position) <= attributes.GetVisibilityRange())
            {
                // Consegue ver o heroi
                //timeToNextPrimaryMagic = Random.Range(2.0f, 3.0f);
                //doPrimaryMagic = true;
                //passedTime = 0;
                if (state != stateMachine.attack)
                {
                    state = stateMachine.attack;
                    Vector3 lookAtPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                    transform.LookAt(lookAtPosition);
                    Anim.SetBool("attack", true);
                }
            }
            
            else
            {
                state = stateMachine.idle;
                Anim.SetBool("attack", false);
            }
        }
    }

    protected void verifyHealth()
    {
        if (attributes.GetCurrentHealth() <= 0 && state != stateMachine.die)
        {
            
            state = stateMachine.die;
            deathEmmiter.SetActive(true);
            attributes.CombatIcon();
            CameraFollowing camScript = Camera.main.gameObject.GetComponent<CameraFollowing>();
            if (camScript.VerifyLocked())
            {
                //camScript.UnlockCamera();
                camScript.UnlockAndRotate();
            }
            Anim.SetTrigger("death");
            Anim.SetBool("secondaryAttack", false);
            Anim.SetBool("attack", false);
            //if (secondaryMagicList.Count > 0)
            //{
            //    for (int i = 0; i < secondaryMagicList.Count; i++)
            //    {
            //        BossSecondaryMagic sMagicScript = secondaryMagicList[i].GetComponent<BossSecondaryMagic>();
            //        sMagicScript.DispelMagic();
            //    }
            //}
            attributes.Select(false);
            secondaryMagicList.Clear();
            //primaryMagicFeedback.SetActive(false);
            //secondaryMagicFeedback.SetActive(false);
            targetStats.GiveXP(attributes.GetRewardPoints());
            feedbackText.text = attributes.GetRewardPoints().ToString() + " XP";
            feedbackCanvas.enabled = true;
            attributes.HideHealth();
            // navigator.isStopped = true;
            myCollider.enabled = false;
            
           // Invoke("Submerge", 3);
        }
    }

    protected void DoSpecial()
    {
        
    }

    public void ThrowBack(float t)
    {
        
    }

    protected void DisableThrowBack()
    {
        

    }

    public void Stun(float t)
    {
        
    }

    protected void DisableStun()
    {
        
    }

    protected void DisableNerf()
    {
        
    }

    protected void Nerf()
    {

    }
}
