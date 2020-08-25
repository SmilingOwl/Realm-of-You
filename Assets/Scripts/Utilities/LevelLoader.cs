using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    private float counter = 5f;

    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;

        //Only auto loading for how to play scene
        if (SceneManager.GetActiveScene().name == "HowToPlay")
            if (Input.GetMouseButtonDown(0) || counter <= 0)
                LoadNextLevel("MainMenu");
    }

    public void LoadNextLevel(string level)
    {
        StartCoroutine(LoadLevel(level));
    }

    IEnumerator LoadLevel(string levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
