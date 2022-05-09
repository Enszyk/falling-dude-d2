using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelCameraPortal : MonoBehaviour
{
    [SerializeField] private Transform nextLevel;

    [SerializeField] private CameraController cam;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            cam.MoveToNextLevel(nextLevel);
        }
    }
}