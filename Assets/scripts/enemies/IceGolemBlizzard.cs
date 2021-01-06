using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGolemBlizzard : MonoBehaviour
{
    private BoxCollider myCollider;
    public float speed;
    public float end;
    public float start;
    private bool hit;

    private void Start()
    {
        myCollider = GetComponent<BoxCollider>();
        hit = false;
    }

    public void StartBlizzardCollider()
    {
        myCollider.enabled = true;
        StartCoroutine("BlizzardUpdate");
        
    }

    public void StopBlizzardCollider()
    {
        myCollider.center = Vector3.zero;
        myCollider.size = Vector3.one;
        myCollider.enabled = false;
        hit = false;
    }

    IEnumerator BlizzardUpdate()
    {
        for (float step = start; step <= end; step += speed)
        {
            Vector3 newCenter = new Vector3(0, 0, step);
            Vector3 newSize = new Vector3(1, 1, step + 1);
            myCollider.center = newCenter;
            myCollider.size = newSize;
            yield return null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (other.isTrigger)
        //    return;

        GameObject target = other.gameObject;


        if (!hit)
        {
            if (target.tag == "Hero")
            {
                hit = true;
                transform.parent.gameObject.GetComponent<IceGolemBehavior>().BlizzardHit();
                //target.GetComponent<HeroBehavior>().EnemySpecial(HeroBehavior.enemySpecial.wrapped, webTime, 0, father);
                //Destroy(gameObject);
            }
        }
    }
}
