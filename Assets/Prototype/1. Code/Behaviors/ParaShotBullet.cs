using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaShotBullet : MonoBehaviour
{
    #region Public Variables 
    public float StunValue { get => _stunValue; set => _stunValue = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private float _paraShotSpeed;
    [SerializeField] private Vector2 _paraShotDirection;
    [SerializeField] private float _autoDestroyTime;
    [SerializeField] private float _stunValue;
    private Rigidbody2D _paraBody;
    #endregion

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _paraBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ParaShotMove();
        AutoDestroy();
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    private void ParaShotMove()
    {
        _paraBody.linearVelocity = new Vector2(_paraShotDirection.x, _paraShotDirection.y) * _paraShotSpeed;
    }

    private void AutoDestroy()
    {
        _autoDestroyTime -= Time.deltaTime;

        if(_autoDestroyTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //collision.GetComponent<DummyEnemy>().Stun(_stunValue);
            collision.GetComponent<DummyEnemy>().IsStunned = true;
            collision.GetComponent<DummyEnemy>().StunTime = _stunValue;
            Destroy(gameObject);
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
