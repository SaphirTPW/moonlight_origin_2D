using UnityEngine;

public class TimedObj : MonoBehaviour
{
    private float _textLifeTime;
    [SerializeField] private float _StartTextLifeTime;
    [SerializeField] private AudioClip _startSFX;
    private bool _isActive = false;

    private void Start()
    {
        _textLifeTime = _StartTextLifeTime;
    }

    private void Update()
    {
        if (_isActive)
        {
            _textLifeTime -= Time.deltaTime;

            if(_textLifeTime <= 0)
            {
                DisableObj();
                _textLifeTime = _StartTextLifeTime;
            }
        }
    }

    public void EnableObj()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlaySFX(_startSFX);
        _isActive = true;
    }

    public void DisableObj()
    {
        gameObject.SetActive(false);
        _isActive = false;
    }
}
