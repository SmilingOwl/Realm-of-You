using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Minigame4Controller : MonoBehaviour
{
    private Transform player;

    public GameObject favBook;
    public GameObject restartOption;
    public GameObject endGameUI;

    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.changeState("minigame4");
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y <= 0)
            Restart();

        EndGame();
    }

    private void Restart()
    {
        restartOption.SetActive(true);

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    private void EndGame()
    {
        if (!favBook.activeInHierarchy) //If not active means we already caught it
        {
            GameController.instance.decisions["played_minigame4"] = true;
            gameObject.GetComponent<BooksController>().gameObject.SetActive(false); //Stop throwing
            player.gameObject.GetComponent<PlayerController>().allowMovement(false);
            endGameUI.SetActive(true);
            if (SoundConfig.instance != null)
                SoundConfig.instance.ActivateSoftMG();
        }
    }

    public void DecreaseThrowForce()
    {
        gameObject.GetComponent<BooksController>().ThrowForce = 700;
    }

    public void IncreaseThrowForce()
    {
        gameObject.GetComponent<BooksController>().ThrowForce = 900;
    }

    public void returnToExploration()
    {
        GameController.instance.returnToExploration();
    }

    public void changeState(string state)
    {
        GameController.instance.changeState(state);
    }
}
