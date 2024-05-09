using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
  
public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float minCamSize = 2f;
    [SerializeField] private float maxCamSize = 5f;

    public GameObject test;

    private void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        // Get MouseWheel-Value and calculate new Orthographic-Size
        // (while using Zoom-Speed-Multiplier)
        float mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        float newZoomLevel = cam.orthographicSize - mouseScrollWheel;

        // Get Position before and after zooming
        Vector3 mouseOnWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        cam.orthographicSize = Mathf.Clamp(newZoomLevel, minCamSize, maxCamSize);
        Vector3 mouseOnWorld1 = cam.ScreenToWorldPoint(Input.mousePosition);

        Debug.Log(mouseOnWorld+ "  " +mouseOnWorld1);

        // Calculate Difference between Positions before and after Zooming
        Vector3 posDiff = mouseOnWorld - mouseOnWorld1;

        // Add Difference to Camera Position
        Vector3 camPos = cam.transform.position;
        Vector3 targetPos = new Vector3(
            camPos.x + posDiff.x,
            camPos.y + posDiff.y,
            camPos.z);

        // Apply Target-Position to Camera
        cam.transform.position = targetPos;


        test.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
