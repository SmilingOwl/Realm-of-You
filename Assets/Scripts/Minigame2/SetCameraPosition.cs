using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraPosition : MonoBehaviour
{
    private Transform target;

    #region CameraSettings
    public Vector3 offset;
    public float pitch = 2f; //target's height
    #endregion

    #region ZoomVars
    private float currentZoom = 10f;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    #endregion

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);
    }

    void Update()
    {
        //Zoom in/out with mouse wheel
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

}
