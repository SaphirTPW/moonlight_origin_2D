using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cinemachine;

public class ChargePunch : Skill
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _knockBackForce = 50f;
    [SerializeField] private float _knockBackUp = 10f;
    [SerializeField] private float _recoilForce;
    [SerializeField] private float _damage;

    [SerializeField] private float _chargePunchTime;
    [SerializeField] private float _maxChargePunchTime;
    [SerializeField] private ParticleSystem _gatherFX;
    [SerializeField] private bool _isActive;
    [SerializeField] private CinemachineImpulseSource _impulse;
    [SerializeField] private AudioClip _chargeSlashSFX;
    [SerializeField] private AudioClip _chargeSFX;
    #endregion

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (CurrentSkillState == SkillState.Ready)
        {
            HandleCharge();
        }
    }
    #endregion

    #region Public Methods 
    public override void EnableSkill(float pSkillCost)
    {
        if (!_isActive && CurrentSkillState == SkillState.Ready)
        {
            base.EnableSkill(pSkillCost);
            _isActive = true;
            _gatherFX.Play();
            AudioManager.Instance.PlaySFX(_chargeSFX);
        }
    }

    public override void SkillOnCoolDown()
    {
        base.SkillOnCoolDown();
        //if (WarmUp.IsWarmnedUp)
        //{
        //    WarmUp.IsWarmnedUp = false;
        //    PCom.DamageMultiplier = 1f;
        //}
    }

    public override void SetSkillInfo()
    {
        base.SetSkillInfo();
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 

    private void HandleCharge()
    {
        if (_isActive)
        {
            _chargePunchTime += Time.deltaTime;
            if (_chargePunchTime != _maxChargePunchTime)
            {
                PC.CanMove = false;
                PC.InputDirection = Vector2.zero;
                PM.Rb.linearVelocity = Vector2.zero;
            }

            if (_chargePunchTime >= _maxChargePunchTime)
            {
                _gatherFX.Stop();
                ChargePunchAttack(_damage * PCom.DamageMultiplier);
                WarmUp.IsWarmnedUp = false;
                PCom.DamageMultiplier = 1f;
                PC.PlayerAnim.SetTrigger("Attack");
                PC.CanMove = true;
                _chargePunchTime = 0;
                CurrentSkillState = SkillState.CoolDown;
            }
        }
    }
    private void ChargePunchAttack(float pDamage)
    {
        if (_isActive)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                AudioManager.Instance.PlaySFX(_chargeSlashSFX);
                CameraShakeManager.instance.CameraShake(_impulse);
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(pDamage * PCom.AttackMod);
                    WarmUp.IsWarmnedUp = false;
                    PCom.DamageMultiplier = 1f;
                    enemy.GetComponent<DummyEnemy>().Knockback(transform, _knockBackForce, _knockBackUp);
                    PlayerAttackRecoil(enemy.transform, _recoilForce);
                    _isActive = false;
                }
                else if (enemy.CompareTag("ChallengeObstacle"))
                {
                    enemy.GetComponent<ChallengeObstacleHealth>().TakeDamage(pDamage * PCom.AttackMod);
                    WarmUp.IsWarmnedUp = false;
                    PCom.DamageMultiplier = 1f;
                    PlayerAttackRecoil(enemy.transform, _recoilForce);
                    _isActive = false;
                }
                else
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(pDamage * PCom.AttackMod);
                    WarmUp.IsWarmnedUp = false;
                    PCom.DamageMultiplier = 1f;
                    PlayerAttackRecoil(enemy.transform, _recoilForce);
                    _isActive = false;
                }
            }

            _isActive = false;
        }
    }

    private void PlayerAttackRecoil(Transform pTransform, float pRecoilForce)
    {
        Vector2 direction = (transform.position - pTransform.position).normalized;
        PM.Rb.linearVelocity = direction * pRecoilForce;
    }
    #endregion

    #region Coroutines
    #endregion
}
