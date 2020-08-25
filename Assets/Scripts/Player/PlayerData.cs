using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField]
    public Dictionary<string, bool> decisions = new Dictionary<string, bool>();
    public float[] playerLastPos;
    [SerializeField]
    public List<string> inventoryItems = new List<string>();
    public int realityLevel;

    public PlayerData()
    {
        GameController controller = GameController.instance;

        decisions = controller.decisions;
        realityLevel = controller.RealityLevel;

        playerLastPos = new float[3];
        playerLastPos[0] = controller.playerLastPos.x;
        playerLastPos[1] = controller.playerLastPos.y;
        playerLastPos[2] = controller.playerLastPos.z;

        foreach (Item item in Inventory.instance.items)
        {
            inventoryItems.Add(item.name);
        }
    }
}
