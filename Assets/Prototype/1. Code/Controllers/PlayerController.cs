using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public Variables 
    public Vector2 InputDirection { get => _inputDirection; set => _inputDirection = value; }
    #endregion

    #region Private Variables 
    private Vector2 _inputDirection;
    [SerializeField] private bool _canMove;
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
    #endregion

    #region Coroutines
    #endregion
}
