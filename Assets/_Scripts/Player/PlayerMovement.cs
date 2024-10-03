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

    [Header("Dash")]
    [SerializeField] private float dashPower = 24f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private InputHandler _inputHandler;
    private PlatformCheck _platformCheck;

    private float horizontalInput;
    private float speed = 0f;
    private float jumpBufferCounter;

    private bool isFacingRight = true;
    private bool canDash = true;
    private bool isDashing = false;

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
        if (isDashing)
        {
            return;
        }

        _inputHandler.HandleMovement();

        ApplyMovement();

        if (jumpBufferCounter > 0 && _platformCheck.canJump())
        {
            ApplyJump();
        }

        if (_inputHandler.JumpTriggered)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (_inputHandler.DashTriggered && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public void ApplyMovement()
    {
        speed = baseSpeed;
        _rigidbody2D.velocity = new Vector2(horizontalInput * speed, _rigidbody2D.velocity.y);
    }

    public void ApplyJump()
    {
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

        float ogGravity = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = 0f;

        _rigidbody2D.velocity = new Vector2(transform.localScale.x * dashPower, 0f);

        yield return new WaitForSeconds(dashTime);

        _rigidbody2D.gravityScale = ogGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    #endregion

}
