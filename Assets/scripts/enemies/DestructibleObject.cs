using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour {

    public int hits;

    public Renderer[] myRender;
    private Color flashColour = new Color(1f, 0f, 0f, 1f);
    private bool damaged;
    private float flashSpeed = 20;
    private GameObject hero;

    public enum destructibleType { ENTANGLE };
    public destructibleType myType;

    public void SetHero(GameObject h)
    {
        hero = h;
    }

    public void Hit()
    {
        hits--;
        damaged = true;
        
    }

    public void DestroyObject()
    {
        

        switch (myType)
        {
            case destructibleType.ENTANGLE:
                hero.GetComponent<HeroBehavior>().DisableSpell(HeroBehavior.enemySpecial.entangle);

                break;
            default: break;
        }
        Destroy(this.gameObject);
    }

    void Update()
    {
        // Se recebeu dano...
        if (damaged)
        {
            // ...torna a tela vermelha com a imagem de flash
            for(int i=0; i < myRender.Length; i++)
                myRender[i].material.color = flashColour;
        }
        else
        {
            // ... se nao, entao volta a limpar a tela
            for (int i = 0; i < myRender.Length; i++)
                myRender[i].material.color = Color.Lerp(myRender[i].material.color, Color.white, flashSpeed * Time.deltaTime);
        }
        damaged = false;

    }
}
