using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private bool opening = false;
    private float initial_angle;
    private float time;

    public override void Start()
    {
        this.initial_angle = this.transform.parent.localEulerAngles.y;
        this.time = 0;
    }

    public override void Interact()
    {
        InteractSound.start();

        string dialogue = "FindDoor";
        if (!(GameController.instance.decisions["pickup_page_1"] && GameController.instance.decisions["pickup_page_2"]))
        {
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("LookDoor");
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            this.isActive = false;
            return;
        }
        if (GameController.instance.decisions["played_minigame4"]
            && this.transform.parent.localEulerAngles.y < this.initial_angle + 110f)
        {
            this.opening = true;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().allowMovement(false);
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            return;
        }
        if (GameController.instance.GetState() == "exploration"
            && this.transform.parent.localEulerAngles.y < this.initial_angle + 110f)
        {
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue(dialogue);
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public override void PickUp() { }

    void Update()
    {
        if (this.opening)
        {
            this.time += Time.deltaTime;
            float x_angle = this.transform.parent.localEulerAngles.x;
            float z_angle = this.transform.parent.localEulerAngles.z;
            float y_angle = Mathf.LerpAngle(initial_angle, initial_angle + 110f, this.time);
            transform.parent.localEulerAngles = new Vector3(x_angle, y_angle, z_angle);

            if (this.transform.parent.localEulerAngles.y >= this.initial_angle + 110f)
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().allowMovement(true);
                GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("OpenDoor");
                this.opening = false;
            }
        }
    }
}
