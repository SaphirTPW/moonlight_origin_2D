using UnityEngine;

public class ChallengeController : MonoBehaviour
{
    public ChallengeSO challengeData;

    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private Transform rewardSpawnPoint;

    [SerializeField] private ChallengeController collectChallengeController;
    [SerializeField] private AudioClip _challengeMusic;
    [SerializeField] private AudioClip _rewardMusic;
    [SerializeField] private AudioClip _failMusic;

    private Challenge _challenge;

    public Challenge Challenge { get => _challenge; set => _challenge = value; }

    private void Start()
    {
        CreateChallenge();
        _challenge.OnChallengeCompleted += OnChallengeCompleted;
        _challenge.OnChallengeStarted += StartChallengeMusic;
        _challenge.OnChallengeFailed += ChallengeFailed;
    }

    private void OnDestroy()
    {
        if (_challenge != null)
        {
            _challenge.OnChallengeCompleted -= OnChallengeCompleted;
            _challenge.OnChallengeStarted -= StartChallengeMusic;
            _challenge.OnChallengeFailed -= ChallengeFailed;
        }
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

    public void OnChallengeCompleted()
    {
        GameObject rewardGO = Instantiate(
            rewardPrefab,
            rewardSpawnPoint.position,
            Quaternion.identity
        );
        AudioManager.Instance.PlaySFX(_rewardMusic);
        
        Key key = rewardGO.GetComponent<Key>();

        if (key != null)
        {
            key.SetOwner(collectChallengeController);
        }

        AudioManager.Instance.PlayMusic(GameManager.Instance.musicLevel);
    }

    public void StartChallengeMusic()
    {
        AudioManager.Instance.PlayMusic(_challengeMusic);
    }

    public void ChallengeFailed()
    {
        AudioManager.Instance.PlaySFX(_failMusic);
        AudioManager.Instance.PlayMusic(GameManager.Instance.musicLevel);
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
