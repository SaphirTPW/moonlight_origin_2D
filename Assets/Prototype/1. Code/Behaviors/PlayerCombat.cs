using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Video;

public class PlayerCombat : MonoBehaviour
{
    #region Public Variables 
    public float AttackMod { get => _attackMod; set => _attackMod = value; }
    public float PlayerComboCounter { get => _playerComboCounter; set => _playerComboCounter = value; }
    public float AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    public bool AngerBuildUpOn { get => _angerBuildUpOn; set => _angerBuildUpOn = value; }
    #endregion

    #region Private Variables 
    private PlayerController _pController;
    private PlayerMovement _pMovement;

    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _knockBackForce = 50f;
    [SerializeField] private float _knockBackUp = 10f;
    [SerializeField] private float _recoilForce;

    [SerializeField] private float _attackDamage;
    [SerializeField] private float _buildUpDamage;
    private float _attackMod = 1f;
    [SerializeField] private float _playerComboCounter;
    [SerializeField] private float _playerComboTime;
    [SerializeField] private float _playerComboTimer;

    private Vector2 _attackDirectionInput;
    //[SerializeField] private Transform _forwardAttackPoint;
    [SerializeField] private Transform _startAttackPoint;
    [SerializeField] private Transform _upAttackPoint;
    [SerializeField] private Transform _downAttackPoint;
    //[SerializeField] private Transform _backAttackPoint;
    
    private bool _isAttacking;
    private bool _angerBuildUpOn = false;

    #endregion

    #region Unity Methods 

    void Start()
    {
        _pController = GetComponent<PlayerController>();
        _pMovement = GetComponent<PlayerMovement>();
        SetComboTimer();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAttack(_attackDamage);
        EnableComboTimer();
        SetAttackDirection();
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    private void PlayerAttack(float pDamage)
    {
        if (_pController.Attack)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);
            _buildUpDamage = pDamage * (1 + (_playerComboCounter / 100));

            foreach (Collider2D enemy in hitEnemies)
            {
                if (_angerBuildUpOn)
                    enemy.GetComponent<EnemyHealth>().TakeDamage(_buildUpDamage * _attackMod);
                else
                    enemy.GetComponent<EnemyHealth>().TakeDamage(pDamage * _attackMod);

                enemy.GetComponent<DummyEnemy>().Knockback(transform, _knockBackForce, _knockBackUp);
                PlayerAttackRecoil(enemy.transform, _recoilForce);
                _isAttacking = true;
                _playerComboCounter++;
                SetComboTimer();
            }
            _pController.Attack = false;
        }
    }

    private void PlayerAttackRecoil(Transform pTransform, float pRecoilForce)
    {
        Vector2 direction = (transform.position - pTransform.position).normalized;
        _pMovement.Rb.linearVelocity = direction * pRecoilForce;
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

    private void SetAttackDirection()
    {
        _attackDirectionInput.x = Input.GetAxisRaw("Horizontal");
        _attackDirectionInput.y = Input.GetAxisRaw("Vertical");

        if(_attackDirectionInput.y > 0)
        {
            _attackPoint.position = _upAttackPoint.position;
        }
        else if(_attackDirectionInput.y < 0)
        {
            _attackPoint.position = _downAttackPoint.position;
        }
        else
        {
            _attackPoint.position = _startAttackPoint.position;
        }
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
