using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Public Variables 
    public float AttackMod { get => _attackMod; set => _attackMod = value; }
    #endregion

    #region Private Variables 
    private PlayerController _pc;

    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackRange;

    [SerializeField] private float _attackDamage;
    private float _attackMod = 1f;
    [SerializeField] private float _playerComboCounter;
    [SerializeField] private float _playerComboTime;
    [SerializeField] private float _playerComboTimer;
    
    private bool _isAttacking;

    #endregion

    #region Unity Methods 

    void Start()
    {
        _pc = GetComponent<PlayerController>();
        SetComboTimer();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAttack(_attackDamage);
        EnableComboTimer();
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    private void PlayerAttack(float pDamage)
    {
        if (_pc.Attack)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(pDamage * _attackMod);
                _isAttacking = true;
                _playerComboCounter++;
                SetComboTimer();
            }
            _pc.Attack = false;
        }
    }

    private void ResetComboCounter()
    {
        _playerComboCounter = 0;
        _isAttacking = false;
        SetComboTimer();
    }

    private void SetComboTimer()
    {
        _playerComboTimer = _playerComboTime;
    }

    private void EnableComboTimer()
    {
        if (_isAttacking)
        {
            _playerComboTimer -= Time.deltaTime;

            if(_playerComboTimer <= 0)
            {
                ResetComboCounter();
            }
        }
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
    #endregion

    #region Coroutines
    #endregion
}
