using UnityEngine;

public enum ChallengeType
{
    Reach,
    Collect,
    Destroy
}

[CreateAssetMenu(menuName = "Challenge/ChallengeSO")]
public class ChallengeSO : ScriptableObject
{
    public ChallengeType type;

    [Header("Conditions")]
    public int targetAmount = 1;

    [Header("Timer")]
    public bool useTimer = false;
    public float timeLimit = 10f;

    [Header("Reward")]
    public GameObject rewardPrefab;
}
