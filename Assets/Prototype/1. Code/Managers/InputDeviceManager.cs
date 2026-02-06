using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputDeviceManager : MonoBehaviour
{
    public ControlType CurrentControl { get => _currentControl; set => _currentControl = value; }
    public Sprite CrashOutIconPAD { get => _crashOutIconPAD; set => _crashOutIconPAD = value; }
    public Sprite CrashOutIconKEY { get => _crashOutIconKEY; set => _crashOutIconKEY = value; }

    public static InputDeviceManager Instance;
    [SerializeField] private ControlType _currentControl;

    [SerializeField] private GameObject _inputPromptObj;
    [SerializeField] private TMP_Text _inputMessage;
    [SerializeField] private Image _inputSprite;

    [SerializeField] private Sprite _crashOutIconPAD;
    [SerializeField] private Sprite _crashOutIconKEY;

    private void OnEnable()
    {
        Emotion.OnCrashOutAvailability += HandleCrashPrompt;
    }

    private void OnDisable()
    {
        Emotion.OnCrashOutAvailability -= HandleCrashPrompt;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DisablePrompt();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInput();
    }

    public void DisablePrompt()
    {
        _inputPromptObj.SetActive(false);
    }

    public void EnablePrompt(string pMessage, Sprite pIcon)
    {
        _inputMessage.text = pMessage;
        _inputSprite.sprite = pIcon;
        _inputPromptObj.SetActive(true);
    }

    private void HandleCrashPrompt(bool available)
    {
        if (available)
        {
            if (_currentControl == ControlType.Gamepad)
                EnablePrompt("Press", CrashOutIconPAD);
            else if (_currentControl == ControlType.Keyboard)
                EnablePrompt("Press", CrashOutIconKEY);
        }
        else
            DisablePrompt();
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
