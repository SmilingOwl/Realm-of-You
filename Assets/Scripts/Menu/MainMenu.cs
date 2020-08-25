using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;

    void Start()
    {
        string path = Application.persistentDataPath + "/player.data";
        if (File.Exists(path))
        {
            continueButton.SetActive(true);
        }
    }

    public void Play()
    {
        //Reset game controller
        GameController.instance.ResetPlayer();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
