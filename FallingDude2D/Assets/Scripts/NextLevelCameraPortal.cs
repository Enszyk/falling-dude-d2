using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelCameraPortal : MonoBehaviour
{
    [SerializeField] private Transform nextLevel;
    [SerializeField] private Transform previousLevel;

    [SerializeField] private CameraController cam;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (col.transform.position.y < transform.position.y)
        {
            cam.MoveToNextLevel(nextLevel);
        }
        else
        {
            cam.MoveToNextLevel(previousLevel);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        
        if (col.transform.position.y < transform.position.y)
        {
            cam.MoveToNextLevel(previousLevel);
        }
        // else
        // {
        //     cam.MoveToNextLevel(previousLevel);
        // }
    }
}