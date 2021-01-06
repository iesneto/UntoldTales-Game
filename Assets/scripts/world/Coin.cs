using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour {

    public ulong value;
    public GameObject hero;
    public float waitTime;
    public float timer;
    public float speed;
    public bool init;
    public GameObject myCanvas;
    public Text valueText;
    public bool collected;
    public bool canCollect;
    public GameObject myMesh;
    public float scaleFactor;


    




    public void Initialize( ulong _value, float scale)
    {
        myMesh.transform.localScale *= scale;
        hero = GameObject.FindGameObjectWithTag("Hero");
        value = _value;
        timer = 0;
        init = true;
        myCanvas.SetActive(false);
        collected = false;
        canCollect = false;
        
    }

    

    private void Update()
    {
        if (init)
        {
            timer += Time.deltaTime;
            if (timer >= waitTime / 5 && !canCollect) canCollect = true;
            if (timer >= waitTime)
            {

                Vector3 direction = (hero.transform.position - transform.position).normalized;
                transform.position += direction * speed;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!collected && canCollect)
        {
            //if (collision.collider.isTrigger)
            //    return;

            GameObject target = other.gameObject;


            if (target.tag == "Hero")
            {

                Collect(target);

            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!collected && canCollect)
        {
            //if (collision.collider.isTrigger)
            //    return;

            GameObject target = other.gameObject;


            if (target.tag == "Hero")
            {

                Collect(target);

            }
        }
    }
    

    void Collect(GameObject target)
    {
        init = false;
        collected = true;
        target.GetComponent<HeroStats>().AddCoin(value);
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | 
                                                            RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        myMesh.SetActive(false);
        valueText.text = "$" + value.ToString();
        myCanvas.SetActive(true);
        Destroy(gameObject, 1.5f);
    }

    



}
