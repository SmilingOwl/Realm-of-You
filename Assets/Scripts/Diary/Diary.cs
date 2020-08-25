using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Diary : MonoBehaviour
{
    public static Diary instance;
    private GameObject canvas;

    [HideInInspector]
    public List<string> pagesFound = new List<string>();
    public Dictionary<string, string> diaryPages = new Dictionary<string, string>();
    private string selectedPage;

    #region Sound
    [FMODUnity.EventRef]
    public string OpenDiaryS;
    protected FMOD.Studio.EventInstance OpenDiarySound;
    #endregion

    void Awake()
    {
        if (instance != null && instance != this)
        {
            instance.ResetCanvas();
            GameObject.Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;

        this.ResetCanvas();
        if (OpenDiaryS != null && OpenDiaryS != "")
        {
            OpenDiarySound = FMODUnity.RuntimeManager.CreateInstance(OpenDiaryS);
        }

    }

    public void ResetCanvas()
    {
        canvas = GameObject.Find("DiaryCanvas");
        canvas.gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void Start()
    {
        diaryPages.Add("DiaryPage1", "Today, I took my favorite book of all time, Space Adventures, to school, to show it during English class. The teacher praised me and said that I presented really well! I was really happy, but Bully said that I was lame and dull for liking books. When I went to the bathroom, Bully’s group wrote all over my desk: Dully! My name is not Dully, it’s Dooley, like the hero of Space Adventures! I even saw someone trying to write in my favorite book. When I took it from them, Bully kicked me and then went away. My leg hurts...  I’m never taking a book to school again!");
        diaryPages.Add("DiaryPage2", "Lately my things have been disappearing. Today, my glasses disappeared. Mommy gave me those glasses. I think Daddy threw them away. He doesn't like it when Mommy buys me stuff. Daddy sometimes steals things that belong to Mommy and throws them out. The other day, I saw Daddy ripping Mommy’s favorite red dress, the one that my late Grammy gave to her on her birthday. When Mommy buys new things, Daddy hits her with the bottle of beer that he’s always holding, saying that she’s wasting money. Mommy cries sometimes. I don’t like it when Mommy cries. I wish I could recover what was taken from Mommy and make her happy.");
        diaryPages.Add("DiaryPage3", "Today, it’s been a year since Misty disappeared. I miss her so much. Mommy said she ran away because I didn’t feed her enough. I was hoping someone would find her and call the number in her collar, but she must have slipped out of it, because I found it in the garage, next to Daddy’s car’s tire. I thought it was weird because the collar was red and I always thought it was white. I even went to get the only picture I had of her to confirm, but I didn’t find it. I later saw Mommy in the kitchen ripping a picture apart and put it in the trash. When I went to the trash bin, I saw Misty’s picture. She must have loved Misty so much that she couldn’t bear seeing the picture, that must be why she ripped it… But I was really sad. If I can’t have Misty, I wish I at least had her picture, to remind me of her…");
        diaryPages.Add("DiaryPage4", "Today, I tried writing a story! It’s about a boy who is very lonely. One day, he went to the library and something magic happened! All books started flying towards him and the book’s characters came to life and started playing with the boy. He was no longer lonely! He made many friends who were all interesting and had plenty of adventures to tell! And he lived happily ever after! So nice, if magic exists I wish I could also go to a library and live there forever with the characters of my favorite books!");
        diaryPages.Add("DiaryPage5", "Today, my brother almost found the key to my diary. I need to be careful! This diary allows me to express myself. It contains all my feelings, my dreams, my wishes… I can’t let him find it! He would definitely mock me and throw it away! Or worse he could show it to Daddy! Oh no, that definitely cannot happen. Daddy would beat me if he found out about the things I wrote about him in the diary! I need a new hiding spot for the key! I used to keep it under my bed! Oh, I know! I’ll put it in my favorite book! No one other than me reads in this house. No one will ever find it!");
        diaryPages.Add("DiaryPage6", "Athy contacted me today. I’m going to play with her! I’m so happy! Athy is the only good thing in my life right now, I’m so glad she’s my friend! You know, life is hard. There are times when I feel suffocated. There are times when I want to give up everything. But knowing Athy is by my side is enough to make me want to hang on. I want to stay by her side. With her by my side, this world, that I find so unbearable, becomes beautiful. I can face everything. I want to grow up with her and always be there for her, like she is for me. Always");
    }

    public void OpenDiary()
    {
        if (canvas.activeInHierarchy) return; //Diary already opened

        //Active canvas
        canvas.gameObject.transform.parent.gameObject.SetActive(true);

        //Sound
        OpenDiarySound.start();

        //Load page
        GameObject page = Resources.Load<GameObject>("Diary/Page");

        //Delete previous page instances
        foreach (Transform child in canvas.transform.GetChild(0))
        {
            GameObject.Destroy(child.gameObject);
        }

        //Spawn pages
        for (int i = 0; i < pagesFound.Count; i++)
        {
            GameObject temp = Instantiate(page);
            temp.name = pagesFound[i];
            temp.GetComponent<Button>().onClick.AddListener(OpenPage);

            //Spawn inside grid without maintaining world position
            temp.transform.SetParent(canvas.transform.GetChild(0), false);
        }

    }

    public void AddPage(string name)
    {
        if (!pagesFound.Contains(name))
        {
            pagesFound.Add(name);
            this.OpenPageByName(name);
        }
        else Debug.LogWarning("Page already found!");
    }

    public void AddPageOnLoad(string name)
    {
        if (!pagesFound.Contains(name))
        {
            pagesFound.Add(name);
        }
        else Debug.LogWarning("Page already found!");
    }

    public void OpenPage()
    {
        Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>() as Button;

        //Activate panel
        GameObject panel = canvas.transform.GetChild(2).gameObject;

        panel.SetActive(true);

        if (GameController.instance.decisions["show_diary_page"] && panel.transform.childCount >= 3)
            panel.transform.GetChild(2).gameObject.SetActive(true);
        else if (panel.transform.childCount >= 3)
            panel.transform.GetChild(2).gameObject.SetActive(false);
        this.selectedPage = button.name;

        panel.GetComponentInChildren<TMP_Text>().text = diaryPages[button.name];
    }

    public void OpenPageByName(string name)
    {
        this.OpenDiary();
        GameObject panel = canvas.transform.GetChild(2).gameObject;

        panel.SetActive(true);
        this.selectedPage = name;

        panel.GetComponentInChildren<TMP_Text>().text = diaryPages[name];
    }

    public void UsePage()
    {
        canvas.transform.GetChild(2).gameObject.SetActive(false);
        canvas.transform.parent.gameObject.SetActive(false);
        if (this.selectedPage == "DiaryPage6")
        {
            if (GameObject.Find("Dooley").GetComponent<Dooley>().IsDooleyActive())
            {
                GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("DooleyRemembers");
            }
            else
            {
                GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("ShowDooley");
            }
        }
        else
        {
            GameObject.Find("DialogueRunner").GetComponent<YarnConfigurations>().RunDialogue("DontShowDooley");
        }
    }
}

