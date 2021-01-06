using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {

    public int myID;
    public GameObject myController;
    public InteractableObject interactScript;
    public bool pulled;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void InitiateLever(int id, GameObject go)
    {
        pulled = false;
        myController = go;
        myID = id;
        animator = GetComponent<Animator>();
        interactScript = gameObject.GetComponent<InteractableObject>();
        interactScript.Initiate(InteractableObject.tipo.LEVER, myID);
    }

	public void Pull()
    {
        if (myController.tag == "Puzzle")
        {
            
            if (!pulled)
            {
                pulled = true;
                animator.SetBool("pull", true);
                Puzzle01 puzzle = myController.GetComponent<Puzzle01>();
                puzzle.PullLever(myID);
            }
        }
        else
        {
            //do something
            animator.SetBool("pull", false);
        }
    }

    public void ResetLever()
    {
        pulled = false;
        animator.SetBool("pull", false);
    }

    public void PullLever()
    {
        animator.SetBool("pull", true);
    }
}
