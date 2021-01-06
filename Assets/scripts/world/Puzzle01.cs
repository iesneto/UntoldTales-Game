using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle01 : MonoBehaviour {

    public GameObject lever01;
    public GameObject lever02;
    public GameObject lever03;
    public GameObject lever04;
    public GameObject chest;
    public int[] sequence = { 3, 1, 4, 2 };
    public bool completed;
    private GameControl control;
    public int moves;
    private float xpReward;

    private void Start()
    {
        control = GameControl.control;
        chest = transform.Find("Chest").gameObject;
        Invoke("HideChest", 2);
        lever01 = transform.Find("Pilar01/Lever").gameObject;
        lever02 = transform.Find("Pilar02/Lever").gameObject;
        lever03 = transform.Find("Pilar03/Lever").gameObject;
        lever04 = transform.Find("Pilar04/Lever").gameObject;
        transform.GetChild(0).Find("Flame").gameObject.GetComponent<ParticleSystem>().Stop();
        transform.GetChild(1).Find("Flame").gameObject.GetComponent<ParticleSystem>().Stop();
        transform.GetChild(2).Find("Flame").gameObject.GetComponent<ParticleSystem>().Stop();
        transform.GetChild(3).Find("Flame").gameObject.GetComponent<ParticleSystem>().Stop();
        lever01.GetComponent<Lever>().InitiateLever(1, this.gameObject);
        lever02.GetComponent<Lever>().InitiateLever(2, this.gameObject);
        lever03.GetComponent<Lever>().InitiateLever(3, this.gameObject);
        lever04.GetComponent<Lever>().InitiateLever(4, this.gameObject);
        moves = 0;
        xpReward = 200;
        if (control.VerifyPuzzle(0)) CompletePuzzle();
        else chest.GetComponent<Chest>().ActivateChest(true);
    }

    void HideChest()
    {
        //chest.SetActive(false);
        chest.GetComponent<Chest>().Hide();

    }

    public void PullLever(int id)
    {
        if (!completed)
        {
            if (sequence[moves] == id)
            {
                transform.GetChild(id - 1).Find("Flame").gameObject.GetComponent<ParticleSystem>().Play();
                //transform.GetChild(id - 1).FindChild("Flame").gameObject.GetComponent<MeshRenderer>().enabled = true;
                moves++;
            }
            else
            {
                Invoke("ResetLevers", 1);
                moves = 0;
                
            }

            if (moves == 4)
            {
                completed = true;
                control.PuzzleCompleted(0);
                Invoke("ShowChest", 0.5f);
                GameObject.FindGameObjectWithTag("Hero").GetComponent<HeroStats>().GiveXP(xpReward);
            }
        }
        

       
    }

    void ResetLevers()
    {
        transform.GetChild(0).Find("Flame").gameObject.GetComponent<ParticleSystem>().Stop();
        transform.GetChild(1).Find("Flame").gameObject.GetComponent<ParticleSystem>().Stop();
        transform.GetChild(2).Find("Flame").gameObject.GetComponent<ParticleSystem>().Stop();
        transform.GetChild(3).Find("Flame").gameObject.GetComponent<ParticleSystem>().Stop();
        //transform.GetChild(1).FindChild("Flame").gameObject.GetComponent<MeshRenderer>().enabled = false;
        //transform.GetChild(2).FindChild("Flame").gameObject.GetComponent<MeshRenderer>().enabled = false;
        //transform.GetChild(3).FindChild("Flame").gameObject.GetComponent<MeshRenderer>().enabled = false;

        lever01.GetComponent<Lever>().ResetLever();
        lever02.GetComponent<Lever>().ResetLever();
        lever03.GetComponent<Lever>().ResetLever();
        lever04.GetComponent<Lever>().ResetLever();
    }

    void ShowChest()
    {
        //chest.SetActive(true);
        chest.GetComponent<Chest>().Show();
    }

    void CompletePuzzle()
    {
        transform.GetChild(0).Find("Flame").gameObject.GetComponent<ParticleSystem>().Play();
        transform.GetChild(1).Find("Flame").gameObject.GetComponent<ParticleSystem>().Play();
        transform.GetChild(2).Find("Flame").gameObject.GetComponent<ParticleSystem>().Play();
        transform.GetChild(3).Find("Flame").gameObject.GetComponent<ParticleSystem>().Play();

        lever01.GetComponent<Lever>().PullLever();
        lever02.GetComponent<Lever>().PullLever();
        lever03.GetComponent<Lever>().PullLever();
        lever04.GetComponent<Lever>().PullLever();
        completed = true;
        chest.GetComponent<Chest>().ActivateChest(false);
    }
}
