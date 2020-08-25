using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dooley : Interactable
{
    public bool IsDooleyActive()
    {
        return this.isActive;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.isActive = true;
        }
    }

    public override void Interact()
    {
        if (!this.hasInteracted)
        {
            string dialogue = "DooleyEncounter";
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue(dialogue);
            this.hasInteracted = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isActive = false;
        }
    }

    public bool hasBeenInteracted()
    {
        return this.hasInteracted;
    }

    public override void PickUp() { }
}
