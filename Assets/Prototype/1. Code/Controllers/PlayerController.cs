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
    public bool Attack { get => _attack; set => _attack = value; }
    public bool IsMoving { get => _IsMoving; set => _IsMoving = value; }
    public bool CanMove { get => _canMove; set => _canMove = value; }
    public bool CanJump { get => _canJump; set => _canJump = value; }
    public bool CanAttack { get => _canAttack; set => _canAttack = value; }
    public Animator PlayerAnim { get => _playerAnim; set => _playerAnim = value; }
    public float JumpBufferCounter { get => _jumpBufferCounter; set => _jumpBufferCounter = value; }
    public float JumpBufferTime { get => _jumpBufferTime; set => _jumpBufferTime = value; }
    #endregion

    #region Private Variables 
    private Vector2 _inputDirection;
    
    private bool _jump = false;
    private bool _holdJump = false;
    [SerializeField] private float _jumpBufferTime = 0.2f;
    [SerializeField] private float _jumpBufferCounter;

    [SerializeField] private bool _attack = false;
    [SerializeField] private bool _IsMoving = false;
    
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _canJump;
    [SerializeField] private bool _canAttack;
    private Animator _playerAnim;
    private PlayerMovement _playerMove;
    #endregion

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerAnim = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMoveInput();
        HandleJumpInput();
        HandleAttackInput();
        //HandleJumpBuffer();
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    private void HandleMoveInput()
    {
        if (_canMove) 
        {
            _inputDirection.x = Input.GetAxis("Horizontal");
            _inputDirection.y = Input.GetAxis("Vertical");

            if (_inputDirection.x > 0 || _inputDirection.x < 0)
                _IsMoving = true;
            else
                _IsMoving = false;

            _playerAnim.SetFloat("HSpeed", Mathf.Abs(_inputDirection.x));
        }
    }

    private void HandleJumpInput()
    {
        if (_canJump)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _jump = true;
                _jumpBufferCounter = _jumpBufferTime;
            }
            else
            {
                _jumpBufferCounter -= Time.deltaTime;
            }

            if (Input.GetButton("Jump"))
            {
                _holdJump = true;

            }
            else
            {
                _holdJump = false;
            }
        }
    }

    public void HandleJumpBuffer()
    {
        if (_jump)
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferTime -= Time.deltaTime;
        }
    }

    private void HandleAttackInput()
    {
        if (_canAttack)
        {
            if (Input.GetButtonDown("Attack"))
            {
                _attack = true;
                _playerAnim.SetTrigger("Attack");
            }
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
