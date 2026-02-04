using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject _padEDriveUI;
    [SerializeField] private GameObject _padSkillUI;
    [SerializeField] private GameObject _padSkillListUI;

    [SerializeField] private GameObject _keyEDriveUI;
    [SerializeField] private GameObject _keySkillUI;
    [SerializeField] private GameObject _keySkillListUI;

    [SerializeField] private PlayerController _controller;

    public GameObject PadEDriveUI { get => _padEDriveUI; set => _padEDriveUI = value; }
    public GameObject PadSkillUI { get => _padSkillUI; set => _padSkillUI = value; }
    public GameObject KeyEDriveUI { get => _keyEDriveUI; set => _keyEDriveUI = value; }
    public GameObject KeySkillUI { get => _keySkillUI; set => _keySkillUI = value; }
    public GameObject PadSkillListUI { get => _padSkillListUI; set => _padSkillListUI = value; }
    public GameObject KeySkillListUI { get => _keySkillListUI; set => _keySkillListUI = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEDriveUI();
    }

    private void UpdateEDriveUI()
    {
       if(InputDeviceManager.Instance.CurrentControl == InputDeviceManager.ControlType.Keyboard)
        {
            _padEDriveUI.SetActive(false);
            _padSkillUI.SetActive(false);

            _keyEDriveUI.SetActive(true);
            _keySkillUI.SetActive(true);

            if (_controller.OpenSkillTab)
            {
                _keySkillUI.SetActive(false);
            }
        }
       else if(InputDeviceManager.Instance.CurrentControl == InputDeviceManager.ControlType.Gamepad)
        {
            _padEDriveUI.SetActive(true);
            _padSkillUI.SetActive(true);

            if (_controller.OpenSkillTab)
            {
                _padSkillUI.SetActive(false);
            }

            _keyEDriveUI.SetActive(false);
            _keySkillUI.SetActive(false);
        }
    }
}
