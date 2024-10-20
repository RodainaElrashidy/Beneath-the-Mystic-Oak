using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    public void Interact(string tag);
}

public class Spell : MonoBehaviour
{

    #region Variables
    [Header("Spell Body")]
    [Tooltip("The main body")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float speed = 20f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Spell Trail")]
    [Tooltip("Particle System")]
    [SerializeField] ParticleSystem _particleSystem;

    private Vector2 direction;
    private PlayerMovement _playerMovement;
    private SpellTrail _spellTrail;
    #endregion

    #region Methods
    private void OnEnable()
    {
        if (_playerMovement != null)
        {
            direction = new Vector2(_playerMovement.transform.localScale.x, 0);
            Debug.Log(direction);
            StartCoroutine(DisableSpellIfNotHit());
        }
        _particleSystem.gameObject.transform.position = transform.position;
        SpellAesthetic();
        _particleSystem.gameObject.SetActive(true);
    }

    private void Start()
    {
        _playerMovement = ServiceLocator.Instance.GetService<PlayerMovement>();
        _spellTrail = ServiceLocator.Instance.GetService<SpellTrail>();
        direction = new Vector2(_playerMovement.transform.localScale.x, 0);
        StartCoroutine(DisableSpellIfNotHit());
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.TryGetComponent(out IInteractable interactablePlatform);

        interactablePlatform?.Interact(tag);

        DisableSpellWithoutTrail();
    }

    private void DisableSpellWithoutTrail()
    {
        _particleSystem.gameObject.transform.SetParent(null);

        _spellTrail.HandleSpellTrail(_particleSystem.gameObject, transform);

        gameObject.SetActive(false);
    }

    private void SpellAesthetic()
    {
        if (CompareTag("ForestPower"))
        {
            var main = _particleSystem.main;
            main.startColor = Color.green;
            _spriteRenderer.color = Color.green;
        }
        else if (CompareTag("NightPower"))
        {
            var main = _particleSystem.main;
            main.startColor = Color.cyan;
            _spriteRenderer.color = Color.cyan;
        }
    }

    private void OnDisable()
    {
        _particleSystem.gameObject.SetActive(false);
    }
    #endregion

    #region CoRoutines

    IEnumerator DisableSpellIfNotHit()
    {
        yield return new WaitForSeconds(10f);
        DisableSpellWithoutTrail();
    }

    #endregion
}
