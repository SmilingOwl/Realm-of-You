using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyBear : Interactable
{
    public override void Start()
    {
        this.isActive = false;
        this.hasInteracted = false;
        if (GameController.instance.decisions["pickup_teddy"])
        {
            this.SetHasInteracted();
        }
    }

    public override void Interact()
    {
        InteractSound.start();

        if (!this.hasInteracted && GameController.instance.GetState() == "exploration")
        {
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("TeddyBear");
            this.hasInteracted = true;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
