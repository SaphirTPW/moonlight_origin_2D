using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    #region Public Variables 
    public bool IsStunned { get => _isStunned; set => _isStunned = value; }
    public float StunTime { get => _stunTime; set => _stunTime = value; }
    #endregion

    #region Private Variables 
    private Rigidbody2D _dummyRb;
    [SerializeField] private bool _isStunned;
    [SerializeField] private float _stunTime;
    #endregion

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _dummyRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Stun();
    }
    #endregion

    #region Public Methods 
    public void Knockback(Transform pTransform, float pknockbackForce, float pknockbackUp)
    {
        Vector2 direction = (transform.position - pTransform.position).normalized;
        //_dummyRb.linearVelocity = direction * pknockbackForce;
        _dummyRb.linearVelocity = new Vector2(direction.x, pknockbackUp) * pknockbackForce;
        //Debug.Log("knockback applied.");
    }

    public void Stun()
    {
        if (_isStunned)
        {
            Debug.Log("Do Some !");
            _stunTime -= Time.deltaTime;
            Debug.Log(_stunTime);

            if (_stunTime <= 0)
            {
                _isStunned = false;
                _stunTime = 0;
            }
        }
    }

    public void SetStunTime(float pStunTime)
    {
        _stunTime = pStunTime;
    }
    #endregion

    #region Private Methods 
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "ParaShot")
    //    {
    //        _isStunned = true;
    //        SetStunTime(collision.gameObject.GetComponent<ParaShotBullet>().StunValue);
    //        Destroy(collision.gameObject);
    //    }
    //}
    #endregion

    #region Coroutines
    #endregion
}
