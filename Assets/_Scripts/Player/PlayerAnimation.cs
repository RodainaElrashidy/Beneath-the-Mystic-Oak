using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region Variables
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody2D _rigidBody2D;

    private PlatformCheck _platformCheck;
    private InputHandler _inputHandler;

    #endregion

    #region Methods

    void Start()
    {
        _platformCheck = ServiceLocator.Instance.GetService<PlatformCheck>();
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>();

        _inputHandler.OnPowerFire += FireSpellAnim;
    }

    private void Update()
    {
        _animator.SetFloat("xVelocity", Mathf.Abs(_rigidBody2D.velocity.x));
        _animator.SetFloat("yVelocity", _rigidBody2D.velocity.y);

        if (_platformCheck.CanJump())
        {
            _animator.SetBool("isJumping", false);
        }
        else if (!_platformCheck.CanJump())
        {
            _animator.SetBool("isJumping", true);
        }
    }

    private void FireSpellAnim()
    {
        _animator.SetTrigger("Attack");
    } 
    #endregion
}
