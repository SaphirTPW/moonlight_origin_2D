using UnityEngine;
using System.Collections;
using TMPro;

public class EnableScript : MonoBehaviour
{
    [SerializeField] private ChallengeController _challengeToEnable;
    [SerializeField] private GameObject _challengePanel;
    [SerializeField] private TMP_Text _challengeText;
    [SerializeField] [TextArea] private string _challengeString;
    [SerializeField] private TimedObj _timedObj;
    private bool _canEnableChallenge = false;
    [SerializeField] private bool _isAutomatic;

    private void Start()
    {
        if (_challengeToEnable == null)
        {
            Debug.LogError("ChallengeController manquant sur EnableScript");
            return;
        }

        if (_isAutomatic)
        {
            _challengeToEnable.Challenge.StartChallenge();

            if (_timedObj != null)
            {
                _timedObj.EnableObj();
            }
        }
    }

    private void Update()
    {
        if (_canEnableChallenge && Input.GetButtonDown("Crash Out") && !_challengeToEnable.Challenge.IsActive)
        {
            Debug.Log("Challenge ?");
            _challengeToEnable.Challenge.StartChallenge();
            _timedObj.EnableObj();
            DisableChallengePanel();
        }
        else
            return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_challengeToEnable.Challenge.IsCompleted)
        {
            EnableChallengePanel();
            _canEnableChallenge = true;
        }
        else
            return;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DisableChallengePanel();
            _canEnableChallenge = false;
        }
    }

    private void EnableChallengePanel()
    {
        _challengePanel.SetActive(true);
        SetChallengePenal();
        //if (!_challengeToEnable.Challenge.IsActive)
        //{
        //    _challengePanel.SetActive(true);
        //    SetChallengePenal();
        //}
        //else
        //    return;
    }

    private void DisableChallengePanel()
    {
        _challengePanel.SetActive(false);
    }

    private void SetChallengePenal()
    {
        _challengeText.text = _challengeString;
    }
}
