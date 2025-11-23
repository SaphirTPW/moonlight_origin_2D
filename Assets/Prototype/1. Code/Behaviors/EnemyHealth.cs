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
    [SerializeField] private GameObject _damageTextPrefab;

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
        ShowDamage(pDamage.ToString("F1"));
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

    private void ShowDamage(string text)
    {
        GameObject prefab = Instantiate(_damageTextPrefab, transform.position, Quaternion.identity);
        prefab.GetComponentInChildren<TextMesh>().text = text;
    }
    #endregion

    #region Coroutines
    #endregion
}
