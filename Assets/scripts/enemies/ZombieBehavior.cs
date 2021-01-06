// Classe que determina o comportamento do zumbi Joe

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ZombieBehavior : MonoBehaviour {

    public float stopWalkAnimationThreshold;
    protected float currentVelocity;
    protected bool verifyBloqued;
    protected float timeBloqued;
    protected float verifyTargetDistanceTimer;
    public float verifyTargetDistanceTimerMax;
    public bool attacking;

    //protected GameControl control;
    protected Animator anim;           // referencia para o controlador de animação
    public float animationSpeed;
    protected UnityEngine.AI.NavMeshAgent navigator;
    protected Collider myCollider;
    protected bool invokeDefined;
    protected float step;              // 
    public enum stateMachine { idle, walk, attack, die, special }; // maquina de estados
    public stateMachine state;  // estado atual
    protected GameObject target;
    protected HeroStats targetStats;
    protected float timerIdle;
    //Rigidbody rb;
    //public float magnitude;
    //public crittersControllerBehav controllerScr;
   // public int type;
    
    //public gameStats gameControlScript;
    protected EnemyAttributes attributes;           // classe com os atributos do inimigo
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
    protected bool nerfed;
    public bool specialReady;
    protected float specialTime;

    
    

    protected enemyRespawner spawnPoint;

    
    protected Canvas enemyCanvas;
    protected Canvas feedbackCanvas;
    protected Text feedbackText;

    protected bool loot;

    

    protected virtual void Start()
    {
        
        target = GameObject.FindGameObjectWithTag("Hero");
        anim = GetComponent<Animator>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        attributes = GetComponent<EnemyAttributes>();
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
        Random.InitState(seed);
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
        specialTime = attributes.GetSpecialCoolDown();


        
    }

    public void DefineSpawnPoint(GameObject spawner)
    {
        spawnPoint = spawner.GetComponent<enemyRespawner>();
    }
    
    public void Initialize(bool _loot)
    {
        loot = _loot;
        Invoke("InitializeAttributes", 1);
        
    }

    public void StartAttack()
    {
       // attacking = true;
    }

    public void EndAttack()
    {
       // attacking = false;
    }

    void InitializeAttributes()
    {
        attributes.Initialize(loot);
    }

    protected virtual void FixedUpdate()
    {
        verifyHealth();
        verifyTargetDistanceTimer += Time.fixedDeltaTime;

        if (verifyTargetDistanceTimer >= verifyTargetDistanceTimerMax)
        {
            verifyTargetDistanceTimer = 0;
            if (state != stateMachine.special && (targetStats.GetCurrentHealth() > 0.0f)) verifyTargetDistance();
        }
        if(!specialReady)
        {
            specialTime += Time.fixedDeltaTime;

            if(specialTime >= attributes.GetSpecialCoolDown())
            {
                specialReady = true;
            }
        }
        
        //rb.velocity = Vector3.zero;
        // Aqui quando todos speciais
        DoSpecial();
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
            case stateMachine.special:
                stateSpecial();
                break;
            default: break;

        }

    }

    // Fica em idle por timerIdle segundos, então começa a andar
    protected virtual void stateIdle()
    {
        move = false;
        //if (!invokeDefined)
        //{
        //    invokeDefined = true;
        //    timerIdle = Random.Range(3, 6);
        //    Invoke("startWalking", timerIdle);
        //}
        if(!invokeDefined)
        {
            invokeDefined = true;
            timerIdle = Random.Range(3, 6);

        }

        if(timerIdle > 0)
        {
            timerIdle -= Time.deltaTime;
            
        }
        else
        {
            startWalking();

        }
    }

    protected virtual void stateSpecial()
    {
       
    }
    //Estado caminhar
    protected virtual void stateWalk()
    {
        
        if (!nerfed)
        {
            //navigator.Resume();
            navigator.isStopped = false;

            //direction = target.transform.position;
            //direction.y = transform.position.y;
            //transform.LookAt(direction);

            //step = Time.deltaTime * attributes.velocity;

            ////transform.position += transform.forward * step;
            //rb.MovePosition(transform.position+(transform.forward * step));
        }
        else
        {
            //navigator.Stop();
            navigator.isStopped = true;
        }
        //if (Vector3.Distance(target.transform.position, transform.position) >= attributes.GetSpecialRangeMin()
        //    && Vector3.Distance(target.transform.position, transform.position) <= attributes.GetSpecialRangeMax()
        //    && specialReady && targetStats.GetCurrentHealth() > 0)
        //{
        //    SpecialAttack();
        //    specialReady = false;
        //    specialTime = 0;
        //}

        // Verifica se a IA está bloqueada com outra IA
        if (navigator.remainingDistance >= navigator.stoppingDistance)
        {
            currentVelocity = navigator.velocity.magnitude;
            if (navigator.velocity.magnitude <= 0.3f)
            {
                verifyBloqued = true;
            }
            else verifyBloqued = false;

            if (verifyBloqued)
            {
                timeBloqued += Time.deltaTime;
                if (timeBloqued >= 0.5f)
                {
                    anim.SetBool("walk", false);
                    state = stateMachine.idle;
                    navigator.isStopped = true;
                    timeBloqued = 0;
                }

            }
            else timeBloqued = 0;
            
        }
    }

    //Estado Atacar
    protected virtual void stateAttack()
    {
        if (targetStats.GetCurrentHealth() > 0)
        {
            //if (Vector3.Distance(target.transform.position, transform.position) >= attributes.GetSpecialRangeMin()
            //     && Vector3.Distance(target.transform.position, transform.position) <= attributes.GetSpecialRangeMax()
            //     && specialReady)
            //{
            //    SpecialAttack();
            //    specialReady = false;
            //    specialTime = 0;
            //}
        }
        else
        {
            state = stateMachine.idle;
            anim.SetBool("attack", false);
            anim.SetBool("walk", false);
            
        }

    }

    protected virtual void SpecialAttack()
    {

    }

    protected virtual void FinishSpecialAttack()
    {

    }

    //Estado Morto
    protected virtual void stateDie()
    {
        if(submerge)
        {
            transform.position -= Vector3.up * 0.05f;       
        }
    }

    // Mudança para o estado caminhar
    protected virtual void startWalking()
    {
        invokeDefined = false;
        if (state == stateMachine.idle)
        {
            
            // Randomiza uma posição de movimento
            movePosition = GetNewPosition();
            navigator.SetDestination(movePosition);
            //move = true;
            
            //transform.LookAt(movePosition);
            anim.SetBool("walk", true);
            
            
        }
    }

    protected Vector3 GetNewPosition()
    {
        
        Vector3 newPosition;
        
        do
        {
            newPosition = new Vector3(transform.position.x + Random.Range(-4, 4),
                                        transform.position.y,
                                        transform.position.z + Random.Range(-4, 4));
            state = stateMachine.walk;
            
            UnityEngine.AI.NavMeshHit hit;
            UnityEngine.AI.NavMesh.SamplePosition(newPosition, out hit, 40.0f, 1);
            //newPosition = new Vector3(hit.position.x, transform.position.y, hit.position.z);
            newPosition = hit.position;


        } while (Vector3.Distance(newPosition, startPosition) >= attributes.GetPatrolRange() || Vector3.Distance(newPosition, transform.position) < attributes.GetPatrolRange()/2);
        //bug.Log("Fora da Area");
        //else Debug.Log("Dentro da Area");
        
        return newPosition;
    }

    // metodo usado pela animação de caminhar em pontos onde a animação não se move
    public void stopMove()
    {
        move = !move;
    }

    // Metodo usado pela animação de ataque para causar dano no Heroi
    public void Hit()
    {
        if(Vector3.Distance(target.transform.position, transform.position) < attributes.GetAttackRange())
            targetStats.TakeDamage(this.gameObject, attributes.GetDamage());
        //towerStats towerScr = target.GetComponent<towerStats>();
      //  towerScr.takeDamage(Random.Range(attributes.minDamage, attributes.maxDamage)/2);
    }

    public void Impact()
    {
        anim.SetBool("impact", true);
        if(attributes.GetCanBeNerfed()) nerfed = true;
    }

    public void EndImpact()
    {
        anim.SetBool("impact", false);
        nerfed = false;
    }


    public void Submerge()
    {
        submerge = true;
        navigator.enabled = false;
        if(spawnPoint)
        {
            spawnPoint.RemoveFromList(this.gameObject);
        }
        Destroy(gameObject, 3);
        enemyCanvas.enabled = false;
        feedbackCanvas.enabled = false;
    }

    // Verifica a distancia do alvo, se necessário inicia o ataque
    protected virtual void verifyTargetDistance()
    {
        // Verifica se morreu, está nerfed ou ja atacando
        // então não pode mover nem atacar
        if ((state != stateMachine.die) && !nerfed && (targetStats.GetCurrentHealth() > 0) )
        {
            // Verifica se o target está fora do alcance de visibilidade, se estiver fora então caminha
            if (Vector3.Distance(target.transform.position, transform.position) >= attributes.GetVisibilityRange())
            {
                
                if (Vector3.Distance(transform.position, startPosition) >= attributes.GetPatrolRange())
                {
                    anim.SetBool("walk", true);
                    state = stateMachine.walk;
                    
                    movePosition = startPosition;
                    navigator.SetDestination(movePosition);
                    //navigator.Resume();
                    navigator.isStopped = false;
                }
                //if (transform.position.x == movePosition.x && transform.position.z == movePosition.z)
                //{
                //    Anim.SetBool("walk", false);
                //    state = stateMachine.idle;
                //    //navigator.Stop();
                //    navigator.isStopped = true;

                //}
                if (navigator.enabled && state == stateMachine.walk)
                {
                    if (!navigator.pathPending)
                    {
                        //if (navigator.remainingDistance <= navigator.stoppingDistance)
                       // {
                            if (/*!navigator.hasPath && */navigator.velocity.sqrMagnitude <= stopWalkAnimationThreshold)
                            {
                                
                                anim.SetBool("walk", false);
                                
                                state = stateMachine.idle;
                                //navigator.isStopped = true;
                            }
                       // }

                    }
                }

            }
            // se estiver dentro do alcance então verifica se está no alcance de ataque
            else if (Vector3.Distance(target.transform.position, transform.position) <= attributes.GetAttackRange())
            {
                
                anim.SetBool("attack", true);
                anim.SetBool("walk", true);
                state = stateMachine.attack;
                Vector3 lookAtPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                transform.LookAt(lookAtPosition);
                //navigator.Stop();
                navigator.isStopped = true;
            }
            else
            {
                anim.SetBool("attack", false);
                anim.SetBool("walk", true);
                
                //Debug.Log("Inimigo: move a partir da perseguição");
                state = stateMachine.walk;
                
                movePosition = target.transform.position;
                //navigator.stoppingDistance = attributes.attackRange;
                navigator.SetDestination(movePosition);
                //navigator.Resume();
                navigator.isStopped = false;

            }
            if (Vector3.Distance(target.transform.position, transform.position) >= attributes.GetSpecialRangeMin()
                 && Vector3.Distance(target.transform.position, transform.position) <= attributes.GetSpecialRangeMax()
                 && specialReady)
            {
                
                SpecialAttack();

            }
        }
    }

    protected void verifyHealth()
    {
        if(attributes.GetCurrentHealth()<= 0 && state != stateMachine.die)
        {
            GiveRewards();
            attributes.Loot();
            state = stateMachine.die;
            anim.SetBool("walk", false);
            anim.SetBool("attack", false);
            
            anim.SetTrigger("die");

            attributes.DisableIcons();
            
            //navigator.enabled = false;
            //navigator.Stop();
            if(navigator.isActiveAndEnabled)
                navigator.isStopped = true;
            myCollider.enabled = false;
           // rb.detectCollisions = false;
           // rb.useGravity = false;
          //  rb.constraints = RigidbodyConstraints.FreezeRotationY;
            Invoke("Submerge", 3);
        }
    }

    public void GiveRewards()
    {
        float xp = targetStats.CalculateXP(attributes.GetRewardPoints());
        feedbackText.text = xp.ToString() + " XP";
        feedbackCanvas.enabled = true;
        
    }

    protected void DoSpecial()
    {
        if (throwBack)
        {
            float step = distanceThrowBack * Time.fixedDeltaTime;
            transform.position -= transform.forward * step;
        }
    }

    public void ThrowBack(float t)
    {
        if (attributes.GetCanBeNerfed() && state != stateMachine.die)
        {
            throwBack = true;
            Invoke("DisableThrowBack", t);
            Invoke("RestoreDefense", targetStats.DefenseReductionTime);
            Nerf();

            //state = stateMachine.idle;
        }
    }

    protected void RestoreDefense()
    {
        attributes.RestoreDefense();
    }

    protected void DisableThrowBack()
    {
        throwBack = false;
        DisableNerf();
    }

    public void Stun(float t)
    {
        
        if (attributes.GetCanBeNerfed() && state != stateMachine.die)
        {
            
            anim.SetBool("dizzy", true);
            stun = true;
            Invoke("DisableStun", t);
            Nerf();
        }
    }

    protected void DisableStun()
    {
        stun = false;
        anim.SetBool("dizzy", false);
        DisableNerf();

    }

    protected void DisableNerf()
    {
        if (!stun && !throwBack)
        {
            nerfed = false;
            
        }
    }

    protected void Nerf()
    {
        //navigator.Stop();
        navigator.isStopped = true;
        //navigator.enabled = false;
        anim.SetBool("attack", false);
        anim.SetBool("walk", false);
        
        nerfed = true;
        stopMove();
    }

    
}
