using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    private GameControl controller;
    public GameObject mainScreen;
    public GameObject continueGameScreen;
    public GameObject newGameScreen;
    
    // Use this for initialization
    void Start () {
        controller = GameControl.control;
        mainScreen = transform.GetChild(0).gameObject;
        continueGameScreen = transform.GetChild(1).gameObject;
        newGameScreen = transform.GetChild(2).gameObject;
        InitMenus();
        


    }

    public void ReturnToLevelSelectMap()
    {
        controller.ReturnToLevelSelect();
    }

    void InitMenus()
    {
        if (controller.VerifyFile(0))
        {
            mainScreen.transform.Find("ButtonsPanel/ContinueGameButton").gameObject.SetActive(true);
            continueGameScreen.transform.Find("ButtonsPanel/SavedGameButton01").gameObject.SetActive(true);
            newGameScreen.transform.Find("ButtonsPanel/StartNewGameButton01/Text").gameObject.GetComponent<Text>().text = "Jogo Salvo 01";
            
        }
        else
        {
            continueGameScreen.transform.Find("ButtonsPanel/SavedGameButton01").gameObject.SetActive(false);
            newGameScreen.transform.Find("ButtonsPanel/StartNewGameButton01/Text").gameObject.GetComponent<Text>().text = "Slot Vazio";
        }
        if (controller.VerifyFile(1))
        {
            mainScreen.transform.Find("ButtonsPanel/ContinueGameButton").gameObject.SetActive(true);
            continueGameScreen.transform.Find("ButtonsPanel/SavedGameButton02").gameObject.SetActive(true);
            newGameScreen.transform.Find("ButtonsPanel/StartNewGameButton02/Text").gameObject.GetComponent<Text>().text = "Jogo Salvo 02";
            
        }
        else
        {
            continueGameScreen.transform.Find("ButtonsPanel/SavedGameButton02").gameObject.SetActive(false);
            newGameScreen.transform.Find("ButtonsPanel/StartNewGameButton02/Text").gameObject.GetComponent<Text>().text = "Slot Vazio";
        }
        if (controller.VerifyFile(2))
        {
            mainScreen.transform.Find("ButtonsPanel/ContinueGameButton").gameObject.SetActive(true);
            continueGameScreen.transform.Find("ButtonsPanel/SavedGameButton03").gameObject.SetActive(true);
            newGameScreen.transform.Find("ButtonsPanel/StartNewGameButton03/Text").gameObject.GetComponent<Text>().text = "Jogo Salvo 03";
            
        }
        else
        {
            continueGameScreen.transform.Find("ButtonsPanel/SavedGameButton03").gameObject.SetActive(false);
            newGameScreen.transform.Find("ButtonsPanel/StartNewGameButton03/Text").gameObject.GetComponent<Text>().text = "Slot Vazio";
        }
        
        controller.InitGameControl(this.gameObject, "MainMenu");
    }

	
    public void ExitGame()
    {
        controller.ExitGame();
    }

    public void StartNewGame(int slot)
    {
        controller.SelectSaveSlot(slot);
    }

    public void ContinueGame(int slot)
    {
        controller.SelectLoadSlot(slot);
    }
	
}
