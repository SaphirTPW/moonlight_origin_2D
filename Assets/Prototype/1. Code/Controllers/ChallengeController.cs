using UnityEngine;

public class ChallengeController : MonoBehaviour
{
    public ChallengeSO challengeData;

    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private Transform rewardSpawnPoint;

    [SerializeField] private ChallengeController collectChallengeController;

    private Challenge _challenge;

    public Challenge Challenge { get => _challenge; set => _challenge = value; }


    //private int _keyCollected = 0;

    private void Start()
    {
        CreateChallenge();
        _challenge.OnChallengeCompleted += OnChallengeCompleted;
        _challenge.StartChallenge();
    }

    private void Update()
    {
        _challenge?.UpdateChallenge();
    }

    private void CreateChallenge()
    {
        switch (challengeData.type)
        {
            case ChallengeType.Reach:
                _challenge = new ReachChallenge(challengeData, rewardSpawnPoint, this);
                break;
            case ChallengeType.Collect:
                _challenge = new CollectChallenge(challengeData, rewardSpawnPoint, this);
                break;
            case ChallengeType.Destroy:
                _challenge = new DestroyChallenge(challengeData, rewardSpawnPoint, this);
                break;
        }
    }

    //public void OnKeyCollected()
    //{
    //    _keyCollected++;
    //    Debug.Log("Key collected by challenge: " + gameObject.name);
    //}

    public void OnChallengeCompleted()
    {
        GameObject rewardGO = Instantiate(
            rewardPrefab,
            rewardSpawnPoint.position,
            Quaternion.identity
        );

        Key key = rewardGO.GetComponent<Key>();

        if (key != null)
        {
            key.SetOwner(collectChallengeController);
        }
    }

    public void AddCollect(int amount)
    {
        if (_challenge is CollectChallenge c)
            c.Collect(amount);
    }

    public void AddDestroy()
    {
        if (_challenge is DestroyChallenge d)
            d.DestroyObject();
    }

    public void ReachGoal()
    {
        if (_challenge is ReachChallenge r)
            r.ReachGoal();
    }
}
