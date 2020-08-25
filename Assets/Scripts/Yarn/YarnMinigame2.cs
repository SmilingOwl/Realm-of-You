using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;

public class YarnMinigame2 : MonoBehaviour
{
    public GameObject dialogueText;
    public bool initialDialogue = true;
    public GameObject continueButton;
    private bool init = true;

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

    void Update() {
        string dialogueNode = "MiniGame2.Intro";
        if (init && GameObject.Find("DialogueRunner").GetComponent<DialogueRunner>().NodeExists(dialogueNode)) {
            GameController.instance.changeState("dialogue");
            this.RunDialogue(dialogueNode);
            init = false;
        }
    }

    [YarnCommand("end_initial_dialogue")]
    public void EndInitialDialogue() {
        this.initialDialogue = false;
    }

    public void RunDialogue(string node_name) {
        GameController.instance.changeState("dialogue");
        this.gameObject.GetComponent<DialogueRunner>().StartDialogue(node_name);
    }

    public void ContinueDialogue() {
        if (this.initialDialogue) {
            this.continueButton.SetActive(true);
        } else {
            this.gameObject.GetComponent<DialogueUI>().MarkLineComplete();
        }
    }
}
