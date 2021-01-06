using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracnyWeb : MonoBehaviour {

    private float webTime;
    private GameObject father;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 2.0f);
	}

    public void SetTime(float t, GameObject f)
    {
        webTime = t;
        father = f;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.isTrigger)
            return;

        GameObject target = collision.gameObject;
        


        if(target.tag == "Hero")
        {
            target.GetComponent<HeroBehavior>().EnemySpecial(HeroBehavior.enemySpecial.wrapped, webTime, 0, father);
            Destroy(gameObject);
        }
    }

    
}
