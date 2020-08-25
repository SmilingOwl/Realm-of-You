using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomPhoto : Interactable
{
    public override void Start()
    {
        this.isActive = false;
        this.hasInteracted = false;
        if (GameController.instance.decisions["found_mom_photo"])
        {
            this.SetHasInteracted();
        }
    }

    public override void Interact()
    {
        InteractSound.start();

        if (!this.hasInteracted && GameController.instance.GetState() == "exploration")
        {
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("MommyPhoto");
            this.hasInteracted = true;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
