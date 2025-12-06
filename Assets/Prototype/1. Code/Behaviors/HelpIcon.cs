using UnityEngine;
using TMPro;

public class HelpIcon : MonoBehaviour
{
    [SerializeField] private TMP_Text _helpText;
    [SerializeField] private GameObject _helpObjBox;
    [SerializeField] private string _helpString;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ShowHelpText(_helpText);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            HideHelpText();
        }
    }

    private void ShowHelpText(TMP_Text pText)
    {
        _helpObjBox.SetActive(true);
        pText.text = _helpString;
    }

    private void HideHelpText()
    {
        _helpObjBox.SetActive(false);
    }
}
