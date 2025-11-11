using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyHealth : MonoBehaviour
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    private DamageFlash _damageFlash;
    [SerializeField] private float _enemyMaxHealth;
    [SerializeField] private float _enemyCurrentHealth;
    private bool _isDead = false;

    private CinemachineImpulseSource _impulseSource;
    #endregion

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetEnemyHealth();
        _damageFlash = GetComponent<DamageFlash>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDeath();
    }
    #endregion

    #region Public Methods 
    public void TakeDamage(float pDamage)
    {
        _enemyCurrentHealth -= pDamage;
        _damageFlash.CallDamageFlash();
        CameraShakeManager.instance.CameraShake(_impulseSource);
    }
    #endregion

    #region Private Methods 
    private void SetEnemyHealth()
    {
        _enemyCurrentHealth = _enemyMaxHealth;
        _isDead = false;
    }

    private void EnemyDeath()
    {
        if (_enemyCurrentHealth <= 0)
            _isDead = true;

        if(_isDead)
            Destroy(gameObject);
    }
    #endregion

    #region Coroutines
    #endregion
}
