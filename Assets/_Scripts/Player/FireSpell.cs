using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpell : MonoBehaviour
{
    #region Variables
    [SerializeField] Transform spellPosition;

    private InputHandler _inputHandler;
    private SwitchPower _switchPower;
    private SpellsPool _spellsPool;
    #endregion

    #region Methods

    void Start()
    {
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>();
        _switchPower = ServiceLocator.Instance.GetService<SwitchPower>();
        _spellsPool = ServiceLocator.Instance.GetService<SpellsPool>();

        _inputHandler.OnPowerFire += Fire;
    }

    private void Fire()
    {
        GameObject SpellPrefab = _spellsPool.GetSpellPrefab();

        if (SpellPrefab != null)
        {
            SpellPrefab.transform.SetPositionAndRotation(spellPosition.position, spellPosition.rotation);
            SpellPrefab.tag = _switchPower.CurrentPower;
            SpellPrefab.SetActive(true);
        }

    } 
    #endregion
}
