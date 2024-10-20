using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Variables

    [Header("Movement")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float baseSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float fallForce = 10;

    [Header("Dash")]
    [SerializeField] private float dashPower = 24f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] SpriteRenderer _spriteRenderer;

    private InputHandler _inputHandler;
    private PlatformCheck _platformCheck;

    private float horizontalInput;
    private float jumpBufferCounter;

    private bool isFacingRight = true;
    private bool canDash = true;
    private bool isDashing = false;
    private bool isJumping = false;
    private bool canJumpAfterDash = false;

    public bool IsJumping { get => isJumping;}

    #endregion

    #region Methods

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
    }

    void Start()
    {
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>();
        _platformCheck = ServiceLocator.Instance.GetService<PlatformCheck>();
    }

    private void Update()
    {
        horizontalInput = _inputHandler.HorizontalInput;

        if (horizontalInput != 0)
        {
            FlipSprite(horizontalInput);
        }
    }

    private void FixedUpdate()
    {
        //no action allowed while dashing
        if (isDashing)
        {
            return;
        }

        _inputHandler.HandleMovement();

        ApplyMovement();

        // Apply jump when jump buffer is active and player can jump
        //or when jump is triggered whithout jumpBuffer and just finished a dash
        if ((jumpBufferCounter > 0 && _platformCheck.CanJump()) || (_inputHandler.JumpTriggered && canJumpAfterDash))
        {
            ApplyJump();
        }

        // Apply fall force when the jump is released early 
        if (!_inputHandler.JumpTriggered && isJumping && _rigidbody2D.velocity.y > 0f)
        {
            _rigidbody2D.AddForce(Vector2.down * fallForce, ForceMode2D.Force);
        }

        // Update jump buffer counter
        if (_inputHandler.JumpTriggered)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        //Handle Dash
        if (_inputHandler.DashTriggered && canDash)
        {
            StartCoroutine(Dash());
        }

        if (_platformCheck.CanJump() && isJumping)
        {
            isJumping = false;  // Reset isJumping when player lands
        }
    }

    public void ApplyMovement()
    {
        _rigidbody2D.velocity = new Vector2(horizontalInput * baseSpeed, _rigidbody2D.velocity.y);
    }

    public void ApplyJump()
    {
        isJumping = true;

        //_rigidbody2D.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpHeight);

        jumpBufferCounter = 0f;
    }

    private void FlipSprite(float horizontalInput)
    {
        if ((isFacingRight && horizontalInput < 0) || (!isFacingRight && horizontalInput > 0))
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    #endregion

    #region CoRoutines

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        _particleSystem.Play();
        _spriteRenderer.enabled = false;

        float ogGravity = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0f;

        _rigidbody2D.velocity = new Vector2(transform.localScale.x * dashPower, 0f);

        yield return new WaitForSeconds(dashTime);

        _rigidbody2D.gravityScale = ogGravity;
        isDashing = false;
        _particleSystem.Stop();
        _spriteRenderer.enabled = true;
        canJumpAfterDash = true;

        yield return new WaitForSeconds(dashCooldown / 4);

        canJumpAfterDash = false; 

        yield return new WaitForSeconds(dashCooldown - (dashCooldown / 4));

        canDash = true;
    }

    #endregion

}
