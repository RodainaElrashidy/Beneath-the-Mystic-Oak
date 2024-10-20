using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePlatform : MonoBehaviour, IInteractable
{

    //Will be changed but same function depends on how i'll make the platforms function shader or tiles created
    [SerializeField] float increaseAmount;

    private Vector3 ogPosition;
    private Vector3 ogScale;
    private bool isActivated = false;

    private void Start()
    {
        ogPosition = gameObject.transform.position;
        ogScale = gameObject.transform.localScale;
    }

    public void Interact(string spellTag)
    {
        if(spellTag == "ForestPower")
        {
            ForestActivated();
        }
        else if(spellTag == "NightPower")
        {
            NightActivated();
        }
    }

    //vertical
    private void ForestActivated()
    {
        if(!isActivated)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y + increaseAmount, gameObject.transform.localScale.z);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (increaseAmount /2), gameObject.transform.position.z);
            
            isActivated = true;
        }
        else if(isActivated)
        {
            gameObject.transform.position = ogPosition;
            gameObject.transform.localScale = ogScale;
            isActivated = false;
        }
    }
    
    //horizontal
    private void NightActivated()
    {
        if (!isActivated)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + (increaseAmount / 2), gameObject.transform.position.y, gameObject.transform.position.z);
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + increaseAmount, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            isActivated = true;
        }
        else if (isActivated)
        {
            gameObject.transform.position = ogPosition;
            gameObject.transform.localScale = ogScale;
            isActivated = false;
        }
       
    }
}
