using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject childText = null; //  or make public and drag

    void Start()
    {
        childText = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        if (childText.transform.GetChild(0).transform.GetComponent<TMP_Text>() != null)
            childText.transform.GetChild(0).transform.GetComponent<TMP_Text>().text = gameObject.name;

        childText.SetActive(false);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        childText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        childText.SetActive(false);
    }
}