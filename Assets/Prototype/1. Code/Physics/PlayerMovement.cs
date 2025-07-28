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

    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _speedMod;
    [SerializeField][Range(0, 0.3f)] private float _playerMoveSmoothing;
    private Vector3 _velocity = Vector3.zero;
    #endregion

    #region Unity Methods 

    void Start()
    {
        _pc = GetComponent<PlayerController>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MovePlayer(_pc.InputDirection.x);
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    private void MovePlayer(float pDirection)
    {
        Vector2 targetVelocity = new Vector2(pDirection * _playerSpeed * _speedMod, _rb.linearVelocityY);
        _rb.linearVelocity = Vector3.SmoothDamp(_rb.linearVelocity, targetVelocity, ref _velocity, _playerMoveSmoothing);
    }
    
    #endregion

    #region Coroutines
    #endregion
}
