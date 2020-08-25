using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Minigame2Controller : MonoBehaviour
{
    #region ItemsVars
    public GameObject[] items;
    private GameObject instResult;
    #endregion

    #region TimeVars
    private float timer = 15f;
    private bool hasStarted = false;
    public TMP_Text timerUI;
    #endregion

    #region PopUpVars
    public GameObject popUp;
    private bool popUpVisible = false;
    private string[] messages = { "Hmm...There’s nothing here!", "I found Dooley’s glasses!", "I found Dooley’s mommy’s red dress!" };
    #endregion

    #region EndGameVars
    public GameObject endGameUI;
    public GameObject EndGameBorder;
    private int objectsFound = 0;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        randomizeItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted && (timer <= 0f || objectsFound == 2))
        {
            if (!popUpVisible)
                endGame();
        }
        else if (hasStarted)
        {
            timer -= Time.deltaTime;
            float timerRounded = Mathf.Round(timer);
            timerUI.text = timerRounded.ToString();
        }

    }

    public void changeState(string state)
    {
        GameController.instance.changeState(state);
    }

    private void randomizeItems()
    {
        int[] numbers = { 0, 0, 0, 0, 0, 0, 1, 2 }; //0 is nothing, 1 is glasses and 2 reddress
        numbers = GameController.instance.shuffleArray(numbers);

        for (int i = 0; i < numbers.Length; i++)
        {
            createResultItem(items[i], numbers[i]);
        }

    }

    public void createResultItem(GameObject obj, int index)
    {

        switch (index)
        {
            case 0:
                instResult = Instantiate(Resources.Load<GameObject>("Minigame2/Empty")) as GameObject;
                instResult.SetActive(false);
                instResult.transform.SetParent(obj.transform);
                instResult.transform.position = obj.transform.position;
                break;
            case 1:
                instResult = Instantiate(Resources.Load<GameObject>("Minigame2/Glasses")) as GameObject;
                instResult.SetActive(false);
                instResult.transform.SetParent(obj.transform);
                instResult.transform.position = obj.transform.position;
                break;
            case 2:
                instResult = Instantiate(Resources.Load<GameObject>("Minigame2/RedDress")) as GameObject;
                instResult.SetActive(false);
                instResult.transform.SetParent(obj.transform);
                instResult.transform.position = obj.transform.position;
                break;
        }
    }

    public void revealImage(string name)
    {
        string fixedName = name;
        if (name.Contains("(Clone)"))
            fixedName = name.Replace("(Clone)", "");

        switch (fixedName)
        {
            case "Empty":
                GameObject.Find("DialogueRunner").GetComponent<YarnMinigame2>().RunDialogue("MiniGame2.NothingFound");
                break;
            case "Glasses":
                objectsFound++;
                GameObject.Find("DialogueRunner").GetComponent<YarnMinigame2>().RunDialogue("MiniGame2.FoundGlasses");
                GameController.instance.decisions["found_glasses"] = true;
                break;
            case "RedDress":
                objectsFound++;
                GameObject.Find("DialogueRunner").GetComponent<YarnMinigame2>().RunDialogue("MiniGame2.FoundDress");
                GameController.instance.decisions["found_mom_dress"] = true;
                break;
        }

    }

    private void endGame()
    {
        string firstText = "", secondText = "";

        switch (objectsFound)
        {
            case 0:
                firstText = "Oh no...";
                secondText = "You couldn't find anything";
                GameController.instance.decisions["played_minigame2"] = true;
                break;
            case 1:
                firstText = "Almost there!";
                secondText = "You only found one of the objects";
                break;
            case 2:
                firstText = "Good job!";
                secondText = "You found everything!";
                break;
        }

        GameObject.FindWithTag("Player").GetComponent<PlayerController>().allowMovement(false);

        endGameUI.transform.GetChild(0).GetComponent<TMP_Text>().text = firstText;
        endGameUI.transform.GetChild(1).GetComponent<TMP_Text>().text = secondText;

        endGameUI.transform.SetAsLastSibling();
        EndGameBorder.SetActive(true);
        if (SoundConfig.instance != null)
            SoundConfig.instance.ActivateSoftMG();
        popUpVisible = true;
    }

    public void returnToExploration()
    {
        GameController.instance.returnToExploration();
    }

    IEnumerator showPopUp(int index, float waitTime)
    {
        //Enable popup
        popUpVisible = true;
        popUp.transform.GetChild(2).GetComponent<TMP_Text>().text = messages[index];
        popUp.SetActive(true);
        popUp.transform.SetAsLastSibling();

        //Prevent mouse input
        gameObject.transform.parent.GetComponent<GraphicRaycaster>().enabled = false;

        yield return new WaitForSeconds(waitTime);

        popUp.SetActive(false);
        gameObject.transform.parent.GetComponent<GraphicRaycaster>().enabled = true;
        popUpVisible = false;
    }

    public void startGame()
    {
        this.hasStarted = true;
    }
}
