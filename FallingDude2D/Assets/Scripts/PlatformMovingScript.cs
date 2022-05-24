using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovingScript : MonoBehaviour
{
    [SerializeField] private float distanceLeft;
    [SerializeField] private float distanceRight;
    [SerializeField] private float speed;
    private bool movingRight = true;

    public GameObject Player;
    private void Update()
    {
        transform.position = movingRight
            ? new Vector3(transform.position.x + speed, transform.position.y, transform.position.z)
            : new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);

        if (movingRight && distanceRight < transform.position.x)
        {
            movingRight = false;
        }
        else if (!movingRight && distanceLeft > transform.position.x)
        {
            movingRight = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = transform;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;
        }
    }
}