using UnityEngine;

public class InputDeviceManager : MonoBehaviour
{
    public ControlType CurrentControl { get => _currentControl; set => _currentControl = value; }
    
    public static InputDeviceManager Instance;
    [SerializeField] private ControlType _currentControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInput();
    }

    private void CheckForInput()
    {
        if(Input.GetAxisRaw("Horizontal") != 0f || 
            Input.GetAxisRaw("Vertical") != 0f || 
            Input.GetButtonDown("Attack") ||
            Input.GetButtonDown("Jump"))
        {
            //Debug.Log("Using Gamepad");
            _currentControl = ControlType.Gamepad;
        }

        if (Input.GetAxisRaw("KEYHorizontal") != 0f || 
            Input.GetAxisRaw("KEYVertical") != 0f || 
            Input.GetButtonDown("KEYJump"))
        {
            //Debug.Log("Using Keboard");
            _currentControl = ControlType.Keyboard;
        }
    }

    public enum ControlType
    {
        Keyboard,
        Gamepad
    }
}
