using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.Events;

public class GameControl : MonoBehaviour {

    public static GameControl control;          // um dado static pode ser acessado diretamente de qualquer parte do jogo
                                                // sem precisar usar GameObject.Find ou .GetComponent
                                                // ver AdjustScript.cs

    public GameObject adjustControlsCanvas;
    public AudioSource musicControl;
    public AudioClip mainMenuAudioClip;
    public AudioClip levelSelectAudioClip;
    

    public int saveSlot;

    //public float health;  
    public float dificulty;
    public delegate void OnAdjustDificultyButtonDelegate();
    public static OnAdjustDificultyButtonDelegate adjustDificultyDelegate;

    public string heroName;
    public float experience;
    public float nextExperienceLevel;
    public ulong coins;
    public int diamonds;
    public int level;
    public int stage;
    public int strength;
    public int dexterity;
    public int inteligence;
    public int shieldBashSkill;
    public int impaleSkill;
    public int mightyLeapSkill;
    public int deepSlashSkill;
    public int skillPointsRemaining;
    public int attributePointsRemaining;
    public int activatedCheckPoint;
    protected const int inventoryCapacity = 6;
    public int[] inventory = new int[inventoryCapacity];
    public int[] inventoryAmount = new int[inventoryCapacity];
    protected const int numOrbs = 5;
    public bool[] orbs = new bool[numOrbs];
    public int activeOrb;
    protected const int numChest = 99;
    public bool[] chests = new bool[numChest];
    protected const int numBosses = 5;
    public bool[] bosses = new bool[numBosses];
    protected const int numPuzzles = 5;
    public bool[] puzzles = new bool[numPuzzles];
    public bool canBlock;
    public bool canRoll;
    protected const int numTutorialMessages = 50;
    public bool[] tutorialMessages = new bool[numTutorialMessages];
    protected const int numHistoryMessages = 50;
    public bool[] historyMessages = new bool[numHistoryMessages];

    public GameObject hero;
    public GameObject heroInstance;
    public Inventory heroInventoryScript;
    public HeroStats heroStatsScript;
    public Vector3 currentRessurectStoneLocation;

    public enum gameState { START, PLAY}
    public gameState currentState;

    public Canvas myCanvas;
    public GameObject configPanel;
    public GameObject mainMenuCanvas;
    public GameObject mainMenu;
    public GameObject continueGameMenu;
    public GameObject newGameMenu;
    public GameObject levelSelectMenu;
    public GameObject currentRessurectStone;

    //public int[] inventory;

    void Awake () {
        // queremos apenas um gameControl
        // para isso verificamos se ele existe
        if (control == null)
        {
            // se não existe então cria e faz desse objeto o gameControl
            DontDestroyOnLoad(gameObject);
            control = this;
            musicControl.Play();
        }
        else if (control != this)
        {
            // se ele ja existe e não é este objeto, então este objeto não deve existir
            // pois queremos apenas um gameControl, ideia de Singleton
            Destroy(gameObject);
        }

        currentState = gameState.START;
        myCanvas = transform.Find("Canvas").gameObject.GetComponent<Canvas>();
        configPanel = myCanvas.transform.Find("Panel").gameObject;
        myCanvas.enabled = false;
        
	}

    private void Start()
    {
        saveSlot = -1;
        //Application.targetFrameRate = 60;
    }

    public void OnAdjustDificultyButtonClick(float d)
    {
        dificulty = d;
        adjustDificultyDelegate();
    }

    public void MarkRessurectLocation(Vector3 location)
    {
        currentRessurectStone = null;
        currentRessurectStoneLocation = location;
    }

    public void MarkRessurectStoneLocation(GameObject _currentRessurectStone, Vector3 location)
    {
        if (currentRessurectStone != null)
            currentRessurectStone.GetComponent<RessurectStone>().DeactivateStone();

        currentRessurectStone = _currentRessurectStone;
        currentRessurectStoneLocation = location;
    }

    public void RessurectHero()
    {
        heroInstance.transform.position = currentRessurectStoneLocation;
        heroStatsScript.ResetPlayer(currentRessurectStoneLocation);
    }

    public void InitHero(GameObject _hero)
    {
        heroInstance = _hero;
        heroStatsScript = heroInstance.GetComponent<HeroStats>();
        heroInventoryScript = heroInstance.transform.Find("HeroInventory").gameObject.GetComponent<Inventory>();
    }

    public void ShowConfigButton()
    {
        myCanvas.enabled = true;
    }

    public void ShowConfigMenu()
    {
        
        heroInstance.GetComponent<MouseInterpreter>().CanBlockRaycast();
        heroInstance.transform.Find("HUDCanvas").gameObject.GetComponent<HUDScript>().HideHUD();
        configPanel.SetActive(true);
        myCanvas.enabled = true;
        //Time.timeScale = 0;
    }

    public void UnBlockRaycast()
    {
        //if(heroInstance.GetComponent<MouseInterpreter>().VerifyRaycastBlock())
        //    heroInstance.GetComponent<MouseInterpreter>().CanBlockRaycast();
        heroInstance.GetComponent<MouseInterpreter>().UnblockRaycast();
    }

    public void HideConfigButton()
    {
        myCanvas.enabled = false;
    }

    public void HideConfigCanvas()
    {
        configPanel.SetActive(false);
        myCanvas.enabled = false;
        //Time.timeScale = 1;
    }

    public int GetActiveScene()
    {
        Scene actualScene = SceneManager.GetActiveScene();
        return actualScene.buildIndex;
    }

    public void UpdateControl()
    {
        
        heroName = heroStatsScript.myName;
        experience = heroStatsScript.experience;
        nextExperienceLevel = heroStatsScript.nextExperienceLevel;
        coins = heroStatsScript.coins;
        diamonds = heroStatsScript.diamonds;
        stage = heroStatsScript.stage;
        level = heroStatsScript.level;
        strength = heroStatsScript.strength;
        dexterity = heroStatsScript.dexterity;
        inteligence = heroStatsScript.inteligence;
        shieldBashSkill = heroStatsScript.shieldBashSkill;
        impaleSkill = heroStatsScript.impaleSkill;
        mightyLeapSkill = heroStatsScript.mightyLeapSkill;
        deepSlashSkill = heroStatsScript.deepSlashSkill;
        skillPointsRemaining = heroStatsScript.skillPointsRemaining;
        attributePointsRemaining = heroStatsScript.attributePointsRemaining;

        for (int i = 0; i < inventoryCapacity; i++)
        {
            inventory[i] = heroInventoryScript.ReadInventory(i);
            inventoryAmount[i] = heroInventoryScript.ReadInventoryAmount(i);
        }

        activeOrb = heroStatsScript.activeOrb;
        for(int i = 0; i < numOrbs; i++)
        {
            orbs[i] = heroStatsScript.orbs[i];
        }

        Save();
    }

    public void InitGameControl(GameObject menuObj,string name)
    {
        
        if (name == "MainMenu")
        {
            mainMenuCanvas = menuObj;
            mainMenu = mainMenuCanvas.transform.GetChild(0).gameObject;
            continueGameMenu = mainMenuCanvas.transform.GetChild(1).gameObject;
            newGameMenu = mainMenuCanvas.transform.GetChild(2).gameObject;
            switch (currentState)
            {
                case gameState.START:
                    mainMenu.SetActive(true);
                    continueGameMenu.SetActive(false);
                    newGameMenu.SetActive(false);
                    
                    break;
                case gameState.PLAY:
                    mainMenu.SetActive(false);
                    continueGameMenu.SetActive(false);
                    newGameMenu.SetActive(false);
                    
                    break;
            }

        }
        else
        {
            
            levelSelectMenu = menuObj;
            switch (currentState)
            {
                case gameState.START:
                    levelSelectMenu.SetActive(false);

                    break;
                case gameState.PLAY:
                    levelSelectMenu.SetActive(true);
                    levelSelectMenu.GetComponent<LevelSelectMenu>().ShowLevels();

                    break;
            }
        }
        
    }

    public void ReturnToLevelSelect()
    {
        ChangeMusic(levelSelectAudioClip);
    }

    public void ReturnToMainMenu()
    {
        
        ChangeMusic(mainMenuAudioClip);
    }

    public void LoadMainMenu()
    {
        currentRessurectStone = null;
        HideConfigCanvas();
        UnBlockRaycast();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        heroInstance.transform.position = Vector3.zero;
        heroInstance.GetComponent<NavMeshAgent>().enabled = false;
        //Time.timeScale = 1;
        //ChangeMusic(levelSelectAudioClip);
        //continueGameMenu.SetActive(false);
        //newGameMenu.SetActive(false);
        //levelSelectMenu.SetActive(true);
    }

    public void FinishStage(int s)
    {
        
        if (stage == s)
            stage++;
        Save();
        heroStatsScript.ReadControl();
        //SceneManager.LoadScene(0, LoadSceneMode.Single);
        //heroInstance.transform.position = Vector3.zero;
        LoadMainMenu();
    }


    public void SelectSaveSlot(int slot)
    {
        saveSlot = slot;
        if (File.Exists(Application.persistentDataPath + "/playerInfo" + saveSlot + ".dat"))
        {
            Debug.Log("Arquivo Existe, precisa perguntar se sobrescrever");
        }
        StartNewGame();
            
    }

    public void ChangeMusic(AudioClip newClip)
    {
        
        musicControl.Stop();
        musicControl.clip = newClip;
        musicControl.Play();
    }

    public void SelectLoadSlot(int slot)
    {
        saveSlot = slot;
        Load();
        heroStatsScript.ReadControl();
        heroInventoryScript.PopulateInventory();
        //ChangeMusic(levelSelectAudioClip);
    }

    public void StartNewGame()
    {
        if (currentState == gameState.PLAY)
        {
            Debug.Log("Deve avisar que o progresso atual será perdido");
            //Destroy(heroInstance);
        }

        experience = 0;
        nextExperienceLevel = 50;
        level = 0;
        stage = 1;
        coins = 0;
        diamonds = 0;
        strength = 1;
        dexterity = 1;
        inteligence = 1;
        shieldBashSkill = 0;
        impaleSkill = 0;
        mightyLeapSkill = 0;
        deepSlashSkill = 0;
        skillPointsRemaining = 0;
        attributePointsRemaining = 0;
        canRoll = false;
        canBlock = false;

        for (int i = 0; i < inventoryCapacity; i++)
        {
            inventory[i] = -1;
            inventoryAmount[i] = 0;
        }

        for (int i = 0; i < numChest; i++)
        {
            chests[i] = false;
        }

        for (int i = 0; i < numBosses; i++)
        {
            bosses[i] = false;
        }

        for (int i = 0; i < numPuzzles; i++)
        {
            puzzles[i] = false;
        }

        for (int i = 0; i < numTutorialMessages; i++)
        {
            tutorialMessages[i] = false;
        }

        for (int i = 0; i < numHistoryMessages; i++)
        {
            historyMessages[i] = false;
        }

        activeOrb = -1;
        for (int i = 0; i < numOrbs; i++)
        {
            orbs[i] = false;
        }

        heroStatsScript.ReadControl();
        heroInventoryScript.PopulateInventory();
        //heroInstance = Instantiate(hero);
        Save();
        
    }

    public void PlayLevelSelectMusic()
    {
        ChangeMusic(levelSelectAudioClip);
    }

    public void LevelSelect(int stage)
    {
        currentState = gameState.PLAY;
        musicControl.Stop();
        SceneManager.LoadScene(stage, LoadSceneMode.Single);
        
    }

    public void ChestOpened(int id)
    {
        chests[id] = true;
    }

    public bool VerifyChest(int id)
    {
        return chests[id];
    }

    public void BossDefeated(int id)
    {
        bosses[id] = true;
    }

    public bool VerifyBoss(int id)
    {
        return bosses[id];
    }

    public void PuzzleCompleted(int id)
    {
        puzzles[id] = true;
    }

    public bool VerifyPuzzle(int id)
    {
        return puzzles[id];
    }

    public void TutorialMessageSent(int id)
    {
        tutorialMessages[id] = true;
    }

    public bool VerifyTutorialMessage(int id)
    {
        return tutorialMessages[id];
    }

    public void HistoryMessageSent(int id)
    {
        historyMessages[id] = true;
    }

    public bool VerifyHistoryMessage(int id)
    {
        return historyMessages[id];
    }

    public void PickUpShield()
    {
        canBlock = true;
        heroStatsScript.PickUpShield();
    }

    public void LearnToRoll()
    {
        canRoll = true;
        heroStatsScript.LearnToRoll();
    }

    void OnGUI()
    {
        if (adjustControlsCanvas.activeSelf)
        {
            //GUI.Label(new Rect(10, 10, 100, 30), "Health: " + health);
            GUI.Label(new Rect(10, 70, 150, 30), "Experience: " + experience);
            GUI.Label(new Rect(10, 100, 150, 30), "Strength: " + strength);
            GUI.Label(new Rect(10, 130, 150, 30), "Dexterity: " + dexterity);
            GUI.Label(new Rect(10, 160, 150, 30), "Inteligence: " + inteligence);
            GUI.Label(new Rect(10, 190, 150, 30), "ShieldBash: " + shieldBashSkill);
            GUI.Label(new Rect(10, 220, 150, 30), "Impale: " + impaleSkill);
            GUI.Label(new Rect(10, 250, 150, 30), "MightyLeap: " + mightyLeapSkill);
            GUI.Label(new Rect(10, 280, 150, 30), "DeepSlash: " + deepSlashSkill);
            GUI.Label(new Rect(10, 310, 150, 30), "SkillPoints: " + skillPointsRemaining);
            GUI.Label(new Rect(10, 340, 150, 30), "AttribPoints: " + attributePointsRemaining);
        }
    }

    public void Save()
    {
        // Inicialização do formatador e do arquivo
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo"+saveSlot+".dat");

        // O que salvar?
        PlayerData data = new PlayerData();
        //data.health = health;
        data.name = heroName;
        data.experience = experience;
        data.nextExperienceLevel = nextExperienceLevel;
        data.coins = coins;
        data.diamonds = diamonds;
        data.level = level;
        data.stage = stage;
        data.strength = strength;
        data.dexterity = dexterity;
        data.inteligence = inteligence;
        data.shieldBashSkill = shieldBashSkill;
        data.impaleSkill = impaleSkill;
        data.mightyLeapSkill = mightyLeapSkill;
        data.deepSlashSkill = deepSlashSkill;
        data.skillPointsRemaining = skillPointsRemaining;
        data.attributePointsRemaining = attributePointsRemaining;
        data.canBlock = canBlock;
        data.canRoll = canRoll;

        data.activeOrb = activeOrb;
        for (int i = 0; i < numOrbs; i++)
        {
            data.orbs[i] = orbs[i];
        }

        for (int i = 0; i < inventoryCapacity; i++)
        {
            data.inventory[i] = inventory[i];
            data.inventoryAmount[i] = inventoryAmount[i];
            
        }

        for (int i = 0; i < numChest; i++)
        {
            data.chests[i] = chests[i];
        }

        for (int i = 0; i < numBosses; i++)
        {
            data.bosses[i] = bosses[i];
        }

        for (int i = 0; i < numPuzzles; i++)
        {
            data.puzzles[i] = puzzles[i];
        }

        for (int i = 0; i < numTutorialMessages; i++)
        {
            data.tutorialMessages[i] = tutorialMessages[i];
        }

        for (int i = 0; i < numHistoryMessages; i++)
        {
            data.historyMessages[i] = historyMessages[i];
        }

        bf.Serialize(file, data);
        file.Close();
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    // Faz alguma coisa se o usuário fechar o jogo via botão home
    private void OnApplicationQuit()
    {
        
    }

    // Faz alguma coisa se o game pausar
    private void OnApplicationPause(bool pause)
    {
        
    }

    public bool VerifyFile(int slot)
    {
        return File.Exists(Application.persistentDataPath + "/playerInfo" + slot + ".dat");
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo" + saveSlot + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo" + saveSlot + ".dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            //health = data.health;
            heroName = data.name;
            experience = data.experience;
            nextExperienceLevel = data.nextExperienceLevel;
            coins = data.coins;
            diamonds = data.diamonds;
            level = data.level;
            stage = data.stage;
            strength = data.strength;
            dexterity = data.dexterity;
            inteligence = data.inteligence;
            shieldBashSkill = data.shieldBashSkill;
            impaleSkill = data.impaleSkill;
            mightyLeapSkill = data.mightyLeapSkill;
            deepSlashSkill = data.deepSlashSkill;
            skillPointsRemaining = data.skillPointsRemaining;
            attributePointsRemaining = data.attributePointsRemaining;
            canBlock = data.canBlock;
            canRoll = data.canRoll;

            activeOrb = data.activeOrb;
            for (int i = 0; i < numOrbs; i++)
            {
                orbs[i] = data.orbs[i];
            }

            for (int i = 0; i < inventoryCapacity; i++)
            {
                inventory[i] = data.inventory[i];
                inventoryAmount[i] = data.inventoryAmount[i];
            }

            for (int i = 0; i < numChest; i++)
            {
                chests[i] = data.chests[i];
            }

            for (int i = 0; i < numBosses; i++)
            {
                bosses[i] = data.bosses[i];
            }

            for (int i = 0; i < numPuzzles; i++)
            {
                puzzles[i] = data.puzzles[i];
            }

            for (int i = 0; i < numTutorialMessages; i++)
            {
                tutorialMessages[i] = data.tutorialMessages[i];
            }

            for (int i = 0; i < numHistoryMessages; i++)
            {
                historyMessages[i] = data.historyMessages[i];
            }
        }
        else Debug.Log("Arquivo não existe");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            heroStatsScript.GiveXP(30);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Break();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            heroStatsScript.UpdateStats(false);
        }
    }
}

// clean class para salvar os dados
[Serializable]      // com isso essa classe pode ser gravada em um arquivo
class PlayerData
{
    //public float health;
    public string name;
    public float experience;
    public float nextExperienceLevel;
    public ulong coins;
    public int diamonds;
    public int level;
    public int stage;
    public int strength;
    public int dexterity;
    public int inteligence;
    public int shieldBashSkill;
    public int impaleSkill;
    public int mightyLeapSkill;
    public int deepSlashSkill;
    public int skillPointsRemaining;
    public int attributePointsRemaining;
    public const int inventoryCapacity = 6;
    public int[] inventory = new int[inventoryCapacity];
    public int[] inventoryAmount = new int[inventoryCapacity];
    public const int numOrbs = 5;
    public bool[] orbs = new bool[numOrbs];
    public int activeOrb;
    public const int numChests = 99;
    public bool[] chests = new bool[numChests];
    public const int numBosses = 5;
    public bool[] bosses = new bool[numBosses];
    public const int numPuzzles = 5;
    public bool[] puzzles = new bool[numPuzzles];
    public bool canBlock;
    public bool canRoll;
    public const int numTutorialMessages = 50;
    public bool[] tutorialMessages = new bool[numTutorialMessages];
    public const int numHistoryMessages = 50;
    public bool[] historyMessages = new bool[numHistoryMessages];

}