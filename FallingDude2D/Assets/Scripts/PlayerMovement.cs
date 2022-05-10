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
    private Animator _anim;

    private bool _isMoveRight = false;
    private bool _isMoveLeft = false;
    private bool _isJumpLoading = false;
    private bool _lastMoveRight = false;
    private float _jumpLoadingTime = 0f;
    private bool duringJump = false; 
    private static readonly int Run = Animator.StringToHash("run");


    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
    ;
        if (_lastMoveRight)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (isMidAir())
        {
            if (_body.velocity.y < 0 && duringJump)
            {
                _anim.SetBool("jumpingDown", true);
            }
            return;
        }

        if (_anim.GetBool("jumpingDown"))
        {
            _anim.SetBool("jumpingDown", false);
        }

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
        _anim.SetBool(Run, !isMidAir());
        _lastMoveRight = true;
        _isMoveLeft = false;
        _isMoveRight = true;
    }

    public void GoLeft()
    {
        _anim.SetBool(Run, !isMidAir());
        _lastMoveRight = false;
        _isMoveLeft = true;
        _isMoveRight = false;
    }

    public void StopMovement()
    {
        _anim.SetBool(Run, false);
        _isMoveLeft = false;
        _isMoveRight = false;
    }

    public void StartloadingJump()
    {
        
        _isJumpLoading = true;
        _anim.SetBool("isLoadingJump", true);
    }

    public void ReleaseJump()
    {
        duringJump = true;
        _anim.SetBool("isLoadingJump", false);
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
        return Math.Abs(_body.velocity.y) > 0.001f;
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