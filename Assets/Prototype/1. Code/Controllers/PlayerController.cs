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
    #endregion

    #region Private Variables 
    private Vector2 _inputDirection;
    private bool _jump;
    private bool _holdJump;
    
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _canJump;
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
    #endregion

    #region Coroutines
    #endregion
}
