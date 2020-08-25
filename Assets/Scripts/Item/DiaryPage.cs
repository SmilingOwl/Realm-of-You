using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryPage : Interactable
{
    public override void Interact()
    {
        InteractSound.start();

        string page = "DiaryPage2";

        if (GameController.instance.decisions["played_minigame1"] == false)
        {
            page = "DiaryPage1";
        }
        string dialogue = "BeforeBullying";
        if (page == "DiaryPage1")
        {
            dialogue = "BeforeBullying";
        }
        else if (page == "DiaryPage2")
        {
            dialogue = "BeforeViolence";
        }
        if (!this.hasInteracted && GameController.instance.GetState() == "exploration")
        {
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue(dialogue);
            this.hasInteracted = true;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public override void PickUp()
    {
        if (!Inventory.instance.items.Contains(this.item))
            Inventory.instance.Add(this.item);

        string page = "DiaryPage2";

        if (GameController.instance.decisions["played_minigame1"] == false)
        {
            page = "DiaryPage1";
        }

        if (this.gameObject.name == "DiaryPage1")
        {
            GameController.instance.decisions["pickup_page_1"] = true;
        }
        else
        {
            GameController.instance.decisions["pickup_page_2"] = true;
        }
        Debug.Log("Adding page " + page);
        Diary.instance.AddPage(page);

        Destroy(this.gameObject);
    }

}
