using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RessurectStone : MonoBehaviour {

    private GameControl data;
    public Transform ressurectLocation;
    //public Image m_image;
    //bool activated;
    //float timer;
    //bool fade;
    public GameObject interactableParticles;
    public GameObject activatedParticles;


    private void Start()
    {
        data = GameControl.control;
        GetComponent<InteractableObject>().Initiate(InteractableObject.tipo.RESSURECT, 0);
        activatedParticles.SetActive(false);
       // Color c = m_image.color;
        //c.a = 0;
       // m_image.color = c;
        //activated = false;
    }

    private void Update()
    {
        //if(activated)
        //{
        //    timer += Time.deltaTime;
        //    if(timer >= 1.0f)
        //    {
        //        timer = 0;
        //        fade = !fade;
        //    }
        //    if(fade)
        //    {
        //        Color c = m_image.color;
        //        c.a += Time.deltaTime;
        //        if (c.a >= 1) c.a = 1;
        //        m_image.color = c;
        //    }
        //    else
        //    {
        //        Color c = m_image.color;
        //        c.a -= Time.deltaTime;
        //        if (c.a <= 0) c.a = 0;
        //        m_image.color = c;
        //    }
        //}
    }

    public void ActivateStone()
    {
        data.MarkRessurectStoneLocation(this.gameObject, ressurectLocation.position);
        interactableParticles.SetActive(false);
        activatedParticles.SetActive(true);
        //activated = true;
        //fade = true;
    }
    public void DeactivateStone()
    {
        interactableParticles.SetActive(true);
        activatedParticles.SetActive(false);
    }
}
