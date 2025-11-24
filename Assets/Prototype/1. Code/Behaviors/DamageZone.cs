using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerHealth.Instance.IsReceivingDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth.Instance.IsReceivingDamage = false;
        }
    }
}
