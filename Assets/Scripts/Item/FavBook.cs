using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavBook : Interactable
{
    public override void Interact()
    {
        InteractSound.start();

        if (!this.hasInteracted && GameController.instance.GetState() == "minigame4")
        {
            //TODO Run end game dialogue
            Debug.Log("You found the book! Game4 completed!");
            this.hasInteracted = true;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
