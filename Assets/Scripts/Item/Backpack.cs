using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : Interactable
{
    public override void Start()
    {
        if (!GameController.instance.decisions["found_backpack"])
            this.isActive = true;
        else
        {
            this.hasInteracted = true;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public override void Interact()
    {
        InteractSound.start();

        if (!this.hasInteracted && GameController.instance.GetState() == "exploration")
        {
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("Tutorial.Options");
            this.hasInteracted = true;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public override void PickUp()
    {
        Inventory.instance.space = 9;
        Destroy(this.gameObject);
    }
}
