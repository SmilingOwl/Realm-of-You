using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [HideInInspector]
    public StateMachine stateMachine = new StateMachine();

    public string[] sceneNames = { "SampleScene", "Minigame1", "Minigame2", "Minigame3", "Minigame4", "FinalBattle", "Ending" };
    public bool testing = true;
    public GameObject damageFlashUI;

    #region GameVars
    public Dictionary<string, bool> decisions = new Dictionary<string, bool>();
    [HideInInspector]
    public Vector3 playerLastPos;

    private int pendingPoints;
    public List<string> pendingItems = new List<string>();

    private string ending;

    //Reality vars
    [HideInInspector]
    public bool realityLost = false;
    public GameObject realityBar;
    private int realityLevel = 100;
    public int RealityLevel
    {
        get { return realityLevel; }
        set
        {
            if (value < realityLevel) //Reality lost
            {
                if (realityBar != null)
                    realityBar.GetComponent<ProgressBar>().DecProgress(realityLevel, realityLevel - value);
                realityLost = true;
                realityLevel = value;
            }
            else
            {
                if (realityBar != null)
                    realityBar.GetComponent<ProgressBar>().IncProgress(realityLevel, value - realityLevel);
                realityLevel = value;
            }
            if (realityLevel <= 0)
            {
                this.SetEnding("Ending.AthyForgetsReality");
            }
        }
    }
    #endregion

    void Awake()
    {
        if (instance != null && instance != this)
        {
            GameObject.Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;

        this.playerLastPos = new Vector3(0f, 2f, 0f);
        SceneManager.sceneLoaded += OnSceneLoaded;

        this.InitDecisions();
        pendingPoints = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            this.changeState("exploration");
        }
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update(); //Calls the execute method of the current state

        damageFlash();
    }

    public void ResetPlayer()
    {
        List<string> tempKeys = new List<string>(decisions.Keys);

        foreach (string decision in tempKeys)
            decisions[decision] = false;

        RealityLevel = 100;

        this.playerLastPos = new Vector3(0f, 2f, 0f);

        if (Inventory.instance != null)
            Inventory.instance.ResetInventory();
    }

    public string GetEnding()
    {
        return this.ending;
    }

    public void SetEnding(string ending)
    {
        this.ending = ending;
        if (this.ending == "Ending.AthyForgetsReality")
        {
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("LoseReality");
        }
        else if (this.ending == "Ending.LeaveDooley")
        {
            this.changeState("ending");
        }
        else if (this.ending == "GoBack")
        {
            if (this.decisions["talk_to_dad"])
            {
                this.ending = "Ending.BackToRealityTalkToDad";
            }
            else if (this.decisions["cherish_dad"])
            {
                this.ending = "Ending.BackToRealityCherishDad";
            }
            else if (this.decisions["stop_friendship"])
            {
                this.ending = "Ending.BackToRealityStopFriendship";
            }
            else if (this.decisions["talk_to_bullies"])
            {
                this.ending = "Ending.BackToRealityBullies";
            }
            else
            {
                this.ending = "Ending.BackToReality";
            }
            this.changeState("ending");
        }
    }

    public void SetPendingPoints(int points)
    {
        pendingPoints = points;
    }

    public void TakeDamage(int damage)
    {
        RealityLevel = RealityLevel - damage;
    }

    private void InitDecisions()
    {
        this.decisions.Add("found_backpack", false);
        this.decisions.Add("pickup_backpack", false);
        this.decisions.Add("found_mom_photo", false);
        this.decisions.Add("pickup_mom_photo", false);
        this.decisions.Add("found_ribbon", false);
        this.decisions.Add("found_teddy", false);
        this.decisions.Add("found_voodoo", false);
        this.decisions.Add("found_candy", false);
        this.decisions.Add("pickup_candy", false);
        this.decisions.Add("pickup_ribbon", false);
        this.decisions.Add("pickup_voodoo", false);
        this.decisions.Add("pickup_rose", false);
        this.decisions.Add("call_doodley", false);
        this.decisions.Add("pickup_teddy", false);
        this.decisions.Add("pickup_page_1", false);
        this.decisions.Add("pickup_page_2", false);
        this.decisions.Add("found_glasses", false);
        this.decisions.Add("found_mom_dress", false);
        this.decisions.Add("played_minigame1", false);
        this.decisions.Add("played_minigame2", false);
        this.decisions.Add("played_minigame3", false);
        this.decisions.Add("played_minigame4", false);
        this.decisions.Add("lost_minigame1", false);
        this.decisions.Add("talk_to_dad", false);
        this.decisions.Add("cherish_dad", false);
        this.decisions.Add("talk_to_bullies", false);
        this.decisions.Add("stop_friendship", false);
        this.decisions.Add("enemy_appeared", false);
        this.decisions.Add("show_diary_page", false);
        this.decisions.Add("dooley_stays", false);
        this.decisions.Add("dooley_being_convinced", false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this.GetState() == "exploration")
        {
            this.LoadObjects();

            damageFlashUI = GameObject.Find("DamageFlash");
            realityBar = GameObject.Find("RealityBar");
            if (this.realityBar != null)
                this.realityBar.GetComponent<ProgressBar>().SetProgress(realityLevel);

            if (this.GetPreviousState() == "minigame1" || this.GetPreviousState() == "minigame2"
                || this.GetPreviousState() == "minigame3" || this.GetPreviousState() == "minigame4")
            {
                GameObject.FindWithTag("Enemy").GetComponent<EnemySpawner>().StartSpawning();
            }

            //Lose reality once if minigame1 lost
            GameController.instance.checkAfterMinigame1();

            //Add caught objects to inventory
            GameController.instance.checkAfterMinigame2();
            GameController.instance.checkAfterMinigame4();

            if (GameObject.FindWithTag("Player") != null)
            {
                GameObject.FindWithTag("Player").transform.position = this.playerLastPos;
            }
        }
    }

    private void LoadObjects()
    {
        GameObject backpack = GameObject.Find("Backpack");
        if (backpack != null && this.decisions["pickup_backpack"])
        {
            Destroy(backpack);
            GameObject.FindObjectsOfType<Inventory>()[0].setSpace(9);
        }
        else if (backpack != null)
        {
            backpack.SetActive(true);
        }
        GameObject ribbon = GameObject.Find("HairRibbon");
        if (ribbon != null && this.decisions["pickup_ribbon"])
        {
            Destroy(ribbon);
        }
        else if (ribbon != null)
        {
            ribbon.SetActive(true);
        }
        GameObject mom_photo = GameObject.Find("MomPhoto");
        if (mom_photo != null && this.decisions["pickup_mom_photo"])
        {
            Destroy(mom_photo);
        }
        else if (mom_photo != null)
        {
            mom_photo.SetActive(true);
        }
        GameObject voodooDoll = GameObject.Find("VoodooDoll");
        if (voodooDoll != null && this.decisions["found_voodoo"])
        {
            Destroy(voodooDoll);
        }
        else if (voodooDoll != null)
        {
            voodooDoll.SetActive(true);
        }
        GameObject teddy = GameObject.Find("TeddyBear");
        if (teddy != null && this.decisions["pickup_teddy"])
        {
            Destroy(teddy);
        }
        else if (teddy != null)
        {
            teddy.SetActive(true);
        }
        GameObject candy = GameObject.Find("Candies");
        if (candy != null && this.decisions["pickup_candy"])
        {
            Destroy(candy);
        }
        else if (candy != null)
        {
            candy.SetActive(true);
        }
        GameObject rose = GameObject.Find("Rose");
        if (rose != null && this.decisions["pickup_rose"])
        {
            Destroy(rose);
        }
        else if (rose != null)
        {
            rose.SetActive(true);
        }
        GameObject diaryPage1 = GameObject.Find("DiaryPage1");
        if (diaryPage1 != null && decisions["pickup_page_1"])
        {
            Destroy(diaryPage1);
        }
        else if (diaryPage1 != null)
        {
            diaryPage1.SetActive(true);
        }
        GameObject diaryPage2 = GameObject.Find("DiaryPage2");
        if (diaryPage2 != null && decisions["pickup_page_2"])
        {
            Destroy(diaryPage2);
        }
        else if (diaryPage2 != null)
        {
            diaryPage2.SetActive(true);
        }
    }

    #region Utilities
    public void returnToExploration()
    {
        stateMachine.ChangeState(new ExplorationState());
    }

    public void damageFlash()
    {
        if (realityLost)
            setDamageScreenAlpha(0.4f);
        else setDamageScreenAlpha(0f);

        realityLost = false;
    }

    public void setDamageScreenAlpha(float alpha)
    {
        if (damageFlashUI == null)
            return;
        Image[] children = damageFlashUI.GetComponentsInChildren<Image>();
        Color newColor;
        if (alpha == 0f)
        {
            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = Color.Lerp(child.color, newColor, 3 * Time.deltaTime);
            }
        }
        else
        {
            damageFlashUI.GetComponent<CloudSpawner>().SpawnClouds();

            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = newColor;
            }
        }
    }

    public int[] shuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        return newArray;
    }

    public void changeState(string state)
    {
        if (state == "exploration")
        {
            stateMachine.ChangeState(new ExplorationState());
        }
        else if (state == "dialogue")
        {
            stateMachine.ChangeState(new DialogueState());
        }
        else if (state == "minigame1")
        {
            stateMachine.ChangeState(new Minigame1State());
        }
        else if (state == "minigame2")
        {
            stateMachine.ChangeState(new Minigame2State());
        }
        else if (state == "minigame3")
        {
            stateMachine.ChangeState(new Minigame3State());
        }
        else if (state == "minigame4")
        {
            stateMachine.ChangeState(new Minigame4State());
        }
        else if (state == "finalBattle")
        {
            stateMachine.ChangeState(new FinalBattle());
        }
        else if (state == "ending")
        {
            stateMachine.ChangeState(new Ending());
        }
    }

    public string GetState()
    {
        return stateMachine.GetState();
    }

    public string GetPreviousState()
    {
        return stateMachine.GetPreviousState();
    }

    public void checkAfterMinigame1()
    {
        if (stateMachine.GetPreviousState() == "minigame1")
        {
            if (decisions["lost_minigame1"] && !decisions["played_minigame1"])
            {
                TakeDamage(pendingPoints/2);
                SetPendingPoints(0);

                decisions["played_minigame1"] = true;
            }
        }
    }

    public void checkAfterMinigame2()
    {
        if (stateMachine.GetPreviousState() == "minigame2")
        {
            if (!decisions["played_minigame2"])
            {
                if (decisions["found_glasses"])
                {
                    Inventory.instance.Add(Resources.Load<Item>("Items/Glasses"));
                }

                if (decisions["found_mom_dress"])
                {
                    Inventory.instance.Add(Resources.Load<Item>("Items/RedDress"));
                }

                decisions["played_minigame2"] = true;

            }
        }
    }

    public void checkAfterMinigame4()
    {
        if (stateMachine.GetPreviousState() == "minigame4")
        {
            if (decisions["played_minigame4"])
            {
                Inventory.instance.Add(Resources.Load<Item>("Items/Key"));
            }
        }
    }

    #endregion

    #region States
    public class ExplorationState : IState
    {
        public ExplorationState() { }

        public void Enter()
        {
            if (SceneManager.GetActiveScene().name != GameController.instance.sceneNames[0])
            {
                SceneManager.LoadScene(GameController.instance.sceneNames[0]);
                SoundConfig.instance.GoBackToExploration();
            }
        }

        public void Execute()
        {

        }

        public void Exit()
        {
            GameController.instance.playerLastPos = GameObject.FindWithTag("Player").transform.position;
        }
    }

    public class DialogueState : IState
    {
        public DialogueState() { }

        public void Enter()
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().allowMovement(false);
            }

            if (GameController.instance.GetPreviousState() == "minigame1"
                || GameController.instance.GetPreviousState() == "minigame2"
                || GameController.instance.GetPreviousState() == "minigame3"
                || GameController.instance.GetPreviousState() == "minigame4")
            {
                SoundConfig.instance.ActivateSoftMG();
            }

        }

        public void Execute()
        {

        }

        public void Exit()
        {
            if (GameObject.FindWithTag("Player") != null)
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().allowMovement(true);

        }
    }

    public class Minigame1State : IState
    {
        public Minigame1State() { }

        public void Enter()
        {
            if (SceneManager.GetActiveScene().name != GameController.instance.sceneNames[1])
            {
                SceneManager.LoadScene(GameController.instance.sceneNames[1]);
                SoundConfig.instance.GoToMinigame("minigame1");
            }
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }

    public class Minigame2State : IState
    {
        public Minigame2State() { }

        public void Enter()
        {
            if (SceneManager.GetActiveScene().name != GameController.instance.sceneNames[2])
            {
                SceneManager.LoadScene(GameController.instance.sceneNames[2]);
                SoundConfig.instance.GoToMinigame("minigame2");
            }
            else
            {
                SoundConfig.instance.DeactivateSoftMG();
            }
        }

        public void Execute()
        {

        }

        public void Exit()
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().allowMovement(true);
        }
    }

    public class Minigame3State : IState
    {
        public Minigame3State() { }

        public void Enter()
        {
            if (SceneManager.GetActiveScene().name != GameController.instance.sceneNames[3])
            {
                SceneManager.LoadScene(GameController.instance.sceneNames[3]);
                GameController.instance.decisions["played_minigame3"] = true;
                SoundConfig.instance.GoToMinigame("minigame3");
            }
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }

    public class Minigame4State : IState
    {
        public Minigame4State() { }

        public void Enter()
        {
            if (SceneManager.GetActiveScene().name != GameController.instance.sceneNames[4])
            {
                SceneManager.LoadScene(GameController.instance.sceneNames[4]);
                SoundConfig.instance.GoToMinigame("minigame4");
            }
            else
            {
                SoundConfig.instance.DeactivateSoftMG();
            }
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }

    public class FinalBattle : IState
    {
        public FinalBattle() { }

        public void Enter()
        {
            if (SceneManager.GetActiveScene().name != GameController.instance.sceneNames[5])
            {
                SceneManager.LoadScene(GameController.instance.sceneNames[5]);
                SoundConfig.instance.GoToMinigame("finalBattle");
            }
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }

    public class Ending : IState
    {
        public Ending() { }

        public void Enter()
        {
            if (SceneManager.GetActiveScene().name != GameController.instance.sceneNames[6])
            {
                SceneManager.LoadScene(GameController.instance.sceneNames[6]);
                SoundConfig.instance.ChangeSnapshot("ambient");
                SoundConfig.instance.ChangeRichness(1.0f);
            }

        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
    #endregion

    #region StateMachine
    public interface IState
    {
        void Enter();
        void Execute();
        void Exit();
    }

    public class StateMachine
    {
        IState currentState;
        IState previousState;

        public void ChangeState(IState newState)
        {
            if (currentState != null)
            {
                previousState = currentState;
                currentState.Exit();
            }

            currentState = newState;
            currentState.Enter();
        }

        public void Update()
        {
            if (currentState != null) currentState.Execute();
        }

        public string GetState()
        {
            if (this.currentState is DialogueState)
            {
                return "dialogue";
            }
            else if (this.currentState is Minigame1State)
            {
                return "minigame1";
            }
            else if (this.currentState is Minigame2State)
            {
                return "minigame2";
            }
            else if (this.currentState is Minigame3State)
            {
                return "minigame3";
            }
            else if (this.currentState is Minigame4State)
            {
                return "minigame4";
            }
            else if (this.currentState is FinalBattle)
            {
                return "finalBattle";
            }
            else if (this.currentState is Ending)
            {
                return "ending";
            }
            return "exploration";
        }

        public string GetPreviousState()
        {
            if (this.previousState is DialogueState)
            {
                return "dialogue";
            }
            else if (this.previousState is Minigame1State)
            {
                return "minigame1";
            }
            else if (this.previousState is Minigame2State)
            {
                return "minigame2";
            }
            else if (this.previousState is Minigame3State)
            {
                return "minigame3";
            }
            else if (this.previousState is Minigame4State)
            {
                return "minigame4";
            }
            else if (this.previousState is FinalBattle)
            {
                return "finalBattle";
            }
            else if (this.previousState is Ending)
            {
                return "ending";
            }
            return "exploration";
        }
    }
    #endregion
}
