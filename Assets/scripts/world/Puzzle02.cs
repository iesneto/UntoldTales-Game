using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle02 : MonoBehaviour {

    public float heroDistance;
    public float minHeroDistance;
    public GameObject hero;
    public bool near;
    public bool completed;
    public Color completedColor;

    public GameObject chest;
    public int[] sequenceID;
    public List<Puzzle02Stone> stones;
    private GameControl control;
    public int moves;
    public int xpReward;
    public Vector3 lookRotation;

    private void Start()
    {
        control = GameControl.control;
        hero = GameObject.FindGameObjectWithTag("Hero");
        stones = new List<Puzzle02Stone>();
        //chest = transform.Find("Chest").gameObject;
        Invoke("HideChest", 2);

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            stones.Add(transform.GetChild(i).gameObject.GetComponent<Puzzle02Stone>());
        }

        moves = 0;
        if (control.VerifyPuzzle(1)) Invoke("CompletePuzzle",1);
        else chest.GetComponent<Chest>().ActivateChest(true);

    }

    void HideChest()
    {
        chest.GetComponent<Chest>().Hide();
    }

    void ShowChest()
    {
        chest.GetComponent<Chest>().Show();
    }

    private void Update()
    {
        if (!completed)
        {
            heroDistance = (transform.position - hero.transform.position).magnitude;

            if (heroDistance <= minHeroDistance)
            {
                if (!near)
                {
                    near = true;
                    Camera.main.GetComponent<CameraFollowing>().LockAndRotate(transform.position, lookRotation);
                }
            }
            else if (near)
            {
                near = false;
                Camera.main.GetComponent<CameraFollowing>().UnlockAndRotate();
            }
        }
    }

    public void StonePressed(int id)
    {
        if (!completed)
        {
            stones[id].LightOn();
            if (sequenceID[moves] == id)
            {
                
                moves++;
            }
            else
            {
                Invoke("ResetStones", 0.3f);
                moves = 0;

            }

            if (moves == sequenceID.Length)
            {
                completed = true;
                control.PuzzleCompleted(1);
                Invoke("ShowChest", 0.5f);
                GameObject.FindGameObjectWithTag("Hero").GetComponent<HeroStats>().GiveXP(xpReward);
                for(int i = 0; i < sequenceID.Length; i++)
                {
                    stones[sequenceID[i]].ChangeLightColor(completedColor);
                }
                Camera.main.GetComponent<CameraFollowing>().UnlockAndRotate();
            }
        }



    }

    void ResetStones()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            stones[i].LightOff();
        }
    }

    void CompletePuzzle()
    {
        for (int i = 0; i < sequenceID.Length; i++)
        {
            stones[sequenceID[i]].LightOn();
            stones[sequenceID[i]].ChangeLightColor(completedColor);
        }
        completed = true;
        chest.GetComponent<Chest>().ActivateChest(false);
    }
}
