using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransparency : MonoBehaviour
{
    public Material translucid;
    private Dictionary<string, Material> originalMaterials = new Dictionary<string, Material>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            originalMaterials.Add(child.name, child.GetComponent<Renderer>().material);
        }

    }

    public void HideTreeTop()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().material = translucid;
        }
    }

    public void ShowTreeTop()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().material = originalMaterials[child.name];
        }
    }
}
