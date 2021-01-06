using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HeroStats : MonoBehaviour {

    public int staggerStrength;
    public int lifeOrbChance;
    public int deathOrbChance;
    public int flameOrbChance;
    public int frostOrbChance;
    public int furyOrbChance;
    public GameObject lifeOrbParticles;
    public GameObject deathOrbParticles;
    public GameObject flameOrbParticles;
    public GameObject frostOrbParticles;
    public GameObject furyOrbParticles;
    public TutorialMessageSystem messageSystem;
    public static HeroStats heroStats;
    public GameObject shield;
    private GameControl data;
    public HeroBehavior heroBehavior;
    public AttributesPanel attributesPanel;
    public SkillPanel skillPanel;
    public OrbsPanel orbsPanel;
    public TabPanelInterface tabOrbs;
    public TabPanelInterface tabInventory;
    public TabPanelInterface tabSkills;
    public Canvas levelUpCanvas;
    public GameObject criticalCanvas;
    public GameObject coinCanvas;
    public HUDScript hud;
    public string feedBackText;
    public const int numOrbs = 5;
    public bool[] orbs = new bool[numOrbs];
    public int activeOrb;
    public string myName;
    public float maxHealth;
    public float currHealth;
    public float maxEnergy;
    public float currEnergy;
    public float baseDamage;
    public float minDamage;
    public float maxDamage;
    public float defense;
    public float weakenDefense;
    public int level;
    public float experience;
    public float nextExperienceLevel;
    public ulong coins;
    public int diamonds;
    public float goldGain;
    public int stage;
    public int strength;
    public int dexterity;
    public int inteligence;
    public int currStrength;
    public int currDexterity;
    public int currInteligence;
    public int shieldBashSkill;
    public int impaleSkill;
    public int mightyLeapSkill;
    public int deepSlashSkill;
    public int skillPointsRemaining;
    public int attributePointsRemaining;
    public float attackDistance;
    public float healthRate;
    public float energyRate;
    public float movement;
    public float energyConsumeReduction;
    public float swipeDownEnergy;
    public float swipeLeftEnergy;
    public float swipeUpEnergy;
    public float swipeRightEnergy;
    public float swipeDownDamage;
    public float swipeLeftDamage;
    public float swipeRightDamage;
    public float swipeUpDamage;
    public float aoeRadius;
    public float stunTime;
    public float blockDamageReduction;      // Redução de damage quando bloqueado
    public float criticalChance;

    public float itemXP;
    public int itemStrength;
    public int itemDexterity;
    public int itemInteligence;
    public int itemHP;
    public int itemEnergy;
    public float itemHPRate;
    public float itemHPRateValue;
    public float itemEnergyRate;
    public float itemEnergyRateValue;
    public int itemDamage;
    public float itemCritChance;
    public int itemDefense;
    public int itemMovement;
    public float itemEnergyConsume;
    
    public float defenseReduction;          // porcentagem da redução de defesa do inimigo
    public float distanceThrowBack;         // distancia que o inimigo é jogado para trás
    public float defenseReductionTime;
    public float speed;
    public float rollSpeed;
    public float rollDistance;
    public float rollEnergy;
    public bool canRoll;
    public bool canBlock;

    public bool blocked;

    void Awake()
    {
        // queremos apenas um gameControl
        // para isso verificamos se ele existe
        if (heroStats == null)
        {
            // se não existe então cria e faz desse objeto o gameControl
            DontDestroyOnLoad(gameObject);
            heroStats = this;
        }
        else if (heroStats != this)
        {
            // se ele ja existe e não é este objeto, então este objeto não deve existir
            // pois queremos apenas um gameControl, ideia de Singleton
            Destroy(gameObject);
        }

        //level = 1;
        //maxHealth = 100;
        //currHealth = maxHealth;
        //damage = 6;
        //maxEnergy = 50;
        //currEnergy = maxEnergy;
        //experience = 0;
        //strength = 1;
        //dexterity = 1;
        //inteligence = 1;
        //attackDistance = 2.2f;
        //energyRate = 2.0f;
        //healthRate = 0.5f;
        //swipeDownEnergy = 10;
        //swipeUpEnergy = 15;
        //swipeLeftEnergy = 8;
        //swipeRightEnergy = 12;
        //swipeDownDamage = 1.3f * GetDamage();
        //swipeUpDamage = 3.0f * GetDamage();
        //swipeLeftDamage = 1.0f * GetDamage();
        //swipeRightDamage = 1.8f * GetDamage();
        //aoeRadius = 1.6f;
        //stunTime = 2.0f;
        //defenseReduction = 50;
        //movement = 0.3f;
        //distanceThrowBack = 0.5f;
        //speed = 3 + GetMovement();
        //runSpeed = speed * 1.8f;
        //blocked = false;
        //blockDamageReduction = 0.7f;
        //defense = 0;


        

        // deixar comentado
        //healthBar = transform.Find("HeroCanvas/HealthBG/Health").GetComponent<Image>();
        //energyBar = transform.Find("HeroCanvas/EnergyBG/Energy").GetComponent<Image>();


    }

    void Start()
    {
        itemCritChance = 0;
        itemDamage = 0;
        itemDefense = 0;
        itemDexterity = 0;
        itemXP = 1;
        itemStrength = 0;
        itemInteligence = 0;
        itemHP = 0;
        itemEnergy = 0;
        itemHPRate = 0;
        itemEnergyRate = 0;
        itemMovement = 0;
        itemEnergyConsume = 1;
        goldGain = 1;

        data = GameControl.control;
        
        heroBehavior = GetComponent<HeroBehavior>();
        data.InitHero(this.gameObject);
        //attributesPanel = GameObject.Find("HeroStatsWindow").GetComponent<AttributesPanel>();
        //skillPanel = GameObject.Find("HeroSkillWindow").GetComponent<SkillPanel>();
        //levelUpCanvas = GameObject.Find("LevelUpCanvas").GetComponent<Canvas>();
        levelUpCanvas.enabled = false;
        ReadControl();
        attributesPanel.Init();
        attributesPanel.UpdateStatsPanel();
        skillPanel.Init();
        skillPanel.UpdateSkillPanel();
        criticalCanvas.SetActive(false);
        
    }

    public void ReadControl()
    {
        experience = data.experience;
        nextExperienceLevel = data.nextExperienceLevel;
        stage = data.stage;
        strength = data.strength;
        level = data.level;
        coins = data.coins;
        diamonds = data.diamonds;
        dexterity = data.dexterity;
        inteligence = data.inteligence;
        shieldBashSkill = data.shieldBashSkill;
        impaleSkill = data.impaleSkill;
        deepSlashSkill = data.deepSlashSkill;
        mightyLeapSkill = data.mightyLeapSkill;
        skillPointsRemaining = data.skillPointsRemaining;
        attributePointsRemaining = data.attributePointsRemaining;
        canBlock = data.canBlock;
        canRoll = data.canRoll;
        if (canBlock) shield.SetActive(true);
        else shield.SetActive(false);

        for(int i = 0; i < numOrbs; i++)
        {
            orbs[i] = data.orbs[i];
            if(orbs[i])
            {
                orbsPanel.UpdateOrbPanel(i);
            }
        }
        activeOrb = data.activeOrb;
        ActivateOrbParticles(true);

        UpdateStats(true);
        skillPanel.UpdateSkillPanel();
        tabSkills.UpdateTabPanelInterface();
        tabOrbs.UpdateTabPanelInterface();
        tabInventory.UpdateTabPanelInterface();
    }

    public void PickUpShield()
    {
        shield.SetActive(true);
        canBlock = true;
    }

    public void LearnToRoll()
    {
        canRoll = true;
    }

    public void ResetPlayer(Vector3 ressurectPosition)
    {
        heroBehavior.ResetPlayer(ressurectPosition);
        currHealth = maxHealth;
        currEnergy = maxEnergy;

    }

    public void UpdateGameControl()
    {
        data.UpdateControl();
    }

    public void UpdateStats(bool h)
    {
        currStrength = strength + itemStrength;
        currDexterity = dexterity + itemDexterity;
        currInteligence = inteligence + itemInteligence;


        maxHealth = (50 + currStrength * 5) + itemHP;
        maxEnergy = (50 + currInteligence * 5) + itemEnergy;
        if (h)
        {
            currHealth = maxHealth;
            currEnergy = maxEnergy;
        }

        baseDamage = (10 + currStrength) + itemDamage;
        minDamage = baseDamage - (baseDamage / 3);
        maxDamage = baseDamage + (baseDamage / 3);
        attackDistance = 2.2f;
        energyConsumeReduction = 2.5f * currInteligence;

        //float baseEnergyRate = (2f + (float)currInteligence / 5);
        float baseEnergyRate = currInteligence;
        itemEnergyRate = baseEnergyRate * itemEnergyRateValue;
        energyRate = baseEnergyRate + itemEnergyRate;

        //float baseHpRate = (0.5f + (float)currStrength / 10);
        //float baseHpRate = (float)currStrength / 5;
        float baseHpRate = ((float)currStrength / 1000) * maxHealth;
        itemHPRate = baseHpRate * itemHPRateValue;
        healthRate =  baseHpRate + itemHPRate;

        swipeDownEnergy = (30 - (30 * energyConsumeReduction / 100)) * itemEnergyConsume;
        swipeUpEnergy = (35 - (35 * energyConsumeReduction / 100)) * itemEnergyConsume;
        swipeLeftEnergy = (20 - (20 * energyConsumeReduction / 100)) * itemEnergyConsume;
        swipeRightEnergy = (25 - (25*energyConsumeReduction/100)) * itemEnergyConsume;

        float mightyLeapSkillDamage = (mightyLeapSkill + (1.0f / (mightyLeapSkill + 1.0f)));
        float impaleSkillDamage = (impaleSkill + (1.0f / (impaleSkill + 1.0f)));
        float shieldBashSkillDamage = (shieldBashSkill + (1.0f / (shieldBashSkill + 1.0f)));
        float deepSlashSkillDamage = (deepSlashSkill + (1.0f / (deepSlashSkill + 1.0f)));

        swipeDownDamage = 0.7f * baseDamage * mightyLeapSkillDamage;
        swipeUpDamage = 2.0f * baseDamage * impaleSkillDamage;
        swipeLeftDamage = 0.5f * baseDamage * shieldBashSkillDamage;
        swipeRightDamage = 1.0f * baseDamage * deepSlashSkillDamage;
        aoeRadius = 1.6f;
        stunTime = 4.0f + (mightyLeapSkill * 1.5f);
        defense = (currDexterity + itemDefense) * ((100 - weakenDefense) / 100);
        defenseReduction = 50 + (10 * (shieldBashSkill + (1 / (shieldBashSkill+1))));
        movement =  ((float)currDexterity / 5) + itemMovement;
        distanceThrowBack = 0.5f;
        defenseReductionTime = 5.0f;
        speed = (3 + (GetMovement()/20)) * 1.5f;
        rollSpeed = speed * 1.8f;
        rollDistance = 4.0f;
        rollEnergy = 5.0f;
        blocked = false;
        blockDamageReduction = 0.3f;
        criticalChance = (10 + ((float)currDexterity / 2)) + itemCritChance;
        heroBehavior.UpdateBehavior();
       
        attributesPanel.UpdateStatsPanel();

        // Define Chance de Stagger depois
        staggerStrength = currStrength;
        
    }

    public void PickUpOrb(int orb)
    {
        orbs[orb] = true;
        orbsPanel.UpdateOrbPanel(orb);
        UpdateGameControl();
    }

    public void SelectOrb(int orb)
    {

        ActivateOrbParticles(false);
        activeOrb = orb;
        ActivateOrbParticles(true);
        UpdateGameControl();
    }

    

    void ActivateOrbParticles(bool flag)
    {

        switch(activeOrb)
        {
            case 0:
                lifeOrbParticles.SetActive(flag);
                break;
            case 1:
                deathOrbParticles.SetActive(flag);
                break;
            case 2:
                flameOrbParticles.SetActive(flag);
                break;
            case 3:
                frostOrbParticles.SetActive(flag);
                break;
            case 4:
                furyOrbParticles.SetActive(flag);
                break;
            case -1:
                lifeOrbParticles.SetActive(false);
                deathOrbParticles.SetActive(false);
                flameOrbParticles.SetActive(false);
                frostOrbParticles.SetActive(false);
                furyOrbParticles.SetActive(false);
                break;
             
        }
    }

    public void SummonOrbPower(GameObject other, float damage)
    {
        if (activeOrb == -1) return;

        float chance = Random.Range(0, 101);

        switch(activeOrb)
        {
            case 0:// Life Orb
                if (damage == 0) break;
                if(chance <= lifeOrbChance)
                {
                    Debug.Log("Health +" + damage);
                    currHealth += damage;
                    if (currHealth > maxHealth) currHealth = maxHealth;
                }
                break;
            case 1:// Death Orb
                if (other == null) break;
                if (chance <= deathOrbChance)
                {
                    Debug.Log("DeathOrb: Morte Instantanea");
                }
                break;
            case 2:// Flame Orb
                if (other == null) break;
                if (chance <= flameOrbChance)
                {
                    Debug.Log("FlameOrb: "+damage/3+" damage/s durante 5 segundos");
                }
                break;
            case 3:// Frost Orb
                if (other == null) break;
                if (chance <= frostOrbChance)
                {
                    Debug.Log("FrostOrb: Gela por 5 segundos");
                }
                break;
            case 4:// Fury Orb
                if (damage != 0) break;
                if (chance <= furyOrbChance)
                {
                    Debug.Log("FuryOrb: aumenta agilidade por 10 segundos");
                }
                break;
            default: break;
        }
    }

    //public void ActivateItemBonus(Item item)
    public void ActivateItemBonus(DataItemScriptableObject item)
    {
        //switch(item.Type)
        switch(item.type)
        {
            case DataItemScriptableObject.modifier.XP:
                float newXPGain = ((float)item.value / 100);
                itemXP += newXPGain;
                break;
            case DataItemScriptableObject.modifier.STRENGTH:
                itemStrength += item.value;
                break;
            case DataItemScriptableObject.modifier.DEXTERITY:
                itemDexterity += item.value;
                break;
            case DataItemScriptableObject.modifier.INTELIGENCE:
                itemInteligence += item.value;
                break;
            case DataItemScriptableObject.modifier.HP:
                itemHP += item.value;
                break;
            case DataItemScriptableObject.modifier.ENERGY:
                itemEnergy += item.value;
                break;
            case DataItemScriptableObject.modifier.HPRATE:
                float hprate = ((float)item.value / 100);
                itemHPRateValue += hprate;
                break;
            case DataItemScriptableObject.modifier.ENERGYRATE:
                float energyrate = ((float)item.value / 100);
                itemEnergyRateValue += energyrate;
                break;
            case DataItemScriptableObject.modifier.DAMAGE:
                itemDamage += item.value;
                break;
            case DataItemScriptableObject.modifier.CRITCHANCE:
                itemCritChance += item.value;
                break;
            case DataItemScriptableObject.modifier.DEFENSE:
                itemDefense += item.value;
                break;
            case DataItemScriptableObject.modifier.AGILITY:
                itemMovement += item.value;
                break;
            case DataItemScriptableObject.modifier.ENERGYCONSUME:
                float e = ((float)item.value / 100);
                itemEnergyConsume -= e;
                if (itemEnergyConsume < 0.2f) itemEnergyConsume = 0.2f;
                break;
            case DataItemScriptableObject.modifier.GOLDGAIN:
                float g = ((float)item.value / 100);
                goldGain += g;
                break;
            case DataItemScriptableObject.modifier.HPRESTORE:
                currHealth += item.value;
                break;
            case DataItemScriptableObject.modifier.ENERGYRESTORE:
                currEnergy += item.value;
                break;
            default: break;
        }
        UpdateStats(false);
    }

    //public void DeactivateItemBonus(Item item)
    public void DeactivateItemBonus(DataItemScriptableObject item)
    {
        switch (item.type)
        {
            case DataItemScriptableObject.modifier.XP:
                float newXPGain = ((float)item.value / 100);
                itemXP -= newXPGain;
                break;
            case DataItemScriptableObject.modifier.STRENGTH:
                itemStrength -= item.value;
                break;
            case DataItemScriptableObject.modifier.DEXTERITY:
                itemDexterity -= item.value;
                break;
            case DataItemScriptableObject.modifier.INTELIGENCE:
                itemInteligence -= item.value;
                break;
            case DataItemScriptableObject.modifier.HP:
                itemHP -= item.value;
                break;
            case DataItemScriptableObject.modifier.ENERGY:
                itemEnergy -= item.value;
                break;
            case DataItemScriptableObject.modifier.HPRATE:
                float hprate = ((float)item.value / 100);
                itemHPRateValue -= hprate;
                break;
            case DataItemScriptableObject.modifier.ENERGYRATE:
                float energyrate = ((float)item.value / 100);
                itemEnergyRateValue -= energyrate;
                break;
            case DataItemScriptableObject.modifier.DAMAGE:
                itemDamage -= item.value;
                break;
            case DataItemScriptableObject.modifier.CRITCHANCE:
                itemCritChance -= item.value;
                break;
            case DataItemScriptableObject.modifier.DEFENSE:
                itemDefense -= item.value;
                break;
            case DataItemScriptableObject.modifier.AGILITY:
                itemMovement -= item.value;
                break;
            case DataItemScriptableObject.modifier.ENERGYCONSUME:
                float e = ((float)item.value / 100);
                itemEnergyConsume += e;
                if (itemEnergyConsume > 1.0f) itemEnergyConsume = 1.0f;
                break;
            case DataItemScriptableObject.modifier.GOLDGAIN:
                float g = ((float)item.value / 100);
                goldGain -= g;
                break;
            //case Item.modifier.HPRESTORE:
            //    currHealth -= item.Value;
            //    break;
            //case Item.modifier.ENERGYRESTORE:
            //    currEnergy += item.Value;
            //    break;
            default: break;
        }
        UpdateStats(false);
    }

    public void ConfirmLevelUp(int p, int str, int dex, int itl)
    {
        strength += str;
        dexterity += dex;
        inteligence += itl;
        attributePointsRemaining = p;
        UpdateStats(true);
        UpdateGameControl();
        if(attributePointsRemaining == 0 && skillPointsRemaining == 0) levelUpCanvas.enabled = false;
        LockHUD(false);
    }

    public void ConfirmSkillUp(int p, int deepSlash, int shieldBash, int impale, int mightyLeap)
    {
        skillPointsRemaining = p;
        shieldBashSkill += shieldBash;
        deepSlashSkill += deepSlash;
        impaleSkill += impale;
        mightyLeapSkill += mightyLeap;
        UpdateStats(false);
        UpdateGameControl();
        skillPanel.UpdateSkillPanel();
        if (attributePointsRemaining == 0 && skillPointsRemaining == 0) levelUpCanvas.enabled = false;

        if(level == 2)
        {
            if(shieldBashSkill > 0)
                messageSystem.ShowMessage("Você Desbloqueou a Habilidade SHIELD BASH. Execute ela com um SWIPE p/ ESQUERDA", 0.5f);
            else if(deepSlashSkill > 0)
                messageSystem.ShowMessage("Você Desbloqueou a Habilidade DEEP SLASH. Execute ela com um SWIPE p/ DIREITA", 0.5f);
            else if(mightyLeapSkill > 0)
                messageSystem.ShowMessage("Você Desbloqueou a Habilidade MIGHTY LEAP. Execute ela com um SWIPE p/ BAIXO", 0.5f);
            else messageSystem.ShowMessage("Você Desbloqueou a Habilidade IMPALE. Execute ela com um SWIPE p/ CIMA", 0.5f);
            messageSystem.StackMessage("Habilidades consomem energia. Use somente quando necessário.", true);
            messageSystem.IgnoreUnblockRaycast();
        }

    }

    public string GetMyName()
    {
        return myName;
    }

    public float GetHealthRate()
    {
        return healthRate;
    }

    public float GetEnergyRate()
    {
        return energyRate;
    }

    public float GetCurrentHealth()
    {
        return currHealth;
    }

    public void SetCurrentHealth(float health)
    {
        currHealth = health;
    }

    public void SetCurrentEnergy(float energy)
    {
        currEnergy = energy;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMaxEnergy()
    {
        return maxEnergy;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetRollSpeed()
    {
        return rollSpeed;
    }

    public void SetSpeed()
    {
        speed = GetSpeed() + GetMovement();
    }

    public float GetRollEnergy()
    {
        return rollEnergy;
    }

    public float GetRollDistance()
    {
        return rollDistance;
    }

    public float GetMovement()
    {
        return movement;
    }

    public float GetDistanceThrowBack()
    {
        return distanceThrowBack;
    }

    public float CurrentEnergy
    {
        get
        {
            return currEnergy;
        }
    }

    public float DefenseReduction
    {
        get
        {
            return defenseReduction;
        }
    }

    public float StunTime
    {
        get
        {
            return stunTime;
        }
    }

    public float DefenseReductionTime
    {
        get
        {
            return defenseReductionTime;
        }
    }

    public float AoERadius
    {
        get
        {
            return aoeRadius;
        }
    }

    public float GetDamage()
    {
        
        float damage = Random.Range(minDamage, maxDamage);
        float chance = Random.Range(0,100);
        
        if (chance <= criticalChance)
        {
            damage *= 2;
            criticalCanvas.SetActive(true);
            
            Invoke("DisableCriticalCanvas", 0.7f);
        }
        return damage;
    }

    public void DisableCriticalCanvas()
    {
        if (criticalCanvas.activeSelf) criticalCanvas.SetActive(false);
    }

    public float GetAttackDistance()
    {
        return attackDistance;
    }

    public void SetBlocked(bool b)
    {
        blocked = b;
    }

    public void TakeDamage(GameObject other, float d)
    {
        if (blocked)
        {
            if(d - (d * blockDamageReduction) - defense >= 0)
                currHealth -= (d - (d * blockDamageReduction) - defense);
            
        }
        else
        {
            if(d - defense >= 0)
                currHealth -= (d - defense);
            
        }

        heroBehavior.TakingDamage(other);
    }

    public float SwipeLeftEnergy
    {
        get
        {
            return swipeLeftEnergy;
        }
    }

    public float SwipeRightEnergy
    {
        get
        {
            return swipeRightEnergy;
        }
    }

    public float SwipeUpEnergy
    {
        get
        {
            return swipeUpEnergy;
        }
    }

    public float SwipeDownEnergy
    {
        get
        {
            return swipeDownEnergy;
        }
    }

    public float SwipeLeftDamage
    {
        get
        {
            return swipeLeftDamage;
        }
    }

    public float SwipeRightDamage
    {
        get
        {
            return swipeRightDamage;
        }
    }

    public float SwipeUpDamage
    {
        get
        {
            return swipeUpDamage;
        }
    }

    public float SwipeDownDamage
    {
        get
        {
            return swipeDownDamage;
        }
    }
    public void ConsumeEnergy(float e)
    {
        currEnergy -= e;
        
    }

    public float CalculateXP(float xp)
    {
        float givenXP = xp * (itemXP);
        GiveXP(givenXP);
        return givenXP;
    }

    public void GiveXP(float xp)
    {
        experience += xp;
        if(experience >= nextExperienceLevel)
        {
            levelUpCanvas.enabled = true;
            level++;
            heroBehavior.PlayLevelUp();
            if(level == 2 || level == 4 || level == 7 || level == 9 || 
                level == 11 || level == 14 || level == 16 || level == 18)
            {
                if(level == 2)
                {
                    messageSystem.ShowMessage("Você passou para o nível 2. Agora você pode desbloquear uma HABILIDADE ESPECIAL", 1);
                }
                skillPointsRemaining++;
                skillPanel.UpdateSkillPanel();
            }
            attributePointsRemaining++;
            nextExperienceLevel +=  nextExperienceLevel + (200 * level);
            attributesPanel.UpdateStatsPanel();
            if (level == 1)
            {
                hud.EnableGlow(true);
                hud.LockHUD(true);
                hud.ShowFirstMessage(messageSystem);
                messageSystem.ShowMessage("Você aumentou de nível. Toque no ícone do personagem no canto superior esquerdo para abrir a janela de Informações", 1);
            }
        }
        UpdateStats(false);
        UpdateGameControl();
        attributesPanel.UpdateXpInfo();
        
    }

    public void LockHUD(bool l)
    {
        hud.LockHUD(l);
        if (l)
        {
            hud.ShowHUD();
        }
    }

    public void ReduceDefense(float value)
    {
        weakenDefense = value;
        defense = (currDexterity + itemDefense) * ((100 -  weakenDefense)/100);
        // Falta indicar a redução de defesa na UI
    }

    public void AddCoin(ulong value)
    {
        
        coins += value;
        
        UpdateStats(false);
        UpdateGameControl();

    }
    
}
