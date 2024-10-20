using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPower : MonoBehaviour
{
    #region Variables
    public static SwitchPower instance;
    private InputHandler _inputHandler;

    //it should be two materials/colors from a serializedfield 
    //one private var of the same like above that becomes one of the above depending on the below
    //one boolean that its value changes by the OnPowerSwitch Action
    //When E/Interact is activated we the result by using the current material/color

    //the following must be the same as the predeifned tags to assign to the spell
    [SerializeField] private string forestPowerTag = "ForestPower";
    [SerializeField] private string NightPowerTag = "NightPower";

    private string currentPower;
    private bool isCurrentPowerDefauult = true; //Default power is Forest 
    #endregion

    #region Properties

    public string CurrentPower { get => currentPower; }

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

    // Start is called before the first frame update
    void Start()
    {
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>();
        _inputHandler.OnPowerSwitched += SwitchCurrentPower;

        currentPower = forestPowerTag;
    }

    private void SwitchCurrentPower()
    {
        if (isCurrentPowerDefauult)
        {
            currentPower = NightPowerTag;
            isCurrentPowerDefauult = false;
        }
        else
        {
            currentPower = forestPowerTag;
            isCurrentPowerDefauult = true;
        }

    } 
    #endregion

}
