using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pole : Interactable
{
    private bool available = true;

    #region Sound
    [FMODUnity.EventRef]
    public string RightPole;
    protected FMOD.Studio.EventInstance RightPoleSound;

    public string WrongPole;
    protected FMOD.Studio.EventInstance WrongPoleSound;
    #endregion

    void Awake()
    {
        RightPoleSound = FMODUnity.RuntimeManager.CreateInstance(RightPole);
        WrongPoleSound = FMODUnity.RuntimeManager.CreateInstance(WrongPole);
    }

    public override void Interact()
    {

        if (!this.hasInteracted && GameController.instance.GetState() == "finalBattle")
        {
            //If true, I can interact and display dialogue
            if (GameObject.Find("Controller").GetComponent<FinalBattleController>().CheckPole(gameObject.name))
            {
                RightPoleSound.start();

                Debug.Log("You found " + gameObject.name);
                this.hasInteracted = true;
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            else if (available)
            {
                WrongPoleSound.start();

                transform.DOPunchRotation(new Vector3(3, 0, 3), 3, 7, 1).OnComplete(() => { available = true; });
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                available = false;
            }
        }
    }
}
