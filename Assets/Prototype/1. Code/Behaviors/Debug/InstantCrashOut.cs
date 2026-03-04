using UnityEngine;

public class InstantCrashOut : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Emotion[] _emotions;

    public void HandleInstantCrashOut()
    {
        for (int i = 0; i < _emotions.Length; i++)
        {
            if (_emotions[i].EmoState == Emotion.EmotionState.Awake)
            {
                _emotions[i].CurrentEmotionEnergy = 75f;
                _playerHealth.PlayerCurrentHealth = 25f;
            }
        }
    }
}
