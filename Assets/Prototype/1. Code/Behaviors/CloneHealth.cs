using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneHealth : MonoBehaviour
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    [SerializeField] private float _cloneMaxHealth;
    [SerializeField] private float _cloneHealth;
    #endregion

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetCloneHealth();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCloneHealth();
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    private void SetCloneHealth()
    {
        _cloneHealth = _cloneMaxHealth;
    }

    private void CloneTakeDamage(float pDamage)
    {
        _cloneHealth -= pDamage;
    }

    private void UpdateCloneHealth()
    {
        if(_cloneHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyAttack")
        {
            CloneTakeDamage(collision.GetComponent<Dummy_Bullet>().BulletDamage);
            Destroy(collision.gameObject);
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
