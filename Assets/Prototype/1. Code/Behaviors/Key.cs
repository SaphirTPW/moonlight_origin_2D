using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private ChallengeController _collectController;
    [SerializeField] private AudioClip _collectKeySFX;

    public void SetOwner(ChallengeController pController)
    {
        _collectController = pController;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _collectController.AddCollect(1);
            AudioManager.Instance.PlaySFX(_collectKeySFX);
            Destroy(gameObject);
        }
    }
}
