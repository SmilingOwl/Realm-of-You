using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractQM : MonoBehaviour
{
    private Vector3 originalScale;
    private bool hasBeenPressed = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            originalScale = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(originalScale.x, 0.02f, originalScale.z);
            revealImage();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            gameObject.transform.localScale = originalScale;
        }
    }

    private void revealImage()
    {
        //If not yet revealed
        if (!hasBeenPressed)
        {
            //Open trash can
            gameObject.transform.parent.GetComponentInChildren<Animator>().SetTrigger("Open");

            //Display new image
            GameObject reveal = gameObject.transform.parent.transform.GetChild(2).gameObject;
            reveal.SetActive(true);
            revealAnimation(reveal.transform, 5.5f);

            //Call parent's parent functions to display UI
            gameObject.transform.parent.gameObject.transform.parent.GetComponent<Minigame2Controller>().revealImage(gameObject.transform.parent.transform.GetChild(2).name);

            hasBeenPressed = true;
        }
    }

    public void revealAnimation(Transform revealObj, float height)
    {
        revealObj.DOMove(new Vector3(revealObj.position.x, height, revealObj.position.z), 1.5f);

        //Endlees rotation
        revealObj.DOLocalRotate(new Vector3(0, 0, 360f), 3f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }
}
