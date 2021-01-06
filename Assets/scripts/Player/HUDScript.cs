using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour, IPointerClickHandler
{
    private TutorialMessageSystem messageSystem;
    private bool showFirstMessage;
    private GameControl controller;
    public Inventory heroInventory;
    public MouseInterpreter heroMouseInterpreter;
    public Canvas myCanvas;
    public float timeElapssed;
    public float timeHUDShow;
	public bool clicked;
    public Image glowImage;
    public bool canGlow;
    public float glowTimer;
    public bool locked;
    public bool fade;
    public bool _init;
    public Image upIcon;
    public bool showLevelUp;
    public float levelUpTimer;
    private Vector3 upIconPosition;

    private void Start()
    {
        controller = GameControl.control;
        //heroInventory = GameObject.Find("HeroInventory").GetComponent<Inventory>();
        heroMouseInterpreter = GameObject.FindGameObjectWithTag("Hero").GetComponent<MouseInterpreter>();
        myCanvas = gameObject.GetComponent<Canvas>();
        timeHUDShow = 5.0f;
        Init();
        
    }

    public void Init()
    {
        if (_init) return;

        _init = true;
        upIconPosition = upIcon.gameObject.GetComponent<RectTransform>().localPosition;
    }

    public void ShowLevelUPIcon(bool show)
    {
        showLevelUp = show;
        if (!show)
        {
            Init();
            levelUpTimer = 0;
            upIcon.gameObject.GetComponent<RectTransform>().localPosition = upIconPosition;
            Color c = upIcon.color;
            c.a = 0;
            upIcon.color = c;
            
        }
    }

    public void LockHUD(bool l)
    {
        locked = l;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
		//clicked = true;
       // if (!heroMouseInterpreter.GetBlockRaycast())
       // {
        //Time.timeScale = 0;
        heroMouseInterpreter.CanBlockRaycast();
        heroInventory.ShowHeroInfoWindow();
        EnableGlow(false);
        HideHUD();
        if(showFirstMessage)
        {
            showFirstMessage = false;
            messageSystem.ShowMessage("Esta é a Janela de Informações do Personagem. Você pode navegar pelas abas do lado Esquerdo para ver as Estatísticas, Inventário, Habilidades e Esferas Mágicas", 0.5f);
            messageSystem.StackMessage("Na Aba de atributos você pode ver informações sobre todas as características do personagem. Toque em LEVEL UP para melhorar seus atributos", true);
            messageSystem.IgnoreUnblockRaycast();
        }
			//Invoke("resetClick", 2.0f);
      //  }
    }

    private void Update()
    {
        if (myCanvas.enabled && !locked)
        {
            timeElapssed += Time.unscaledDeltaTime;
            if (timeElapssed >= timeHUDShow)
            {
                HideHUD();
            }

            
        }

        if(canGlow)
        {
            glowTimer += Time.unscaledDeltaTime;
            
            if (glowTimer >= 0.7f)
            {
                glowTimer = 0;
                fade = !fade;
            }
            if (fade)
            {
                Color c = glowImage.color;
                c.a += Time.deltaTime;
                if (c.a >= 1) c.a = 1;
                glowImage.color = c;
            }
            else
            {
                Color c = glowImage.color;
                c.a -= Time.deltaTime;
                if (c.a <= 0) c.a = 0;
                glowImage.color = c;
            }
        }
        
        if(showLevelUp)
        {
            levelUpTimer += Time.unscaledDeltaTime;
            //Vector3 newPos = upIcon.gameObject.GetComponent<RectTransform>().localPosition;
            Vector3 newPos = new Vector3(0, 0.02f, 0);
            upIcon.gameObject.GetComponent<RectTransform>().localPosition += newPos;
            Color c = upIcon.color;
            c.a += 0.8f * Time.unscaledDeltaTime;
            if (c.a >= 1.0f) c.a = 1.0f;
            upIcon.color = c;

            if (levelUpTimer >= 1.5f)
            {
                levelUpTimer = 0;
                upIcon.gameObject.GetComponent<RectTransform>().localPosition = upIconPosition;
                c = upIcon.color;
                c.a = 0;
                upIcon.color = c;
            }


        }

    }

    public void EnableGlow(bool e)
    {
        if(e)
        {
            fade = true;
            canGlow = true;
            glowTimer = 0;
        }
        else
        {
            Color c = glowImage.color;
            c.a = 0;
            glowImage.color = c;
            canGlow = false;
        }
    }

	public void resetClick()
	{
		clicked = false;
	}

    public bool isHUDActive()
    {
        return myCanvas.enabled;
    }

    public void HideHUD()
    {
        myCanvas.enabled = false;
        controller.HideConfigButton();

    }

    public void ShowHUD()
    {
        myCanvas.enabled = true;
        timeElapssed = 0;
    }

    public void ShowFirstMessage(TutorialMessageSystem system)
    {
        messageSystem = system;
        showFirstMessage = true;
    }
}
