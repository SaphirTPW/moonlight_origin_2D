using UnityEngine;

public class ChallengeCollectManager : MonoBehaviour
{
    [SerializeField] private ChallengeController _collectChallengeController;

    [SerializeField] private GameObject _targetHolder;
    [SerializeField] private GameObject[] _targets;

    private void Start()
    {
        var _challenge = _collectChallengeController.Challenge;

        _challenge.OnChallengeFailed += OnChallengeFailed;
        //_challenge.OnChallengeCompleted += _collectChallengeController.OnChallengeCompleted;

        int childCount = _targetHolder.transform.childCount;
        _targets = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            _targets[i] = _targetHolder.transform.GetChild(i).gameObject;
        }
    }

    private void OnChallengeFailed()
    {
        Debug.Log("Failed Mission");
        _collectChallengeController.gameObject.SetActive(false);

        for (int i = 0; i < _targets.Length; i++)
        {
            _targets[i].SetActive(true);
            _targets[i].gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }
}
