using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    private Rigidbody2D _dummyRb;
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
        
    }
    #endregion

    #region Public Methods 
    public void Knockback(Transform pTransform, float pknockbackForce, float pknockbackUp)
    {
        Vector2 direction = (transform.position - pTransform.position).normalized;
        //_dummyRb.linearVelocity = direction * pknockbackForce;
        _dummyRb.linearVelocity = new Vector2(direction.x, pknockbackUp) * pknockbackForce;
        Debug.Log("knockback applied.");
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
