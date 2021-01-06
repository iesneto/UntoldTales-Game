using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    public enum tipo  {CHEST,ITEM,DOOR,LEVER, FINISHSTAGE, WEAPONRY, RESSURECT, ORB}
    public tipo myType;
    public int myID;
    private GameObject finishStage;
    
    public GameObject myAnimatedMesh;
    private float animationStep;

    private void Awake()
    {
        //timeAnimation = 0.01f;
        animationStep = 6;
    }

    public void Initiate(tipo type, int id)
    {
        myType = type;
        myID = id;
        if (myType == tipo.ORB)         
            Invoke("DisableRigidbody", 2);
        
    }

    public void FinishStage(GameObject finish)
    {
        finishStage = finish;
    }

    public void Interact()
    {
        
        switch(myType)
        {
            case tipo.CHEST:
                gameObject.GetComponent<Chest>().Open();
                break;
            case tipo.ITEM:
                gameObject.GetComponent<WorldItem>().TryToAddItem();
                break;
            case tipo.DOOR:
                break;
            case tipo.LEVER:
                gameObject.GetComponent<Lever>().Pull();
                break;
            case tipo.FINISHSTAGE:
                gameObject.GetComponent<FinishStagePoint>().Interact();
                break;
            case tipo.WEAPONRY:
                gameObject.GetComponent<Weaponry>().PickUp();
                break;
            case tipo.RESSURECT:
                GetComponent<RessurectStone>().ActivateStone();
                break;
            case tipo.ORB:
                GameObject hero = GameObject.FindGameObjectWithTag("Hero");
                hero.GetComponent<HeroStats>().PickUpOrb(myID);
                if(myID == 0)
                {
                    TutorialMessageSystem heroMessages = hero.transform.Find("Messages").gameObject.GetComponent<TutorialMessageSystem>();
                    heroMessages.ShowMessage("Parabéns! Você recuperou a primeira Esfera Mágica. ATIVE seu poder abrindo o Painel das Esferas!");
                }
                if(finishStage != null) finishStage.GetComponent<FinishStagePoint>().ShowFinishPoint();
                Destroy(gameObject);
                break;
        }
    }

    void DisableRigidbody()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    // Metodo chamado quando o usuario toca neste objeto
    public void TouchAnimate()
    {
        if (myType == tipo.RESSURECT) return;
        StartCoroutine("AnimateTouch");
    }

    private IEnumerator AnimateTouch()
    {
        for (float i = -(Mathf.PI)/2; i <= (Mathf.PI)/2; i += (Mathf.PI)/animationStep)
        {
            
            Vector3 newScale = new Vector3(1 + Mathf.Cos(i) / 2, 1 + Mathf.Cos(i) / 2, 1 + Mathf.Cos(i) / 2);
            myAnimatedMesh.transform.localScale = newScale;
            
            yield return null;
        }

        

    }

}
