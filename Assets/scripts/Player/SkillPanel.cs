using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour {

    public int skillPointsRemaining;
    //public int allocatedPoints;
    public int tempDeepSlash;
    public int tempShieldBash;
    public int tempImpale;
    public int tempMightyLeap;
    public HeroStats stats;
    //public GameObject skillPanel;
    public GameObject skillPanel;
    public GameObject deepSlashSkilllvl1;
    public GameObject shieldBashSkilllvl1;
    public GameObject mightyLeapSkilllvl1;
    public GameObject impaleSkilllvl1;
    public GameObject deepSlashSkilllvl2;
    public GameObject shieldBashSkilllvl2;
    public GameObject mightyLeapSkilllvl2;
    public GameObject impaleSkilllvl2;
    public GameObject skillSelected;
    public GameObject skillRemainingText;
    public GameObject upgradeButton;
    private bool _init;
    public Image upIcon;
    public bool showLevelUp;
    public float levelUpTimer;
    private Vector3 upIconPosition;

    //public Text deepSlashText;
    //public Text shieldBashText;
    //public Text impaleText;
    //public Text mightyLeapText;
    //public Text skillPointsText;
    //public Text tempDeepSlashText;
    //public Text tempShieldBashText;
    //public Text tempImpaleText;
    //public Text tempMightyLeapText;
    //public GameObject confirmButton;
    //public GameObject deepSlashUpButton;
    //public GameObject deepSlashDownButton;
    //public GameObject shieldBashUpButton;
    //public GameObject shieldBashDownButton;
    //public GameObject impaleUpButton;
    //public GameObject impaleDownButton;
    //public GameObject mightyLeapUpButton;
    //public GameObject mightyLeapDownButton;

    private void Start()
    {

        Init();
        
    }

    public void Init ()
    {
        if (_init) return;

        //allocatedPoints = 0;
        upIconPosition = upIcon.gameObject.GetComponent<RectTransform>().localPosition;
        stats = GameObject.FindGameObjectWithTag("Hero").GetComponent<HeroStats>();
        skillPointsRemaining = stats.skillPointsRemaining;
        //skillPanel = transform.GetChild(0).gameObject;
        skillPanel = transform.GetChild(2).gameObject;
        upgradeButton = transform.GetChild(3).gameObject;
        upgradeButton.SetActive(false);
        deepSlashSkilllvl1 = skillPanel.transform.GetChild(0).gameObject;
        shieldBashSkilllvl1 = skillPanel.transform.GetChild(1).gameObject;
        mightyLeapSkilllvl1 = skillPanel.transform.GetChild(2).gameObject;
        impaleSkilllvl1 = skillPanel.transform.GetChild(3).gameObject;
        deepSlashSkilllvl2 = skillPanel.transform.GetChild(4).gameObject;
        shieldBashSkilllvl2 = skillPanel.transform.GetChild(5).gameObject;
        mightyLeapSkilllvl2 = skillPanel.transform.GetChild(6).gameObject;
        impaleSkilllvl2 = skillPanel.transform.GetChild(7).gameObject;
        skillRemainingText = skillPanel.transform.GetChild(8).gameObject;
        

        UpdateSkills();
        _init = true;
        //deepSlashText = skillPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
        //shieldBashText = skillPanel.transform.GetChild(1).gameObject.GetComponent<Text>();
        //impaleText = skillPanel.transform.GetChild(2).gameObject.GetComponent<Text>();
        //mightyLeapText = skillPanel.transform.GetChild(3).gameObject.GetComponent<Text>();
        //skillPointsText = skillPanel.transform.GetChild(4).gameObject.GetComponent<Text>();
        //tempDeepSlashText = skillPanel.transform.GetChild(5).gameObject.GetComponent<Text>();
        //tempShieldBashText = skillPanel.transform.GetChild(6).gameObject.GetComponent<Text>();
        //tempImpaleText = skillPanel.transform.GetChild(7).gameObject.GetComponent<Text>();
        //tempMightyLeapText = skillPanel.transform.GetChild(8).gameObject.GetComponent<Text>();
        //confirmButton = skillPanel.transform.GetChild(9).gameObject;
        //deepSlashUpButton = skillPanel.transform.GetChild(10).gameObject;
        //deepSlashDownButton = skillPanel.transform.GetChild(11).gameObject;
        //shieldBashUpButton = skillPanel.transform.GetChild(12).gameObject;
        //shieldBashDownButton = skillPanel.transform.GetChild(13).gameObject;
        //impaleUpButton = skillPanel.transform.GetChild(14).gameObject;
        //impaleDownButton = skillPanel.transform.GetChild(15).gameObject;
        //mightyLeapUpButton = skillPanel.transform.GetChild(16).gameObject;
        //mightyLeapDownButton = skillPanel.transform.GetChild(17).gameObject;
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

    void UpdateSkills()
    {
        deepSlashSkilllvl1.GetComponent<Skills>().Init(gameObject, stats.deepSlashSkill);
        deepSlashSkilllvl2.GetComponent<Skills>().Init(gameObject, stats.deepSlashSkill - 1);
        shieldBashSkilllvl1.GetComponent<Skills>().Init(gameObject, stats.shieldBashSkill);
        shieldBashSkilllvl2.GetComponent<Skills>().Init(gameObject, stats.shieldBashSkill - 1);
        mightyLeapSkilllvl1.GetComponent<Skills>().Init(gameObject, stats.mightyLeapSkill);
        mightyLeapSkilllvl2.GetComponent<Skills>().Init(gameObject, stats.mightyLeapSkill - 1);
        impaleSkilllvl1.GetComponent<Skills>().Init(gameObject, stats.impaleSkill);
        impaleSkilllvl2.GetComponent<Skills>().Init(gameObject, stats.impaleSkill - 1);
        
    }

    public void UpdateSkillPanel()
    {
        Init();

        //deepSlashText.text = "Swipe Right " + stats.deepSlashSkill.ToString();
        //shieldBashText.text = "Swipe Left " + stats.shieldBashSkill.ToString();
        //impaleText.text = "Swipe Up " + stats.impaleSkill.ToString();
        //mightyLeapText.text = "Swipe Down " + stats.mightyLeapSkill.ToString();
        skillPointsRemaining = stats.skillPointsRemaining;
        UpdateSkills();
        skillRemainingText.GetComponent<Text>().text = "Habilidades para Aprender " + stats.skillPointsRemaining.ToString();
        if (stats.skillPointsRemaining > 0)
        {
            ShowLevelUPIcon(true);
            
            //UpdateSkillPoints(stats.skillPointsRemaining);
            //deepSlashUpButton.SetActive(true);
            //deepSlashDownButton.SetActive(true);
            //shieldBashUpButton.SetActive(true);
            //shieldBashDownButton.SetActive(true);
            //impaleUpButton.SetActive(true);
            //impaleDownButton.SetActive(true);
            //mightyLeapUpButton.SetActive(true);
            //mightyLeapDownButton.SetActive(true);

            if(stats.shieldBashSkill == 0)
            {
                shieldBashSkilllvl1.GetComponent<Skills>().EnableOutline();
            }
            else if (stats.shieldBashSkill == 1)
            {
                shieldBashSkilllvl2.GetComponent<Skills>().EnableOutline();
            }

            if (stats.deepSlashSkill == 0)
            {
                deepSlashSkilllvl1.GetComponent<Skills>().EnableOutline();
            }
            else if (stats.deepSlashSkill == 1)
            {
                deepSlashSkilllvl2.GetComponent<Skills>().EnableOutline();
            }

            if (stats.mightyLeapSkill == 0)
            {
                mightyLeapSkilllvl1.GetComponent<Skills>().EnableOutline();
            }
            else if (stats.mightyLeapSkill == 1)
            {
                mightyLeapSkilllvl2.GetComponent<Skills>().EnableOutline();
            }

            if (stats.impaleSkill == 0)
            {
                impaleSkilllvl1.GetComponent<Skills>().EnableOutline();
            }
            else if (stats.impaleSkill == 1)
            {
                impaleSkilllvl2.GetComponent<Skills>().EnableOutline();
            }
            ShowUpgradeButton();
        }
        else
        {
            ShowLevelUPIcon(false);
            //skillPointsText.text = "";
            //tempDeepSlashText.text = "0";
            //tempShieldBashText.text = "0";
            //tempImpaleText.text = "0";
            //tempMightyLeapText.text = "0";
            //tempDeepSlashText.enabled = false;
            //tempShieldBashText.enabled = false;
            //tempImpaleText.enabled = false;
            //tempMightyLeapText.enabled = false;
            //deepSlashUpButton.SetActive(false);
            //deepSlashDownButton.SetActive(false);
            //shieldBashUpButton.SetActive(false);
            //shieldBashDownButton.SetActive(false);
            //impaleUpButton.SetActive(false);
            //impaleDownButton.SetActive(false);
            //mightyLeapUpButton.SetActive(false);
            //mightyLeapDownButton.SetActive(false);

            
            deepSlashSkilllvl1.GetComponent<Skills>().DisableOutline();
            deepSlashSkilllvl2.GetComponent<Skills>().DisableOutline();
            shieldBashSkilllvl1.GetComponent<Skills>().DisableOutline();
            shieldBashSkilllvl2.GetComponent<Skills>().DisableOutline();
            mightyLeapSkilllvl1.GetComponent<Skills>().DisableOutline();
            mightyLeapSkilllvl2.GetComponent<Skills>().DisableOutline();
            impaleSkilllvl1.GetComponent<Skills>().DisableOutline();
            impaleSkilllvl2.GetComponent<Skills>().DisableOutline();

        }
        //if (allocatedPoints > 0)
        //{
        //    confirmButton.SetActive(true);
        //}
        //else confirmButton.SetActive(false);
        ShowUpgradeButton();
        

    }

    public void SelectSkill(GameObject skill)
    {
        if(skillSelected != null)
        {
            if (skillSelected != skill)
            {
                skillSelected.GetComponent<Skills>().ResetColor();
                skillSelected = skill;
                skillSelected.GetComponent<Skills>().ChangeToSelectedColor();
            }
            else
            {
                skillSelected.GetComponent<Skills>().ResetColor();
                skillSelected = null;
            }
        }
        else
        {
            skillSelected = skill;
            skillSelected.GetComponent<Skills>().ChangeToSelectedColor();
        }
        ShowUpgradeButton();
    }

    public void ShowUpgradeButton()
    {
        if (skillSelected != null)
        {
            if (skillSelected.GetComponent<Skills>().canBeUpgraded() && stats.skillPointsRemaining != 0)
                upgradeButton.SetActive(true);
            else upgradeButton.SetActive(false);
        }
        else upgradeButton.SetActive(false);
    }

    //public void UpdateSkillPoints(int ap)
    //{
    //    skillPointsRemaining = ap;
    //    ShowButtons();
    //}

    //public void ShowButtons()
    //{
    //    tempDeepSlashText.enabled = true;
    //    tempShieldBashText.enabled = true;
    //    tempImpaleText.enabled = true;
    //    tempMightyLeapText.enabled = true;
    //    skillPointsText.text = "Skill Points Left " + stats.skillPointsRemaining.ToString();
    //}

    //public void UpdateConfirmButton()
    //{
    //    if (allocatedPoints > 0)
    //    {
    //        confirmButton.SetActive(true);
    //    }
    //    else confirmButton.SetActive(false);
    //}

    //public void DeepSlashUp()
    //{
    //    if (allocatedPoints < skillPointsRemaining)
    //    {
    //        allocatedPoints++;
    //        tempDeepSlash++;
    //        tempDeepSlashText.text = "+" + tempDeepSlash.ToString();
    //        int i = skillPointsRemaining - allocatedPoints;
    //        skillPointsText.text = "Skill Points Left " + i.ToString();
    //    }
    //    UpdateConfirmButton();
    //}

    //public void DeepSlashDown()
    //{
    //    if (tempDeepSlash > 0)
    //    {
    //        allocatedPoints--;
    //        tempDeepSlash--;
    //        if (tempDeepSlash == 0) tempDeepSlashText.text = "0";
    //        else tempDeepSlashText.text = "+" + tempDeepSlash.ToString();
    //        int i = skillPointsRemaining - allocatedPoints;
    //        skillPointsText.text = "Skill Points Left " + i.ToString();
    //    }
    //    UpdateConfirmButton();
    //}

    //public void ShieldBashUp()
    //{
    //    if (allocatedPoints < skillPointsRemaining)
    //    {
    //        allocatedPoints++;
    //        tempShieldBash++;
    //        tempShieldBashText.text = "+" + tempShieldBash.ToString();
    //        int i = skillPointsRemaining - allocatedPoints;
    //        skillPointsText.text = "Skill Points Left " + i.ToString();
    //    }
    //    UpdateConfirmButton();
    //}

    //public void ShieldBashDown()
    //{
    //    if (tempShieldBash > 0)
    //    {
    //        allocatedPoints--;
    //        tempShieldBash--;
    //        if (tempShieldBash == 0) tempShieldBashText.text = "0";
    //        else tempShieldBashText.text = "+" + tempShieldBash.ToString();
    //        int i = skillPointsRemaining - allocatedPoints;
    //        skillPointsText.text = "Skill Points Left " + i.ToString();
    //    }
    //    UpdateConfirmButton();
    //}

    //public void ImpaleUp()
    //{
    //    if (allocatedPoints < skillPointsRemaining)
    //    {
    //        allocatedPoints++;
    //        tempImpale++;
    //        tempImpaleText.text = "+" + tempImpale.ToString();
    //        int i = skillPointsRemaining - allocatedPoints;
    //        skillPointsText.text = "Skill Points Left " + i.ToString();
    //    }
    //    UpdateConfirmButton();
    //}

    //public void ImpaleDown()
    //{
    //    if (tempImpale > 0)
    //    {
    //        allocatedPoints--;
    //        tempImpale--;
    //        if (tempImpale == 0) tempImpaleText.text = "0";
    //        else tempImpaleText.text = "+" + tempImpale.ToString();
    //        int i = skillPointsRemaining - allocatedPoints;
    //        skillPointsText.text = "Skill Points Left " + i.ToString();
    //    }
    //    UpdateConfirmButton();
    //}

    //public void MightyLeapUp()
    //{
    //    if (allocatedPoints < skillPointsRemaining)
    //    {
    //        allocatedPoints++;
    //        tempMightyLeap++;
    //        tempMightyLeapText.text = "+" + tempMightyLeap.ToString();
    //        int i = skillPointsRemaining - allocatedPoints;
    //        skillPointsText.text = "Skill Points Left " + i.ToString();
    //    }
    //    UpdateConfirmButton();
    //}
    //public void MightyLeapDown()
    //{
    //    if (tempImpale > 0)
    //    {
    //        allocatedPoints--;
    //        tempMightyLeap--;
    //        if (tempMightyLeap == 0) tempMightyLeapText.text = "0";
    //        else tempMightyLeapText.text = "+" + tempMightyLeap.ToString();
    //        int i = skillPointsRemaining - allocatedPoints;
    //        skillPointsText.text = "Skill Points Left " + i.ToString();
    //    }
    //    UpdateConfirmButton();
    //}
    //public void Confirm()
    //{
    //    skillPointsRemaining -= allocatedPoints;
    //    allocatedPoints = 0;
    //    stats.ConfirmSkillUp(skillPointsRemaining, tempDeepSlash, tempShieldBash, tempImpale, tempMightyLeap);
    //    tempDeepSlash = 0;
    //    tempShieldBash = 0;
    //    tempImpale = 0;
    //    tempMightyLeap = 0;
    //    tempDeepSlashText.text = "0";
    //    tempShieldBashText.text = "0";
    //    tempImpaleText.text = "0";
    //    tempMightyLeapText.text = "0";
    //}

    public void UpgradeSkill()
    {
        skillPointsRemaining--;
        if (skillSelected == shieldBashSkilllvl1 || skillSelected == shieldBashSkilllvl2) tempShieldBash++;
        else if (skillSelected == deepSlashSkilllvl1 || skillSelected == deepSlashSkilllvl2) tempDeepSlash++;
        else if (skillSelected == mightyLeapSkilllvl1 || skillSelected == mightyLeapSkilllvl2) tempMightyLeap++;
        else tempImpale++;
        skillSelected.GetComponent<Skills>().Upgrade();
        stats.ConfirmSkillUp(skillPointsRemaining, tempDeepSlash, tempShieldBash, tempImpale, tempMightyLeap);
        skillSelected = null;
        tempDeepSlash = 0;
        tempShieldBash = 0;
        tempImpale = 0;
        tempMightyLeap = 0;
    }
}
