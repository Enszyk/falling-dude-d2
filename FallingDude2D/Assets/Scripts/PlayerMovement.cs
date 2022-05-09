using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedIncrementPerTick;
    [SerializeField] private float maxJumpPower;
    [SerializeField] private float minJumpPower;
    [SerializeField] private float bounceForce;
    [SerializeField] private float speedWhenJumping;
    private Rigidbody2D _body;
    private BoxCollider2D _boxCollider2D;

    private bool _isMoveRight = false;
    private bool _isMoveLeft = false;
    private bool _isJumpLoading = false;
    private float _jumpLoadingTime = 0f;


    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {

        if (_isMoveRight)
        {
            transform.localScale = Vector3.one;
        }
        else if (_isMoveLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (isMidAir()) return;
        
        if (_isJumpLoading)
        {
            _jumpLoadingTime += Time.deltaTime;
        }

        if (_isMoveLeft && !_isJumpLoading)
        {
            if (_body.velocity.x >= -maxSpeed)
            {
                _body.velocity = new Vector2(_body.velocity.x - speedIncrementPerTick, _body.velocity.y);
            }
        }

        if (_isMoveRight && !_isJumpLoading)
        {
            if (_body.velocity.x <= maxSpeed)
            {
                _body.velocity = new Vector2(_body.velocity.x + speedIncrementPerTick, _body.velocity.y);
            }
        }
    }

    public void GoRight()
    {
        _isMoveLeft = false;
        _isMoveRight = true;
    }

    public void GoLeft()
    {
        _isMoveLeft = true;
        _isMoveRight = false;
    }

    public void StopMovement()
    {
        _isMoveLeft = false;
        _isMoveRight = false;
    }

    public void StartloadingJump()
    {
        
        _isJumpLoading = true;
    }

    public void ReleaseJump()
    {
        _isJumpLoading = false;
        if (!isMidAir())
        {
            _body.velocity = new Vector2(GetSpeedWhenJumping(), GetJumpPower());
        }
        _jumpLoadingTime = 0f;
    }

    private float GetJumpPower()
    {
        var power = minJumpPower + _jumpLoadingTime * 4;
        if (power > maxJumpPower) return maxJumpPower;
        return power;
    }

    private float GetSpeedWhenJumping()
    {
        if (_isMoveLeft) return -speedWhenJumping;
        if (_isMoveRight) return speedWhenJumping;
        return 0f;
    }

    private bool isMidAir()
    {
        // return false;
        return _body.velocity.y > 0.0000001f || _body.velocity.y < -0.0000001f;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var relativePosition = col.GetContact(0);

        var pos = relativePosition.normal[0];
        if (!isMidAir()) return;
        
        if (Mathf.Approximately(pos, -1))
        {
            _body.velocity = new Vector2(-bounceForce, _body.velocity.y);
        }
        else if (Mathf.Approximately(pos, 1))
        {
            _body.velocity = new Vector2(bounceForce, _body.velocity.y);
        }
    }
}