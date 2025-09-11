using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_Bullet : MonoBehaviour
{
    #region Public Variables 
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Vector2 _bulletDirection;
    [SerializeField] private float _autoDestroyTime;
    [SerializeField] private float _knockBackForce = 5f;
    [SerializeField] private float _knockBackUp = 3f;
    [SerializeField] private float _bulletDamage;
    private Rigidbody2D _bulletBody;

    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bulletBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        BulletMove();
        AutoDestroy();
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    private void BulletMove()
    {
        _bulletBody.linearVelocity = new Vector2(_bulletDirection.x, _bulletDirection.y) * _bulletSpeed;
    }

    private void AutoDestroy()
    {
        _autoDestroyTime -= Time.deltaTime;

        if (_autoDestroyTime <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerHealth>().PlayerTakeDamage(_bulletDamage);
            collision.GetComponent<PlayerMovement>().PlayerKnockback(transform, _knockBackForce, _knockBackUp);
            Destroy(gameObject);
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
