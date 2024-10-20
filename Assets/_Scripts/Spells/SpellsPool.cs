using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsPool : MonoBehaviour
{
    #region Variables
    public static SpellsPool instance;

    [SerializeField] GameObject spellPrefab;
    [SerializeField] int spellPrefabCount = 20;
    [SerializeField] bool enableExpandPool = true;
    [SerializeField] int expandPool = 3;

    private List<GameObject> pooledSpells = new List<GameObject>();

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

        for (int i = 0; i < spellPrefabCount; i++)
        {
            GameObject obj = Instantiate(spellPrefab);
            obj.SetActive(false);
            pooledSpells.Add(obj);
        }
    }

    public GameObject GetSpellPrefab()
    {
        for (int i = 0; i < pooledSpells.Count; i++)
        {
            if (!pooledSpells[i].activeInHierarchy)
            {
                return pooledSpells[i];
            }
        }

        if (enableExpandPool)
        {
            ExpandPool();
            return pooledSpells[pooledSpells.Count - 1];
        }

        return null;
    }

    private void ExpandPool()
    {
        for (int i = 0; i < expandPool; i++)
        {
            GameObject obj = Instantiate(spellPrefab);
            obj.SetActive(false);
            pooledSpells.Add(obj);
        }
    }

    #endregion
}
