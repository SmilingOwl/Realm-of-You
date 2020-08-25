using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class FinalBattleController : MonoBehaviour
{
    public List<GameObject> poles;
    public Material interactedMaterial;
    [HideInInspector]
    public bool polesHaveAppeared = false;
    private bool polesAnimation = false;

    private Dictionary<string, bool> polesInteracted = new Dictionary<string, bool>();

    #region DooleyIndecision
    public GameObject indecisionFlashUI;
    public GameObject indecisionBar;
    [HideInInspector]
    public bool indecisionLost = false;
    private int indecisionLevel = 30;
    public int IndecisionLevel
    {
        get { return indecisionLevel; }
        set
        {
            if (value < indecisionLevel)
            {
                if (value < 0)
                {
                    value = 0;
                }
                indecisionBar.GetComponent<ProgressBar>().DecProgress(indecisionLevel, indecisionLevel - value);
                indecisionLost = true;
                indecisionLevel = value;
            }
            else
            {
                if (value > 100)
                {
                    value = 100;
                }
                indecisionBar.GetComponent<ProgressBar>().IncProgress(indecisionLevel, value - indecisionLevel);
                indecisionLevel = value;
            }
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.changeState("finalBattle");
        GameController.instance.decisions["found_backpack"] = true;

        //Randomize initial positions
        int[] pos = new int[] { 0, 1, 2, 3 };
        pos = GameController.instance.shuffleArray(pos);

        for (int i = 0; i < poles.Count; i++)
        {
            polesInteracted.Add(poles[i].name, false);
            UpdatePosition(poles[i].transform, pos[i]);
        }

        indecisionBar.GetComponent<ProgressBar>().SetProgress(indecisionLevel);

        CountOptionalItems();
    }

    void CountOptionalItems()
    {
        List<Item> items = Inventory.instance.items;
        List<string> optionalItems = new List<string>();
        optionalItems.AddRange(new string[] { "TeddyBear", "VoodooDoll", "Glasses", "RedDress", "Candies", "Rose" });

        int count = 0;

        foreach (Item item in items)
            if (optionalItems.Contains(item.name))
                count++;

        if (count > 2)
            TakeDamage(-10); //Progress bar already set
    }

    // Update is called once per frame
    void Update()
    {
        if (this.polesAnimation)
        {
            for (int i = 0; i < poles.Count; i++)
            {
                AnimatePole(poles[i].transform);
            }
            bool finished = true;
            for (int i = 0; i < poles.Count; i++)
            {
                if (poles[i].transform.position.y < 0f)
                {
                    finished = false;
                }
            }
            if (finished)
            {
                this.polesAnimation = false;
                GameObject.FindWithTag("Player").GetComponent<PlayerController>().allowMovement(true);
            }
        }

        IndecisionFlash();
    }

    #region IndecisionAux

    public void TakeDamage(int damage)
    {
        IndecisionLevel = IndecisionLevel - damage;
    }

    public void IndecisionFlash()
    {
        if (indecisionLost)
            setDamageScreenAlpha(0.4f);
        else setDamageScreenAlpha(0f);

        indecisionLost = false;
    }

    public void setDamageScreenAlpha(float alpha)
    {
        if (indecisionFlashUI == null)
            return;
        Image[] children = indecisionFlashUI.GetComponentsInChildren<Image>();
        Color newColor;
        if (alpha == 0f)
        {
            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = Color.Lerp(child.color, newColor, 3 * Time.deltaTime);
            }
        }
        else
        {
            indecisionFlashUI.GetComponent<CloudSpawner>().SpawnClouds();

            foreach (Image child in children)
            {
                newColor = child.color;
                newColor.a = alpha;
                child.color = newColor;
            }
        }
    }

    #endregion

    private void UpdatePosition(Transform pole, int index)
    {
        switch (index)
        {
            case 0:
                pole.position = new Vector3(-7.75f, -6f, 10.75f);
                pole.localEulerAngles = new Vector3(pole.rotation.x, 216.26f, pole.rotation.z);
                break;
            case 1:
                pole.position = new Vector3(8.04f, -6f, 21.12f);
                pole.localEulerAngles = new Vector3(pole.rotation.x, -60.028f, pole.rotation.z);
                break;
            case 2:
                pole.position = new Vector3(0f, -6f, 31f);
                pole.localEulerAngles = new Vector3(pole.rotation.x, -90f, pole.rotation.z);
                break;
            case 3:
                pole.position = new Vector3(7.66f, -6f, 42.16f);
                pole.localEulerAngles = new Vector3(pole.rotation.x, -43.731f, pole.rotation.z);
                break;
        }
    }

    public void ShowPoles()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().allowMovement(false);
        this.polesAnimation = true;
        polesHaveAppeared = true;
    }

    public void AnimatePole(Transform pole)
    {
        if (pole.position.y < 0f)
        {
            float new_y = pole.position.y + 5f * Time.deltaTime;
            if (new_y > 0f)
            {
                new_y = 0f;
            }
            pole.position = new Vector3(pole.position.x, new_y, pole.position.z);
        }
    }

    //Each pole can only be interacted with if and only if all the ones before him have been interacted with
    //Order in switch is the order they should be interacted with
    public bool CheckPole(string name)
    {
        bool[] interacted = polesInteracted.Values.ToArray();

        switch (name)
        {
            case "Family":
                //Family is the first one to be interacted with
                if (!polesInteracted[name])
                {
                    poles[0].transform.GetChild(2).gameObject.GetComponent<Renderer>().material = interactedMaterial;
                    polesInteracted[name] = true;
                    GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("FinalBattle.FamilyProblemsPt1");
                    return true;
                }
                break;
            case "Violence":
                //If I already interacted with family
                if (polesInteracted["Family"] && !polesInteracted[name])
                {
                    poles[1].transform.GetChild(2).gameObject.GetComponent<Renderer>().material = interactedMaterial;
                    polesInteracted[name] = true;
                    GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("FinalBattle.FamilyProblemsPt2");
                    return true;
                }
                break;
            case "School":
                //If I already interacted with family and violence
                if (polesInteracted["Violence"] && !polesInteracted[name])
                {
                    poles[2].transform.GetChild(2).gameObject.GetComponent<Renderer>().material = interactedMaterial;
                    polesInteracted[name] = true;
                    GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("FinalBattle.Bullying");
                    return true;
                }
                break;
            case "Friends":
                if (polesInteracted["School"] && !polesInteracted[name])
                {
                    poles[3].transform.GetChild(2).gameObject.GetComponent<Renderer>().material = interactedMaterial;
                    polesInteracted[name] = true;
                    GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("FinalBattle.AthyPt1");
                    return true;
                }
                break;
        }

        return false;
    }
}
