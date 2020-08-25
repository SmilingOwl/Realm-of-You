using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;

public class YarnConfigurations : MonoBehaviour
{
    public GameObject dialogueText;
    private VariableStorage varStorage;
    private bool runDialogue = true;

    void Start() {
        this.varStorage = this.gameObject.GetComponent<VariableStorage>();
        this.runDialogue = true;
    }

    void Update() {
        if (this.runDialogue && GameController.instance.GetState() == "exploration") {
            string dialogueNode = null;
            if (!GameController.instance.decisions["found_backpack"]) {
                dialogueNode = "RealmOfYou";
            }
            switch (GameController.instance.GetPreviousState()) {
                case "minigame1": dialogueNode = "AfterMemoriesShooting"; break;
                case "minigame2": dialogueNode = "AfterDetectiveMission"; break;
                case "minigame3": dialogueNode = "AfterGlueWork"; break;
                case "minigame4": dialogueNode = "AfterFindKey"; break;
            }
            if (!GameController.instance.testing && dialogueNode != null
                && GameObject.Find("DialogueRunner").GetComponent<DialogueRunner>().NodeExists(dialogueNode)) {
                GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue(dialogueNode);
                this.runDialogue = false;
            }
        } else if (this.runDialogue && GameController.instance.GetState() == "minigame4"
            && !GameController.instance.decisions["played_minigame4"]) {
            this.runDialogue = false;
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("MiniGame4.Intro");
            GameController.instance.decisions["played_minigame4"] = true;
            GameObject.Find("Minigame Controller").GetComponent<Minigame4Controller>().DecreaseThrowForce();
        } else if (this.runDialogue && GameController.instance.GetState() == "ending") {
            string dialogueNode = GameController.instance.GetEnding();
            if (GameObject.Find("DialogueRunner").GetComponent<DialogueRunner>().NodeExists(dialogueNode)) {
                GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue(dialogueNode);
                this.runDialogue = false;
            }
        }
    }

    public void UsePage() {
        GameObject.FindObjectsOfType<Diary>()[0].UsePage();
    }

    [YarnCommand("set_color")]
    public void SetColor(string color) {
        if (color == "blue") {
            dialogueText.GetComponent<TextMeshProUGUI>().color = new Color32(76, 145, 203, 255);
        } else if (color == "red") {
            dialogueText.GetComponent<TextMeshProUGUI>().color = new Color32(193, 34, 34, 255);
        } else if (color == "green") {
            dialogueText.GetComponent<TextMeshProUGUI>().color = new Color32(34, 135, 25, 255);
        }
    }

    [YarnCommand("set_ending")]
    public void SetEnding(string ending) {
        GameController.instance.SetEnding(ending);
    }

    [YarnCommand("start_counting")]
    public void StartCounting() {
        GameObject.FindWithTag("Enemy").GetComponent<EnemySpawner>().StartCounting();
    }

    [YarnCommand("set_variable")]
    public void SetVariable(string[] args) {
        bool state = false;
        if (args.Length > 1 && args[1] == "true") {
            state = true;
        }
        GameController.instance.decisions[args[0]] = state;
        this.varStorage.SetValue("$"+args[0], state);
    }

    [YarnCommand("show_poles")]
    public void ShowPoles() {
        GameObject.Find("Controller").GetComponent<FinalBattleController>().ShowPoles();
    }

    [YarnCommand("pickup_object")]
    public void PickUpObject(string object_name) {
        GameObject game_object = null;
        switch (object_name) {
            case "backpack": game_object = GameObject.Find("Backpack"); break;
            case "ribbon": game_object = GameObject.Find("HairRibbon"); break;
            case "mom_photo": game_object = GameObject.Find("MomPhoto"); break;
            case "teddy_bear": game_object = GameObject.Find("TeddyBear"); break;
            case "voodoo_doll": game_object = GameObject.Find("VoodooDoll"); break;
            case "candy": game_object = GameObject.Find("Candies"); break;
            case "rose": game_object = GameObject.Find("Rose"); break;
            case "diary_page1":
                game_object = GameObject.Find("DiaryPage1");
                if (game_object == null || !game_object.GetComponent<DiaryPage>().IsActive()) {
                    game_object = GameObject.Find("DiaryPage2");
                }
                break;
            case "diary_page2":
                game_object = GameObject.Find("DiaryPage2");
                if (game_object == null || !game_object.GetComponent<DiaryPage>().IsActive()) {
                    game_object = GameObject.Find("DiaryPage1");
                }
                break;
        }
        if(game_object != null) {
            game_object.GetComponent<Interactable>().PickUp();
        }
    }

    [YarnCommand("go_to_minigame")]
    public void GoToMinigame(string minigame) {
        GameController.instance.changeState(minigame);
    }

    [YarnCommand("add_diary_page")]
    public void AddDiaryPage(string name) {
        Diary.instance.AddPage(name);
    }

    [YarnCommand("take_damage")]
    public void TakeDamage(string damage) {
        if (GameObject.Find("Controller") != null && GameObject.Find("Controller").GetComponent<FinalBattleController>() != null) {
            GameObject.Find("Controller").GetComponent<FinalBattleController>().TakeDamage(int.Parse(damage));
        } else {
            GameController.instance.TakeDamage(int.Parse(damage));
        }
    }

    [YarnCommand("check_dooley_stays")]
    public void CheckDooleyStays() {
        if (GameObject.Find("Controller") != null && GameObject.Find("Controller").GetComponent<FinalBattleController>() != null) {
            if (GameObject.Find("Controller").GetComponent<FinalBattleController>().IndecisionLevel <= 30) {
                this.SetVariable(new string[]{"dooley_stays", "true"});
            }
        }
    }

    [YarnCommand("check_dooley_being_convinced")]
    public void CheckDooleyBeingConvinced() {
        if (GameObject.Find("Controller") != null && GameObject.Find("Controller").GetComponent<FinalBattleController>() != null) {
            if (GameObject.Find("Controller").GetComponent<FinalBattleController>().IndecisionLevel >= 60) {
                this.SetVariable(new string[]{"dooley_being_convinced", "true"});
            }
        }
    }

    public void ChangeState(string state) {
        GameController.instance.changeState(state);
    }

    public void RunDialogue(string node_name) {
        GameController.instance.changeState("dialogue");
        this.gameObject.GetComponent<DialogueRunner>().StartDialogue(node_name);
    }

    public void SetVariable(string variable, bool state) {
        Yarn.Value value = new Yarn.Value(state);
        this.varStorage.SetValue("$"+variable, value);
    }
}
