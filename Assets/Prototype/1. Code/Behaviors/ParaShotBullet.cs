using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaShotBullet : MonoBehaviour
{
    #region Public Variables 
    public float StunValue { get => _stunValue; set => _stunValue = value; }
    public Vector2 ParaShotDirection { get => _paraShotDirection; set => _paraShotDirection = value; }
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
        ParaShotMove();
    }

    // Update is called once per frame
    void Update()
    {
        AutoDestroy();
    }

    private void FixedUpdate()
    {
        
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    private void ParaShotMove()
    {
        _paraBody.linearVelocity = _paraShotDirection * _paraShotSpeed;
    }

    public void SetDirection(Vector2 pDir)
    {
        _paraShotDirection = pDir.normalized;
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
        else if(collision.gameObject.tag == "Untagged")
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
