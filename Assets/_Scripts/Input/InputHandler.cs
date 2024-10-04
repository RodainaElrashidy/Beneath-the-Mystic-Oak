using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    #region Variables

    public static InputHandler instance;

    /*[SerializeField] */private PlayerControls playerControls;

    private InputActionMap basicMovement;
    private InputActionMap uiMap;

    private Vector2 movementInput;

    public event Action<bool> OnPauseStateChanged;

    #endregion

    #region Properties

    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }
    public bool CanMove { get; set; }
    public bool JumpTriggered { get; private set; }
    public bool DashTriggered { get; private set; }
    public bool IsJumping { get; set; }
    public bool PlayerInteracted { get; set; }
    public bool GreenPower { get; set; }
    public bool BluePower { get; set; }
    public bool IsPaused { get; set; }

    #endregion

    #region Methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ServiceLocator.Instance.RegisterService(this);
        }

        playerControls = new PlayerControls();
        basicMovement = playerControls.BasicMap;
        uiMap = playerControls.UI;
    }

    private void OnEnable()
    {
        playerControls.Enable();
        //RegisterInputActions();
        RegisterBasicMovementInputActions();
    }

    void Start()
    {
        CanMove = true;
        JumpTriggered = false;
        IsJumping = false;
        PlayerInteracted = false;

        GreenPower = true;
        BluePower = false;

        IsPaused = false;
    }

    //Method that handles subscribing event handlers to input actions.
    //private void RegisterInputActions()
    //{
    //    //basicMovement.
    //    playerControls.BasicMap.Movement.performed += value => movementInput = value.ReadValue<Vector2>();
    //    playerControls.BasicMap.Movement.canceled += value => movementInput = Vector2.zero;

    //    playerControls.BasicMap.Jump.performed += context => JumpTriggered = true;
    //    playerControls.BasicMap.Jump.canceled += context => JumpTriggered = false;

    //    playerControls.BasicMap.Sprint.performed += value => DashTriggered = true;
    //    playerControls.BasicMap.Sprint.canceled += value => DashTriggered = false;

    //    playerControls.BasicMap.PowerSwitch.performed += context => SwitchPower();

    //    playerControls.BasicMap.Interact.performed += context => Interact();
    //    playerControls.BasicMap.Interact.canceled += context => CancelInteract();

    //    playerControls.BasicMap.Pause.performed += context => OnPauseStateChanged?.Invoke(true);
    //    //problem bc both maps are enabled pause is triggered twice solve it
    //    playerControls.UI.Resume.performed += context => OnPauseStateChanged?.Invoke(false);

    //}


    //make them public or have a method to handle that?
    
    public void RegisterBasicMovementInputActions()
    {

        basicMovement.Enable();
        uiMap.Disable();

        playerControls.BasicMap.Movement.performed += value => movementInput = value.ReadValue<Vector2>();
        playerControls.BasicMap.Movement.canceled += value => movementInput = Vector2.zero;

        playerControls.BasicMap.Jump.performed += context => JumpTriggered = true;
        playerControls.BasicMap.Jump.canceled += context => JumpTriggered = false;

        playerControls.BasicMap.Sprint.performed += value => DashTriggered = true;
        playerControls.BasicMap.Sprint.canceled += value => DashTriggered = false;

        playerControls.BasicMap.PowerSwitch.performed += context => SwitchPower();

        playerControls.BasicMap.Interact.performed += context => Interact();
        playerControls.BasicMap.Interact.canceled += context => CancelInteract();

        playerControls.BasicMap.Pause.performed += context => OnPauseStateChanged?.Invoke(true);

    }

    public void RegisterUIMapInputActions()
    {

        basicMovement.Disable();
        uiMap.Enable();

        playerControls.UI.Resume.performed += context => OnPauseStateChanged?.Invoke(false);

    }


    public void HandleMovement()
    {
        HorizontalInput = movementInput.x;
        VerticalInput = movementInput.y;
    }

    private void Interact()
    {
        PlayerInteracted = true;
    }

    private void CancelInteract()
    {
        PlayerInteracted = false;
    }

    private void SwitchPower()
    {
        GreenPower = !GreenPower;
        BluePower = !BluePower;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        //Not sure if below needed
        // Unsubscribe from events if necessary
        //playerControls.BasicMap.Movement.performed -= value => movementInput = value.ReadValue<Vector2>();
        //playerControls.BasicMap.Jump.performed -= context => JumpTriggered = true;
    }

    #endregion

}
