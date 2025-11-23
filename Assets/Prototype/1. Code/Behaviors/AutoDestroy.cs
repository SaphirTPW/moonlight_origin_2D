using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private float _autoDestroyTime = 1f;

    private void Update()
    {
        AutoDestroyGameObject();
    }

    private void AutoDestroyGameObject()
    {
        _autoDestroyTime -= Time.deltaTime;
        if (_autoDestroyTime <= 0)
            Destroy(gameObject);
    }
}
