using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTrail : MonoBehaviour
{
    #region Methods

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
    }

    public void HandleSpellTrail(GameObject _spellTrail, Transform _spell)
    {
        StartCoroutine(DisableAndReattach(_spellTrail, _spell));
    }
    #endregion

    #region CoRoutines

    private IEnumerator DisableAndReattach(GameObject _spellTrail, Transform _spell)
    {
        yield return new WaitForSeconds(0.17f);

        _spellTrail.gameObject.SetActive(false);

        _spellTrail.transform.SetParent(_spell);
        _spellTrail.transform.position = Vector3.zero;
        _spellTrail.transform.position = _spell.position;
    } 
    #endregion
}
