using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour
{
    public Animator animator;
    public GameObject nextButton;

    void Start()
    {
        StartCoroutine(ShowButton(animator.GetCurrentAnimatorStateInfo(0).length));
    }

    IEnumerator ShowButton(float length)
    {
        yield return new WaitForSeconds(length);
        nextButton.SetActive(true);
    }

    public void LoadNextLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
