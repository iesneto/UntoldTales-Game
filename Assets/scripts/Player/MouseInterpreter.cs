using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

static class gestos
{
    public static readonly int[] swipeLeft0 = { 0, 0, 0, 0, 0, 0, 1, 0 };
    public static readonly int[] swipeLeft1 = { 0, 0, 0, 0, 0, 2, 1, 0 };
    public static readonly int[] swipeLeft2 = { 0, 0, 0, 0, 0, 0, 1, 2 };
    public static readonly int[] swipeRight0 = { 0, 0, 1, 0, 0, 0, 0, 0 };
    public static readonly int[] swipeRight1 = { 0, 2, 1, 0, 0, 0, 0, 0 };
    public static readonly int[] swipeRight2 = { 0, 0, 1, 2, 0, 0, 0, 0 };
    public static readonly int[] swipeUp0 = { 1, 0, 0, 0, 0, 0, 0, 0 };
    public static readonly int[] swipeUp1 = { 1, 2, 0, 0, 0, 0, 0, 0 };
    public static readonly int[] swipeUp2 = { 1, 0, 0, 0, 0, 0, 0, 2 };
    public static readonly int[] swipeDown0 = { 0, 0, 0, 0, 1, 0, 0, 0 };
    public static readonly int[] swipeDown1 = { 0, 0, 0, 2, 1, 0, 0, 0 };
    public static readonly int[] swipeDown2 = { 0, 0, 0, 0, 1, 2, 0, 0 };
}


public class MouseInterpreter : MonoBehaviour {

    public HUDScript hudScript;
    private GameControl gameControl;
    private double timeTouchPressed;
    //public double timeEnableAttack;
    private double mousePositionThresholdTime = 0.1f;
    private Vector3 touchStartPosition;
    private Vector3 touchEndPosition;
    private Vector3 touchMidPosition;
    private Vector3 movePosition;
    private float mousePositionThreshold;
    private float mousePositionBlockThreshold;
    private float distance;
    private double deltaTouch;
    private float camRayLength;
  //  public float velocity;
  //  public float timetoMove;
    public bool move;
  //  public bool attack;
  //  public float rotationSpeed;
    private HeroBehavior heroBehavScr;
    public bool hasTarget;
    private float doubleClickTime;
    private float firstClickTime;
    private float secondClickTime;
    private float blockTime;
    public bool blocked;
    public bool doSpecial;
    public bool blockRaycast;
    public bool unBlockRaycast;

    private enum Dir {N,NE,E,SE,S,SO,O,NO};
    private int[] arrayDirections = { 0, 0, 0, 0, 0, 0, 0, 0 };
    private int directionsCount;

    public GameObject interactableObject;
    public int touchCount;
    

    //public int raiosDisparados;

   // private Rigidbody rb;
    


    void Start()
    {

        timeTouchPressed = 0.0f;
       // rb = GetComponent<Rigidbody>();
        camRayLength = 100.0f;
        // move = false;
        // attack = false;
        mousePositionThreshold = 80;
        mousePositionBlockThreshold = 30;
        heroBehavScr = GetComponent<HeroBehavior>();
        hudScript = gameObject.transform.Find("HUDCanvas").GetComponent<HUDScript>();
        //raiosDisparados = 0;
        doubleClickTime = 0.5f;
        blockTime = 0.3f;
        firstClickTime = 0;
        secondClickTime = 0;
        blocked = false;
        gameControl = GameControl.control;
    }
	// Update is called once per frame
	void Update () {

        //if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#if UNITY_IOS || UNITY_ANDROID || UNITY_TVOS
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            
            return;
        }
#else


        if (EventSystem.current.IsPointerOverGameObject())
        {
            
            return;
        }
#endif

        if (gameControl.GetActiveScene() != 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                StartTouch();
            }

            if (Input.GetButton("Fire1"))
            {
                UpdateTouch();
            }

            if (Input.GetButtonUp("Fire1"))
            {
                StopTouch();
            }

            //Codigo a seguir serve para modificar a movimentação e o bloqueio: movimentação com toque continuo e bloqueio com toque duplo
            //deve-se atentaar para a modificação de VerificaAlvo() que deve ocorrer dentro da atualização do GetMouseButton(0) quando touchCount é 1
            //verificar conflito entre movimentação e gesture, pela movimentação do mouseInput.Position
            //

            //if(Input.GetMouseButtonDown(0))
            //{
            //    touchCount++;
            //}
            //if (Input.GetMouseButton(0))
            //{
                
            //    if (Input.GetMouseButtonDown(1))
            //    {
            //        touchCount++;

            //    }

            //    if (Input.GetMouseButton(1))
            //    {
                    
            //    }

            //    if (Input.GetMouseButtonUp(1))
            //    {
            //        touchCount--;
            //    }
            //}
            //if (Input.GetMouseButtonUp(0))
            //{
            //    touchCount--;
            //}

            //if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && touchCount > 0) touchCount = 0;
        }
        else
        {
            if(hudScript.isHUDActive())
                hudScript.HideHUD();
        }

        //codigo para debug dos golpes

        //int swipeLeft = 1;
        //int swipeRight = 2;
        //int swipeUp = 3;
        //int swipeDown = 4;

        //if (Input.GetKeyDown("w"))
        //{
        //    heroBehavScr.canSwipe(swipeUp);
        //}

        //if (Input.GetKeyDown("a"))
        //{
        //    heroBehavScr.canSwipe(swipeLeft);
        //}

        //if (Input.GetKeyDown("s"))
        //{
        //    heroBehavScr.canSwipe(swipeDown);
        //}

        //if (Input.GetKeyDown("d"))
        //{
        //    heroBehavScr.canSwipe(swipeRight);
        //}
        //**************************************

    }

    //void FixedUpdate()
    //{
    //    if(move)
    //    {
    //        SmoothLook();
    //        Move();
    //    }
    //}

    void StartTouch()
    {
        timeTouchPressed = 0.0f;
        touchStartPosition = Input.mousePosition;
        
        //move = false;
        touchMidPosition = touchStartPosition; // Faz a posição intermediaria ser igual a inicial
        directionsCount = 1;
        for(int i = 0; i <= 7; i++)
        {
            arrayDirections[i] = 0;
        }
        //VerificaAlvo();
    }

    void UpdateTouch()
    {
        timeTouchPressed += Time.deltaTime;
        //if(timeTouchPressed >= timeEnableAttack)
        //{
        //    attack = true;
        //}
        if (timeTouchPressed >= mousePositionThresholdTime)
        {
            //analisa diferença entre os vetores de posicao do mouse
            //e computa direção N,S,L,O, NO,NL, SO,SL
            // pode usar essa direção em um vetor de ocorrencia
            // para fazer uma analise de gesture
            // deve guardar a contagem das ocorrencias
            // e também posicao intermediaria a cada analise
            // então precisa de um vetor de direções
            // um contador de ocorrencia das direções
            //

            Vector3 tempTouchPosition = Input.mousePosition;  //guarda posição intermediaria temporaria
            Vector2 eixos = VerificaEixos(tempTouchPosition);

            // verifica direções
            ComputaDirecao(tempTouchPosition, eixos);
        }

        touchEndPosition = Input.mousePosition;
        deltaTouch = (touchEndPosition - touchStartPosition).magnitude;
        if (timeTouchPressed >= blockTime && !blocked && deltaTouch <= mousePositionBlockThreshold && !blockRaycast)
        {
            blocked = true;
            //blocked = heroBehavScr.StartBlock();
            heroBehavScr.TouchEvents(true, false, Vector3.zero, false, null, false, 0);
        }
        

        
 
    }

    void StopTouch()
    {
        touchEndPosition = Input.mousePosition;
        deltaTouch = (touchEndPosition - touchStartPosition).magnitude;
        //movePosition = CalculaPosicao();
        // move = true;
        // attack = false;
        if (!blockRaycast)
        {
            if (blocked)
            {
                blocked = false;

                heroBehavScr.TouchEvents(false, false, Vector3.zero, false, null, false, 0);
            }
            else
            {

                //if (!AnalisaGesto())
                //{

                bool run = false;
                
                int specialType = AnalisaGesto();
                
                if (specialType == 0)
                {
                    if (firstClickTime == 0)
                        firstClickTime = Time.time;
                    else if (secondClickTime == 0)
                    {
                        secondClickTime = Time.time;
                        if (secondClickTime - firstClickTime <= doubleClickTime)
                        {
                            run = true;
                            firstClickTime = 0;
                            secondClickTime = 0;
                        }
                        else
                        {
                            firstClickTime = secondClickTime;
                            secondClickTime = 0;
                        }
                    }
                }

                Collider target = VerificaAlvo();

                //if (deltaTouch <= mousePositionThreshold)
                //{
                //    movePosition = CalculaPosicao();
                //    move = true;
                //}
                //else
                //{
                //    movePosition = transform.position;
                //    move = false;
                //}
                heroBehavScr.TouchEvents(false, move, movePosition, run, target, doSpecial, specialType);
                // Faz o player olhar na direção do movimento
                //transform.LookAt(movePosition);

                //if (Vector3.Distance(touchEndPosition, touchStartPosition) <= mousePositionThreshold && !hasTarget)
                //if (deltaTouch <= mousePositionThreshold && !hasTarget)
                //{
                //    movePosition = CalculaPosicao();
                //    heroBehavScr.UpdateMovePosition(movePosition, run);
                //}
                //}
            }

            blocked = false;
            doSpecial = false;
            hasTarget = false;
            move = false;
        }


        if (unBlockRaycast)
        {

            blockRaycast = false;
            unBlockRaycast = false;
        }
    }

    //Vector3 CalculaPosicao()
    //{
    //    // Cria um raycast do mouse em direcao a camera
    //    Ray camRay = Camera.main.ScreenPointToRay(touchEndPosition);

    //    // Cria uma variavel de RaycastHit para guardar o que foi atingido
    //    RaycastHit floorHit;

    //    int floorMask = LayerMask.GetMask("floor");
        
    //    // Dispara o Raycast e se algo foi atingido...
    //    if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
    //    {

    //        Vector3 movePosition = new Vector3(floorHit.point.x, floorHit.point.y , floorHit.point.z);
      
    //        return movePosition;
    //    }
    //    return Vector3.zero;
    //}

    Collider VerificaAlvo()
    {
        // Cria um raycast do mouse em direcao a camera
        Ray camRay = Camera.main.ScreenPointToRay(touchStartPosition);

        // Cria uma variavel de RaycastHit para guardar o que foi atingido
        RaycastHit floorHit;

        //int floorMask = LayerMask.GetMask("floor");
        // Dispara o Raycast e se algo foi atingido...
        

        if (Physics.Raycast(camRay, out floorHit, camRayLength, LayerMask.GetMask("destructibleobject")))
        {
            if (interactableObject != null)
            {
                InteractableObject objScr = interactableObject.GetComponent<InteractableObject>();

                if (objScr.myType == InteractableObject.tipo.ITEM)
                {
                    objScr.gameObject.GetComponent<WorldItem>().HideItemTip();
                    interactableObject = null;
                }
            }

            
            hasTarget = true;
            //heroBehavScr.DoNormalAttack(floorHit.collider, run);
            //Vector3 movePosition = new Vector3(floorHit.point.x, rb.transform.position.y, floorHit.point.z);
            //return movePosition;
            //raiosDisparados++;
            return floorHit.collider;
        }

        if (Physics.Raycast(camRay, out floorHit, camRayLength, LayerMask.GetMask("enemy")))
        {
            if (interactableObject != null)
            {
                InteractableObject objScr = interactableObject.GetComponent<InteractableObject>();

                if (objScr.myType == InteractableObject.tipo.ITEM)
                {
                    objScr.gameObject.GetComponent<WorldItem>().HideItemTip();
                    interactableObject = null;
                }
            }

            //Debug.Log("Atingiu Inimigo");
            hasTarget = true;
            //heroBehavScr.DoNormalAttack(floorHit.collider, run);
            //Vector3 movePosition = new Vector3(floorHit.point.x, rb.transform.position.y, floorHit.point.z);
            //return movePosition;
            //raiosDisparados++;
            return floorHit.collider;
        }


        if (Physics.Raycast(camRay, out floorHit, camRayLength, LayerMask.GetMask("object")))
        {
            floorHit.collider.gameObject.GetComponent<InteractableObject>().TouchAnimate();


            if (interactableObject == null)
            {
                
                interactableObject = floorHit.collider.gameObject;
                InteractableObject objScr = interactableObject.GetComponent<InteractableObject>();
                if (objScr.myType == InteractableObject.tipo.ITEM)
                {
                    objScr.gameObject.GetComponent<WorldItem>().ShowItemTip();
                    move = false;
                    return null;
                }   
                hasTarget = true;
                return floorHit.collider;
                

            }
            else
            {
                
                if (interactableObject == floorHit.collider.gameObject)
                {
                    
                    //interactableObject = null;
                    hasTarget = true;
                    return floorHit.collider;
                }
                else
                {
                    InteractableObject objScr = interactableObject.GetComponent<InteractableObject>();

                    if (objScr.myType == InteractableObject.tipo.ITEM)
                    {
                        objScr.gameObject.GetComponent<WorldItem>().HideItemTip();
                    }

                    interactableObject = floorHit.collider.gameObject;
                    objScr = interactableObject.GetComponent<InteractableObject>();
                    if (objScr.myType == InteractableObject.tipo.ITEM)
                    {
                        objScr.gameObject.GetComponent<WorldItem>().ShowItemTip();
                        move = false;
                        return null;
                    }

                    hasTarget = true;
                    return floorHit.collider;
                }
            }
            //return null;
        }

        if (Physics.Raycast(camRay, out floorHit, camRayLength, LayerMask.GetMask("player"), QueryTriggerInteraction.Collide))
        {

            hudScript.ShowHUD();
            gameControl.ShowConfigButton();
            //Vector3 movePosition = new Vector3(floorHit.point.x, rb.transform.position.y, floorHit.point.z);
            //return movePosition;
            // raiosDisparados++;
            // return null;
            if (interactableObject != null)
            {
                InteractableObject objScr = interactableObject.GetComponent<InteractableObject>();

                if (objScr.myType == InteractableObject.tipo.ITEM)
                {
                    objScr.gameObject.GetComponent<WorldItem>().HideItemTip();
                    interactableObject = null;
                }
            }
            return null;
        }
        if (Physics.Raycast(camRay, out floorHit, camRayLength, LayerMask.GetMask("floor")))
        {
            if (interactableObject != null)
            {
                InteractableObject objScr = interactableObject.GetComponent<InteractableObject>();

                if (objScr.myType == InteractableObject.tipo.ITEM)
                {
                    objScr.gameObject.GetComponent<WorldItem>().HideItemTip();
                    interactableObject = null;
                }
            }


			if (!blockRaycast)
            {
                
                hasTarget = false;
                if (deltaTouch <= mousePositionThreshold)
                {
                    Vector3 newPosition = new Vector3(floorHit.point.x, floorHit.point.y, floorHit.point.z);
                    movePosition = newPosition;

                    move = true;
                }
                else
                {
                    movePosition = transform.position;
                    move = false;
                }
                //Debug.Log("Atingiu Chão");
                //Vector3 movePosition = new Vector3(floorHit.point.x, rb.transform.position.y, floorHit.point.z);
                //return movePosition;
                //raiosDisparados++;
                return null;
            }
        }
        return null;

    }

    public bool VerifyRaycastBlock()
    {
        return blockRaycast;
    }

    public void CanBlockRaycast()
    {
        //if(blockRaycast)
        //{
        //    unBlockRaycast = true;
        //}

        // codigo do black raycast
        //if(deltaTouch <= mousePositionThreshold)
        //{
            
        //    blockRaycast = true;
        //}

    }

    public void UnblockRaycast()
    {
        //unBlockRaycast = true;
        
    }

    // Move Player
    //void Move()
    //{
    //    float step = Time.fixedDeltaTime * velocity;
    //    distance = Vector3.Distance(transform.position, movePosition);
    //    if (distance > 1.1f)
    //    {
    //        Vector3 directionToMove = (rb.transform.position - movePosition).normalized;
    //        rb.MovePosition(transform.position - directionToMove * step);
    //    }
    //    else move = false;
    //}

    

    //void SmoothLook()
    //{
        
    //    Quaternion newRotation = Quaternion.LookRotation(movePosition - transform.position);
    //    newRotation.x = 0.0f;
    //    newRotation.z = 0.0f;
    //    transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime);
    //}

    Vector2 VerificaEixos(Vector3 tempTouchPosition)
    {
        Vector2 eixos = Vector2.zero;

        // verifica eixo x
        if (tempTouchPosition.x - touchMidPosition.x >= 0)
        {
            if (tempTouchPosition.x - touchMidPosition.x >= mousePositionThreshold)
            {
                eixos.x = 1;
            }
            else { eixos.x = 0; }
        }
        else {
            if (touchMidPosition.x - tempTouchPosition.x >= mousePositionThreshold)
            {
                eixos.x = -1;
            }
            else { eixos.x = 0; }

        }

        //Verifica eixo y
        if (tempTouchPosition.y - touchMidPosition.y >= 0)
        {
            if (tempTouchPosition.y - touchMidPosition.y >= mousePositionThreshold)
            {
                eixos.y = 1;
            }
            else { eixos.y = 0; }
        }
        else {
            if (touchMidPosition.y - tempTouchPosition.y >= mousePositionThreshold)
            {
                eixos.y = -1;
            }
            else { eixos.y = 0; }

        }

        return eixos;
    }

    void ComputaDirecao(Vector3 tempTouchPosition, Vector2 eixos)
    {
        if (eixos.x == 1)
        {
            if (eixos.y == 1)
            {
                if (arrayDirections[(int)Dir.NE] == 0)
                {
                    arrayDirections[(int)Dir.NE] = directionsCount;
                    directionsCount++;
                    touchMidPosition = tempTouchPosition;
                }
            }
            else if (eixos.y == -1)
            {
                if (arrayDirections[(int)Dir.SE] == 0)
                {
                    arrayDirections[(int)Dir.SE] = directionsCount;
                    directionsCount++;
                    touchMidPosition = tempTouchPosition;
                }
            }
            else
            {
                if (arrayDirections[(int)Dir.E] == 0)
                {
                    arrayDirections[(int)Dir.E] = directionsCount;
                    directionsCount++;
                    touchMidPosition = tempTouchPosition;
                }
            }
        }
        else if (eixos.x == -1)
        {
            if (eixos.y == 1)
            {
                if (arrayDirections[(int)Dir.NO] == 0)
                {
                    arrayDirections[(int)Dir.NO] = directionsCount;
                    directionsCount++;
                    touchMidPosition = tempTouchPosition;
                }
            }
            else if (eixos.y == -1)
            {
                if (arrayDirections[(int)Dir.SO] == 0)
                {
                    arrayDirections[(int)Dir.SO] = directionsCount;
                    directionsCount++;
                    touchMidPosition = tempTouchPosition;
                }
            }
            else
            {
                if (arrayDirections[(int)Dir.O] == 0)
                {
                    arrayDirections[(int)Dir.O] = directionsCount;
                    directionsCount++;
                    touchMidPosition = tempTouchPosition;
                }
            }
        }
        else
        {
            if (eixos.y == 1)
            {
                if (arrayDirections[(int)Dir.N] == 0)
                {
                    arrayDirections[(int)Dir.N] = directionsCount;
                    directionsCount++;
                    touchMidPosition = tempTouchPosition;
                }
            }
            else if (eixos.y == -1)
            {
                if (arrayDirections[(int)Dir.S] == 0)
                {
                    arrayDirections[(int)Dir.S] = directionsCount;
                    directionsCount++;
                    touchMidPosition = tempTouchPosition;
                }
            }

        }
    }

    int AnalisaGesto()
    {
        int swipeLeft = 1;
        int swipeRight = 2;
        int swipeUp = 3;
        int swipeDown = 4;

        if (ArraysCompare(arrayDirections,gestos.swipeDown0) || 
            ArraysCompare(arrayDirections, gestos.swipeDown1) || 
            ArraysCompare(arrayDirections, gestos.swipeDown2))
        {
            //heroBehavScr.canSwipe(swipeDown);
            doSpecial = true;
            return swipeDown;
        }

        else if (ArraysCompare(arrayDirections, gestos.swipeUp0) || 
            ArraysCompare(arrayDirections, gestos.swipeUp1) || 
            ArraysCompare(arrayDirections, gestos.swipeUp2))
        {
            //heroBehavScr.canSwipe(swipeUp);
            doSpecial = true;
            return swipeUp;
        }

        else if (ArraysCompare(arrayDirections, gestos.swipeLeft0) ||
            ArraysCompare(arrayDirections, gestos.swipeLeft1) ||
            ArraysCompare(arrayDirections, gestos.swipeLeft2))
        {
            //heroBehavScr.canSwipe(swipeLeft);
            doSpecial = true;
            return swipeLeft;
        }

        else if (ArraysCompare(arrayDirections, gestos.swipeRight0) ||
            ArraysCompare(arrayDirections, gestos.swipeRight1) ||
            ArraysCompare(arrayDirections, gestos.swipeRight2))
        {
            //heroBehavScr.canSwipe(swipeRight);
            doSpecial = true;
            return swipeRight;
        }
        doSpecial = false;
        return 0;
    }

    bool ArraysCompare(int[] array1, int[] array2)
    {
        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i]) return false;
        }
        return true;
    }

    public bool GetBlockRaycast()
    {
        return blockRaycast;
    }
}
