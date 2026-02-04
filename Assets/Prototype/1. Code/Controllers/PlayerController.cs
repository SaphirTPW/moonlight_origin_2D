using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    #region Public Variables 
    public Vector2 InputDirection { get => _inputDirection; set => _inputDirection = value; }
    public bool Jump { get => _jump; set => _jump = value; }
    public bool HoldJump { get => _holdJump; set => _holdJump = value; }
    public bool Attack { get => _isAttacking; set => _isAttacking = value; }
    public bool IsMoving { get => _IsMoving; set => _IsMoving = value; }
    public bool CanMove { get => _canMove; set => _canMove = value; }
    public bool CanJump { get => _canJump; set => _canJump = value; }
    public bool CanAttack { get => _canAttack; set => _canAttack = value; }
    public Animator PlayerAnim { get => _playerAnim; set => _playerAnim = value; }
    public float JumpBufferCounter { get => _jumpBufferCounter; set => _jumpBufferCounter = value; }
    public float JumpBufferTime { get => _jumpBufferTime; set => _jumpBufferTime = value; }
    public bool OpenSkillTab { get => _openSkillTab; set => _openSkillTab = value; }
    #endregion

    #region Private Variables 
    private Vector2 _inputDirection;
    private float _inputDirThreshold = 0.2f;
    private float _inputDirPrevValue = 0;
    private float _currentInputDirValue;
    
    private bool _jump = false;
    private bool _holdJump = false;
    [SerializeField] private float _jumpBufferTime = 0.2f;
    [SerializeField] private float _jumpBufferCounter;

    [SerializeField] private bool _isAttacking = false;
    [SerializeField] private float _startAttackTime;
    [SerializeField] private float _attackTime;
    [SerializeField] private bool _IsMoving = false;
    
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _canJump;
    [SerializeField] private bool _canAttack;
    [SerializeField] private bool _openSkillTab;

    [SerializeField] private ParticleSystem _dustFX;
    [SerializeField] private ParticleSystem _landingFX;
    private Animator _playerAnim;
    private PlayerMovement _pm;
    #endregion

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerAnim = GetComponent<Animator>();
        _pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMoveInput();
        HandleJumpInput();
        HandleAttackInput();
        HandleSkillTab();
        UpdateAttackState();
    }
    #endregion

    #region Public Methods 
    public void CreateDust()
    {
        if(_pm.PlayerGrounded)
            _dustFX.Play();
    }

    public void CreateLandingDust()
    {
        _landingFX.Play();
    }
    #endregion

    #region Private Methods 
    private void HandleMoveInput()
    {
        if (_canMove) 
        {
            if(InputDeviceManager.Instance.CurrentControl == InputDeviceManager.ControlType.Gamepad)
            {
                _inputDirection.x = Input.GetAxis("Horizontal");
                _inputDirection.y = Input.GetAxis("Vertical");

                //Debug.Log($"InputDirectionX {_inputDirection.x} ; InputDirectionY {_inputDirection.y}");
            }
            else if(InputDeviceManager.Instance.CurrentControl == InputDeviceManager.ControlType.Keyboard)
            {
                _inputDirection.x = Input.GetAxis("KEYHorizontal");
                _inputDirection.y = Input.GetAxis("KEYVertical");

                //Debug.Log($"InputDirectionX {_inputDirection.x} ; InputDirectionY {_inputDirection.y}");
            }

                _currentInputDirValue = _inputDirection.x;

            if (_inputDirection.x > 0 || _inputDirection.x < 0)
            {
                _IsMoving = true;
            }
            else
            {
                _IsMoving = false;
            }

            if (Mathf.Abs(_currentInputDirValue) > _inputDirThreshold && Mathf.Abs(_inputDirPrevValue) <= _inputDirThreshold)
            {
                CreateDust();
            }

            _inputDirPrevValue = _currentInputDirValue;

            _playerAnim.SetFloat("HSpeed", Mathf.Abs(_inputDirection.x));
        }
    }

    private void HandleJumpInput()
    {
        if (_canJump)
        {
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("KEYJump"))
            {
                _jump = true;
                _jumpBufferCounter = _jumpBufferTime;
                CreateDust();
            }
            else
            {
                _jumpBufferCounter -= Time.deltaTime;
            }

            if (Input.GetButton("Jump") || Input.GetButton("KEYJump"))
            {
                _holdJump = true;

            }
            else
            {
                _holdJump = false;
            }
        }
    }

    private void HandleAttackInput()
    {
        if (_canAttack)
        {
            if (Input.GetButtonDown("Attack") || Input.GetKeyDown(KeyCode.F))
            {
                _isAttacking = true;
                _playerAnim.SetTrigger("Attack");
                _canAttack = false;
                _pm.Rb.linearDamping = 1000f;
            }

            if (Input.GetButtonDown("Attack"))
                InputDeviceManager.Instance.CurrentControl = InputDeviceManager.ControlType.Gamepad;
            else if (Input.GetKeyDown(KeyCode.F))
                InputDeviceManager.Instance.CurrentControl = InputDeviceManager.ControlType.Keyboard;
        }
    }

    private void HandleSkillTab()
    {
        if (Input.GetButton("LB") || Input.GetKey(KeyCode.LeftShift))
        {
            _openSkillTab = true;
        }
        else if (Input.GetButtonUp("LB") || Input.GetKeyUp(KeyCode.LeftShift))
        {
            _openSkillTab = false;
        }
    }

    private void UpdateAttackState()
    {
        if (!_canAttack)
        {
            _attackTime += Time.deltaTime;

            if(_attackTime >= _startAttackTime)
            {
                _canAttack = true;
                _isAttacking = false;
                _attackTime = 0;
            }
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
