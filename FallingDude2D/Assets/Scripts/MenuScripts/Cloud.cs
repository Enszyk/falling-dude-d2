using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float cloudSpeed;
    private BoxCollider2D boxCollider;
    private float direction;
    private bool hit;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        gameObject.SetActive(false);

    }

    private void FixedUpdate()
    {
        if (hit) return;
        float movementSpeed = cloudSpeed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "CloudsDestroyer")
            Destroy(gameObject);
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
    }
}
