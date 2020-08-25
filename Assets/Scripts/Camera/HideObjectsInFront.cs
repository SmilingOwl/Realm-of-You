using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjectsInFront : MonoBehaviour
{
    private Transform target;
    private RaycastHit[] hits = null;
    private Vector3 offset = new Vector3(0, 2, 0);

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

        if (hits != null)
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null) {
                    GameObject obj = hit.collider.gameObject;
                    if (obj.tag == "Decoration")
                        if (obj.GetComponent<ObjectTransparency>() != null)
                            obj.GetComponent<ObjectTransparency>().ShowTreeTop();
                }
            }

        HideObjects();
    }

    private void HideObjects()
    {
        hits = Physics.RaycastAll(target.position, (this.transform.position - target.position - Vector3.up), Vector3.Distance(target.position, (this.transform.position)));

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null) {
                GameObject obj = hit.collider.gameObject;
                if (obj.tag == "Decoration")
                    if (obj.GetComponent<ObjectTransparency>() != null)
                        obj.GetComponent<ObjectTransparency>().HideTreeTop();
            }
        }
    }
}
