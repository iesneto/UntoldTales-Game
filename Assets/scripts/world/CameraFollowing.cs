using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraFollowing : MonoBehaviour {

    //public Vector3 offsetND;         //Private variable to store the offset distance between the player and camera
    //public Vector3 offsetSE;         //Private variable to store the offset distance between the player and camera
    //public Vector3 offsetNE;
    //public Vector3 offsetSD;
    //public Vector3 offsetN;
    //public Vector3 offsetS;
    public Vector3 initialOffset;
    public Vector3 offset;         //Private variable to store the offset distance between the player and camera
    public GameObject hero;
    public float dampTime;
    public float rotateDampTime;
    public float currentRotateDampTime;
    public float newDampTime;
    private Vector3 velocity = Vector3.zero;
    //public Vector3 forward;
    private bool locked;
    private Vector3 lockedPosition;
    private bool rotate;
    private bool newLookTarget;
    private Vector3 initialForward;
    private Vector3 targetForward;
    private Vector3 newDir;
    public float rotateSpeed;
    protected Vector3 pastPosition;
    protected Vector3 currentPosition;
    protected float positionDelta;
    public float minPositionDelta;
    public bool idle;
    public bool fadeIn;
    public bool fadeOut;
    public Image fadeImage;
    public float fadeSpeed;
  


	// Use this for initialization
	void Start () {

        
        hero = GameObject.FindGameObjectWithTag("Hero");
        if (hero)
        {
            updateCamera();
        }
        locked = false;
        initialOffset = offset;
        initialForward = transform.forward;
        targetForward = transform.forward;
        currentRotateDampTime = rotateDampTime;
        pastPosition = currentPosition = transform.position;
        fadeIn = true;
    }

    void updateCamera()
    {
        //float x = 8.0f;
        //float y = 15.0f;
        //float z = -4.5f;
        //initialOffset = hero.transform.position + new Vector3(8.0f, 5.0f, -1.0f);
        //Vector3 startOffset = new Vector3(x, y, z);
        transform.position = hero.transform.position;
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        // Antigo sistema de camera
        /* offsetND = transform.position - hero.transform.position + new Vector3(4.0f, 0.0f, -10.0f);
         offsetSD = transform.position - hero.transform.position + new Vector3(-4.0f, 0.0f, -7.0f);
         offsetNE = transform.position - hero.transform.position + new Vector3(6.0f, 0.0f, 8.0f);
         offsetSE = transform.position - hero.transform.position + new Vector3(-2.0f, 0.0f, 5.0f);
         offsetN = transform.position - hero.transform.position + new Vector3(5.0f, 0.0f, -1.0f);
         offsetS = transform.position - hero.transform.position + new Vector3(-3.0f, 0.0f, -1.0f);*/
        // Novo Sistema de camera: Menos afastado do centro
        //offsetND = transform.position - hero.transform.position + new Vector3(5.0f, 0.0f, -5.0f);
        //offsetSD = transform.position - hero.transform.position + new Vector3(-3.0f, 0.0f, -4.0f);
        //offsetNE = transform.position - hero.transform.position + new Vector3(5.0f, 0.0f, 3.0f);
        //offsetSE = transform.position - hero.transform.position + new Vector3(-3.0f, 0.0f, 2.0f);
        //offsetN = transform.position - hero.transform.position + new Vector3(5.0f, 0.0f, -1.0f);
        //offsetS = transform.position - hero.transform.position + new Vector3(-3.0f, 0.0f, -1.0f);
        // N = camera postada ao norte -> diminui x
        // S = camera postada ao sul -> aumenta x
        // D = camera postada na direita -> aumenta z
        // E = camera postada na esquerda -> diminui z
        //offsetND = startOffset + new Vector3(-5 * x / 6, 0.0f, -z*4);
        //offsetSD = startOffset + new Vector3(-x/3, 0.0f, -z*5);
        //offsetNE = startOffset + new Vector3(-5 * x / 6, 0.0f, z*3);
        //offsetSE = startOffset  + new Vector3(-x/3, 0.0f, z*4);
        //offsetN = startOffset  + new Vector3(-5 * x / 6, 0.0f, -z);
        //offsetS = startOffset  + new Vector3(-x/3, 0.0f, -z);
        //offset = startOffset;
        //dampTime = 0.5f;
        
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log(transform.forward);
        //}

        float step = rotateSpeed * Time.deltaTime;
        
        if (hero)
        {
            if (!locked)
            {
                //forward = hero.transform.forward;
                ////if (hero.transform.forward.z <= 0)
                ////{
                ////    if (hero.transform.forward.x <= 0)
                ////        offset = offsetSD;
                ////    else offset = offsetND;
                ////    //if (transform.position.z - hero.transform.position.z > 0)
                ////    //offset = offsetSE;
                ////}
                ////else
                ////{
                ////    if (hero.transform.forward.x <= 0)
                ////        offset = offsetSE;
                ////    else offset = offsetNE;
                ////    //if (transform.position.z - hero.transform.position.z < 0)
                ////    //offset = offsetND;
                ////}
                //if (hero.transform.forward.z >= 0.45f)
                //{
                //    if (hero.transform.forward.x <= 0)
                //        offset = offsetND;
                //    else offset = offsetSD;
                //}
                //else if (hero.transform.forward.z <= -0.45f)
                //{
                //    if (hero.transform.forward.x <= 0)
                //        offset = offsetNE;
                //    else offset = offsetSE;
                //}
                //else
                //{
                //    if (hero.transform.forward.x <= 0)
                //        offset = offsetN;
                //    else offset = offsetS;
                //}



                Vector3 destination = hero.transform.position + offset;


                // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.

                // pode-se fazer a camera movimentar somente quando o personagem estiver se movimentando
                // ou pode-se verificar o estado do personagem, se estiver em ataque a camera fica estática
                // if(hero.GetComponent<HeroBehavior>().GetState() == 1)
                // isso evita movimentos de camera quando o personagem esta atacando muitos inimigos e
                // se movimentando entre eles, causando erros de touch.
                // if (hero.GetComponent<NavMeshAgent>().velocity.magnitude > 0)

                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
                
                //transform.position = destination;

                //if (!rotate)
                //{
                //    newDir = Vector3.RotateTowards(transform.forward, initialForward, step, 0.0f);
                //    transform.rotation = Quaternion.LookRotation(newDir);
                //}
                Quaternion targetRotation = Quaternion.LookRotation(hero.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentRotateDampTime);
            }
            else
            {
                Vector3 destination = lockedPosition + offset;
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
                if (rotate)
                {
                    newDir = Vector3.RotateTowards(transform.forward, targetForward, step, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDir);

                }
                if(newLookTarget)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(targetForward - transform.position);
                    //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentRotateDampTime);
                    transform.rotation = targetRotation;
                }

            }

            
            //transform.LookAt(hero.transform.position, Vector3.up);

        }

        else
        {
            hero = GameObject.FindGameObjectWithTag("Hero");
            updateCamera();
        }

        VerifyMovement();

        if(fadeIn && idle)
        {
            fadeIn = false;
            StartCoroutine("StartFadeIn");
        }

        if(fadeOut)
        {
            fadeOut = false;
            StartCoroutine("StartFadeOut");
        }
    }

    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeOut()
    {
        fadeOut = true;
    }

    IEnumerator StartFadeIn()
    {
        for(float i = 1f; i > 0f; i -= fadeSpeed)
        {
            Color fadeColor = fadeImage.color;
            fadeColor.a = i;
            fadeImage.color = fadeColor;
            yield return null;
        }

        fadeImage.raycastTarget = false;
    }

    IEnumerator StartFadeOut()
    {
        for (float i = 0f; i <= 1f; i += fadeSpeed)
        {
            Color fadeColor = fadeImage.color;
            fadeColor.a = i;
            fadeImage.color = fadeColor;
            yield return null;
        }

        fadeImage.raycastTarget = true;
    }

    //Verifica se a camera esta em movimento, se não então marca flag de idle;
    void VerifyMovement()
    {
        
        
        
        pastPosition = currentPosition;
        currentPosition = transform.position;
        positionDelta = Vector3.Distance(pastPosition, currentPosition);
        if (positionDelta <= minPositionDelta)
        {
            idle = true;
            
        }
        else idle = false;
    }



    public void UnlockAndRotate()
    {
        locked = false;
        rotate = false;
        newLookTarget = false;
        currentRotateDampTime = newDampTime;
        Invoke("RestoreRotateDampTime", 2.0f);
    }

    void RestoreRotateDampTime()
    {
        currentRotateDampTime = rotateDampTime;
    }

    public void LockCamera( Vector3 position, Vector3 newOffset)
    {
        locked = true;
        offset = newOffset;
        lockedPosition = position;
    }

    public void LockCamera()
    {
        locked = true;
        lockedPosition = transform.position - initialOffset;
    }

    public void LockAndRotate(Vector3 position, Vector3 newForward)
    {
        if (!locked)
        {
            locked = true;
            rotate = true;
            targetForward = newForward;
            lockedPosition = position - offset;

            
        }
    }

    public void LockAndRotate(Vector3 position, Vector3 newRotation, bool newTarget)
    {
        if(!locked)
        {
            locked = true;
            newLookTarget = true;
            targetForward = newRotation;
            lockedPosition = position - offset;
                
        }
    }

    public void UnlockCamera()
    {
        locked = false;
        offset = initialOffset;
    }

    public bool VerifyLocked()
    {
        return locked;
    }

}
