using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New item"; //Name of the icon
    public Sprite icon = null; //Item's icon that will appear in inventory slot

    public virtual void Use()
    {
        if (name == "Diary")
            Diary.instance.OpenDiary();
        Debug.Log("Using" + name);
    }

    public void Remove()
    {
        Debug.Log(name);
        switch (name) {
            case "TeddyBear":
                GameController.instance.TakeDamage(10);
                break;
            case "HairRibbon":
                GameController.instance.decisions["pickup_ribbon"] = false;
                GameController.instance.TakeDamage(10);
                break;
            case "MomPhoto":
                GameController.instance.decisions["pickup_mom_photo"] = false;
                GameController.instance.TakeDamage(10);
                break;
        }
    }
}
