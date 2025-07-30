using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    private PlayerController _pc;
    private Rigidbody2D _rb;
    private SpriteRenderer _spr;

    //Movement Variables 
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _speedMod;
    [SerializeField][Range(0, 0.3f)] private float _playerMoveSmoothing;
    private Vector3 _velocity = Vector3.zero;

    //Jump Variables
    [SerializeField] private float _playerJumpForce;
    [SerializeField] private float _playerFallMultiplier = 2.5f;
    [SerializeField] private float _playerLowMultiplier = 2f;
    [SerializeField] private float _playerDefaultGravityScale;
    [SerializeField] private bool _playerGrounded;
    [SerializeField] private Transform _playerGroundCheck;
    [SerializeField] private float _groundCheckRad;
    [SerializeField] private LayerMask _groundIdentifier;

    //Change Sprite Orientation
    private bool _facingRight = true;

    #endregion

    #region Unity Methods 

    void Start()
    {
        _pc = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerGroundChecker();
    }

    private void FixedUpdate()
    {
        PlayerMove(_pc.InputDirection.x);
        PlayerJump();
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    private void PlayerMove(float pDirection)
    {
        Vector2 targetVelocity = new Vector2(pDirection * _playerSpeed * _speedMod, _rb.linearVelocityY);
        _rb.linearVelocity = Vector3.SmoothDamp(_rb.linearVelocity, targetVelocity, ref _velocity, _playerMoveSmoothing);

        if (pDirection > 0 && !_facingRight)
            PlayerFlipSprite();
        else if (pDirection < 0 && _facingRight)
            PlayerFlipSprite();
    }

    private void PlayerGroundChecker()
    {
        _playerGrounded = false;

        Collider2D[] _colliders = Physics2D.OverlapCircleAll(_playerGroundCheck.position, _groundCheckRad, _groundIdentifier);

        for (int i = 0; i < _colliders.Length; i++)
        {
            if (_colliders[i].gameObject != gameObject)
                _playerGrounded = true;
        }
    }

    private void PlayerJump()
    {
        if (_rb.linearVelocityY < 0)
            _rb.gravityScale = _playerFallMultiplier;
        else if (_rb.linearVelocityY > 0 && !_pc.HoldJump)
            _rb.gravityScale = _playerLowMultiplier;
        else
            _rb.gravityScale = _playerDefaultGravityScale;

        if(_playerGrounded && _pc.Jump)
        {
            _playerGrounded = false;
            _pc.Jump = false;
            _rb.AddForce(new Vector2(0f, _playerJumpForce), ForceMode2D.Impulse);
        }
    }

    private void PlayerFlipSprite()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    
    #endregion

    #region Coroutines
    #endregion
}
