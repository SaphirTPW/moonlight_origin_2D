using UnityEngine;

public class ChallengeCollectManager : MonoBehaviour
{
    [SerializeField] private ChallengeController _collectChallengeController;

    [SerializeField] private GameObject _targetHolder;
    [SerializeField] private GameObject[] _targets;

    private void Start()
    {
        var challenge = _collectChallengeController.Challenge;

        challenge.OnChallengeStarted += OnChallengeStarted;
        challenge.OnChallengeFailed += OnChallengeFailed;
    }

    private void OnDisable()
    {
        var _challenge = _collectChallengeController.Challenge;

        _challenge.OnChallengeStarted -= OnChallengeFailed;
        _challenge.OnChallengeFailed -= OnChallengeStarted;
    }

    private void Awake()
    {
        int childCount = _targetHolder.transform.childCount;
        _targets = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            _targets[i] = _targetHolder.transform.GetChild(i).gameObject;
            _targets[i].SetActive(false);
            _targets[i].gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnChallengeFailed()
    {
        Debug.Log("Failed Mission");

        for (int i = 0; i < _targets.Length; i++)
        {
            _targets[i].SetActive(false);
            _targets[i].gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnChallengeStarted()
    {
        for (int i = 0; i < _targets.Length; i++)
        {
            _targets[i].SetActive(true);
            _targets[i].gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }
}
