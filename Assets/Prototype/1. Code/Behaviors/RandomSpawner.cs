using UnityEngine;
using System.Collections.Generic;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private ChallengeController _challengeController;

    [SerializeField] private Transform _spawnPointParent;
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();

    [SerializeField] private GameObject _objPrefab;

    private GameObject _currentObj;

    private void Awake()
    {
        foreach (Transform child in _spawnPointParent)
        {
            _spawnPoints.Add(child);
        }

        SpawnObject();
    }

    private void Start()
    {

        _challengeController.Challenge.OnChallengeFailed += HandleChallengeFailed;
        _challengeController.Challenge.OnChallengeStarted += HandleChallengeStarted;
    }

    //private void OnDisable()
    //{
    //    _challengeController.Challenge.OnChallengeFailed -= HandleChallengeFailed;
    //    _challengeController.Challenge.OnChallengeStarted -= SpawnObject;
    //}

    private void TrySpawn()
    {
        var challenge = _challengeController.Challenge;

        if (!challenge.IsActive)
            return;

        if (challenge.CurrentAmount >= challenge.Data.targetAmount)
            return;

        SpawnObject();
    }

    private void SpawnObject()
    {
        Debug.Log("Call SpawnObject");

        if (_spawnPoints.Count == 0)
            return;

        int index = Random.Range(0, _spawnPoints.Count);
        Transform spawnPoint = _spawnPoints[index];

        _currentObj = Instantiate(_objPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("SpawnObj");

        ChallengeObstacleHealth obj = _currentObj.GetComponent<ChallengeObstacleHealth>();

        if(obj != null)
        {
            obj.SetController(_challengeController, this);
        }
    }

    public void OnObjectDestroyed()
    {
        var challenge = _challengeController.Challenge;

        if (challenge.CurrentAmount >= challenge.Data.targetAmount)
            return;

        TrySpawn();
    }

    private void HandleChallengeFailed()
    {
        Debug.Log("Failed Mission");
        if (_currentObj != null)
        {
            Destroy(_currentObj);
        }
    }

    private void HandleChallengeStarted()
    {
        Debug.Log("Spawner Start");
        SpawnObject();
    }
}
