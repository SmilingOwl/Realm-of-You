using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public Transform itemsParent;
    public GameObject inventoryUI;
    public GameObject slotPrefab;
    public GameObject fullPrompt;
    List<GameObject> slots = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        this.InstantiateSlots();
        this.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory")){
           inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
        if (this.slots.Count < inventory.space) {
            this.InstantiateSlots();
        }
    }

    void UpdateUI()
    {
        for(int i = 0; i < this.slots.Count; i++){
            if (i < inventory.items.Count){
                this.slots[i].GetComponent<InventorySlot>().AddItem(inventory.items[i]);
            } else {
                this.slots[i].GetComponent<InventorySlot>().ClearSlot();
            }
        }
    }

    void InstantiateSlots() {
        for(int i = this.slots.Count; i < inventory.space; i++){
            GameObject slot = Instantiate(slotPrefab) as GameObject;
            slot.transform.SetParent(itemsParent, true);
            slot.transform.localPosition = new Vector3(1, 1, 0);
            slot.transform.localScale = new Vector3(1, 1, 1);
            this.slots.Add(slot);
        }
    }

    void OnDestroy() {
        Inventory.instance.onItemChangedCallback -= UpdateUI;
    }

    public void AddPlaceholder () {
        if (Inventory.instance.items.Count >= Inventory.instance.space)
        {
            string prompt_text = "You need to drop an item so you can save this one!\n\n" + 
                "You can select DROP to drop the new item or you can drop an item from the inventory and then press SAVE!\n\n" +
                "What will you do?";
            fullPrompt.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = prompt_text;
        }
        else 
        {
            Inventory.instance.Add(Inventory.instance.placeholder);
            fullPrompt.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void ClearPlaceholder() {
        Inventory.instance.placeholder = null;
        fullPrompt.SetActive(false);
        string prompt_text = "Your backpack is full!\n\n" + 
                "You can select DROP to drop the new item or you can drop an item from the inventory and then press SAVE!\n\n" +
                "What will you do?";
        fullPrompt.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = prompt_text;
        Time.timeScale = 1f;
    }
}
