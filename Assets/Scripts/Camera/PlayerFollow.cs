using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    private Transform target;
    public Vector3 offset;
    [Range(0.01f, 1f)]
    public float SmoothFactor = 0.5f;

    #region ZoomVars
    private float currentZoom = 10f;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    #endregion


    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //Zoom in/out with mouse wheel
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }


    private void LateUpdate()
    {
        Vector3 newPos = target.position + offset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
    }

}