using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Interactable : MonoBehaviour
{
    public Item item;
    protected bool isActive;
    protected bool hasInteracted;

    #region Sound
    [FMODUnity.EventRef]
    public string InteractEvent;
    protected FMOD.Studio.EventInstance InteractSound;
    #endregion

    private void Awake()
    {
        InteractSound = FMODUnity.RuntimeManager.CreateInstance(InteractEvent);
    }

    public virtual void Start()
    {
        isActive = false;
        hasInteracted = false;
    }

    public void SetHasInteracted()
    {
        this.hasInteracted = true;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !this.hasInteracted)
        {
            isActive = true;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isActive = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (this.isActive)
        {
            this.Interact();
        }
    }

    public virtual void PickUp()
    {
        bool wasPickedUp = Inventory.instance.Add(this.item);
        if (wasPickedUp)
        {
            Destroy(this.gameObject);
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    public virtual void Interact()
    {

    }
}
