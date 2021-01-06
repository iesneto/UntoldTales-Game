using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntangleSpell : MonoBehaviour {


    private GameObject father;





    public MeshRenderer[] renders;
    public BoxCollider[] colliders;
    private bool nextRoot;
    private int maxRoots;
    private int rootsCount;
    private bool render;
    public float alphaLimit;
    private float alphaValue;
    public float alphaSpeed;



    // Use this for initialization
    void Start()
    {
        

        //currentRootPosition = transform.position - rootPrefabPosition;
        //GameObject newRoot = Instantiate(rootPrefab, currentRootPosition, transform.rotation);
        //newRoot.transform.Rotate(new Vector3(0, 1, 0), 90);
        //Destroy(newRoot, rootLifeTime);
        maxRoots = renders.Length;
        rootsCount = 0;
        alphaValue = 1;
        for (int i = 0; i < maxRoots; i++)
        {
            renders[i].material.SetFloat("_Cutoff", alphaValue);
            colliders[i].enabled = false;
        }
        render = true;
        nextRoot = false;
        
    }

    

    public void Update()
    {

        
        if (!render) nextRoot = true;
        else
        {
            alphaValue -= Time.deltaTime * alphaSpeed;
            renders[rootsCount].material.SetFloat("_Cutoff", alphaValue);
            if (alphaValue <= alphaLimit)
            {
                renders[rootsCount].material.SetFloat("_Cutoff", alphaLimit);
                render = false;
                alphaValue = 1;
                colliders[rootsCount].enabled = true;
                rootsCount++;
                //Liga o COllider
                
            }
        }

        if(nextRoot)
        {

            nextRoot = false;
            if (rootsCount < maxRoots)
            {
                render = true;
            }
            else Destroy(gameObject, 0.5f);
        }

        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.isTrigger)
            return;

        GameObject target = col.gameObject;



        if (target.tag == "Hero")
        {
            target.GetComponent<HeroBehavior>().EnemySpecial(HeroBehavior.enemySpecial.entangle, 0, 0, this.gameObject);
            
            Destroy(gameObject);
        }
    }
}
