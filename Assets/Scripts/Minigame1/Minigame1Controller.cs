using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

[System.Serializable]
public class EndGameUI
{
    public GameObject UI;
    public TMP_Text firstText;
    public TMP_Text secondText;
}

public class Minigame1Controller : MonoBehaviour
{

    #region Prefabs Load
    public GameObject photoPrefab;
    #endregion

    #region Spawner
    private float spawnerTimer;
    private float spawnerSpeed;
    private float zIndex;
    #endregion

    #region Time
    private float timer = 15f;
    public TMP_Text timerUI;
    #endregion

    #region Points
    private int points = 0;
    #endregion

    #region Constants
    const int GOODPHOTODROPPED = -20;
    const int BADPHOTODROPPED = 10;
    const int BADPHOTO = -10;
    const int GOODPHOTO = 5;
    const int POINTSIFLOSE = 50;
    #endregion

    #region EndGame
    public EndGameUI endGameUI;
    private bool hasGameEnded = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.changeState("minigame1");

        spawnerTimer = 1f;
        spawnerSpeed = 1f;
        timerUI.text = Mathf.Round(timer).ToString();

        zIndex=0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasGameEnded) {
            photoSpawner();
            endGame();
        }
    }

    private void photoSpawner()
    {
        spawnerTimer += Time.deltaTime;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timerUI.text = Mathf.Round(timer).ToString();

            if (spawnerTimer >= spawnerSpeed)
            {
                GameObject photo = ObjectPooler.instance.GetPooledObject("MG1Photo");

                int good = Random.Range(0, 2);

                if (good == 0)
                {
                    photo.GetComponent<PhotoController>().goodMemory = true;
                    points += GOODPHOTO;
                }
                else
                {
                    photo.GetComponent<PhotoController>().goodMemory = false;
                    points += BADPHOTO;
                }

                float x = Random.Range(-350f, 350f);
                float y = Random.Range(-130f, 90f);
                float rotation = Random.Range(-5f, 5f);

                photo.transform.localPosition = new Vector3(x, y, zIndex);
                photo.transform.localScale = new Vector3(1, 1, 1);
                photo.transform.localRotation = Quaternion.Euler(0, 0, rotation);
                photo.transform.SetAsLastSibling();

                photo.SetActive(true);

                spawnerTimer = 0;
            }
        }
        else
        {
            hasGameEnded = true;
        }
    }

    private void endGame()
    {
        if (hasGameEnded)
        {
            if (points < 0)
            {
                GameController.instance.decisions["lost_minigame1"] = true;
                GameController.instance.SetPendingPoints(POINTSIFLOSE);
            }
            else
            {
                GameController.instance.decisions["played_minigame1"] = true;
                GameController.instance.SetPendingPoints(0);
                endGameUI.firstText.text = "Hurrah!";
                endGameUI.secondText.text = "You destroyed Dooley's bad pictures!";
            }

            endGameUI.UI.SetActive(true);
            if (SoundConfig.instance != null)
                SoundConfig.instance.ActivateSoftMG();
        }
    }

    public void returnToExploration()
    {
        GameController.instance.returnToExploration();
    }

    public void addPoints(GameObject photo)
    {
        if (photo.GetComponent<PhotoController>().goodMemory)
            points += GOODPHOTODROPPED;
        else points += BADPHOTODROPPED;
    }

    public bool hasTime() {
        return timer>0;
    }
}
