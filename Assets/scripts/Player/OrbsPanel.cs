using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbsPanel : MonoBehaviour {

    public HeroStats heroStats;
    public GameObject selectedOrb;
    public GameObject lifeOrb;
    public GameObject deathOrb;
    public GameObject flameOrb;
    public GameObject frostOrb;
    public GameObject furyOrb;
    public bool init;

    
    public void Init()
    {
        if (init) return;

        init = true;
        heroStats = GameObject.FindGameObjectWithTag("Hero").GetComponent<HeroStats>();
        if(heroStats.activeOrb != -1)
        {
            switch (heroStats.activeOrb)
            {
                case 0:
                    lifeOrb.GetComponent<OrbDescription>().Activate(true);
                    selectedOrb = lifeOrb;
                    break;
                case 1:
                    deathOrb.GetComponent<OrbDescription>().Activate(true);
                    selectedOrb = deathOrb;
                    break;
                case 2:
                    flameOrb.GetComponent<OrbDescription>().Activate(true);
                    selectedOrb = flameOrb;
                    break;
                case 3:
                    frostOrb.GetComponent<OrbDescription>().Activate(true);
                    selectedOrb = frostOrb;
                    break;
                case 4:
                    furyOrb.GetComponent<OrbDescription>().Activate(true);
                    selectedOrb = furyOrb;
                    break;
            }
        }
    }

    public void UpdateOrbPanel(int orb)
    {
        Init();

        switch(orb)
        {
            case 0:
                lifeOrb.GetComponent<OrbDescription>().RevealOrb(orb);
                break;
            case 1:
                deathOrb.GetComponent<OrbDescription>().RevealOrb(orb);
                break;
            case 2:
                flameOrb.GetComponent<OrbDescription>().RevealOrb(orb);
                break;
            case 3:
                frostOrb.GetComponent<OrbDescription>().RevealOrb(orb);
                break;
            case 4:
                furyOrb.GetComponent<OrbDescription>().RevealOrb(orb);
                break;
        }
    }

    public void SelectOrb(GameObject select, int id)
    {
        if (selectedOrb == null)
        {
            selectedOrb = select;
            selectedOrb.GetComponent<OrbDescription>().Activate(true);
            heroStats.SelectOrb(id);

        }
        else
        {
            if(selectedOrb == select)
            {
                selectedOrb.GetComponent<OrbDescription>().Activate(false);
                selectedOrb = null;
                heroStats.SelectOrb(-1);
            }
            else
            {
                selectedOrb.GetComponent<OrbDescription>().Activate(false);
                selectedOrb = select;
                selectedOrb.GetComponent<OrbDescription>().Activate(true);
                heroStats.SelectOrb(id);
            }
        }
    }

}
