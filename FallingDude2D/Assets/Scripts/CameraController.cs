using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed;
    private float currentPosY;


    private void Update()
    {
        transform.position = new Vector3(transform.position.x, currentPosY, transform.position.z);
    }

    public void MoveToNextLevel(Transform nextLevel)
    {
        Debug.Log(nextLevel.position.y);
        currentPosY = nextLevel.position.y;
    }
}