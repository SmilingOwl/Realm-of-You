using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundConfig : MonoBehaviour
{
    [FMODUnity.EventRef]
    private FMOD.Studio.EventInstance ambientSound;
    private FMOD.Studio.EventInstance minigameSound;
    private FMOD.Studio.EventInstance ambientSnapshot;
    private FMOD.Studio.EventInstance mgSnapshot;
    private FMOD.Studio.EventInstance softMGSnapshot;
    private float richness = 1.0f;
    public static SoundConfig instance;
    public FMOD.Studio.EventInstance activeSnapshot;
    private float targetRichness = -1.0f;
    private bool changingRichness = false;
    private float step = 0.02f;
    private bool waitAfterGame = false;
    private float timePassed = 0f;
    private string state = "exploration";

    void Start () {
        if (instance != null && instance != this)
        {
            GameObject.Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;

        ambientSound = FMODUnity.RuntimeManager.CreateInstance("event:/Environment/Game/game_ambient");
        ambientSound.setParameterByName("richness", richness);
        ambientSound.start();
        ambientSnapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/MainAmbient");
        mgSnapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/MiniGameAmbient");
        softMGSnapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/SoftMuteMG");
        ambientSnapshot.start();
        activeSnapshot = ambientSnapshot;
    }

    public void GoToMinigame(string minigame) {
        state = minigame;
        string soundEventName = "event:/Environment/MG1/MG1_Ambient";
        ChangeSnapshot("mg");
        switch (minigame) {
            case "minigame1":
                soundEventName = "event:/Environment/MG1/MG1_Ambient";
                break;
            case "minigame2":
                soundEventName = "event:/Environment/MG2/MG2_Ambient";
                ActivateSoftMG();
                break;
            case "minigame3":
                soundEventName = "event:/Environment/MG3/MG3_Ambient";
                break;
            case "minigame4":
                soundEventName = "event:/Environment/MG4/MG4_Ambient";
                ActivateSoftMG();
                break;
            case "finalBattle":
                soundEventName = "event:/Environment/FinalBattle/FinalBattle";
                break;
        }
        
        minigameSound = FMODUnity.RuntimeManager.CreateInstance(soundEventName);
        if (minigame == "finalBattle") {
            minigameSound.setParameterByName("Life", 100);
        }
        minigameSound.start();
    }

    public void GoBackToExploration() {
        switch (state) {
            case "minigame1":
                waitAfterGame = true;
                targetRichness = 3.0f;
                break;
            case "minigame2":
                waitAfterGame = true;
                break;
            case "minigame3":
                waitAfterGame = true;
                targetRichness = 4.0f;
                break;
            case "minigame4":
                waitAfterGame = true;
                break;
        }
        state = "exploration";
        ChangeSnapshot("ambient");
    }

    public void DeactivateSoftMG() {
        softMGSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void ActivateSoftMG() {
        softMGSnapshot.start();
    }
    public void ChangeRichness(float value) {
        richness = value;
        ambientSound.setParameterByName("richness", richness);
    }

    public void ChangeSnapshot(string snapshot) {
        if (snapshot == "ambient") {
            DeactivateSoftMG();
            activeSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ambientSnapshot.start();
            activeSnapshot = ambientSnapshot;
        } else if (snapshot == "mg") {
            activeSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            mgSnapshot.start();
            activeSnapshot = mgSnapshot;
        }
    }

    public void IncrementRichness(float new_richness) {
        targetRichness = new_richness;
        changingRichness = true;
    }

    void Update() {
        if (changingRichness) {
            ChangeRichness(richness + step);
            if (richness >= targetRichness) {
                targetRichness = -1f;
                changingRichness = false;
                ChangeSnapshot("ambient");
            }
        }
        if (waitAfterGame) {
            timePassed += Time.deltaTime;
            if (timePassed >= 1f) {
                waitAfterGame = false;
                timePassed = 0f;
                minigameSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                changingRichness = true;
            }
        }
    }
}
