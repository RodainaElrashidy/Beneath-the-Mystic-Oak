using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCheck : MonoBehaviour
{
    #region Variables

    private InputHandler _inputHandler;
    public static PlatformCheck instance;

    [Header("Coyote Jump")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;

    private float coyoteCounter;

    #endregion

    #region Methods

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            ServiceLocator.Instance.RegisterService(this);
        }
    }

    void Start()
    {
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>();
    }

    void Update()
    {
        if (IsGrounded())
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public bool CanJump()
    {
        return (IsGrounded() || coyoteCounter > 0);
    }

    #endregion

}
