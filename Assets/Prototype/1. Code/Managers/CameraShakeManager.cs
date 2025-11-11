using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    [SerializeField] private float _shakeForce = 1f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource pImpulseSource)
    {
        pImpulseSource.GenerateImpulseWithForce(_shakeForce);
    }
}
