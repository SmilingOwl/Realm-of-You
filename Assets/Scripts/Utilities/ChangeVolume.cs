using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class ChangeVolume : MonoBehaviour
{
    public float volume = 1000f;
    FMODUnity.StudioEventEmitter emitter;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<FMODUnity.StudioEventEmitter>() != null)
        {
            emitter = GetComponent<FMODUnity.StudioEventEmitter>();
            emitter.SetParameter("RPM", volume);
        }
    }

}
