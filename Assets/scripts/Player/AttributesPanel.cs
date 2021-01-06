using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributesPanel : MonoBehaviour {

    public HUDScript hud;
    public int attributePointsRemaining;
    public int allocatedPoints;
    public int tempStrength;
    public int tempInteligence;
    public int tempDexterity;
    public HeroStats stats;
    public GameObject levelUpPanel;
    public GameObject levelUpButton;
    public GameObject infoPanel;
    public GameObject statsPanel;
    public Text strengthText;
    public Text dexterityText;
    public Text inteligenceText;
    public Text attributePointsText;
    public Text tempStrengthText;
    public Text tempInteligenceText;
    public Text tempDexterityText;
    public GameObject confirmButton;
    public GameObject strengthUpButton;
    public GameObject strengthDownButton;
    public GameObject dexterityUpButton;
    public GameObject dexterityDownButton;
    public GameObject inteligenceUpButton;
    public GameObject inteligenceDownButton;
    private bool _init;

    public Image upIcon;
    public bool showLevelUp;
    public float levelUpTimer;
    private Vector3 upIconPosition;

    private void Start()
    {
        Init();

        //UpdateStatsPanel();

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

    private void Update()
    {
        if (showLevelUp)
        {
            levelUpTimer += Time.unscaledDeltaTime;
            Vector3 newPos = new Vector3(0, 0.02f, 0);
            upIcon.gameObject.GetComponent<RectTransform>().localPosition += newPos;
            Color c = upIcon.color;
            c.a += 0.5f * Time.unscaledDeltaTime;
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

    public void Init()
    {
        if (_init) return;

        upIconPosition = upIcon.gameObject.GetComponent<RectTransform>().localPosition;
        allocatedPoints = 0;
        stats = GameObject.FindGameObjectWithTag("Hero").GetComponent<HeroStats>();
        attributePointsRemaining = stats.attributePointsRemaining;
        levelUpPanel = transform.GetChild(5).gameObject;
        statsPanel = transform.GetChild(3).gameObject;
        statsPanel.GetComponent<StatsPanel>().Init();
        infoPanel = transform.GetChild(2).gameObject;
        infoPanel.GetComponent<InfoPanel>().Init();
        levelUpButton = infoPanel.transform.GetChild(1).gameObject;
        strengthText = levelUpPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
        dexterityText = levelUpPanel.transform.GetChild(1).gameObject.GetComponent<Text>();
        inteligenceText = levelUpPanel.transform.GetChild(2).gameObject.GetComponent<Text>();
        attributePointsText = levelUpPanel.transform.GetChild(3).gameObject.GetComponent<Text>();
        tempStrengthText = levelUpPanel.transform.GetChild(4).gameObject.GetComponent<Text>();
        tempDexterityText = levelUpPanel.transform.GetChild(5).gameObject.GetComponent<Text>();
        tempInteligenceText = levelUpPanel.transform.GetChild(6).gameObject.GetComponent<Text>();
        confirmButton = levelUpPanel.transform.GetChild(7).gameObject;
        strengthUpButton = levelUpPanel.transform.GetChild(8).gameObject;
        strengthDownButton = levelUpPanel.transform.GetChild(9).gameObject;
        dexterityUpButton = levelUpPanel.transform.GetChild(10).gameObject;
        dexterityDownButton = levelUpPanel.transform.GetChild(11).gameObject;
        inteligenceUpButton = levelUpPanel.transform.GetChild(12).gameObject;
        inteligenceDownButton = levelUpPanel.transform.GetChild(13).gameObject;
        levelUpPanel.SetActive(false);
        levelUpButton.SetActive(false);
        _init = true;
    }

    public void UpdateStatsPanel()
    {
        Init();
        strengthText.text =  stats.strength.ToString();
        dexterityText.text =  stats.dexterity.ToString();
        inteligenceText.text =  stats.inteligence.ToString();
        if (stats.attributePointsRemaining != 0)
        {
            
            attributePointsText.text = "Pontos de Atributo Restando " + stats.attributePointsRemaining.ToString();
            UpdateAttributePoints(stats.attributePointsRemaining);
            strengthUpButton.SetActive(true);
            strengthDownButton.SetActive(true);
            dexterityUpButton.SetActive(true);
            dexterityDownButton.SetActive(true);
            inteligenceUpButton.SetActive(true);
            inteligenceDownButton.SetActive(true);
            levelUpButton.SetActive(true);
            hud.ShowLevelUPIcon(true);
            ShowLevelUPIcon(true);
        }
        else
        {
            attributePointsText.text = "";
            tempStrengthText.text = "0";
            tempDexterityText.text = "0";
            tempInteligenceText.text = "0";
            tempStrengthText.enabled = false;
            tempDexterityText.enabled = false;
            tempInteligenceText.enabled = false;
            strengthUpButton.SetActive(false);
            strengthDownButton.SetActive(false);
            dexterityUpButton.SetActive(false);
            dexterityDownButton.SetActive(false);
            inteligenceUpButton.SetActive(false);
            inteligenceDownButton.SetActive(false);

            levelUpButton.SetActive(false);
            hud.ShowLevelUPIcon(false);
            ShowLevelUPIcon(false);
        }
        if (allocatedPoints > 0)
        {
            confirmButton.SetActive(true);
        }
        else confirmButton.SetActive(false);
        infoPanel.GetComponent<InfoPanel>().UpdateValues();
        statsPanel.GetComponent<StatsPanel>().UpdateValues();
        UpdateXpInfo();
    }

    public void UpdateXpInfo()
    {
        infoPanel.GetComponent<InfoPanel>().UpdateXPBar();
    }

    public void UpdateAttributePoints(int ap)
    {
        attributePointsRemaining = ap;
        ShowButtons();
    }

    public void ShowButtons()
    {
        tempStrengthText.enabled = true;
        tempDexterityText.enabled = true;
        tempInteligenceText.enabled = true;
        attributePointsText.text = "Pontos de Atributo Restando " + stats.attributePointsRemaining.ToString();
    }

    public void OpenLevelUP()
    {
        levelUpPanel.SetActive(true);
    }

    public void CloseLevelUP()
    {
        levelUpPanel.SetActive(false);
    }

    public void UpdateConfirmButton()
    {
        if (allocatedPoints > 0)
        {
            confirmButton.SetActive(true);
        }
        else confirmButton.SetActive(false);
    }

    public void StrengthUp()
    {
        if(allocatedPoints < attributePointsRemaining)
        {
            allocatedPoints++;
            tempStrength++;
            tempStrengthText.text = "+" + tempStrength.ToString();
            int i = attributePointsRemaining - allocatedPoints;
            attributePointsText.text = "Pontos de Atributo Restando " + i.ToString();
        }
        UpdateConfirmButton();
    }
    public void StrengthDown()
    {
        if (tempStrength > 0)
        {
            allocatedPoints--;
            tempStrength--;
            if (tempStrength == 0) tempStrengthText.text = "0";
            else tempStrengthText.text = "+" + tempStrength.ToString();
            int i = attributePointsRemaining - allocatedPoints;
            attributePointsText.text = "Pontos de Atributo Restando " + i.ToString();
        }
        UpdateConfirmButton();
    }

    public void DexterityUp()
    {
        if (allocatedPoints < attributePointsRemaining)
        {
            allocatedPoints++;
            tempDexterity++;
            tempDexterityText.text = "+" + tempDexterity.ToString();
            int i = attributePointsRemaining - allocatedPoints;
            attributePointsText.text = "Pontos de Atributo Restando " + i.ToString();
        }
        UpdateConfirmButton();
    }
    public void DexterityDown()
    {
        if (tempDexterity > 0)
        {
            allocatedPoints--;
            tempDexterity--;
            if (tempDexterity == 0) tempDexterityText.text = "0";
            else tempDexterityText.text = "+" + tempDexterity.ToString();
            int i = attributePointsRemaining - allocatedPoints;
            attributePointsText.text = "Pontos de Atributo Restando " + i.ToString();
        }
        UpdateConfirmButton();
    }

    public void InteligenceUp()
    {
        if (allocatedPoints < attributePointsRemaining)
        {
            allocatedPoints++;
            tempInteligence++;
            tempInteligenceText.text = "+" + tempInteligence.ToString();
            int i = attributePointsRemaining - allocatedPoints;
            attributePointsText.text = "Pontos de Atributo Restando " + i.ToString();
        }
        UpdateConfirmButton();
    }
    public void InteligenceDown()
    {
        if (tempInteligence > 0)
        {
            allocatedPoints--;
            tempInteligence--;
            if (tempInteligence == 0) tempInteligenceText.text = "0";
            else tempInteligenceText.text = "+" + tempInteligence.ToString();
            int i = attributePointsRemaining - allocatedPoints;
            attributePointsText.text = "Pontos de Atributo Restando " + i.ToString();
        }
        UpdateConfirmButton();
    }

    public void Confirm()
    {
        attributePointsRemaining -= allocatedPoints;
        allocatedPoints = 0;
        stats.ConfirmLevelUp(attributePointsRemaining, tempStrength, tempDexterity, tempInteligence);
        tempStrength = 0;
        tempInteligence = 0;
        tempDexterity = 0;
        tempStrengthText.text = "0";
        tempDexterityText.text = "0";
        tempInteligenceText.text = "0";
        
    }
}
