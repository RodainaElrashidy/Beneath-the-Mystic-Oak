using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    #region Variables

    private InputHandler _inputHandler;
    private PlayerMovement _playerMovement;

    #endregion

    #region Methods

    void Start()
    {
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>();
        _playerMovement = ServiceLocator.Instance.GetService<PlayerMovement>();

        _inputHandler.OnPauseStateChanged += ToggleGameState;
    }

    void Update()
    {
        //create a load manager and replace the below
        if (SceneManager.GetActiveScene().buildIndex == 0)
            ToggleGameState(true);
    }

    private void ToggleGameState(bool isPaused)
    {
        if (isPaused)
        {
            _inputHandler.RegisterUIMapInputActions();
        }
        else
        {
            _inputHandler.RegisterBasicMovementInputActions();
        }

        _playerMovement.enabled = !isPaused;
        Debug.Log("toggled" + isPaused);
    }

    private void OnDestroy()
    {
        _inputHandler.OnPauseStateChanged -= ToggleGameState;
    }

    #endregion

}
