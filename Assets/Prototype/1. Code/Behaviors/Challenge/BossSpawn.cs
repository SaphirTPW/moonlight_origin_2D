using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] private ChallengeController _challengeController;
    [SerializeField] private GameObject _bossObj;

    private void Awake()
    {
        _bossObj.SetActive(false);
    }

    private void Start()
    {

        //_challengeController.Challenge.OnChallengeFailed += HandleChallengeFailed;
        //_challengeController.Challenge.OnChallengeStarted += HandleChallengeStarted;
    }

    private void OnDisable()
    {
        //_challengeController.Challenge.OnChallengeFailed -= HandleChallengeFailed;
        _challengeController.Challenge.OnChallengeStarted -= HandleChallengeStarted;
    }

    //private void HandleChallengeFailed()
    //{
    //    Debug.Log("Failed Mission");
    //    if (_currentObj != null)
    //    {
    //        Destroy(_currentObj);
    //    }
    //}

    private void SpawnBoss()
    {
        _bossObj.SetActive(true);
    }

    private void HandleChallengeStarted()
    {
        Debug.Log("Boss Start");
        SpawnBoss();
    }
}
