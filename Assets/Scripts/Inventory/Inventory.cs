using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            GameObject.Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;

        if (NewItem != null && NewItem != "")
        {
            NewItemSound = FMODUnity.RuntimeManager.CreateInstance(NewItem);
        }
    }

    void Start()
    {
        if (GameController.instance.pendingItems.Count != 0)
        {
            foreach (string item in GameController.instance.pendingItems)
            {
                Item newItem = Resources.Load<Item>("Items/" + item);
                AddOnLoad(newItem);

                if (item == "Diary")
                    LoadDiaryPages();
            }

            GameController.instance.pendingItems.Clear();
        }
    }

    private void LoadDiaryPages()
    {
        GameController gc = GameController.instance;

        if (gc.decisions["played_minigame1"])
            Diary.instance.AddPageOnLoad("DiaryPage1");
        if (gc.decisions["played_minigame2"])
            Diary.instance.AddPageOnLoad("DiaryPage2");
        if (gc.decisions["played_minigame3"])
        {
            Diary.instance.AddPageOnLoad("DiaryPage3");
            Diary.instance.AddPageOnLoad("DiaryPage4");
        }
        if (gc.decisions["played_minigame4"])
            Diary.instance.AddPageOnLoad("DiaryPage5");
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public int space = 6;
    public List<Item> items = new List<Item>();

    public Item placeholder;

    #region Sound
    [FMODUnity.EventRef]
    public string NewItem;
    protected FMOD.Studio.EventInstance NewItemSound;
    #endregion

    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            GameObject.Find("UI Canvas").GetComponent<InventoryUI>().fullPrompt.SetActive(true);
            Time.timeScale = 0f;
            placeholder = item;
            return false;
        }

        items.Add(item);

        NewItemSound.start();
        NewItemSound.setVolume(5f);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        return true;
    }

    public void Remove(Item item)
    {
        item.Remove();
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public bool AddOnLoad(Item item)
    {
        items.Add(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        return true;
    }

    public void ResetInventory()
    {
        items.Clear();
    }

    public void setSpace(int newSpace)
    {
        space = newSpace;
    }
}
