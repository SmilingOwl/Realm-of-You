using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    public static PlayerSave instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            GameObject.Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void SavePlayer()
    {
        Debug.Log("Saving...");
        SaveSystem.SavePlayer();
    }

    public void LoadPlayer()
    {
        Debug.Log("Loading...");
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            return;
        }
        GameController gc = GameController.instance;

        //Load decisions and reality lvl
        gc.decisions = data.decisions;
        gc.RealityLevel = data.realityLevel;

        //Load position
        Vector3 pos;
        pos.x = data.playerLastPos[0];
        pos.y = data.playerLastPos[1];
        pos.z = data.playerLastPos[2];

        gc.playerLastPos = pos;

        //Load inventory items
        if (Inventory.instance == null)
            gc.pendingItems.AddRange(data.inventoryItems);
        else
            foreach (string item in data.inventoryItems)
            {
                Item newItem = Resources.Load<Item>("Items/" + item);
                Inventory.instance.AddOnLoad(newItem);
            }

        GameController.instance.changeState("exploration");
        SoundConfig.instance.IncrementRichness(2.0f);
    }

}
