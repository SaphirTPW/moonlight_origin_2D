using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    #endregion

    #region Private Variables 
    private Vector2 _inputDirection;
    private bool _jump = false;
    private bool _holdJump = false;
    [SerializeField] private bool _attack = false;
    [SerializeField] private bool _IsMoving = false;
    
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _canJump;
    [SerializeField] private bool _canAttack;
    #endregion

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMoveInput();
        HandleJumpInput();
        HandleAttackInput();
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
        }
    }

    private void HandleJumpInput()
    {
        if (_canJump)
        {
            if (Input.GetButtonDown("Jump"))
                _jump = true;

            if (Input.GetButton("Jump"))
                _holdJump = true;
            else
                _holdJump = false;
        }
    }

    private void HandleAttackInput()
    {
        if (_canAttack)
        {
            if (Input.GetButtonDown("Attack"))
            {
                _attack = true;
            }
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
