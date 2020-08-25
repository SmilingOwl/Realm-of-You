using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using FMODUnity;
using FMOD.Studio;

public class PhotoController : MonoBehaviour
{
    #region Properties
    private Image photo;
    [HideInInspector]
    public bool goodMemory;
    public List<Sprite> goodImages;
    public List<Sprite> badImages;
    #endregion

    #region Falling Object
    private bool fallingObject = false;
    private float fallingSpeed = 10f;
    #endregion

    #region Sound
    [FMODUnity.EventRef]
    public string RightPicSelected;
    FMOD.Studio.EventInstance RightPicSound;

    public string WrongPicSelected;
    FMOD.Studio.EventInstance WrongPicSound;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        RightPicSound = FMODUnity.RuntimeManager.CreateInstance(RightPicSelected);
        WrongPicSound = FMODUnity.RuntimeManager.CreateInstance(WrongPicSelected);
    }

    void OnEnable()
    {
        photo = this.GetComponent<Image>();
        int idx = 0;

        if (goodMemory)
        {
            idx = Random.Range(0, goodImages.Count);
            photo.sprite = goodImages[idx];
        }
        else
        {
            idx = Random.Range(0, badImages.Count);
            photo.sprite = badImages[idx];
        }

        photo.preserveAspect = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (fallingObject)
        {
            this.transform.position += new Vector3(0, -fallingSpeed * Time.deltaTime, 0);

            float x = Random.Range(0.0f, 1.0f);
            this.transform.Rotate(-x, 0.0f, 0.0f, Space.Self);

            fallingSpeed += 5.0f;

            StartCoroutine(ExecuteAfterTime(2f));
        }
    }

    public void handlePress()
    {
        if(this.transform.parent.gameObject.GetComponent<Minigame1Controller>().hasTime()){
            if (goodMemory)
                WrongPicSound.start();
            else RightPicSound.start();

            GameObject photo = EventSystem.current.currentSelectedGameObject;
            fallingObject = true;
            this.transform.parent.gameObject.GetComponent<Minigame1Controller>().addPoints(photo);
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        fallingObject = false;
        fallingSpeed = 10f;
        gameObject.SetActive(false);
    }
}
