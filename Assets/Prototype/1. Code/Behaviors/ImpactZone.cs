using UnityEngine;

public class ImpactZone : MonoBehaviour
{
    [SerializeField] private float _knockBackForce = 5f;
    [SerializeField] private float _knockBackUp = 3f;
    [SerializeField] private float _damage = 50f;
    [SerializeField] private AudioClip _damageSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            AudioManager.Instance.PlaySFX(_damageSound);

            if (playerHealth == null || playerMovement == null)
            {
                Debug.LogWarning("Le joueur n'a pas le composant attendu !");
                return;
            }

            if (collision.GetComponent<PlayerMovement>().RageArmorOn)
            {
                playerHealth.PlayerTakeDamage(_damage);
            }
            else
            {
                playerHealth.PlayerTakeDamage(_damage);
                playerMovement.PlayerKnockback(transform, _knockBackForce, _knockBackUp);
            }
        }
    }
}
