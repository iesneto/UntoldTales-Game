using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectMenu : MonoBehaviour {

    private GameControl controller;
    private Transform stagePanel;
    // Use this for initialization
    void Start()
    {
        controller = GameControl.control;

        stagePanel = gameObject.transform.Find("Canvas/StagePanel");
        ResetLevels();
        controller.InitGameControl(this.gameObject, "LevelSelectMenu");
    }

    public void ResetLevels()
    {
        for (int i = 0; i < stagePanel.childCount; i++)
        {
            stagePanel.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void LevelSelect(int level)
    {
        controller.LevelSelect(level);
    }

    public void ShowLevels()
    {
        controller.PlayLevelSelectMusic();
        ResetLevels();
        for(int i = 0; i < controller.stage; i++)
        {
            if(stagePanel.childCount > i)
                stagePanel.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        controller.ReturnToMainMenu();
    }

    public void SaveGame()
    {
        controller.Save();
    }
}
