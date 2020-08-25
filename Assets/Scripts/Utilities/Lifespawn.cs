using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifespawn : MonoBehaviour
{
    public float lifeSpawn; //Destroy after x seconds
    public bool dontDestroy; //If selected, the object will just became inactive. Good for pooled objects
    private float counter;
    private bool onScreen = false;
    public bool OnScreen
    {
        get
        {
            return onScreen;
        }
        set
        {
            onScreen = value;
        }
    }

    // Called after script is active
    void OnEnable()
    {
        if (dontDestroy)
            counter = lifeSpawn;
        else Destroy(gameObject, lifeSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (dontDestroy)
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                resetRB();
                gameObject.SetActive(false);
            }
        }
    }

    void OnBecameVisible()
    {
        onScreen = true;
    }

    private void resetRB()
    {
        Rigidbody rg = gameObject.GetComponent<Rigidbody>();
        rg.velocity = Vector3.zero;
        rg.angularVelocity = Vector3.zero;
    }
}
