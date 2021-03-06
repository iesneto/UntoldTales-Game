﻿using UnityEngine;
using System.Collections;

/// <summary>
/// WORLD SPACE CANVAS FACES PLAYER AT ALL TIMES
/// </summary>
public class CameraFacingBillboard : MonoBehaviour
{
    private Camera m_Camera;

    void Start()
    {
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        
        if(cam != null) m_Camera = cam.GetComponent<Camera>();
    }

    void Update()
    {
        if(m_Camera != null)
            transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
        else m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
}