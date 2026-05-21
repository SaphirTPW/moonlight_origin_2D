using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private ChallengeController _challenge;
    [SerializeField] private AudioClip _collectSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _challenge.AddCollect(1);
            gameObject.SetActive(false);
            gameObject.GetComponent<Collider2D>().enabled = false;
            AudioManager.Instance.PlaySFX(_collectSFX, false, 0.75f);
        }
    }
}
