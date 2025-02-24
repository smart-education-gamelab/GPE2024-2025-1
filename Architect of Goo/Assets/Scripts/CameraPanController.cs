using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Ruben Verheul
/// 14/01/2024
/// This script handles the camera panning based on the grappling points.
/// Link to documentation: N.A.
/// </summary> 
public class CameraPanController : MonoBehaviour
{
    public static CameraPanController Instance { get; private set; }
    public Transform Player;
    public float PanSpeed = 2f;
    public float DefaultSize = 5f;

    private Camera _camera;
    private Transform _currentGrapplePoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        AdjustCameraSize();
    }

    public void EnterSolutionZone(Transform grapplePoint)
    {
        _currentGrapplePoint = grapplePoint;
    }

    public void ExitSolutionZone()
    {
        _currentGrapplePoint = null;
    }

    private void AdjustCameraSize()
    {
        if (_currentGrapplePoint != null)
        {
            float targetSize = CalculateSize(_currentGrapplePoint);
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetSize, Time.deltaTime * PanSpeed);
        }
        else
        {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, DefaultSize, Time.deltaTime * PanSpeed);
        }
    }

    private float CalculateSize(Transform grapplePoint)
    {
        Vector3 playerPos = Player.position;
        Vector3 grapplePos = grapplePoint.position;

        float verticalDistance = Mathf.Abs(grapplePos.y - playerPos.y);
        float horizontalDistance = Mathf.Abs(grapplePos.x - playerPos.x);
        float buffer = 1.75f;

        float sizeByHeight = verticalDistance / 2f * buffer;
        float sizeByWidth = (horizontalDistance / _camera.aspect) / 2f * buffer;

        Debug.Log("height = " + sizeByHeight);
        Debug.Log("width = " + sizeByWidth);
        return DefaultSize + Mathf.Max(sizeByHeight, sizeByWidth);
    }
}
