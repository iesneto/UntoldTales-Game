using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;
//using UnityEditor.Animations;


public class HeroBehavior : MonoBehaviour {

    private GameControl data;
    private Animator anim;
    public AudioSource audioVoice;
    public AudioSource audioShield;
    public AudioSource audioHit;
    public AudioSource audioSteps;
    public AudioSource audioSkills;
    public AudioSource audioRoll;
    public AudioSource audioSecondary;
    public AudioClip footstepsClip;
    public AudioClip swordSwoshAndHitClip;
    public AudioClip rollClip;
    public AudioClip swipeUpClip;
    public AudioClip swipeDownClip;
    public AudioClip swipeRightClip;
    public AudioClip swipeLeftClip;
    public AudioClip skillSwordHit;
    public AudioClip skillShieldHit;
    public AudioClip levelUpClip;
    private bool swordSound;

    public UnityEngine.AI.NavMeshAgent navigator;
    public GameObject wayPointPrefab;
    public GameObject wayPoint;
    public Vector3 movePosition;
    public Vector3 touchedPosition;

    public bool startWalk;
    public enum states { idle, walk, attack, special, block, death };
    public states currentState;
    public states previousState;
    public enum enemySpecial { wrapped, entangle, reduceDef, blizzard };
    public bool entangled;
    public bool wrapped;
    public bool trumbled;
    public bool dizzy;
    public bool frosted;
    public float speedReduction;
    public Color frostedColor;
    public SkinnedMeshRenderer myRender;

    public GameObject wrappedPrefab;
    private GameObject wrappedInstance;
    public GameObject entanglePrefab;
    private GameObject entangleInstance;

    public bool rolling;
    public bool block;
    public bool specialAttack;
    public bool attack;
    public bool walk;
    public bool roll;
    public Vector3 rollDirection;
    public string specialAttackName;
    public bool updateTouchEvents;
    public bool doingBlock;
    public bool dead;
    public bool died;

    public GameObject target;
    public bool doingSpecial;
    private HeroStats heroAttributes;
    private EnemyAttributes targetAttributes;
    private BossAttributes bossAttributes;
    public Transform aoePosition;


    private float energyToConsume;
    public float distanceFromPlayer;

    public ParticleSystem mightyLeapWaveParticle;
    public ParticleSystem shieldBashParticle;
    public ParticleSystem impaleParticle;


    private float tempSpecialDamage;
    private bool tempStun;
    private bool tempThrowBack;
    private bool tempRadius;

    //[SerializeField]
    public MeleeWeaponTrail trail;

    public Image healthBar;
    public Image energyBar;
    public HUDScript hudScript;

    public float playerToTargetDistance;

    public int GetState()
    {
        return (int)currentState;
    }

    public void ResetPlayer(Vector3 ressurectPosition)
    {
        anim.SetBool("dead", false);
        updateTouchEvents = false;
        currentState = states.idle;
        target = null;
        navigator.enabled = true;
        navigator.SetDestination(ressurectPosition);
        navigator.isStopped = true;
        entangled = false;
        wrapped = false;
        dizzy = false;
        trumbled = false;
        
    }

    void Start()
    {

        currentState = states.idle;
        doingSpecial = false;
        data = GameControl.control;
        aoePosition = transform.Find("aoePosition");
        wayPoint = Instantiate(wayPointPrefab);
        
        wayPoint.GetComponent<MeshRenderer>().enabled = false;

        trail.Emit = false;
        hudScript = transform.Find("HUDCanvas").gameObject.GetComponent<HUDScript>();
        //healthBar = GameObject.Find("HUDCanvas/Bars/HealthBG/Health").GetComponent<Image>();
        //energyBar = GameObject.Find("HUDCanvas/Bars/EnergyBG/Energy").GetComponent<Image>();

        UpdateBehavior();


    }

    public void UpdateBehavior()
    {
        anim = GetComponent<Animator>();
        heroAttributes = GetComponent<HeroStats>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (!frosted) navigator.speed = heroAttributes.GetSpeed();
        else navigator.speed = speedReduction;
        SetAnimationSpeed();
    }



    void Update()
    {
        UpdateBars();

        if (!wrapped && !dizzy)
            StateMachine();

    }

    void StateMachine()
    {

        if (!entangled)
        {
            switch (currentState)
            {
                case states.idle:
                    StateIdle();
                    break;
                case states.walk:
                    StateWalk();
                    break;
                case states.attack:
                    hudScript.ShowHUD();
                    StateAttack();
                    break;
                case states.special:
                    StateSpecial();
                    break;
                case states.block:
                    hudScript.ShowHUD();
                    StateBlock();
                    break;
                case states.death:
                    StateDeath();
                    break;
                default: break;

            }
        }
        else
        {
            
            currentState = states.attack;
            StateAttack();
            
        }
    }

    void StateIdle()
    {

        if (updateTouchEvents)
        {
            if (block)
            {
                
                currentState = states.block;
                previousState = states.idle;
                

            }
            else if (specialAttack)
            {
                currentState = states.special;
                previousState = states.idle;


            }
            else if (walk)
            {
                currentState = states.walk;
                previousState = states.idle;

            }
            else if (attack)
            {
                currentState = states.attack;
                previousState = states.idle;

            }


        }
        else if (target)
        {
            if (target.tag == "Enemy")
            {
                
                if (targetAttributes.GetCurrentHealth() >= 0)
                    walk = true;
                currentState = states.walk;
                previousState = states.idle;
            }
            else if (target.tag == "Interactable")
            {
                target.GetComponent<InteractableObject>().Interact();
                target = null;
            }
            else if (target.tag == "Boss")
            {
                if (bossAttributes.GetCurrentHealth() >= 0)
                    walk = true;
                currentState = states.walk;
                previousState = states.idle;
            }

        }


    }

    public void DoneRolling()
    {
        
        anim.SetBool("roll", false);
        anim.SetBool("walk", false);
        rolling = false;
    }

    void StateWalk()
    {
        if (updateTouchEvents)
        {
            if (walk)
            {
                if (!rolling)
                {
                    navigator.enabled = true;
                    navigator.SetDestination(movePosition);

                    //navigator.Resume();
                    navigator.isStopped = false;

                    anim.SetBool("walk", true);
                    EnableWayPoint(movePosition);
                    if (roll)
                    {
                        anim.SetBool("roll", true);
                        if(currentState != states.death)
                            rolling = true;
                        if (!frosted)
                        {
                            navigator.speed = heroAttributes.GetRollSpeed();
                        }
                        else
                        {
                            navigator.speed = speedReduction * heroAttributes.GetRollSpeed();
                        }
                        DisableWayPoint();

                    }
                    else
                    {
                        anim.SetBool("roll", false);
                        if (!frosted)
                        {
                            navigator.speed = heroAttributes.GetSpeed();
                        }
                        else navigator.speed = speedReduction;
                        rolling = false;

                    }

                    walk = false;
                    roll = false;
                    updateTouchEvents = false;
                }
            }

            else if (specialAttack)
            {
                //navigator.Stop();
                navigator.isStopped = true;
                anim.SetBool("roll", false);
                anim.SetBool("walk", false);
                currentState = states.special;
                previousState = states.walk;
                rolling = false;
                if (!frosted) navigator.speed = heroAttributes.GetSpeed();
                else navigator.speed = speedReduction;
                
            }

            else if (block)
            {
                
                //navigator.Stop();
                navigator.isStopped = true;
                anim.SetBool("roll", false);
                anim.SetBool("walk", false);
                currentState = states.block;
                previousState = states.walk;
                rolling = false;
                if (!frosted) navigator.speed = heroAttributes.GetSpeed();
                else navigator.speed = speedReduction;



            }
            else if (attack)
            {
                anim.SetBool("roll", false);
                anim.SetBool("walk", false);
                currentState = states.attack;
                previousState = states.walk;
                rolling = false;
                if (!frosted) navigator.speed = heroAttributes.GetSpeed();
                else navigator.speed = speedReduction;

            }

        }
        else
        {

            // Verifica se chegou no destino
            //if (transform.position.x == movePosition.x && transform.position.z == movePosition.z)
            //{
            //    Debug.Log("Chegou no destino");
            //    anim.SetBool("run", false);
            //    anim.SetBool("walk", false);
            //    currentState = states.idle;
            //    previousState = states.walk;

            //}
            if (navigator.enabled)
            {
                if (!navigator.pathPending)
                {
                    if (navigator.remainingDistance <= navigator.stoppingDistance)
                    {
                        if (!navigator.hasPath || navigator.velocity.sqrMagnitude == 0f)
                        {

                            anim.SetBool("roll", false);
                            anim.SetBool("walk", false);
                            currentState = states.idle;
                            previousState = states.walk;
                            DisableWayPoint();
                            rolling = false;
                            
                        }
                    }

                }
            }
            // Verifica se tem alvo
            if (target)
            {
                DisableWayPoint();
                if (walk)
                {
                    anim.SetBool("walk", true);
                    walk = false;
                }
                playerToTargetDistance = Vector3.Distance(transform.position, target.transform.position);
                if (Vector3.Distance(transform.position, target.transform.position) <= heroAttributes.GetAttackDistance())
                {

                    anim.SetBool("roll", false);
                    anim.SetBool("walk", false);
                    rolling = false;
                    //Vector3 lookAtPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                    //transform.LookAt(lookAtPosition);
                    //Debug.Log("LookAt de walk p/ attack");
                    //navigator.Stop();
                    if (target.tag == "Enemy" || target.tag == "Boss")
                    {
                        if (navigator.enabled)
                            navigator.isStopped = true;
                        navigator.enabled = false;
                        currentState = states.attack;
                        previousState = states.walk;
                        attack = true;
                    }
                    else if (target.tag == "Interactable")
                    {
                        if (navigator.enabled)
                            navigator.isStopped = true;
                        navigator.enabled = false;
                        currentState = states.idle;
                        previousState = states.walk;
                    }

                }
                else
                {
                    navigator.enabled = true;
                    movePosition = target.transform.position;
                    navigator.SetDestination(movePosition);
                    //navigator.Resume();
                    navigator.isStopped = false;
                }
            }
            //else navigator.stoppingDistance = 0;
            // Verifica se tomou um caminho muito longo
            //if (navigator.enabled)
            //{
            //    if (navigator.pathStatus == NavMeshPathStatus.PathComplete)
            //    {
                    
            //        distanceFromPlayer = navigator.remainingDistance;

            //        if (distanceFromPlayer >= 40.0f)
            //        {

            //            navigator.SetDestination(transform.position);
            //            //navigator.Stop();
            //            navigator.isStopped = true;
            //            //navigator.enabled = false;
            //            anim.SetBool("run", false);
            //            anim.SetBool("walk", false);
            //            currentState = states.idle;
            //            previousState = states.walk;
            //            DisableWayPoint();

            //        }
            //    }
            //}
        }


    }

    void StateAttack()
    {
        
        if (updateTouchEvents )
        {

            
            if (attack)
            {
                
                anim.SetBool("attackNormal", true);
                attack = false;
                updateTouchEvents = false;
                if (target)
                {
                    Vector3 lookAtPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                    transform.LookAt(lookAtPosition);
                }

            }
            else if (walk)
            {
                
                anim.SetBool("attackNormal", false);
                currentState = states.walk;
                previousState = states.attack;

            }
            else if (specialAttack)
            {
               
                anim.SetBool("attackNormal", false);
                currentState = states.special;
                previousState = states.attack;

            }
            else if (block)
            {
                
                anim.SetBool("attackNormal", false);
                currentState = states.block;
                previousState = states.attack;
                
            }
            
        }
        else
        {
            
            if (target)
            {
                
                
                if (attack || !anim.GetBool("attackNormal"))
                {
                    
                    anim.SetBool("attackNormal", true);
                    attack = false;
                    Vector3 lookAtPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                    transform.LookAt(lookAtPosition);
                }
                if (Vector3.Distance(transform.position, target.transform.position) >= heroAttributes.GetAttackDistance())
                {
                    
                    anim.SetBool("attackNormal", false);
                    currentState = states.walk;
                    previousState = states.attack;
                    walk = true;
                }
                if (target.tag == "Enemy")
                {
                    
                    if (targetAttributes.GetCurrentHealth() <= 0)
                    {
                        target = null;
                        anim.SetBool("attackNormal", false);
                        currentState = states.idle;
                        previousState = states.attack;
                    }
                }
                else if (target.tag == "Boss")
                {
                    
                    if (bossAttributes.GetCurrentHealth() <= 0)
                    {
                        target = null;
                        anim.SetBool("attackNormal", false);
                        currentState = states.idle;
                        previousState = states.attack;
                    }
                }
                else if (target.tag == "destructibleobject")
                {
                    
                    if (target.GetComponent<DestructibleObject>().hits <= 0)
                    {
                        target.GetComponent<DestructibleObject>().DestroyObject();
                        target = null;
                        anim.SetBool("attackNormal", false);
                        currentState = states.idle;
                        previousState = states.attack;
                    }
                }
            }
            else
            {
                
                if (!entangled)
                {
                    anim.SetBool("attackNormal", false);
                    currentState = states.idle;
                    previousState = states.attack;
                }
            }
        }
    }

    void StateSpecial()
    {
        if (specialAttack)
        {
            specialAttack = false;
            doingSpecial = true;
            anim.SetBool(specialAttackName, true);
            ModifyAnimationSpeed();
            heroAttributes.ConsumeEnergy(energyToConsume);
            updateTouchEvents = false;
        }
        if (!doingSpecial)
        {
            anim.SetBool(specialAttackName, false);
            SetAnimationSpeed();
            switch (previousState)
            {
                case states.idle: currentState = states.idle;
                    previousState = states.special;
                    break;
                case states.walk: currentState = states.walk;
                    previousState = states.special;
                    break;
                case states.attack: currentState = states.attack;
                    previousState = states.special;
                    attack = true;
                    break;
                default: break;
            }
        }
    }

    void StateBlock()
    {
        if (updateTouchEvents)
        {
            updateTouchEvents = false;
            if (block)
            {
                doingBlock = true;
                anim.SetBool("startBlock", true);
                heroAttributes.SetBlocked(true);
            }
            else
            {
                anim.SetBool("exitBlock", true);
                anim.SetBool("enterBlock", false);
            }
        }
        if (!doingBlock)
        {
            anim.SetBool("exitBlock", false);
            heroAttributes.SetBlocked(false);
            switch (previousState)
            {
                case states.idle:
                // currentState = states.idle;
                // previousState = states.block;
                //  break;
                case states.walk:
                    currentState = states.idle;
                    previousState = states.block;
                    break;
                case states.attack:
                    currentState = states.attack;
                    previousState = states.block;
                    attack = true;
                    break;
                default: break;
            }
        }
    }

    void StateDeath()
    {
        if (dead)
        {
            
            dead = false;
            block = false;
            rolling = false;
            anim.SetBool("roll", false);
            anim.SetBool("walk", false);
            anim.SetBool("attackNormal", false);
            anim.SetBool("swipeLeft", false);
            anim.SetBool("swipeRight", false);
            anim.SetBool("swipeUp", false);
            anim.SetBool("swipeDown", false);
            anim.SetBool("startBlock", false);
            anim.SetBool("enterBlock", false);
            doingSpecial = false;
            if (doingBlock)
            {
                doingBlock = false;
                anim.SetBool("exitBlock", true);
            }
            else
            {
                anim.SetBool("exitBlock", false);
                anim.SetBool("dead", true);
            }
            if(target != null)
            {
                targetAttributes.Select(false);
            }
            target = null;
            if (navigator.isActiveAndEnabled)
            {
                navigator.isStopped = true;
                navigator.enabled = false;
            }
            if(entangled)
            {
                entangleInstance.GetComponent<DestructibleObject>().DestroyObject();
            }
            // Modificar aqui para usar uma booleana para invocar o Respawn
            // fora deste if, mas dentro do bloco StateDeath()
            Invoke("Respawn", 3);

        }
    }

    public void Respawn()
    {
        data.RessurectHero();
    }

    public void TouchEvents(bool _block, bool _walk, Vector3 _movePos, bool _roll, Collider _target, bool _specialAttack, int _specialAttackType)
    {
        if (currentState == states.death) return;

        if (_specialAttack)
        {
            canSwipe(_specialAttackType);
        }
        else if (_target)
        {
            DisableWayPoint();
            //walk = true;
            // run = _run;
            //if (_target)
            //{
            // Ataque normal o player deve ser deslocado até o target
            // coloca o player no estado de ataque
            if (!entangled && !(_target.tag == "destructibleobject"))
            {
                if (_target.tag == "Enemy")
                {
                    if (_target.gameObject.GetComponent<EnemyAttributes>().GetCurrentHealth() >= 0)
                    {
                        if (target != _target.gameObject)
                        {
                            if (target)
                            {
                                if (target.tag == "Enemy")
                                    targetAttributes.Select(false);
                                else if (target.tag == "Boss")
                                    bossAttributes.Select(false);
                            }
                            target = _target.gameObject;
                            targetAttributes = target.GetComponent<EnemyAttributes>();
                            targetAttributes.Select(true);

                        }

                        // seta o destino como o do target                                               
                        movePosition = _target.transform.position;

                        //verifica se o target está fora do alcance

                        if (Vector3.Distance(movePosition, transform.position) >= heroAttributes.GetAttackDistance())
                        {
                            walk = true;
                            //run = _run;
                            attack = false;
                            //if (Vector3.Distance(movePosition, transform.position) <= 1.5f * heroAttributes.GetAttackDistance())
                            //{
                            //    transform.LookAt(_target.transform);
                            //}
                            //define a distancia de stop de parada dentro do alcance de ataque
                            //navigator.stoppingDistance = heroAttributes.GetAttackDistance()/3.0f;
                        }
                        //se dentro do alcance, apenas olha em direção ao target
                        else
                        {

                            if (navigator.enabled)
                            {
                                //navigator.Stop();
                                navigator.isStopped = true;
                                navigator.enabled = false;
                            }
                            Vector3 lookAtPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                            transform.LookAt(lookAtPosition);


                            attack = true;
                            walk = false;

                        }
                    }
                    else attack = false;
                }
                else if (_target.tag == "Interactable")
                {


                    if (target != _target.gameObject)
                    {
                        if (target != null)
                        {
                            if (target.tag == "Enemy")
                                targetAttributes.Select(false);
                            else if (target.tag == "Boss")
                                bossAttributes.Select(false);
                        }

                        target = _target.gameObject;
                    }
                    // seta o destino como o do target                                               
                    // movePosition = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);
                    movePosition = _target.transform.position;

                    if (Vector3.Distance(movePosition, transform.position) >= heroAttributes.GetAttackDistance())
                    {

                        attack = false;
                        walk = true;
                    }
                    //se dentro do alcance, apenas olha em direção ao target
                    else
                    {

                        if (navigator.enabled)
                        {
                            //navigator.Stop();
                            navigator.isStopped = true;
                            navigator.enabled = false;
                        }
                        Vector3 lookAtPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                        transform.LookAt(lookAtPosition);
                        walk = false;

                    }

                }
                else if (_target.tag == "Boss")
                {
                    if (_target.gameObject.GetComponent<BossAttributes>().GetCurrentHealth() >= 0)
                    {
                        if (target != _target.gameObject)
                        {
                            if (target) targetAttributes.Select(false);
                            target = _target.gameObject;
                            bossAttributes = target.GetComponent<BossAttributes>();
                            bossAttributes.Select(true);

                        }

                        // seta o destino como o do target                                               
                        movePosition = _target.transform.position;

                        //verifica se o target está fora do alcance

                        if (Vector3.Distance(movePosition, transform.position) >= heroAttributes.GetAttackDistance())
                        {
                            attack = false;
                            walk = true;
                            //run = _run;
                            //if (Vector3.Distance(movePosition, transform.position) <= 1.5f * heroAttributes.GetAttackDistance())
                            //{
                            //    transform.LookAt(_target.transform);
                            //}
                            //define a distancia de stop de parada dentro do alcance de ataque
                            //navigator.stoppingDistance = heroAttributes.GetAttackDistance()/3.0f;
                        }
                        //se dentro do alcance, apenas olha em direção ao target
                        else
                        {
                            if (navigator.enabled)
                            {
                                //navigator.Stop();
                                navigator.isStopped = true;
                                navigator.enabled = false;
                            }
                            Vector3 lookAtPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                            transform.LookAt(lookAtPosition);


                            attack = true;
                            walk = false;

                        }
                    }
                    else attack = false;
                }
            }
            else if (_target.tag == "destructibleobject")
            {


                if (target != _target.gameObject)
                {
                    if (target != null)
                    {
                        if (target.tag == "Enemy")
                            targetAttributes.Select(false);
                        else if (target.tag == "Boss")
                            bossAttributes.Select(false);
                    }

                    target = _target.gameObject;
                }
                // seta o destino como o do target                                               
                // movePosition = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);
                movePosition = _target.transform.position;

                if (Vector3.Distance(movePosition, transform.position) >= heroAttributes.GetAttackDistance())
                {

                    attack = false;
                    walk = true;
                }
                //se dentro do alcance, apenas olha em direção ao target
                else
                {

                    if (navigator.enabled)
                    {
                        //navigator.Stop();
                        navigator.isStopped = true;
                        navigator.enabled = false;
                    }
                    Vector3 lookAtPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                    transform.LookAt(lookAtPosition);
                    walk = false;
                    attack = true;

                }

            }

        }
        //Arrumar depois, testar o _walk primeiro e dentro verificar o _target
        else if (_walk && !rolling)
        {
            
            
            walk = true;
            //run = _run;
            touchedPosition = _movePos;
            UnityEngine.AI.NavMeshHit hit;
            UnityEngine.AI.NavMesh.SamplePosition(_movePos, out hit, 20.0f, 1);
            
            navigator.enabled = true;
            navigator.isStopped = true;
            movePosition = hit.position;
            
            //**************
            if (_roll && heroAttributes.CurrentEnergy >= heroAttributes.GetRollEnergy() && heroAttributes.canRoll )
            {

                
                
                //if (heroAttributes.canRoll)
                //{
                    hudScript.ShowHUD();
                    roll = true;
                    heroAttributes.ConsumeEnergy(heroAttributes.GetRollEnergy());
                //}
                rollDirection = (touchedPosition - transform.position).normalized;
                movePosition = transform.position + (rollDirection * heroAttributes.GetRollDistance());
                UnityEngine.AI.NavMeshPath path = new NavMeshPath();
                path.ClearCorners();
                
                if (NavMesh.CalculatePath(transform.position, movePosition, 1, path))
                {
                    float lng = 0.0f;
                    if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
                    {
                        for (int i = 1; i < path.corners.Length; ++i)
                        {
                            lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                        }

                        if (lng <= 20.0f)
                        {
                            navigator.SetDestination(movePosition);
                            //EnableWayPoint(movePosition);
                        }
                        else
                        {
                            movePosition = transform.position;
                            navigator.SetDestination(movePosition);
                        }
                    }
                }

                
                
            }
            else
            {
                
                UnityEngine.AI.NavMeshPath path = new NavMeshPath();
                path.ClearCorners();
                if (NavMesh.CalculatePath(transform.position, movePosition, 1, path))
                {
                    float lng = 0.0f;
                    if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
                    {
                        for (int i = 1; i < path.corners.Length; ++i)
                        {
                            lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                        }

                        if (lng <= 20.0f)
                        {
                            navigator.SetDestination(movePosition);
                            //EnableWayPoint(movePosition);
                        }
                        else
                        {
                            movePosition = transform.position;
                            navigator.SetDestination(movePosition);
                        }
                    }
                }
                
            }


            
            
            //Debug.Log("Touch - Distancia: " + navigator.remainingDistance);
            //if (navigator.remainingDistance >= 40.0f || navigator.remainingDistance <= 0.2f)
            //{ 
                
            //    UnityEngine.AI.NavMesh.SamplePosition(movePosition, out hit, 20.0f, 1);
            //    movePosition = hit.position;
                
            //}
            //attack = false;
            //EnableWayPoint(movePosition);
 
            navigator.stoppingDistance = 0;
            if (target)
            {
                if (target.tag == "Enemy") targetAttributes.Select(false);
                else if (target.tag == "Boss") bossAttributes.Select(false);
                target = null;
            }
        }



        if (walk || attack || specialAttack)
        {
            updateTouchEvents = true;

        }

        if(block != _block && heroAttributes.canBlock)
        {
            block = _block;
            updateTouchEvents = true;
        }

    }


    // SHIELD BASH
    public void SwipeLeft()
    {
        // faz detalhes especificos deste swipe
        // quantidade de energia consumida
        // dano e efeitos
        shieldBashParticle.Play();

        tempSpecialDamage = heroAttributes.SwipeLeftDamage;
        tempStun = false;
        tempThrowBack = true;
        tempRadius = true;
        swordSound = false;
        // FAzer depois com Coroutines
        Invoke("waitTime", 0.2f);
    }

    // DEEP SLASH
    public void SwipeRight()
    {

        trail.Emit = true;
        swordSound = true;
        tempSpecialDamage = heroAttributes.SwipeRightDamage;
        tempStun = false;
        tempThrowBack = false;
        tempRadius = true;
        
        Invoke("waitTime", 0.15f);

    }

    // IMPALE
    public void SwipeUp()
    {
        impaleParticle.Play();
        swordSound = true;
        tempSpecialDamage = heroAttributes.SwipeUpDamage;
        tempStun = false;
        tempThrowBack = false;
        tempRadius = false;
        
        Invoke("waitTime", 0.15f);

    }

    // MIGHTY LEAP
    public void SwipeDown()
    {
        trail.Emit = true;
        mightyLeapWaveParticle.Play();

        tempSpecialDamage = heroAttributes.SwipeDownDamage;
        tempStun = true;
        tempThrowBack = false;
        tempRadius = true;

        Invoke("waitTime", 0.4f);
    }


    // Metodo de delay para aplicar os efeitos do ataque especial
    void waitTime()
    {
        trail.Emit = false;

        DoSpecialDamage();
    }


    // Aplica os efeitos do ataque especial
    // com um delay para se adequar à animação
    void DoSpecialDamage()
    {

        if (tempRadius)
        {
            Collider[] targets = Physics.OverlapSphere(aoePosition.position, heroAttributes.AoERadius, LayerMask.GetMask("enemy"));

            foreach (Collider col in targets)
            {

                if (col.gameObject.tag == "Enemy")
                {
                    EnemyAttributes scr = col.gameObject.GetComponent<EnemyAttributes>();
                    if (scr.GetCurrentHealth() >= 0)
                    {
                        scr.ShowHealth();
                        scr.Damage(tempSpecialDamage, heroAttributes.staggerStrength * 2);
                        heroAttributes.SummonOrbPower(col.gameObject, tempSpecialDamage);
                        if (swordSound) PlaySkillHit(skillSwordHit);
                        else PlaySkillHit(skillShieldHit);
                    }

                    if (tempThrowBack)
                    {
                        scr.ThrowBack(heroAttributes.GetDistanceThrowBack(), heroAttributes.DefenseReduction);
                    }

                    if (tempStun)
                    {
                        scr.Stun(heroAttributes.StunTime);
                    }
                }
                else if (col.gameObject.tag == "Boss")
                {
                    BossAttributes scr = col.gameObject.GetComponent<BossAttributes>();
                    scr.Damage(tempSpecialDamage);
                    heroAttributes.SummonOrbPower(col.gameObject, tempSpecialDamage);
                    if (swordSound) PlaySkillHit(skillSwordHit);
                    else PlaySkillHit(skillShieldHit);
                }

            }


        }
        else
        {
            Collider[] targets = Physics.OverlapBox(aoePosition.position, new Vector3(0.1f, 0.1f, 1.0f), Quaternion.LookRotation(transform.forward), LayerMask.GetMask("enemy"));
            if (targets.Length != 0)
            {
                Collider nearest = targets[0];
                if (targets.Length > 1)
                {

                    foreach (Collider col in targets)
                    {
                        float nearestDist = Vector3.Distance(transform.position, nearest.transform.position);
                        float thisDist = Vector3.Distance(transform.position, col.transform.position);
                        if (nearestDist >= thisDist)
                            nearest = col;
                    }


                }

                if (nearest.gameObject.tag == "Enemy")
                {
                    EnemyAttributes scr = nearest.gameObject.GetComponent<EnemyAttributes>();
                    if (scr.GetCurrentHealth() >= 0)
                    {
                        scr.ShowHealth();
                        scr.Damage(tempSpecialDamage,heroAttributes.staggerStrength * 2);
                        heroAttributes.SummonOrbPower(nearest.gameObject, tempSpecialDamage);
                        PlaySkillHit(skillSwordHit);
                    }
                }
                else if (nearest.gameObject.tag == "Boss")
                {
                    BossAttributes scr = nearest.gameObject.GetComponent<BossAttributes>();
                    scr.Damage(tempSpecialDamage);
                    heroAttributes.SummonOrbPower(nearest.gameObject, tempSpecialDamage);
                    PlaySkillHit(skillSwordHit);
                }
            }

        }


    }


    // Metodo chamado pelo Interpretador do mouse
    // verifica se pode aplicar o swipe, de acordo com a quantidade de energia do player
    // ou se ja está aplicando um golpe
    public void canSwipe(int s)
    {
        specialAttack = false;
        if (!doingSpecial)
        {
            switch (s)
            {
                case 1:
                    if (heroAttributes.CurrentEnergy >= heroAttributes.SwipeLeftEnergy && heroAttributes.shieldBashSkill > 0)
                    {
                        energyToConsume = heroAttributes.SwipeLeftEnergy;
                        specialAttack = true;
                        specialAttackName = "swipeLeft";
                    }

                    break;
                case 2:
                    if (heroAttributes.CurrentEnergy >= heroAttributes.SwipeRightEnergy && heroAttributes.deepSlashSkill > 0)
                    {
                        energyToConsume = heroAttributes.SwipeRightEnergy;
                        specialAttack = true;
                        specialAttackName = "swipeRight";

                    }

                    break;
                case 3:
                    if (heroAttributes.CurrentEnergy >= heroAttributes.SwipeUpEnergy && heroAttributes.impaleSkill > 0)
                    {
                        energyToConsume = heroAttributes.SwipeUpEnergy;
                        specialAttack = true;
                        specialAttackName = "swipeUp";
                    }

                    break;
                case 4:
                    if (heroAttributes.CurrentEnergy >= heroAttributes.SwipeDownEnergy && heroAttributes.mightyLeapSkill > 0)
                    {
                        energyToConsume = heroAttributes.SwipeDownEnergy;
                        specialAttack = true;
                        specialAttackName = "swipeDown";
                    }

                    break;
                default: break;
            }
        }
    }


    // Método que finaliza o ataque especial
    // e libera o player para demais comandos
    public void FinishAttack(string attackType)
    {
        doingSpecial = false;
    }

    public void DoDamage()
    {
        PlaySwordSwosh();
        if (target && (Vector3.Distance(transform.position, target.transform.position) < heroAttributes.GetAttackDistance()))
        {
            float tempDamage = heroAttributes.GetDamage();
            heroAttributes.SummonOrbPower(target, tempDamage);
            // if (Vector3.Distance(transform.position, target.transform.position) <= heroAttributes.GetAttackDistance())
            if (target.tag == "Enemy")
                targetAttributes.Damage(tempDamage, heroAttributes.staggerStrength);
            else if (target.tag == "Boss")
                bossAttributes.Damage(tempDamage);
            else if (target.tag == "destructibleobject")
                target.GetComponent<DestructibleObject>().Hit();
        }
        
    }

    // Define a velocidade de animação para os movimentos básicos
    // que são influenciados pelo atributo de movimento
    void SetAnimationSpeed()
    {
        if(!frosted) anim.speed = 1.0f + (heroAttributes.GetMovement() / 50);
        //Debug.Log(anim.speed);
    }

    // Define a velocidade de animação sem a influencia do atributo
    // de movimento, a velocidade de animação fica a cargo do clip de animação
    // Isto é usado para as animações dos especiais
    public void ModifyAnimationSpeed()
    {
        if(!frosted) anim.speed = 1.0f;
    }

    public void ConsumeRunEnergy()
    {
        //if(heroAttributes.CurrentEnergy >= heroAttributes.GetRunEnergy())
        //    heroAttributes.ConsumeEnergy(heroAttributes.GetRunEnergy());
        //else
        //{
        //    anim.SetBool("run", false);
        //    navigator.speed = heroAttributes.GetSpeed();
        //}
    }

    

    //sem serventia por hora
    /*void OnTriggerEnter(Collider other)
    {
       // if (other.gameObject.layer == LayerMask.NameToLayer("enemy"))
         //   Debug.Log("Colidiu inimigo");
    }*/

    void UpdateBars()
    {
        
        if (heroAttributes.GetCurrentHealth() > 0)
        {
            if (heroAttributes.GetCurrentHealth() < heroAttributes.GetMaxHealth())
            {
                float healthAmount = heroAttributes.GetHealthRate() * Time.deltaTime;
                if (heroAttributes.GetCurrentHealth() + healthAmount >= heroAttributes.GetMaxHealth())
                {
                    heroAttributes.SetCurrentHealth(heroAttributes.GetMaxHealth());
                }
                else heroAttributes.SetCurrentHealth(heroAttributes.GetCurrentHealth() + healthAmount);
            }
            else heroAttributes.SetCurrentHealth(heroAttributes.GetMaxHealth());

            if (heroAttributes.CurrentEnergy < heroAttributes.GetMaxEnergy())
            {
                
                float energyAmount = heroAttributes.GetEnergyRate() * Time.deltaTime;
                
                if (heroAttributes.CurrentEnergy + energyAmount >= heroAttributes.GetMaxEnergy())
                {
                    heroAttributes.SetCurrentEnergy(heroAttributes.GetMaxEnergy());
                }
                else heroAttributes.SetCurrentEnergy(heroAttributes.CurrentEnergy + energyAmount);
            }
            else heroAttributes.SetCurrentEnergy(heroAttributes.GetMaxEnergy());
        }

        energyBar.fillAmount = heroAttributes.CurrentEnergy / heroAttributes.GetMaxEnergy();
        healthBar.fillAmount = heroAttributes.GetCurrentHealth() / heroAttributes.GetMaxHealth();
    }

    public void EnterBlock()
    {
        anim.SetBool("enterBlock", true);
        anim.SetBool("startBlock", false);

    }

    public void ContinueAfterBlock()
    {
        doingBlock = false;
        if (currentState == states.death)
        {
            anim.SetBool("exitBlock", false);
            anim.SetBool("dead", true);
        }
    }

    public void TakingDamage(GameObject other)
    {
        heroAttributes.SummonOrbPower(null, 0);
        hudScript.ShowHUD();
        if (!target && currentState == states.idle)
        {
            
            if (other)
                TouchEvents(false, true, Vector3.zero, false, other.GetComponent<Collider>(), false, 0);
        }

        if (heroAttributes.GetCurrentHealth() <= 0 && currentState != states.death)
        {
            currentState = states.death;
            dead = true;
            //se tiver entangled, desabilita

            if(entangled)
            {
                entangleInstance.GetComponent<DestructibleObject>().DestroyObject();
            }
        }
    }

    public void EnemySpecial(enemySpecial type, float time, float damage, GameObject other)
    {
        hudScript.ShowHUD();
        switch (type)
        {
            case enemySpecial.wrapped:
                if (!wrapped && !doingBlock)
                {
                    if (navigator.isActiveAndEnabled)
                        navigator.isStopped = true;
                    wrappedInstance = Instantiate(wrappedPrefab, transform.position, Quaternion.identity);
                    wrapped = true;
                    anim.SetBool("walk", false);
                    anim.SetBool("roll", false);
                    anim.SetBool("attackNormal", false);
                    if (specialAttackName != "") anim.SetBool(specialAttackName, false);
                    doingSpecial = false;
                    rolling = false;
                   
                    Invoke("DisableWrapped", time);

                }

                break;
            case enemySpecial.reduceDef:
                if(!trumbled && !doingBlock)
                {
                    if (navigator.isActiveAndEnabled)
                        navigator.isStopped = true;
                    //Indica que ficou com Trumbled

                    trumbled = true;

                    anim.SetBool("walk", false);
                    anim.SetBool("roll", false);
                    anim.SetBool("attackNormal", false);
                    if (specialAttackName != "") anim.SetBool(specialAttackName, false);
                    doingSpecial = false;
                    rolling = false;
                    heroAttributes.ReduceDefense(damage);
                    Invoke("DisableDizzy", 1.0f);
                    Invoke("DisableTrumble", time);

                }

                break;
            case enemySpecial.entangle:
                if(!entangled && !doingBlock)
                {
                    if (navigator.isActiveAndEnabled)
                        navigator.isStopped = true;
                    entangled = true;
                    anim.SetBool("walk", false);
                    anim.SetBool("roll", false);
                    anim.SetBool("attackNormal", false);
                    if (specialAttackName != "") anim.SetBool(specialAttackName, false);
                    doingSpecial = false;
                    rolling = false;

                    entangleInstance = Instantiate(entanglePrefab, transform.position, Quaternion.identity);
                    entangleInstance.GetComponent<DestructibleObject>().SetHero(this.gameObject);

                }
                break;
            case enemySpecial.blizzard:
                if(!frosted && !doingBlock)
                {
                    speedReduction = damage;
                    frosted = true;
                    myRender.material.color = frostedColor;
                    anim.speed = speedReduction;
                    navigator.speed = speedReduction;

                    Invoke("DisableFrosted", time);
                }
                break;
            default: break;

        }
    }


    public void DisableSpell(enemySpecial spell)
    {
        switch(spell)
        {
            case enemySpecial.wrapped:
                wrapped = false;
                Destroy(wrappedInstance);

                break;
            case enemySpecial.reduceDef:
                heroAttributes.ReduceDefense(0);

                break;
            case enemySpecial.entangle:
                entangled = false;
                
                break;
            case enemySpecial.blizzard:
                frosted = false;
                SetAnimationSpeed();
                navigator.speed = heroAttributes.GetSpeed();
                myRender.material.color = Color.white;
                break;
                
            default: break;
        }
    }
    

    void DisableDizzy()
    {
        dizzy = false;
    }

    void DisableWrapped()
    {
        DisableSpell(enemySpecial.wrapped);
    }

    void DisableTrumble()
    {
        DisableSpell(enemySpecial.reduceDef);
    }

    void DisableFrosted()
    {
        DisableSpell(enemySpecial.blizzard);
    }

    void EnableWayPoint(Vector3 pos)
    {
        if(wayPoint == null)
            wayPoint = Instantiate(wayPointPrefab);
        wayPoint.transform.position = pos;
        wayPoint.GetComponent<MeshRenderer>().enabled = true;
    }

    void DisableWayPoint()
    {
        if (wayPoint == null)
            wayPoint = Instantiate(wayPointPrefab);
        wayPoint.GetComponent<MeshRenderer>().enabled = false;
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
    //    Gizmos.DrawWireSphere(aoePosition.position, heroAttributes.aoeRadius);
    //    Gizmos.DrawCube(aoePosition.position, new Vector3(0.1f, 0.1f, 1.0f));
    //}

    public void PlaySwordSwosh()
    {
        
        audioHit.clip = swordSwoshAndHitClip;
        audioHit.Play();
    }

    public void PlaySkillHit(AudioClip clip)
    {
        audioHit.clip = clip;
        audioHit.Play();
    }

    public void PlayFootSteps()
    {
        audioSteps.clip = footstepsClip;
        audioSteps.Play();
    }


    public void PlayRoll()
    {
        audioRoll.clip = rollClip;
        audioRoll.Play();
    }

    public void PlaySkillSound(int skill)
    {
        
        switch(skill)
        {
            case 0:
                audioSkills.clip = swipeLeftClip; 
                break;
            case 1:
                audioSkills.clip = swipeRightClip;
                break;
            case 2:
                audioSkills.clip = swipeUpClip;
                break;
            case 3:
                audioSkills.clip = swipeDownClip;
                break;
            default: break;
        }

        audioSkills.Play();
    }
    
    public void PlayLevelUp()
    {
        audioSecondary.clip = levelUpClip;
        audioSecondary.Play();
    }

    public void EnableNavigator()
    {
        GetComponent<NavMeshAgent>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "limit")
        {
            Camera.main.GetComponent<CameraFollowing>().LockCamera();
        }
        if(other.tag == "Puzzle")
        {
            other.gameObject.GetComponent<Puzzle02Stone>().Pressed();
           
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "limit")
        {

            Camera.main.GetComponent<CameraFollowing>().UnlockCamera();
        }
        //if (other.tag == "Puzzle")
        //{
        //    other.gameObject.GetComponent<Puzzle02Stone>().LightOff();
        //}
    }

}
