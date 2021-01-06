using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle02Stone : MonoBehaviour {

    public Light myLight;
    public int id;
    public bool lit;
    public float litLevel;
    public float maxLightIntensity;
    public float litSpeed;
    public Puzzle02 puzzle;
    public bool pressed;

    private void Start()
    {
        myLight = transform.GetChild(0).GetComponent<Light>();
        myLight.intensity = 0.0f;
        pressed = false;
    }

    private void Update()
    {
        if(lit)
        {
            
            if(litLevel <= maxLightIntensity)
            {
                litLevel += Time.deltaTime * litSpeed;
                myLight.intensity = litLevel;
            }
        }
        else
        {
            if (litLevel >= 0.0f)
            {
                litLevel -= Time.deltaTime * litSpeed;
                myLight.intensity = litLevel;
            }
        }
    }

    public void ChangeLightColor(Color newColor)
    {
        myLight.color = newColor;
    }

    public void Pressed()
    {
        if (!pressed)
        {
            pressed = true;
            puzzle.StonePressed(id);
        }
    }

    public void LightOn()
    {
        lit = true;
    }

    public void LightOff()
    {
        lit = false;
        pressed = false;
    }
}
