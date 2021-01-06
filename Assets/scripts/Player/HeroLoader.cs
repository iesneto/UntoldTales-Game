using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroLoader : MonoBehaviour {

    public GameObject hero;
    public GameObject cam;
    private GameControl data;
    public AudioClip music;

    // Use this for initialization
    void Start () {

        hero = GameObject.FindGameObjectWithTag("Hero");
        
        hero.transform.position = this.transform.position;
        data = GameControl.control;
        //Instantiate(hero, transform.position, Quaternion.identity);
        Instantiate(cam);
        data.MarkRessurectLocation(transform.position);
        //Application.targetFrameRate = 60;
        if(hero) hero.GetComponent<HeroBehavior>().EnableNavigator();
        PlayMusic();
	}
	

    public void PlayMusic()
    {
        GameControl.control.ChangeMusic(music);
    }
	
}
