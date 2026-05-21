using UnityEngine;
using System.Collections;

public class CubeOrbAction : BossAction
{
    private CubeOrbSO _actionData;

    private Transform _bossTransform;
    private Transform _playerTransform;
    private AudioClip _orbShotSFX;
    private AudioClip _audioCue;

    private int _currentOrbIndex = 0;
    private GameObject[] _spawnedOrbs;
    private int _finishedOrbCount;

    public CubeOrbAction(CubeOrbSO data, Transform boss, Transform player, AudioClip orbShotSFX, AudioClip audioCue) : base(data)
    {
        _actionData = data;
        _bossTransform = boss;
        _playerTransform = player;
        _orbShotSFX = orbShotSFX;
        _audioCue = audioCue;
    }

    public override void StartAction()
    {
        AudioManager.Instance.PlaySFX(_audioCue);
        _currentOrbIndex = 0;
        _finishedOrbCount = 0;

        _spawnedOrbs = new GameObject[_actionData.orbCount];

        for (int i = 0; i < _actionData.orbCount; i++)
        {
            Vector3 spawnPos = _bossTransform.position + _bossTransform.right * 1.5f;
            _spawnedOrbs[i] = GameObject.Instantiate(_actionData.orbPrefab, spawnPos, Quaternion.identity);

            _spawnedOrbs[i].SetActive(false);
        }

        CoroutineRunner.Instance.StartCoroutine(OrbSequence());
    }

    public override void UpdateAction()
    {
        
    }

    private IEnumerator OrbSequence()
    {
        while (_currentOrbIndex < _spawnedOrbs.Length)
        {
            yield return new WaitForSeconds(_actionData.spawnDelay);

            GameObject orb = _spawnedOrbs[_currentOrbIndex];
            orb.SetActive(true);

            OrbProjectile orbScript = orb.GetComponent<OrbProjectile>();

            orbScript.OnOrbFinished += HandleOrbFinished;

            AudioManager.Instance.PlaySFX(_orbShotSFX, false, 1f);
            orbScript.Launch(_playerTransform, 
                _actionData.followDuration, 
                _actionData.followSpeed, 
                _actionData.projectileSpeed);

            _currentOrbIndex++;
        }
    }

    private void HandleOrbFinished()
    {
        _finishedOrbCount++;

        if(_finishedOrbCount >= _actionData.orbCount)
        {
            NotifyFinished();
        }
    }
}
