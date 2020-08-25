using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSound : MonoBehaviour
{
    private Transform player;

    #region Sound
    [FMODUnity.EventRef]
    public string BallComing;
    protected FMOD.Studio.EventInstance BallComingSound;
    #endregion

    void OnEnable()
    {
        BallComingSound = FMODUnity.RuntimeManager.CreateInstance(BallComing);
        player = GameObject.FindGameObjectWithTag("Player").transform;

        BallComingSound.start();
        BallComingSound.setVolume(.1f);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(gameObject.transform.position, player.position);

        BallComingSound.setParameterByName("distance", Mathf.Abs(distanceToPlayer - 100));

        if (distanceToPlayer < 2f)
            BallComingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    }

    void OnDisable()
    {
        BallComingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
