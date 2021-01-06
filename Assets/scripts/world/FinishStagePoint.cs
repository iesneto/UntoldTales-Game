using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishStagePoint : MonoBehaviour {

    public GameObject monitorObj;
    private GameControl controller;
    public int myStage;
    [Header("Mark if is a Boss Level")]
    public bool isBoss;

    private void Start()
    {
        controller = GameControl.control;
        gameObject.SetActive(false);
        if (!isBoss) monitorObj.GetComponent<enemyRespawner>().DefineFinishPoint(gameObject);
        else monitorObj.GetComponent<BossBehavior>().DefineFinishStage(gameObject);
        gameObject.GetComponent<InteractableObject>().Initiate(InteractableObject.tipo.FINISHSTAGE, myStage);
        // gameObject.GetComponent<MeshRenderer>().enabled = false;
        
    }

    public void ShowFinishPoint()
    {
        //gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.SetActive(true);
    }

    public void HideFinishPoint()
    {
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.SetActive(false);
    }

    public void Interact()
    {
        controller.FinishStage(myStage);
    }
}
