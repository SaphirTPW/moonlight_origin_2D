using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Public Variables 
    public float SpeedMod { get => _speedMod; set => _speedMod = value; }
    public float PlayerSpeed { get => _playerSpeed; set => _playerSpeed = value; }
    public Rigidbody2D Rb { get => _rb; set => _rb = value; }
    public float PlayerMoveSmoothing { get => _playerMoveSmoothing; set => _playerMoveSmoothing = value; }
    public bool PlayerGrounded { get => _playerGrounded; set => _playerGrounded = value; }
    public float PlayerFallMultiplier { get => _playerFallMultiplier; set => _playerFallMultiplier = value; }
    public float DefPlayerSpeed { get => _defPlayerSpeed; set => _defPlayerSpeed = value; }
    public Collider2D PlayerCollider { get => _playerCollider; set => _playerCollider = value; }
    public float GroundDamping { get => _groundDamping; set => _groundDamping = value; }
    #endregion

    #region Private Variables 
    private PlayerController _pc;
    private PlayerCombat _pCom;
    private Rigidbody2D _rb;
    private SpriteRenderer _spr;
    private Collider2D _playerCollider;

    //Movement Variables 
    private float _defPlayerSpeed;
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _speedMod;
    [SerializeField][Range(0, 0.75f)] private float _playerMoveSmoothing;
    private Vector3 _velocity = Vector3.zero;

    //Jump Variables
    [SerializeField] private float _playerJumpForce;
    [SerializeField] private float _playerFallMultiplier = 2.5f;
    [SerializeField] private float _playerLowMultiplier = 2f;
    [SerializeField] private float _playerDefaultGravityScale;
    [SerializeField] private bool _playerGrounded;
    private bool _wasGrounded = false;
    [SerializeField] private Transform _playerGroundCheck;
    [SerializeField] private float _groundCheckRad;
    [SerializeField] private LayerMask _groundIdentifier;
    
    [SerializeField] private float _coyoteTime = 0.2f;
    [SerializeField] private float _coyoteTimeCounter;

    [SerializeField] private float _groundDamping = 0f;
    [SerializeField] private float _airDamping = 0.75f;

    //Change Sprite Orientation
    private bool _facingRight = true;

    #endregion

    #region Unity Methods 

    void Start()
    {
        _pc = GetComponent<PlayerController>();
        _pCom = GetComponent<PlayerCombat>();
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
        _playerCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerGroundChecker();
        CoyoteTime();
    }

    private void FixedUpdate()
    {
        PlayerMove(_pc.InputDirection.x);
        PlayerJump();
    }
    #endregion

    #region Public Methods 
    public void PlayerKnockback(Transform pTransform, float pknockbackForce, float pknockbackUp)
    {
        Vector2 direction = (transform.position - pTransform.position).normalized;
        _rb.linearVelocity = new Vector2(direction.x, pknockbackUp) * pknockbackForce;
    }
    #endregion

    #region Private Methods 
    private void PlayerMove(float pDirection)
    {
        Vector2 targetVelocity = new Vector2(pDirection * _playerSpeed * _speedMod, _rb.linearVelocityY);
        _rb.linearVelocity = Vector3.SmoothDamp(_rb.linearVelocity, targetVelocity, ref _velocity, _playerMoveSmoothing);

        if (pDirection > 0 && !_facingRight)
        {
            PlayerFlipSprite();
            _pc.CreateDust();
        }
        else if (pDirection < 0 && _facingRight)
        {
            PlayerFlipSprite();
            _pc.CreateDust();
        }
    }

    private void PlayerGroundChecker()
    {
        _playerGrounded = false;

        Collider2D[] _colliders = Physics2D.OverlapCircleAll(_playerGroundCheck.position, _groundCheckRad, _groundIdentifier);

        for (int i = 0; i < _colliders.Length; i++)
        {
            if (_colliders[i].gameObject != gameObject)
            {
                _playerGrounded = true;
                _rb.linearDamping = _groundDamping;
                _pCom.RecoilForce = 30;
            }
        }

        if (!_playerGrounded)
        {
            _rb.linearDamping = _airDamping;
            _pCom.RecoilForce = 10;
        }

        if(_playerGrounded && !_wasGrounded)
        {
            _pc.CreateLandingDust();
        }

        _wasGrounded = _playerGrounded;
    }

    private void PlayerJump()
    {
        if (_rb.linearVelocityY < 0)
        {
            _rb.gravityScale = _playerFallMultiplier;
            _pc.PlayerAnim.SetFloat("VSpeed", _rb.linearVelocityY);
        }
        else if (_rb.linearVelocityY > 0 && !_pc.HoldJump)
        {
            _rb.gravityScale = _playerLowMultiplier;
            _pc.PlayerAnim.SetFloat("VSpeed", _rb.linearVelocityY);
            _pc.PlayerAnim.SetBool("IsJumping", true);
            //_pc.PlayerAnim.SetTrigger("Attack");
            _coyoteTimeCounter = 0;
        }
        else if(_rb.linearVelocityY > 0 && _pc.HoldJump)
        {
            _pc.PlayerAnim.SetFloat("VSpeed", _rb.linearVelocityY);
            _pc.PlayerAnim.SetBool("IsJumping", true);
            //_pc.PlayerAnim.SetTrigger("Attack");
        }
        else
        {
            _rb.gravityScale = _playerDefaultGravityScale;
            _pc.PlayerAnim.SetBool("IsJumping", false);
        }

        if(_coyoteTimeCounter > 0f && _pc.JumpBufferCounter > 0f)
        {
            _pc.JumpBufferCounter = 0;
            _playerGrounded = false;
            _pc.Jump = false;
            _rb.AddForce(new Vector2(0f, _playerJumpForce), ForceMode2D.Impulse);
        }
    }

    private void CoyoteTime()
    {
        if (_playerGrounded)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
            _rb.gravityScale = _playerDefaultGravityScale;
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
