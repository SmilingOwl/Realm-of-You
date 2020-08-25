using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class pieces : MonoBehaviour
{
    private Vector3 RightPosition;
    public bool InRightPosition;
    public bool Selected;

    void Start()
    {
        InRightPosition = false;
        RightPosition = transform.position;
        transform.position = new Vector3(Random.Range(9f,16f),Random.Range(-6f,2f));
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, RightPosition) < 0.6f){
            if(!Selected){
                if(!InRightPosition){
                    transform.position = RightPosition;
                    InRightPosition = true;
                    GetComponent<SortingGroup>().sortingOrder = 0;
                    Camera.main.GetComponent<DragAndDrop>().PlacedPieces++;
                }
                
            }
        }
    }
}
