using UnityEngine;

public class ScaleUp : MonoBehaviour
{
    [Header("Paramètres")]
    public Vector3 targetScale;  // Taille finale
    public float speed = 1f;                   // Vitesse d'agrandissement

    private Vector3 initialScale;
    private bool scaling = false;

    void Start()
    {
        initialScale = transform.localScale;
        StartScaling();
    }

    void Update()
    {
        if (scaling)
        {
            // Lerp vers la taille cible
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, speed * Time.deltaTime);

            // Arrêter quand on est assez proche de la taille cible
            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                transform.localScale = targetScale;
                scaling = false;
            }
        }
    }

    // Appel depuis un autre script ou un event pour lancer l'agrandissement
    public void StartScaling()
    {
        scaling = true;
    }
}
