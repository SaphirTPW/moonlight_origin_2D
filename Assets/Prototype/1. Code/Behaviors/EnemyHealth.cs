using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyHealth : MonoBehaviour
{
    #region Public Variables 
    public float EnemyCurrentHealth { get => _enemyCurrentHealth; set => _enemyCurrentHealth = value; }
    public float EnemyMaxHealth { get => _enemyMaxHealth; set => _enemyMaxHealth = value; }
    #endregion

    #region Private Variables 
    private DamageFlash _damageFlash;
    [SerializeField] private float _enemyMaxHealth;
    [SerializeField] private float _enemyCurrentHealth;
    private bool _isDead = false;
    [SerializeField] private GameObject _damageTextPrefab;

    private CinemachineImpulseSource _impulseSource;

    public event Action OnEnemyDeath;

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
        //_damageFlash.CallDamageFlash();
        ShowDamage(pDamage.ToString("F1"));
        CameraShakeManager.instance.CameraShake(_impulseSource);

        if(_enemyCurrentHealth <= 0)
        {
            OnEnemyDeath?.Invoke();
        }
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

        if(_isDead && gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        //else if(_isDead && gameObject.CompareTag("Boss"))
        //{

        //}
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
